FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos de projeto das dependências primeiro para otimizar cache
COPY . .

# COPY ["src/CashFlow.Api/CashFlow.Api.csproj", "CashFlow.Api/"]
# COPY ["src/CashFlow.Core/CashFlow.Application.csproj", "CashFlow.Application/"]
# COPY ["src/CashFlow.Core/CashFlow.Domain.csproj", "CashFlow.Domain/"]
# COPY ["src/CashFlow.Infrastructure/CashFlow.Infrastructure.csproj", "CashFlow.Infrastructure/"]
# COPY ["src/ServiceDefaults/ServiceDefaults.csproj", "ServiceDefaults/"]

RUN dotnet restore "CashFlow.Api/CashFlow.Api.csproj"

COPY . .

WORKDIR "/src/CashFlow.Api"
RUN dotnet publish "CashFlow.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CashFlow.Api.dll"]
