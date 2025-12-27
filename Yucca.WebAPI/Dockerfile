# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Yucca.WebAPI/Yucca.WebAPI.csproj", "Yucca.WebAPI/"]
RUN dotnet restore "Yucca.WebAPI/Yucca.WebAPI.csproj"

COPY . .
WORKDIR /src/Yucca.WebAPI
RUN dotnet publish "Yucca.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Yucca.WebAPI.dll"]
