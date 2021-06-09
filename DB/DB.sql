USE [Database1]
GO
ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [Usuarios_Perfil]
GO
ALTER TABLE [dbo].[Tareas] DROP CONSTRAINT [tareas_proyecto]
GO
ALTER TABLE [dbo].[Sesion] DROP CONSTRAINT [Sesion_usuario]
GO
ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [DF_Usuarios_Correo]
GO
ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [DF_Usuarios_User]
GO
ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [DF__Usuarios__habili__2A4B4B5E]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 06/06/2021 0:22:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') AND type in (N'U'))
DROP TABLE [dbo].[Usuarios]
GO
/****** Object:  Table [dbo].[Tareas]    Script Date: 06/06/2021 0:22:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tareas]') AND type in (N'U'))
DROP TABLE [dbo].[Tareas]
GO
/****** Object:  Table [dbo].[Sesion]    Script Date: 06/06/2021 0:22:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sesion]') AND type in (N'U'))
DROP TABLE [dbo].[Sesion]
GO
/****** Object:  Table [dbo].[Proyectos]    Script Date: 06/06/2021 0:22:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proyectos]') AND type in (N'U'))
DROP TABLE [dbo].[Proyectos]
GO
/****** Object:  Table [dbo].[Perfiles]    Script Date: 06/06/2021 0:22:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Perfiles]') AND type in (N'U'))
DROP TABLE [dbo].[Perfiles]
GO
/****** Object:  Table [dbo].[Perfiles]    Script Date: 06/06/2021 0:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Perfiles](
	[Id] [int] NOT NULL,
	[Perfil] [nvarchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proyectos]    Script Date: 06/06/2021 0:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proyectos](
	[ProyectoId] [int] NOT NULL,
	[Nombre] [varchar](max) NOT NULL,
	[Descripcon] [varchar](200) NOT NULL,
	[Estado] [varchar](200) NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProyectoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sesion]    Script Date: 06/06/2021 0:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sesion](
	[UsuarioId] [int] NOT NULL,
	[Token] [varchar](max) NOT NULL,
	[FechaIni] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tareas]    Script Date: 06/06/2021 0:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tareas](
	[TareaId] [int] NOT NULL,
	[ProyectoId] [int] NOT NULL,
	[Nombre] [varchar](max) NOT NULL,
	[Descripcon] [varchar](200) NOT NULL,
	[Estado] [varchar](200) NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TareaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 06/06/2021 0:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] NOT NULL,
	[Nombre ] [varchar](200) NOT NULL,
	[PerfilId] [int] NOT NULL,
	[Clave] [varchar](100) NOT NULL,
	[habilitado] [bit] NOT NULL,
	[User] [varchar](20) NOT NULL,
	[Correo] [varchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Perfiles] ([Id], [Perfil]) VALUES (1, N'Administrador')
GO
INSERT [dbo].[Perfiles] ([Id], [Perfil]) VALUES (2, N'Operador')
GO
INSERT [dbo].[Usuarios] ([Id], [Nombre ], [PerfilId], [Clave], [habilitado], [User], [Correo]) VALUES (11, N'nombre', 1, N'prueba123', 1, N'prueba1', N'')
GO
INSERT [dbo].[Usuarios] ([Id], [Nombre ], [PerfilId], [Clave], [habilitado], [User], [Correo]) VALUES (12, N'nombre 2', 1, N'prueba123', 1, N'prueba2', N'')
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((1)) FOR [habilitado]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_User]  DEFAULT ('prueba') FOR [User]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_Correo]  DEFAULT ('') FOR [Correo]
GO
ALTER TABLE [dbo].[Sesion]  WITH CHECK ADD  CONSTRAINT [Sesion_usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[Sesion] CHECK CONSTRAINT [Sesion_usuario]
GO
ALTER TABLE [dbo].[Tareas]  WITH CHECK ADD  CONSTRAINT [tareas_proyecto] FOREIGN KEY([ProyectoId])
REFERENCES [dbo].[Proyectos] ([ProyectoId])
GO
ALTER TABLE [dbo].[Tareas] CHECK CONSTRAINT [tareas_proyecto]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [Usuarios_Perfil] FOREIGN KEY([PerfilId])
REFERENCES [dbo].[Perfiles] ([Id])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [Usuarios_Perfil]
GO
