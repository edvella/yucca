# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj and restore
COPY ["Yucca.Web/Yucca.Web.csproj", "Yucca.Web/"]
RUN dotnet restore "Yucca.Web/Yucca.Web.csproj"

# copy everything and publish
COPY . .
WORKDIR /src/Yucca.Web
RUN dotnet publish "Yucca.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Yucca.Web.dll"]
