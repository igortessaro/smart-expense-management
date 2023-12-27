FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
COPY . .
RUN dotnet build "SmartExpenseManagement.Api/SmartExpenseManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartExpenseManagement.Api/SmartExpenseManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartExpenseManagement.Api.dll"]
