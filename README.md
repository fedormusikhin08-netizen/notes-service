# Учётная заметок

Учебный сервис для работы с текстовыми заметками.

## Описание

Приложение позволяет пользователю создавать текстовые заметки, редактировать,
удалять и искать по ним. Основная цель - надежно хранить заметки в базе данных.

В рамках контрольной точки по контейнерам реализовано минимальное API:
список заметок, получение одной заметки и проверка живости сервиса.
Данные пока лежат в памяти приложения, база данных подключится позже
по плану из api-plan.md.

## Технологии

- Язык: C# (.NET 8)
- Фреймворк: ASP.NET Core Web API
- ORM: Entity Framework Core
- Хранение: SQLite
- Тестирование: автоматическое тестирование
- Контейнеризация: Docker

## Состав проекта

- README.md - описание проекта (этот файл)
- project-notes.md - рабочие заметки проекта
- api-plan.md - план REST API
- Program.cs - код API
- NotesService.csproj - файл проекта
- Dockerfile - сборка образа контейнера
- .dockerignore - файлы, которые не нужны внутри образа

## Endpoints

- GET /health - проверка, что сервис жив
- GET /notes - список всех заметок
- GET /notes/{id} - одна заметка по номеру

Формат заметки: id, title, text, created_at - как в api-plan.md.

## Запуск в Docker

Сборка образа:

docker build -t notes-service .

Запуск контейнера:

docker run -d -p 8080:8080 --name notes-service-container notes-service

Проверка в браузере или через curl:

curl http://localhost:8080/health
curl http://localhost:8080/notes
curl http://localhost:8080/notes/2

## Выводы команд

Сборка образа (docker build):

#13 6.779   NotesService -> /app/bin/Release/net8.0/NotesService.dll
#13 6.868   NotesService -> /app/out/
#15 naming to docker.io/library/notes-service:latest
#15 DONE 11.2s

Запуск контейнера (docker run):

63333c619452a7976664549b533ec72f1a17dd804f4ffd52a017b78a54f4727b

docker ps:

CONTAINER ID   IMAGE           COMMAND                  CREATED        STATUS        PORTS                                          NAMES
63333c619452   notes-service   "dotnet NotesService…"   7 seconds ago   Up 3 seconds   0.0.0.0:8080->8080/tcp, [::]:8080->8080/tcp   notes-service-container

Ответ GET /notes:

[{"id":1,"title":"Покупки","text":"Молоко, хлеб, кофе","created_at":"2026-10-27T00:00:00"},{"id":2,"title":"Идеи для проекта","text":"Сделать API заметок и собрать его в Docker","created_at":"2026-10-28T00:00:00"},{"id":3,"title":"Учёба","text":"Дочитать конспект по ASP.NET Core","created_at":"2026-10-29T00:00:00"}]

docker logs notes-service-container:

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

## Основные команды

docker build -t notes-service . - сборка образа
docker run -d -p 8080:8080 --name notes-service-container notes-service - запуск контейнера
docker ps - список запущенных контейнеров
docker logs notes-service-container - логи контейнера
docker stop notes-service-container - остановить контейнер
