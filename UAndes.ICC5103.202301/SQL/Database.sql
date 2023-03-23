Create Database InscripcionesBrDb
GO


USE [InscripcionesBrDb]
GO

CREATE TABLE [dbo].[Enajenacion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CNE] nvarchar(28) NULL,
	[Comuna] nvarchar(20) NOT NULL,
	[Manzana] [int] NOT NULL,
	[Predio] [int] NOT NULL,
	[Fojas] [int] NOT NULL,
	[FechaInscripcion] [date] NOT NULL,
	[IdInscripcion] [int] NOT NULL,
	CONSTRAINT [PK_Enajenacion] PRIMARY KEY CLUSTERED(
		[Id] ASC
	)
)
GO


USE [InscripcionesBrDb]
GO
SET IDENTITY_INSERT [dbo].[Enajenacion] ON 
GO
INSERT [dbo].[Enajenacion] ([Id], [CNE], [Comuna], [Manzana], [Predio], [Fojas], [FechaInscripcion], [IdInscripcion]) VALUES (1, N'Compraventa', N'Las Condes', 12345, 123, 62145, CAST(N'2022-03-18' AS Date), 1)
GO
SET IDENTITY_INSERT [dbo].[Enajenacion] OFF
GO


CREATE TABLE [dbo].[Enajenante](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RutEnajenante] nvarchar(10) NOT NULL,
	[PorcentajeEnajenante] [int] NOT NULL,
	[CheckEnajenante] [int] NOT NULL,
	[IdEnajenacion] [int] NOT NULL,
	CONSTRAINT [PK_Enajenante] PRIMARY KEY CLUSTERED(
		[Id] ASC
	),
	CONSTRAINT [FK_Enajenante_Enajenacion] FOREIGN KEY([IdEnajenacion])
	REFERENCES [dbo].Enajenacion
)
GO

USE [InscripcionesBrDb]
GO
SET IDENTITY_INSERT [dbo].[Enajenante] ON 
GO
INSERT [dbo].[Enajenante] ([Id], [RutEnajenante], [PorcentajeEnajenante], [CheckEnajenante], [IdEnajenacion]) VALUES (1, N'12345678-9', 20, 10, 1)
GO
SET IDENTITY_INSERT [dbo].[Enajenante] OFF
GO

CREATE TABLE [dbo].[Adquiriente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RutAdquiriente] nvarchar(10) NOT NULL,
	[PorcentajeAdquiriente] [int] NOT NULL,
	[CheckAdquiriente] [int] NOT NULL,
	[IdEnajenacion] [int] NOT NULL,
	CONSTRAINT [PK_Adquiriente] PRIMARY KEY CLUSTERED(
		[Id] ASC
	),
	CONSTRAINT [FK_Adquiriente_Enajenacion] FOREIGN KEY([IdEnajenacion])
	REFERENCES [dbo].Enajenacion
)
GO

USE [InscripcionesBrDb]
GO
SET IDENTITY_INSERT [dbo].[Adquiriente] ON 
GO
INSERT [dbo].[Adquiriente] ([Id], [RutAdquiriente], [PorcentajeAdquiriente], [CheckAdquiriente], [IdEnajenacion]) VALUES (1, N'19876543-2', 30, 10, 1)
GO
SET IDENTITY_INSERT [dbo].[Adquiriente] OFF
GO




USE [InscripcionesBrDb]
GO
SET IDENTITY_INSERT [dbo].[Enajenante] ON 
GO
INSERT [dbo].[Enajenante] ([Id], [RutEnajenante], [PorcentajeEnajenante], [CheckEnajenante], [IdEnajenacion]) VALUES (2, N'13345123-k', 80, 90, 1)
GO
SET IDENTITY_INSERT [dbo].[Enajenante] OFF
GO