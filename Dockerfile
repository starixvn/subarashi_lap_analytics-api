FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-nodejs-v20 AS builder

WORKDIR /src

COPY ClientApp/package.json ./
COPY ClientApp/package-lock.json ./

RUN node -v
RUN npm -v
RUN npm install

COPY NuGet.Config .

COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.Core/Fanex.Athena.AnalyticsAPI.Core.csproj ./Fanex.Athena.AnalyticsAPI.Core/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Fanex.Athena.AnalyticsAPI.Core/Fanex.Athena.AnalyticsAPI.Core.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.Common/Fanex.Athena.AnalyticsAPI.Common.csproj ./Fanex.Athena.AnalyticsAPI.Common/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Fanex.Athena.AnalyticsAPI.Common/Fanex.Athena.AnalyticsAPI.Common.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Summary/Fanex.Athena.AnalyticsAPI.Summary.csproj ./Modules/Fanex.Athena.AnalyticsAPI.Summary/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Modules/Fanex.Athena.AnalyticsAPI.Summary/Fanex.Athena.AnalyticsAPI.Summary.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Performance/Fanex.Athena.AnalyticsAPI.Performance.csproj ./Modules/Fanex.Athena.AnalyticsAPI.Performance/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Modules/Fanex.Athena.AnalyticsAPI.Performance/Fanex.Athena.AnalyticsAPI.Performance.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Sportsbook/Fanex.Athena.AnalyticsAPI.Sportsbook.csproj ./Modules/Fanex.Athena.AnalyticsAPI.Sportsbook/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Modules/Fanex.Athena.AnalyticsAPI.Sportsbook/Fanex.Athena.AnalyticsAPI.Sportsbook.csproj -v n

COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj ./Fanex.Athena.AnalyticsAPI/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj -v n

COPY ClientApp/.babelrc ./
COPY ClientApp/config ./config

COPY ClientApp/src ./src

RUN node node_modules/webpack/bin/webpack.js  \
	--config config/webpack.config.js \
	--env production \
	--env disableAnalyzer=true \
	--env output=../../publish/wwwroot

COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.Core ./Fanex.Athena.AnalyticsAPI.Core
COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.Common ./Fanex.Athena.AnalyticsAPI.Common
COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Summary ./Modules/Fanex.Athena.AnalyticsAPI.Summary
COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Performance ./Modules/Fanex.Athena.AnalyticsAPI.Performance
COPY Fanex.Athena.AnalyticsAPI/Modules/Fanex.Athena.AnalyticsAPI.Sportsbook ./Modules/Fanex.Athena.AnalyticsAPI.Sportsbook
COPY Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI/ ./Fanex.Athena.AnalyticsAPI

RUN dotnet publish "Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI.csproj" -c Release -o /publish \
	--runtime linux-x64 \
	--self-contained false

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS final

ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000

# Dont run as root
RUN adduser --disabled-password \
	--home /app \
	--gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

WORKDIR /app

COPY --from=builder /publish .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.Athena.AnalyticsAPI.dll"]