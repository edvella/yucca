# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Yucca/Yucca.csproj", "Yucca/"]
RUN dotnet restore "Yucca/Yucca.csproj"

COPY . .
WORKDIR /src/Yucca
RUN dotnet publish "Yucca.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Yucca.dll"]
