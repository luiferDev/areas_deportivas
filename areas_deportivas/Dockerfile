FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["areas_deportivas/areas_deportivas.csproj", "areas_deportivas/"]
RUN dotnet restore "areas_deportivas/areas_deportivas.csproj"

COPY . .
WORKDIR "/src/areas_deportivas"
RUN dotnet publish "areas_deportivas.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "areas_deportivas.dll"]
