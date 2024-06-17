FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY . .

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DiscountManager.Client/DiscountManager.Client.csproj", "DiscountManager.Client/"]
RUN dotnet restore "DiscountManager.Client/DiscountManager.Client.csproj"
COPY . .
WORKDIR "/src/DiscountManager.Client"
RUN dotnet build "DiscountManager.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscountManager.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscountManager.Client.dll"]
