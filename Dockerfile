FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["downr.csproj", "./"]
RUN dotnet restore "./downr.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "downr.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "downr.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "downr.dll"]
