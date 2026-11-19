-- Script SQL Server para Sistema de Restaurante (Semana 13)
-- Crea la base de datos RestauranteDb, tablas Mesas y Reservas (relación 1:N) y datos de ejemplo.

IF DB_ID(N'RestauranteDb') IS NULL
BEGIN
    CREATE DATABASE [RestauranteDb];
END;
GO

USE [RestauranteDb];
GO

DROP TABLE IF EXISTS [dbo].[Reservas];
DROP TABLE IF EXISTS [dbo].[Mesas];
DROP TABLE IF EXISTS [dbo].[Usuarios];
GO

CREATE TABLE [dbo].[Mesas] (
    [Id]        INT           IDENTITY(1,1) NOT NULL,
    [Numero]    INT           NOT NULL,
    [Capacidad] INT           NOT NULL,
    [Ubicacion] NVARCHAR(80)  NOT NULL,
    CONSTRAINT [PK_Mesas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Mesas_Numero] UNIQUE NONCLUSTERED ([Numero] ASC),
    CONSTRAINT [CK_Mesas_Numero] CHECK ([Numero] BETWEEN 1 AND 200),
    CONSTRAINT [CK_Mesas_Capacidad] CHECK ([Capacidad] BETWEEN 1 AND 20)
);
GO

CREATE TABLE [dbo].[Reservas] (
    [Id]               INT            IDENTITY(1,1) NOT NULL,
    [NombreCliente]    NVARCHAR(100)  NOT NULL,
    [Telefono]         NVARCHAR(20)   NOT NULL,
    [FechaHora]        DATETIME2(0)   NOT NULL,
    [CantidadPersonas] INT            NOT NULL,
    [Observaciones]    NVARCHAR(300)  NULL,
    [MesaId]           INT            NOT NULL,
    CONSTRAINT [PK_Reservas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Reservas_Mesas] FOREIGN KEY ([MesaId])
        REFERENCES [dbo].[Mesas] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [CK_Reservas_CantidadPersonas] CHECK ([CantidadPersonas] BETWEEN 1 AND 20)
);
GO

CREATE NONCLUSTERED INDEX [IX_Reservas_MesaId] ON [dbo].[Reservas] ([MesaId] ASC);
CREATE NONCLUSTERED INDEX [IX_Reservas_FechaHora] ON [dbo].[Reservas] ([FechaHora] ASC);
GO

INSERT INTO [dbo].[Mesas] ([Numero], [Capacidad], [Ubicacion]) VALUES
    (1, 4, N'Salón principal'),
    (2, 6, N'Terraza'),
    (3, 2, N'Ventana'),
    (4, 8, N'Salón familiar'),
    (5, 4, N'Zona VIP');
GO

INSERT INTO [dbo].[Reservas] ([NombreCliente], [Telefono], [FechaHora], [CantidadPersonas], [Observaciones], [MesaId]) VALUES
    (N'Ana Torres',  N'999111222', '2026-12-10T13:00:00', 4, N'Almuerzo familiar', 1),
    (N'Luis Ramos',  N'999333444', '2026-12-10T20:00:00', 5, NULL,                 2),
    (N'María López', N'999555666', '2026-12-11T19:30:00', 2, N'Junto a la ventana', 3),
    (N'Carlos Vega', N'999777888', '2026-12-12T14:00:00', 6, N'Cumpleaños',         4),
    (N'Elena Cruz',  N'999888999', '2026-12-12T21:00:00', 2, N'Cena romántica',     1);
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
