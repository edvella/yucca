# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Yucca.AppHost/Yucca.AppHost.csproj", "Yucca.AppHost/"]
RUN dotnet restore "Yucca.AppHost/Yucca.AppHost.csproj"

COPY . .
WORKDIR /src/Yucca.AppHost
RUN dotnet publish "Yucca.AppHost.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Yucca.AppHost.dll"]
