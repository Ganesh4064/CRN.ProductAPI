# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["CRN.ProductAPI.csproj", "./"]
RUN dotnet restore "CRN.ProductAPI.csproj"

# Copy the remaining source code
COPY . .

# Build and publish the application
RUN dotnet publish "CRN.ProductAPI.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose HTTP and HTTPS ports
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CRN.ProductAPI.dll"]