FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["RailwayTracker.API/RailwayTracker.API.csproj", "RailwayTracker.API/"]
COPY ["RailwayTracker.Application/RailwayTracker.Application.csproj", "RailwayTracker.Application/"]
COPY ["RailwayTracker.Domain/RailwayTracker.Domain.csproj", "RailwayTracker.Domain/"]
COPY ["RailwayTracker.Infrastructure/RailwayTracker.Infrastructure.csproj", "RailwayTracker.Infrastructure/"]
RUN dotnet restore "RailwayTracker.API/RailwayTracker.API.csproj"
COPY . .
WORKDIR "/src/RailwayTracker.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RailwayTracker.API.dll"]