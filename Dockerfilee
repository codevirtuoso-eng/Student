# Build and publish MvcWebApplication
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore ./WebAppArchitectureSetupNet8.sln
RUN dotnet publish ./MvcWebApplication/MvcWebApplication.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MvcWebApplication.dll"]
