# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["DotNetAngularApi.csproj", "./"]
RUN dotnet restore "DotNetAngularApi.csproj"

COPY . .
RUN dotnet publish "DotNetAngularApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "DotNetAngularApi.dll"]
