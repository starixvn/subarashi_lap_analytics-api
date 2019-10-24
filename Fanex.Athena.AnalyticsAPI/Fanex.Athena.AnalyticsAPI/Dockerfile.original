FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj", "Fanex.Athena.AnalyticsAPI/"]
COPY ["Modules/Fanex.Athena.AnalyticsAPI.Performance/Fanex.Athena.AnalyticsAPI.Performance.csproj", "Modules/Fanex.Athena.AnalyticsAPI.Performance/"]
COPY ["Fanex.Athena.AnalyticsAPI.Common/Fanex.Athena.AnalyticsAPI.Common.csproj", "Fanex.Athena.AnalyticsAPI.Common/"]
COPY ["SSAS/Fanex.Athena.LGAPI.SSAS.Bridge/Fanex.Athena.LGAPI.SSAS.Bridge.csproj", "SSAS/Fanex.Athena.LGAPI.SSAS.Bridge/"]
COPY ["Fanex.Athena.AnalyticsAPI.Core/Fanex.Athena.AnalyticsAPI.Core.csproj", "Fanex.Athena.AnalyticsAPI.Core/"]
RUN dotnet restore "Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj"
COPY . .
WORKDIR "/src/Fanex.Athena.AnalyticsAPI"
RUN dotnet build "Fanex.Athena.AnalyticsAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Fanex.Athena.AnalyticsAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Fanex.Athena.AnalyticsAPI.dll"]