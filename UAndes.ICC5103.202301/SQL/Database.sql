Create Database Grupo10ConchaMunozBrDb
GO

USE [Grupo10ConchaMunozBrDb];
GO

CREATE TABLE [dbo].[CNEOptions](
    [Valor] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Descripcion] [nvarchar](100) NOT NULL
);
GO

CREATE TABLE [dbo].[ComunaOptions](
    [Valor] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Comuna] [nvarchar](30) NOT NULL
);
GO

CREATE TABLE [dbo].[Enajenacion](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CNE] [int] NOT NULL,
    [Comuna] [int] NOT NULL,
    [Manzana] [int] NOT NULL,
    [Predio] [int] NOT NULL,
    [Fojas] [int] NOT NULL,
    [FechaInscripcion] [date] NOT NULL,
    [IdInscripcion] [int] NOT NULL,
    CONSTRAINT [PK_Enajenacion] PRIMARY KEY CLUSTERED(
        [Id] ASC
    ),
    CONSTRAINT [FK_Enajenacion_CNEOptions] FOREIGN KEY([CNE])
        REFERENCES [dbo].[CNEOptions]([Valor]),
    CONSTRAINT [FK_Enajenacion_ComunaOptions] FOREIGN KEY([Comuna])
        REFERENCES [dbo].[ComunaOptions]([Valor])
);
GO

CREATE TABLE [dbo].[Enajenante](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RutEnajenante] nvarchar(10) NOT NULL,
	[PorcentajeEnajenante] [int] NOT NULL,
	[CheckEnajenante] [bit] NOT NULL,
	[IdEnajenacion] [int] NOT NULL,
	CONSTRAINT [PK_Enajenante] PRIMARY KEY CLUSTERED(
		[Id] ASC
	),
	CONSTRAINT [FK_Enajenante_Enajenacion] FOREIGN KEY([IdEnajenacion])
	REFERENCES [dbo].Enajenacion
);
GO

CREATE TABLE [dbo].[Adquiriente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RutAdquiriente] nvarchar(10) NOT NULL,
	[PorcentajeAdquiriente] [int] NOT NULL,
	[CheckAdquiriente] [bit] NOT NULL,
	[IdEnajenacion] [int] NOT NULL,
	CONSTRAINT [PK_Adquiriente] PRIMARY KEY CLUSTERED(
		[Id] ASC
	),
	CONSTRAINT [FK_Adquiriente_Enajenacion] FOREIGN KEY([IdEnajenacion])
	REFERENCES [dbo].Enajenacion
);
GO

SET IDENTITY_INSERT [dbo].[CNEOptions] ON;
GO
INSERT [dbo].[CNEOptions] ([Valor], [Descripcion]) VALUES (1, 'Regularizacion De Patrimonio');
GO
SET IDENTITY_INSERT [dbo].[CNEOptions] OFF;
GO

SET IDENTITY_INSERT [dbo].[ComunaOptions] ON;
GO
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (0, 'Arica');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (1, 'Alto del Carmen');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (2, 'Alto Hospicio');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (3, 'Ancud');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (4, 'Andacollo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (5, 'Angol');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (6, 'Antofagasta');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (7, 'Antuco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (8, 'Arauco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (9, 'Arica');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (10, 'Aysen');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (11, 'Buin');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (12, 'Bulnes');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (13, 'Cabildo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (14, 'Cabo de Hornos');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (15, 'Cabrero');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (16, 'Calama');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (17, 'Calbuco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (18, 'Caldera');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (19, 'Calera de Tango');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (20, 'Calle Larga');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (21, 'Camarones');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (22, 'Camina');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (23, 'Canela');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (24, 'Carahue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (25, 'Cartagena');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (26, 'Casablanca');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (27, 'Castro');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (28, 'Catemu');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (29, 'Cauquenes');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (30, 'Cerrillos');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (31, 'Cerro Navia');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (32, 'Chaitén');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (33, 'Chanco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (34, 'Chañaral');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (35, 'Chépica');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (36, 'Chiguayante');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (37, 'Chile Chico');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (38, 'Chillán');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (39, 'Chillán Viejo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (40, 'Chimbarongo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (41, 'Cholchol');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (42, 'Chonchi');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (43, 'Cisnes');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (44, 'Cobquecura');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (45, 'Cochamó');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (46, 'Cochrane');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (47, 'Codegua');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (48, 'Coelemu');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (49, 'Coihueco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (50, 'Coinco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (51, 'Colbun');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (52, 'Colina');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (53, 'Collipulli');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (54, 'Coltauco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (55, 'Combarbala');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (56, 'Concepcion');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (57, 'Conchali');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (58, 'Concon');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (59, 'Constitucion');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (60, 'Contulmo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (61, 'Copiapo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (62, 'Coquimbo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (63, 'Coronel');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (64, 'Corral');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (65, 'Coyhaique');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (66, 'Cunco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (67, 'Curacautin');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (68, 'Curacavi');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (69, 'Curaco de Velez');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (70, 'Curanilahue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (71, 'Curarrehue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (72, 'Curepto');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (73, 'Curico');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (74, 'Dalcahue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (75, 'Diego de Almagro');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (76, 'Donihue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (77, 'El Bosque');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (78, 'El Carmen');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (79, 'El Monte');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (80, 'El Quisco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (81, 'El Tabo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (82, 'Empedrado');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (83, 'Ercilla');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (84, 'Estacion Central');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (85, 'Florida');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (86, 'Freire');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (87, 'Freirina');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (88, 'Frutillar');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (89, 'Fresia');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (90, 'Futrono');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (91, 'Galvarino');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (92, 'General Lagos');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (93, 'Gorbea');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (94, 'Graneros');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (95, 'Guaitecas');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (96, 'Hijuelas');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (97, 'Hualaihue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (98, 'Hualane');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (99, 'Hualpen');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (100, 'Hualqui');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (101, 'Huara');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (102, 'Huasco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (103, 'Huechuraba');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (104, 'Illapel');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (105, 'Independencia');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (106, 'Iquique');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (107, 'Isla de Maipo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (108, 'Isla de Pascua');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (109, 'Juan Fernandez');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (110, 'La Calera');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (111, 'La Cisterna');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (112, 'La Cruz');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (113, 'La Estrella');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (114, 'La Florida');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (115, 'La Granja');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (116, 'La Higuera');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (117, 'La Ligua');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (118, 'La Pintana');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (119, 'La Reina');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (120, 'La Serena');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (121, 'Lago Ranco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (122, 'Lago Verde');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (123, 'Laguna Blanca');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (124, 'Laja');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (125, 'Lampa');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (126, 'Lanco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (127, 'Las Cabras');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (128, 'Las Condes');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (129, 'Lautaro');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (130, 'Lebu');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (131, 'Licanten');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (132, 'Limache');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (133, 'Linares');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (134, 'Litueche');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (135, 'Llanquihue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (136, 'Lo Barnechea');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (137, 'Lo Espejo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (138, 'Lo Prado');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (139, 'Lolol');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (140, 'Loncoche');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (141, 'Longavi');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (142, 'Lonquimay');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (143, 'Los Alamos');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (144, 'Los Andes');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (145, 'Los Angeles');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (146, 'Los Lagos');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (147, 'Los Muermos');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (148, 'Los Sauces');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (149, 'Los Vilos');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (150, 'Lota');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (151, 'Lumaco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (152, 'Machali');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (153, 'Macul');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (154, 'Mafil');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (155, 'Maipu');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (156, 'Malloa');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (157, 'Marchihue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (158, 'Maria Elena');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (159, 'Maria Pinto');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (160, 'Mariquina');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (161, 'Maule');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (162, 'Maullin');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (163, 'Mejillones');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (164, 'Melipeuco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (165, 'Melipilla');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (166, 'Molina');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (167, 'Monte Patria');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (168, 'Mostazal');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (169, 'Mulchen');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (170, 'Nacimiento');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (171, 'Nancagua');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (172, 'Natales');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (173, 'Navidad');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (174, 'Negrete');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (175, 'Ninhue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (176, 'Nogales');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (177, 'Nueva Imperial');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (178, 'Niquen');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (179, 'Nunoa');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (180, 'Olivar');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (181, 'Ollagüe');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (182, 'Olmue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (183, 'Osorno');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (184, 'Otra Banda');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (185, 'Ovalle');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (186, 'Padre Hurtado');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (187, 'Padre Las Casas');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (188, 'Paihuano');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (189, 'Paillaco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (190, 'Paipote');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (191, 'Palena');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (192, 'Palmilla');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (193, 'Panguipulli');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (194, 'Panquehue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (195, 'Papudo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (196, 'Paredones');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (197, 'Parral');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (198, 'Pedro Aguirre Cerda');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (199, 'Pelarco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (200, 'Pelluhue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (201, 'Pemuco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (202, 'Pencahue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (203, 'Penco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (204, 'Penaflor');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (205, 'Penalolen');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (206, 'Perquenco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (207, 'Petorca');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (208, 'Peumo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (209, 'Pica');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (210, 'Pichidegua');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (211, 'Pichilemu');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (212, 'Pinto');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (213, 'Pirque');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (214, 'Pitrufquen');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (215, 'Placilla');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (216, 'Portezuelo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (217, 'Porvenir');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (218, 'Pozo Almonte');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (219, 'Primavera');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (220, 'Providencia');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (221, 'Puchuncavi');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (222, 'Pucon');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (223, 'Pudahuel');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (224, 'Puente Alto');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (225, 'Puerto Montt');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (226, 'Puerto Octay');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (227, 'Puerto Varas');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (228, 'Pumanque');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (229, 'Punta Arenas');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (230, 'Puqueldon');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (231, 'Puren');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (232, 'Purranque');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (233, 'Puyehue');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (234, 'Queilen');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (235, 'Quellon');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (236, 'Quemchi');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (237, 'Quilaco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (238, 'Quilicura');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (239, 'Quilleco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (240, 'Quillon');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (241, 'Quillota');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (242, 'Quinta de Tilcoco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (243, 'Quintero');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (244, 'Rancagua');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (245, 'Ranquil');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (246, 'Rauco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (247, 'Recoleta');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (248, 'Renaico');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (249, 'Renca');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (250, 'Rengo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (251, 'Requinoa');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (252, 'Retiro');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (253, 'Rinconada de Los Andes');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (254, 'Rio Bueno');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (255, 'Rio Claro');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (256, 'Rio Hurtado');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (257, 'Rio Ibanez');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (258, 'Rio Negro');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (259, 'Rio Verde');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (260, 'Romeral');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (261, 'Saavedra');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (262, 'Sagrada Familia');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (263, 'Salamanca');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (264, 'San Antonio');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (265, 'San Bernardo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (266, 'San Carlos');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (267, 'San Clemente');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (268, 'San Esteban');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (269, 'San Fabian');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (270, 'San Felipe');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (271, 'San Fernando');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (272, 'San Gregorio');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (273, 'San Ignacio');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (274, 'San Javier');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (275, 'San Joaquin');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (276, 'San Jose de Maipo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (277, 'San Juan de la Costa');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (278, 'San Miguel');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (279, 'San Nicolas');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (280, 'San Pablo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (281, 'San Pedro');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (282, 'San Pedro de Atacama');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (283, 'San Pedro de la Paz');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (284, 'San Rafael');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (285, 'San Ramon');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (286, 'San Rosendo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (287, 'San Vicente de Tagua Tagua');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (288, 'Santa Barbara');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (289, 'Santa Cruz');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (290, 'Santa Juana');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (291, 'Santa Maria');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (292, 'Santiago');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (293, 'Santo Domingo');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (294, 'Sierra Gorda');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (295, 'Talagante');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (296, 'Talca');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (297, 'Talcahuano');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (298, 'Taltal');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (299, 'Temuco');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (300, 'Teno');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (301, 'Teodoro Schmidt');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (302, 'Tierra Amarilla');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (303, 'Tiltil');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (304, 'Timaukel');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (305, 'Tirua');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (306, 'Tocopilla');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (307, 'Toltén');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (308, 'Tomé');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (309, 'Torres del Paine');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (310, 'Tortel');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (311, 'Tucapel');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (312, 'Valdivia');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (313, 'Vallenar');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (314, 'Valparaíso');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (315, 'Vichuquén');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (316, 'Victoria');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (317, 'Vicuña');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (318, 'Vilcún');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (319, 'Villa Alegre');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (320, 'Villarrica');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (321, 'Viña del Mar');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (322, 'Vitacura');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (323, 'Yerbas Buenas');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (324, 'Yumbel');
INSERT [dbo].[ComunaOptions] ([Valor], [Comuna]) VALUES (325, 'Yungay');
GO
SET IDENTITY_INSERT [dbo].[ComunaOptions] OFF;


SET IDENTITY_INSERT [dbo].[Enajenacion] ON; 
GO
INSERT [dbo].[Enajenacion] ([Id], [CNE], [Comuna], [Manzana], [Predio], [Fojas], [FechaInscripcion], [IdInscripcion]) VALUES (1, 1, 128, 12345, 123, 62145, CAST(N'2022-03-18' AS Date), 1)
GO
SET IDENTITY_INSERT [dbo].[Enajenacion] OFF;
GO


SET IDENTITY_INSERT [dbo].[Adquiriente] ON;
GO
INSERT [dbo].[Adquiriente] ([Id], [RutAdquiriente], [PorcentajeAdquiriente], [CheckAdquiriente], [IdEnajenacion]) VALUES (1, N'19876543-2', 30, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Adquiriente] OFF;
GO
