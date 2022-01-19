FROM registry.nexdev.net:5050/docker/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder

RUN apt-get update  \
	&& curl --silent --location https://deb.nodesource.com/setup_14.x | bash -  \
	&& apt-get install -y nodejs

RUN node -v
RUN npm -v

WORKDIR /src

COPY ClientApp/package.json ./
COPY ClientApp/package-lock.json ./

RUN npm install

COPY NuGet.Config .

COPY Fanex.Athena.AnalyticsAPI/SSAS/Fanex.Athena.LGAPI.SSAS.Bridge/Fanex.Athena.LGAPI.SSAS.Bridge.csproj ./SSAS/Fanex.Athena.LGAPI.SSAS.Bridge/
RUN dotnet restore SSAS/Fanex.Athena.LGAPI.SSAS.Bridge/Fanex.Athena.LGAPI.SSAS.Bridge.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.Core/Fanex.Athena.AnalyticsAPI.Core.csproj ./Fanex.Athena.AnalyticsAPI.Core/
RUN dotnet restore Fanex.Athena.AnalyticsAPI.Core/Fanex.Athena.AnalyticsAPI.Core.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.Common/Fanex.Athena.AnalyticsAPI.Common.csproj ./Fanex.Athena.AnalyticsAPI.Common/
RUN dotnet restore Fanex.Athena.AnalyticsAPI.Common/Fanex.Athena.AnalyticsAPI.Common.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Summary/Fanex.Athena.AnalyticsAPI.Summary.csproj ./Modules/Fanex.Athena.AnalyticsAPI.Summary/
RUN dotnet restore Modules/Fanex.Athena.AnalyticsAPI.Summary/Fanex.Athena.AnalyticsAPI.Summary.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Performance/Fanex.Athena.AnalyticsAPI.Performance.csproj ./Modules/Fanex.Athena.AnalyticsAPI.Performance/
RUN dotnet restore Modules/Fanex.Athena.AnalyticsAPI.Performance/Fanex.Athena.AnalyticsAPI.Performance.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Sportsbook/Fanex.Athena.AnalyticsAPI.Sportsbook.csproj ./Modules/Fanex.Athena.AnalyticsAPI.Sportsbook/
RUN dotnet restore Modules/Fanex.Athena.AnalyticsAPI.Sportsbook/Fanex.Athena.AnalyticsAPI.Sportsbook.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj ./Fanex.Athena.AnalyticsAPI/
RUN dotnet restore Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj -v n

COPY ClientApp/.babelrc ./
COPY ClientApp/config ./config

COPY ClientApp/src ./src

RUN node node_modules/webpack/bin/webpack.js  \
	--config config/webpack.config.js \
	--env.mode=production \
	--env.disableAnalyzer=true \
	--env.output=../../publish/wwwroot

COPY Fanex.Athena.AnalyticsAPI/SSAS/Fanex.Athena.LGAPI.SSAS.Bridge ./SSAS/Fanex.Athena.LGAPI.SSAS.Bridge
COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.Core ./Fanex.Athena.AnalyticsAPI.Core
COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.Common ./Fanex.Athena.AnalyticsAPI.Common
COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Summary ./Modules/Fanex.Athena.AnalyticsAPI.Summary
COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Performance ./Modules/Fanex.Athena.AnalyticsAPI.Performance
COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Sportsbook ./Modules/Fanex.Athena.AnalyticsAPI.Sportsbook
COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI/ ./Fanex.Athena.AnalyticsAPI

RUN dotnet publish "Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj" -c Release -o /publish \
  --runtime alpine-x64 \
  --self-contained false

FROM registry.nexdev.net:5050/docker/dotnet/core/aspnet:5.0-buster-slim

ENV ASPNETCORE_URLS=http://*:5000
EXPOSE 5000

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

WORKDIR /app

COPY --from=builder /publish .

ENTRYPOINT ["dotnet", "Fanex.Athena.AnalyticsAPI.dll"]