FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AtonWebApi/AtonWebApi.csproj", "AtonWebApi/"]
COPY ["AtonWebApi.Application/AtonWebApi.Application.csproj", "AtonWebApi.Application/"]
COPY ["AtonWebApi.Domain/AtonWebApi.Domain.csproj", "AtonWebApi.Domain/"]
COPY ["AtonWebApi.Infrastructure/AtonWebApi.Infrastructure.csproj", "AtonWebApi.Infrastructure/"]

RUN dotnet restore "AtonWebApi/AtonWebApi.csproj"
COPY . .
WORKDIR "/src/AtonWebApi"
RUN dotnet build "AtonWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AtonWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AtonWebApi.dll"]
