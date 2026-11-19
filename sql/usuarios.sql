-- Script incremental Semana 14: Tabla Usuarios y usuarios de prueba por rol
-- Claves de prueba (hasheadas con BCrypt):
--   recepcion@restaurante.com -> Recepcion123! (Rol: Recepcionista)
--   gerente@restaurante.com   -> Gerente123!   (Rol: Gerente)

USE [RestauranteDb];
GO

DROP TABLE IF EXISTS [dbo].[Usuarios];
GO

CREATE TABLE [dbo].[Usuarios] (
    [Id]        INT            IDENTITY(1,1) NOT NULL,
    [Correo]    NVARCHAR(150)  NOT NULL,
    [ClaveHash] NVARCHAR(255)  NOT NULL,
    [Rol]       NVARCHAR(50)   NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Usuarios_Correo] UNIQUE NONCLUSTERED ([Correo] ASC)
);
GO

INSERT INTO [dbo].[Usuarios] ([Correo], [ClaveHash], [Rol]) VALUES
    (N'recepcion@restaurante.com', N'$2a$11$YbkZJ0gwvHKJzOVUQ2xZbOaFhw8oUtQZF4abOQDs7x3cGrd9UGzF2', N'Recepcionista'),
    (N'gerente@restaurante.com',   N'$2a$11$edlGjPg55S60lvPuQb954.IbLpma1DK6GSU8JZGvR78BqptmWYNlK', N'Gerente');
GO
