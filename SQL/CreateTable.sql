CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](128) NOT NULL,
	[FirstName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[Sex] [smallint] NOT NULL,
	[Address] [nvarchar](500) NULL,
	[Zip] [nvarchar](255) NULL,
	[Photo] [nvarchar](1000) NULL,
	[SchoolId] [uniqueidentifier] NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](500) NULL,
	[SecurityStamp] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](255) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[SendSmsCodeTime] [datetime] NULL,
	[Language] [varchar](20) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

