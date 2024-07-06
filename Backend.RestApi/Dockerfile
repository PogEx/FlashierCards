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

ENV FLASHIERCARDS_CONFIG_PATH "/flashiercards/config/"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["Backend.RestApi/Backend.RestApi.csproj", "Backend.RestApi/"]
RUN dotnet restore "Backend.RestApi/Backend.RestApi.csproj"
COPY . .

WORKDIR "/src/Backend.RestApi"
RUN dotnet build "Backend.RestApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "Backend.RestApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.RestApi.dll"]
