#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Pokemon.Api/Pokemon.Api.csproj", "Pokemon.Api/"]
COPY ["Pokemon.Client/Pokemon.Client.csproj", "Pokemon.Client/"]
COPY ["Pokemon.Application/Pokemon.Application.csproj", "Pokemon.Application/"]
COPY ["Pokemon.Domain/Pokemon.Domain.csproj", "Pokemon.Domain/"]
COPY ["Pokemon.Infrastructure/Pokemon.Infrastructure.csproj", "Pokemon.Infrastructure/"]
COPY ["Pokemon.Contracts/Pokemon.Contracts.csproj", "Pokemon.Contracts/"]
RUN dotnet restore "Pokemon.Api/Pokemon.Api.csproj"
COPY . .
WORKDIR "/src/Pokemon.Api"
RUN dotnet build "Pokemon.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pokemon.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokemon.Api.dll"]