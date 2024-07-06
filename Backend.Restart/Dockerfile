FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER root
RUN mkdir -p /flashiercards/data
RUN mkdir -p /flashiercards/config

RUN chown -R $APP_UID:$APP_UID /flashiercards

USER $APP_UID

ENV FLASHIERCARDS_BACKEND_CONFIG_PATH "/flashiercards/config/"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Backend.Restart/Backend.Restart.csproj", "Backend.Restart/"]
COPY ["Backend.Database/Backend.Database.csproj", "Backend.Database/"]
RUN dotnet restore "Backend.Restart/Backend.Restart.csproj"
COPY . .
WORKDIR "/src/Backend.Restart"
RUN dotnet build "Backend.Restart.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Backend.Restart.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.Restart.dll"]
