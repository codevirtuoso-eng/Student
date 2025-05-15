# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files
COPY MvcWebApplication/*.csproj ./MvcWebApplication/
RUN dotnet restore ./MvcWebApplication/MvcWebApplication.csproj

# Copy everything and build
COPY . .
WORKDIR /app/MvcWebApplication
RUN dotnet publish -c Release -o /out MvcWebApplication.csproj

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Set environment variable and entry point
ENV ASPNETCORE_URLS=http://+:${PORT}
ENTRYPOINT ["dotnet", "Student.dll"]
