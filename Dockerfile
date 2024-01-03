FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env
WORKDIR /app

COPY ["TaskManagementTool.Host/TaskManagementTool.Host.csproj", "TaskManagementTool.Host/"]
COPY ["TaskManagementTool.BusinessLogic/TaskManagementTool.BusinessLogic.csproj", "TaskManagementTool.BusinessLogic/"]
COPY ["TaskManagementTool.Common/TaskManagementTool.Common.csproj", "TaskManagementTool.Common/"]
COPY ["TaskManagementTool.DataAccess/TaskManagementTool.DataAccess.csproj", "TaskManagementTool.DataAccess/"]

RUN dotnet restore "./TaskManagementTool.Host/./TaskManagementTool.Host.csproj"

COPY . ./
RUN dotnet publish -c Release -o out 

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
EXPOSE 8080
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TaskManagementTool.Host.dll"]



