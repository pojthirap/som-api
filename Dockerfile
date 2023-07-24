FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
RUN mkdir MyApp
COPY ["MyFirstAzureWebApp.csproj", "MyApp/"]
RUN dotnet restore "./MyApp/MyFirstAzureWebApp.csproj"
COPY . MyApp/
WORKDIR "/src/MyApp"
RUN dotnet build "MyFirstAzureWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFirstAzureWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFirstAzureWebApp.dll"]
