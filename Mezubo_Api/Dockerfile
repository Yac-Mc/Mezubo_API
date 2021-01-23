#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Mezubo_Api/Mezubo_Api.csproj", "Mezubo_Api/"]
RUN dotnet restore "Mezubo_Api/Mezubo_Api.csproj"
COPY . .
WORKDIR "/src/Mezubo_Api"
RUN dotnet build "Mezubo_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mezubo_Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mezubo_Api.dll"]