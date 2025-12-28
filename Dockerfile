FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env
WORKDIR /app

COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore ".WebApi/./WebApi.csproj"
RUN dotnet restore ".Application/./Application.csproj"
RUN dotnet restore ".Shared/./Shared.csproj"
RUN dotnet restore ".Infrastructure/./Infrastructure.csproj"

COPY . ./
RUN dotnet publish -c Release -o out 

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
EXPOSE 8080
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WebApi.dll"]