# Этап 1: сборка в образе с SDK — там компилируем и публикуем проект
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# сначала только csproj: слой с restore переиспользуется, пока не меняются зависимости
COPY NotesService.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Этап 2: финальный образ только с рантаймом, SDK внутрь не тащим
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "NotesService.dll"]
