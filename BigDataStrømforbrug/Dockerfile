#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
#WORKDIR /app
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["BigDataStrømforbrug/BigDataStrømforbrug.csproj", "BigDataStrømforbrug/"]
#RUN dotnet restore "BigDataStrømforbrug/BigDataStrømforbrug.csproj"
#COPY . .
#WORKDIR "/src/BigDataStrømforbrug"
#RUN dotnet build "BigDataStrømforbrug.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "BigDataStrømforbrug.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "BigDataStrømforbrug.dll"]
#
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
ENV Town TestBy
ENV Endopint https://localhost:7215
ENV Amount 100
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "BigDataStrømforbrug.dll"]