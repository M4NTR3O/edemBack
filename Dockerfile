# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

COPY edemBack.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Этап выполнения
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Не копируем Params.json — он будет примонтирован
EXPOSE 5077
EXPOSE 7159

ENTRYPOINT ["dotnet", "edemBack.dll"]