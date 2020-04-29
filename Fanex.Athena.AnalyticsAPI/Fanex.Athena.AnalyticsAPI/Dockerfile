#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-nanoserver-1803 AS build
WORKDIR /src
COPY ["Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj", "Fanex.Athena.AnalyticsAPI/"]
COPY ["Modules/Fanex.Athena.AnalyticsAPI.Performance/Fanex.Athena.AnalyticsAPI.Performance.csproj", "Modules/Fanex.Athena.AnalyticsAPI.Performance/"]
COPY ["Fanex.Athena.AnalyticsAPI.Common/Fanex.Athena.AnalyticsAPI.Common.csproj", "Fanex.Athena.AnalyticsAPI.Common/"]
COPY ["SSAS/Fanex.Athena.LGAPI.SSAS.Bridge/Fanex.Athena.LGAPI.SSAS.Bridge.csproj", "SSAS/Fanex.Athena.LGAPI.SSAS.Bridge/"]
COPY ["Fanex.Athena.AnalyticsAPI.Core/Fanex.Athena.AnalyticsAPI.Core.csproj", "Fanex.Athena.AnalyticsAPI.Core/"]
COPY ["Modules/Fanex.Athena.AnalyticsAPI.Summary/Fanex.Athena.AnalyticsAPI.Summary.csproj", "Modules/Fanex.Athena.AnalyticsAPI.Summary/"]
RUN dotnet restore "Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj"
COPY . .
WORKDIR "/src/Fanex.Athena.AnalyticsAPI"
RUN dotnet build "Fanex.Athena.AnalyticsAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fanex.Athena.AnalyticsAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fanex.Athena.AnalyticsAPI.dll"]