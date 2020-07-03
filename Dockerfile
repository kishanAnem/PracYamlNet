FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["PracYamlNet.csproj", "./"]
RUN dotnet restore "./PracYamlNet.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PracYamlNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PracYamlNet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PracYamlNet.dll"]
