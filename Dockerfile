# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files
COPY Student/*.csproj ./Student/
RUN dotnet restore ./Student/Student.csproj

# Copy everything and build
COPY . . 
WORKDIR /app/Student
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Set environment variable and entry point
ENV ASPNETCORE_URLS=http://+:${PORT}
ENTRYPOINT ["dotnet", "Student.dll"]
