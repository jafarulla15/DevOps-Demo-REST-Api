# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file first so restore is cached unless dependencies change
COPY dotnet-angular-api-sample/*.csproj ./dotnet-angular-api-sample/
RUN dotnet restore ./dotnet-angular-api-sample/DotNetAngularApi.csproj

COPY dotnet-angular-api-sample/. ./dotnet-angular-api-sample/
RUN dotnet publish ./dotnet-angular-api-sample/DotNetAngularApi.csproj -c Release -o /app/publish --no-restore

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8070
ENV ASPNETCORE_URLS=http://+:8070

ENTRYPOINT ["dotnet", "DotNetAngularApi.dll"]