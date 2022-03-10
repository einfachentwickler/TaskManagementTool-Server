CREATE DATABASE TaskManagementTool_Test;
USE TaskManagementTool_Test;

exec sp_create_AspNetRoles;
exec sp_create_AspNetRoleClaims;

exec sp_create_AspNetUsers;
exec sp_create_AspNetUserClaims;
exec sp_create_AspNetUserLogins;
exec sp_create_AspNetUserRoles;

exec sp_create_todos;
exec sp_create_AspNetUserTokens;

INSERT INTO [AspNetRoles] VALUES(NEWID(), 'Admin', 'ADMIN', NEWID());
INSERT INTO [AspNetRoles] VALUES(NEWID(), 'User', 'USER', NEWID());

INSERT INTO [AspNetUsers]
([Id], [FirstName], [LastName], [Age], [IsBlocked],[Role],[UserName],[NormalizedUserName],[Email],[NormalizedEmail], [PasswordHash],[SecurityStamp],[ConcurrencyStamp])
VALUES(NEWID(), 'First name 1', 'Last name 1', 35, 0,'User', 'user1@example.com', 'USER1@EXAMPLE.COM', 'user1@example.com', 'USER1@EXAMPLE.COM', 'AQAAAAEAACcQAAAAEM0PBQk7EtkeDodFLhzr7/xw/2X3TglLQ0DUC34Q7EPg2dYoYYj5xS1ch56HrLS6tg==', '4I4EXTYWIO625IBB64I46F7HMNQ5CWLL', 'e121b29a-1d2b-4d5b-baf5-504836aef73d');