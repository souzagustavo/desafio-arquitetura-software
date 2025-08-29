FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CashFlow.Api.csproj", "CashFlow.Api/"]
COPY ["../CashFlow.Application/CashFlow.dadad.csproj", "src/CashFlow.Application/"]

RUN dotnet restore "CashFlow.Api/CashFlow.Api.csproj"
COPY . .
WORKDIR "/src/CashFlow.Api"
RUN dotnet build "CashFlow.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CashFlow.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CashFlow.Api.dll"]