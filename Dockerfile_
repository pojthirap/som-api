#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
COPY /app /app
ENTRYPOINT ["dotnet", "MyFirstAzureWebApp.dll"]

#
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
#WORKDIR /src
#COPY ["MyFirstAzureWebApp.csproj", ""]
#RUN dotnet restore "./MyFirstAzureWebApp.csproj"
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "MyFirstAzureWebApp.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "MyFirstAzureWebApp.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "MyFirstAzureWebApp.dll", "--server.urls"]
#