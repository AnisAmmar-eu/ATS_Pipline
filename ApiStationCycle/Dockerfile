FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CNFIGURATION=RELEASE
WORKDIR /src
COPY ["ApiStationCycle/ApiStationCycle.csproj","ApiStationCycle"]
RUN dotnet restore "./ApiStationCycle/./ApiStationCycle.csproj"
COPY . . 
WORKDIR "/src/ApiStationCycle"
RUN dotnet build "./ApiStationCycle.csproj" -c $BUILD_CNFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CNFIGURATION=RELEASE
RUN dotnet publish "./ApiStationCycle.csproj" -c $BUILD_CNFIGURATION -o /app/publish /p:UseAppHost=false

From base AS final
WORKDIR /appCOPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","ApiStationCycle.dll"]