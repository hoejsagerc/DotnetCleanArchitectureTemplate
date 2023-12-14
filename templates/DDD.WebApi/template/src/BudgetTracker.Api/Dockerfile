FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["template/src/BudgetTracker.Api/BudgetTracker.Api.csproj", "template/src/BudgetTracker.Api/"]
COPY ["template/src/BudgetTracker.Contracts/BudgetTracker.Contracts.csproj", "template/src/BudgetTracker.Contracts/"]
COPY ["template/src/BudgetTracker.Infrastructure/BudgetTracker.Infrastructure.csproj", "template/src/BudgetTracker.Infrastructure/"]
COPY ["template/src/BudgetTracker.Application/BudgetTracker.Application.csproj", "template/src/BudgetTracker.Application/"]
COPY ["template/src/BudgetTracker.Domain/BudgetTracker.Domain.csproj", "template/src/BudgetTracker.Domain/"]
RUN dotnet restore "template/src/BudgetTracker.Api/BudgetTracker.Api.csproj"
COPY . .
WORKDIR "/src/template/src/BudgetTracker.Api"
RUN dotnet build "BudgetTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetTracker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetTracker.Api.dll"]
