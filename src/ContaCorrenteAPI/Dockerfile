FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# COPIA A PUBLICAÇÃO
COPY --from=build /app/publish .

# COPIA A PASTA DE SCRIPTS QUE ESTÁ EM /src/Scripts PARA /app/Scripts
COPY --from=build /src/Scripts /app/Scripts

ENTRYPOINT ["dotnet", "ContaCorrenteAPI.dll"]
