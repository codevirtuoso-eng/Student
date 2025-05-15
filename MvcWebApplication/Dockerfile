FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore ./MvcWebApplication.csproj
RUN dotnet publish ./MvcWebApplication.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "MvcWebApplication.dll"]
