FROM microsoft/dotnet:2.1.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1.400-sdk AS builder
ARG Configuration=Release
WORKDIR /src
COPY *.sln ./
COPY MarsRover.UnitTest/MarsRover.UnitTest.csproj MarsRover.UnitTest/
COPY MarsRover.IntegrationTest/MarsRover.IntegrationTest.csproj MarsRover.IntegrationTest/
COPY MarsRover.Web/MarsRover.Web.csproj MarsRover.Web/
COPY MarsRover.WebApi/MarsRover.WebApi.csproj MarsRover.WebApi/
COPY MarsRover.Core/MarsRover.Core.csproj MarsRover.Core/
COPY MarsRover.Data/MarsRover.Data.csproj MarsRover.Data/
COPY MarsRover.Service/MarsRover.Service.csproj MarsRover.Service/
COPY MarsRover.Host/MarsRover.Host.csproj MarsRover.Host/
COPY MarsRover.Host/dates.txt MarsRover.Host/
COPY MarsRover.Host/dates.txt /app/
COPY MarsRover.Host/appsettings.json MarsRover.Host/
COPY MarsRover.Host/appsettings.Development.json MarsRover.Host/
COPY MarsRover.Host/appsettings.Production.json MarsRover.Host/
COPY MarsRover.Host/hostsettings.json MarsRover.Host/

RUN dotnet restore
COPY . .
WORKDIR /src/MarsRover.Host
RUN dotnet build -c $Configuration -o /app

FROM builder AS publish
ARG Configuration=Release
RUN dotnet publish -c $Configuration -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MarsRover.Host.dll"]
