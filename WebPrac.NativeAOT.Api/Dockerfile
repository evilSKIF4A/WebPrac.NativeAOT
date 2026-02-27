FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebPrac.NativeAOT.Api/WebPrac.NativeAOT.Api.csproj", "WebPrac.NativeAOT.Api/"]
RUN dotnet restore "WebPrac.NativeAOT.Api/WebPrac.NativeAOT.Api.csproj"
COPY . .
WORKDIR "/src/WebPrac.NativeAOT.Api"
RUN dotnet build "./WebPrac.NativeAOT.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebPrac.NativeAOT.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebPrac.NativeAOT.Api.dll"]
