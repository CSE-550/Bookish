#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Bookish/Server/Bookish.Server.csproj", "Bookish/Server/"]
COPY ["Bookish/Data/Bookish.Data.csproj", "Bookish/Data/"]
COPY ["Bookish/DataServices/Bookish.DataServices.csproj", "Bookish/DataServices/"]
COPY ["Bookish/Models/Bookish.Models.csproj", "Bookish/Models/"]
COPY ["Bookish/Client/Bookish.Client.csproj", "Bookish/Client/"]
RUN dotnet restore "Bookish/Server/Bookish.Server.csproj"
COPY . .
WORKDIR "/src/Bookish/Server"
RUN dotnet build "Bookish.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bookish.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookish.Server.dll"]