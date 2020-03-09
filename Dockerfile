FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /app

COPY Core/Core.csproj Core/Core.csproj
COPY Api/DataAccess/Api.DataAccess.csproj Api/DataAccess/Api.DataAccess.csproj
COPY Api/Host/Api.Host.csproj Api/Host/Api.Host.csproj
RUN dotnet restore Api/Host/Api.Host.csproj
COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=base /app/out .
ENTRYPOINT ["dotnet", "Shakespokemon.Api.Host.dll"]