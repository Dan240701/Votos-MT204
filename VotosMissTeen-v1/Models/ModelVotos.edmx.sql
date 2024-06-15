
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/14/2024 19:52:02
-- Generated from EDMX file: C:\Users\LaboratorioAPC3CBT\source\repos\VotosMissTeen-v1\VotosMissTeen-v1\Models\ModelVotos.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [VOTOSBD2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Fases'
CREATE TABLE [dbo].[Fases] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Foto] nvarchar(max)  NOT NULL,
    [NombreFase] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Fecha] nvarchar(max)  NOT NULL,
    [Estado] bit  NOT NULL
);
GO

-- Creating table 'Votos'
CREATE TABLE [dbo].[Votos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Puntuacion] float  NOT NULL,
    [JuradoId] int  NOT NULL,
    [FaseId] int  NOT NULL,
    [ParticipanteId] int  NOT NULL
);
GO

-- Creating table 'Jurados'
CREATE TABLE [dbo].[Jurados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Lugar] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Participantes'
CREATE TABLE [dbo].[Participantes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Foto] nvarchar(max)  NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Edad] nvarchar(max)  NOT NULL,
    [Departamento] nvarchar(max)  NOT NULL,
    [Clasificacion] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Fases'
ALTER TABLE [dbo].[Fases]
ADD CONSTRAINT [PK_Fases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Votos'
ALTER TABLE [dbo].[Votos]
ADD CONSTRAINT [PK_Votos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Jurados'
ALTER TABLE [dbo].[Jurados]
ADD CONSTRAINT [PK_Jurados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Participantes'
ALTER TABLE [dbo].[Participantes]
ADD CONSTRAINT [PK_Participantes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [JuradoId] in table 'Votos'
ALTER TABLE [dbo].[Votos]
ADD CONSTRAINT [FK_JuradoVotos]
    FOREIGN KEY ([JuradoId])
    REFERENCES [dbo].[Jurados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JuradoVotos'
CREATE INDEX [IX_FK_JuradoVotos]
ON [dbo].[Votos]
    ([JuradoId]);
GO

-- Creating foreign key on [FaseId] in table 'Votos'
ALTER TABLE [dbo].[Votos]
ADD CONSTRAINT [FK_FaseVotos]
    FOREIGN KEY ([FaseId])
    REFERENCES [dbo].[Fases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FaseVotos'
CREATE INDEX [IX_FK_FaseVotos]
ON [dbo].[Votos]
    ([FaseId]);
GO

-- Creating foreign key on [ParticipanteId] in table 'Votos'
ALTER TABLE [dbo].[Votos]
ADD CONSTRAINT [FK_ParticipanteVotos]
    FOREIGN KEY ([ParticipanteId])
    REFERENCES [dbo].[Participantes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ParticipanteVotos'
CREATE INDEX [IX_FK_ParticipanteVotos]
ON [dbo].[Votos]
    ([ParticipanteId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------