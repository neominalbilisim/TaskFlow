# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["src/TaskFlow.API/TaskFlow.API.csproj", "TaskFlow.API/"]
COPY ["src/TaskFlow.Application/TaskFlow.Application.csproj", "TaskFlow.Application/"]
COPY ["src/TaskFlow.Domain/TaskFlow.Domain.csproj", "TaskFlow.Domain/"]
COPY ["src/TaskFlow.Infrastructure/TaskFlow.Infrastructure.csproj", "TaskFlow.Infrastructure/"]

RUN dotnet restore "TaskFlow.API/TaskFlow.API.csproj"

# Copy everything and build
COPY src/ .
WORKDIR "/src/TaskFlow.API"
RUN dotnet build "TaskFlow.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "TaskFlow.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskFlow.API.dll"]
