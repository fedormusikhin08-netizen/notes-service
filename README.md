# Сервис заметок

Небольшое API для текстовых заметок на .NET 8. Сделано для контрольной точки
по контейнерам, названия полей и формат ответов взяты из api-plan.md — это
зафиксированная тема проекта.

## Что умеет API

- GET /health — возвращает OK, простая проверка что сервис жив
- GET /notes — все заметки списком
- GET /notes/{id} — одна заметка по номеру, если её нет — 404

Данные пока лежат в памяти процесса: перезапустил контейнер — список вернулся
к исходному. База (по плану это EF Core и SQLite) подключится следующим
этапом, для этой КТ хватает и такого варианта.

Поля заметки в JSON: id, title, text, created_at — как в api-plan.md.

## Из чего состоит проект

Program.cs — сам API, весь в одном файле (minimal API);
NotesService.csproj — проект .NET 8;
Dockerfile — многоступенчатая сборка: первый этап компилируем в образе с SDK,
затем в образ только с рантаймом переносим опубликованное приложение;
.dockerignore — чтобы bin, obj и md не попадали в контекст сборки;
api-plan.md и project-notes.md — документация по теме;
README.md — этот файл.

## Сборка и запуск

Нужен Docker Desktop с запущенным движком (в трее пишет Engine running).

Собираю образ:

docker build -t notes-service .

Запускаю контейнер, пробрасываю порт 8080 наружу:

docker run -d -p 8080:8080 --name notes-service-container notes-service

Проверяю API через curl (в браузере по адресу localhost:8080 тоже откроется):

curl http://localhost:8080/health
curl http://localhost:8080/notes
curl http://localhost:8080/notes/2

## Реальные выводы

Сборка прошла без ошибок, хвост вывода docker build:

#13 6.779   NotesService -> /app/bin/Release/net8.0/NotesService.dll
#13 6.868   NotesService -> /app/out/
#15 naming to docker.io/library/notes-service:latest
#15 DONE 11.2s

Команда docker run вернула ID контейнера:

63333c619452a7976664549b533ec72f1a17dd804f4ffd52a017b78a54f4727b

docker ps — контейнер поднят, порт проброшен:

CONTAINER ID   IMAGE           COMMAND                  CREATED        STATUS        PORTS                                          NAMES
63333c619452   notes-service   "dotnet NotesService…"   7 seconds ago   Up 3 seconds   0.0.0.0:8080->8080/tcp, [::]:8080->8080/tcp   notes-service-container

curl http://localhost:8080/notes:

[{"id":1,"title":"Покупки","text":"Молоко, хлеб, кофе","created_at":"2026-10-27T00:00:00"},{"id":2,"title":"Идеи для проекта","text":"Сделать API заметок и собрать его в Docker","created_at":"2026-10-28T00:00:00"},{"id":3,"title":"Учёба","text":"Дочитать конспект по ASP.NET Core","created_at":"2026-10-29T00:00:00"}]

docker logs notes-service-container — видно, что сервис слушает нужный порт:

warn: Microsoft.AspNetCore.Hosting.Diagnostics[15]
      Overriding HTTP_PORTS '8080' and HTTPS_PORTS ''. Binding to values defined by URLS instead 'http://0.0.0.0:8080'.
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://0.0.0.0:8080
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /app

## Что ещё пригодится

docker stop notes-service-container — остановить контейнер
docker rm notes-service-container — удалить, чтобы запустить заново
docker images — проверить, что образ на месте
