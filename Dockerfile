FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build

WORKDIR /
COPY src .

WORKDIR "/TrueLayer.Pokedex.Api"
RUN dotnet build "TrueLayer.Pokedex.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrueLayer.Pokedex.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "TrueLayer.Pokedex.Api.dll"]
