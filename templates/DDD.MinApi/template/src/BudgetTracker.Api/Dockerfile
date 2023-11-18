FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/BudgetTracker.Api/BudgetTracker.Api.csproj", "src/BudgetTracker.Api/"]
COPY ["src/BudgetTracker.Contracts/BudgetTracker.Contracts.csproj", "src/BudgetTracker.Contracts/"]
COPY ["src/BudgetTracker.Infrastructure/BudgetTracker.Infrastructure.csproj", "src/BudgetTracker.Infrastructure/"]
COPY ["src/BudgetTracker.Application/BudgetTracker.Application.csproj", "src/BudgetTracker.Application/"]
COPY ["src/BudgetTracker.Domain/BudgetTracker.Domain.csproj", "src/BudgetTracker.Domain/"]
RUN dotnet restore "src/BudgetTracker.Api/BudgetTracker.Api.csproj"
COPY . .
WORKDIR "/src/src/BudgetTracker.Api"
RUN dotnet build "BudgetTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetTracker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetTracker.Api.dll"]
