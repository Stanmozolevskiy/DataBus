CREATE TABLE [dbo].[User](
    [Id] BIGINT NOT NULL  IDENTITY(100, 1), 
    [EmailAddress] NVARCHAR(100) NOT NULL, 
    [EmailAddressConfirmed] BIT NOT NULL CONSTRAINT [DF_User_EmailAddressConfirmed] DEFAULT 0, 
    [FirstName] NVARCHAR(100) NOT NULL, 
    [LastName] NVARCHAR(100) NOT NULL,
    [PhoneNumber] NVARCHAR(50) NULL, 
    [PhoneNumberConfirmed] BIT NULL CONSTRAINT [DF_User_PhoneNumberConfirmed] DEFAULT 0,
    [IsActive] BIT CONSTRAINT [DF_User_IsActive] DEFAULT ((0)) NOT NULL,
    [FullName] AS ([FirstName] + ' ' + [LastName])
)
GO

CREATE NONCLUSTERED INDEX [NC_IX-EmailAddress] ON [dbo].[User] ([EmailAddress] ASC) INCLUDE ([FirstName], [LastName], [IsActive])
GO