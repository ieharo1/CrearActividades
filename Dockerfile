FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore src/EnterpriseMediaVault.API/EnterpriseMediaVault.API.csproj
RUN dotnet publish src/EnterpriseMediaVault.API/EnterpriseMediaVault.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EnterpriseMediaVault.API.dll"]

