FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["LoggerService/LoggerService.csproj", "LoggerService/"]

RUN dotnet restore "WebApi/WebApi.csproj"

COPY . ./
RUN dotnet publish -c Release -o out 

FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app
EXPOSE 8080
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WebApi.dll"]