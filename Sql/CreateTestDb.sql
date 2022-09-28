USE [master]
GO

/****** Object:  Database [TaskManagementTool_Test]    Script Date: 28.09.2022 15:09:54 ******/
CREATE DATABASE [TaskManagementTool_Test];

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TaskManagementTool_Test].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [TaskManagementTool_Test] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET ARITHABORT OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [TaskManagementTool_Test] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [TaskManagementTool_Test] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [TaskManagementTool_Test] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET  ENABLE_BROKER 
GO

ALTER DATABASE [TaskManagementTool_Test] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [TaskManagementTool_Test] SET READ_COMMITTED_SNAPSHOT ON 
GO

ALTER DATABASE [TaskManagementTool_Test] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [TaskManagementTool_Test] SET  MULTI_USER 
GO

ALTER DATABASE [TaskManagementTool_Test] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [TaskManagementTool_Test] SET DB_CHAINING OFF 
GO

ALTER DATABASE [TaskManagementTool_Test] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [TaskManagementTool_Test] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [TaskManagementTool_Test] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [TaskManagementTool_Test] SET QUERY_STORE = OFF
GO

USE [TaskManagementTool_Test]
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE [TaskManagementTool_Test] SET  READ_WRITE 
GO

