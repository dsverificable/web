﻿Create Database Grupo10ConchaMunozBrDb
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
	[Vigente] [bit] NOT NULL,
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
	[PorcentajeEnajenante] [float] NOT NULL,
	[CheckEnajenante] [bit] NOT NULL,
	[IdEnajenacion] [int] NOT NULL,
	[Fojas] [int] NOT NULL,
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
	[PorcentajeAdquiriente] [float] NOT NULL,
	[CheckAdquiriente] [bit] NOT NULL,
	[IdEnajenacion] [int] NOT NULL,
	[Fojas] [int] NOT NULL,
	CONSTRAINT [PK_Adquiriente] PRIMARY KEY CLUSTERED(
		[Id] ASC
	),
	CONSTRAINT [FK_Adquiriente_Enajenacion] FOREIGN KEY([IdEnajenacion])
	REFERENCES [dbo].Enajenacion
);
GO

CREATE TABLE [dbo].[Historial](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eliminado] [bit] NOT NULL,
	[IdEnajenacion] [int] NOT NULL,
	[Comuna] [int] NOT NULL,
	[Manzana] [int] NOT NULL,
	[Predio] [int] NOT NULL,
	[Fojas] [int] NOT NULL,
	[FechaInscripcion] [date] NOT NULL,
	[IdInscripcion] [int] NOT NULL,
	[Rut] nvarchar(10) NOT NULL,
	[Porcentaje] [float] NOT NULL,
	[CNE] [int] NOT NULL,
	[Check] [bit] NOT NULL,
	[Participante] nvarchar(11) NOT NULL,
	CONSTRAINT [PK_Historial] PRIMARY KEY CLUSTERED(
		[Id] ASC
	),
);
GO



SET IDENTITY_INSERT [dbo].[CNEOptions] ON;
GO
INSERT [dbo].[CNEOptions] ([Valor], [Descripcion]) VALUES (0, 'Compraventa');
INSERT [dbo].[CNEOptions] ([Valor], [Descripcion]) VALUES (1, 'Regularizacion De Patrimonio');
GO
SET IDENTITY_INSERT [dbo].[CNEOptions] OFF;
GO



SET IDENTITY_INSERT [dbo].[ComunaOptions] ON;
GO
INSERT INTO [dbo].[ComunaOptions] ([Valor], [Comuna])
VALUES (0, 'Algarrobo'),
       (1, 'Alhué'),
       (2, 'Alto Biobío'),
       (3, 'Alto del Carmen'),
       (4, 'Alto Hospicio'),
       (5, 'Ancud'),
       (6, 'Andacollo'),
       (7, 'Angol'),
       (8, 'Antártica'),
       (9, 'Antofagasta'),
       (10, 'Antuco'),
       (11, 'Arauco'),
       (12, 'Arica'),
       (13, 'Aysén'),
		 (14, 'Buin'),
       (15, 'Bulnes'),
		 (16, 'Cabildo'),
       (17, 'Cabo de Hornos'),
       (18, 'Cabrero'),
       (19, 'Calama'),
       (20, 'Calbuco'),
       (21, 'Caldera'),
       (22, 'Calera'),
       (23, 'Calera de Tango'),
       (24, 'Calle Larga'),
       (25, 'Camarones'),
       (26, 'Camiña'),
       (27, 'Canela'),
       (28, 'Cañete'),
       (29, 'Carahue'),
       (30, 'Cartagena'),
       (31, 'Casablanca'),
       (32, 'Castro'),
       (33, 'Catemu'),
       (34, 'Cauquenes'),
       (35, 'Cerrillos'),
       (36, 'Cerro Navia'),
       (37, 'Chaitén'),
       (38, 'Chañaral'),
       (39, 'Chanco'),
       (40, 'Chépica'),
       (41, 'Chiguayante'),
       (42, 'Chile Chico'),
       (43, 'Chillán'),
       (44, 'Chillán Viejo'),
       (45, 'Chimbarongo'),
       (46, 'Cholchol'),
       (47, 'Chonchi'),
       (48, 'Cisnes'),
       (49, 'Cobquecura'),
       (50, 'Cochamó'),
       (51, 'Cochrane'),
       (52, 'Codegua'),
       (53, 'Coelemu'),
       (54, 'Coihueco'),
       (55, 'Coínco'),
       (56, 'Colbún'),
       (57, 'Colchane'),
       (58, 'Colina'),
       (59, 'Collipulli'),
       (60, 'Coltauco'),
       (61, 'Combarbalá'),
       (62, 'Concepción'),
       (63, 'Conchalí'),
       (64, 'Concón'),
       (65, 'Constitución'),
       (66, 'Contulmo'),
       (67, 'Copiapó'),
       (68, 'Coquimbo'),
       (69, 'Coronel'),
       (70, 'Corral'),
       (71, 'Coyhaique'),
       (72, 'Cunco'),
       (73, 'Curacautín'),
       (74, 'Curacaví'),
       (75, 'Curaco de Vélez'),
       (76, 'Curanilahue'),
       (77, 'Curarrehue'),
       (78, 'Curepto'),
       (79, 'Curicó'),
		 (80, 'Dalcahue'),
       (81, 'Diego de Almagro'),
       (82, 'Doñihue'),
		 (83, 'El Bosque'),
       (84, 'El Carmen'),
       (85, 'El Monte'),
       (86, 'El Quisco'),
       (87, 'El Tabo'),
       (88, 'Empedrado'),
       (89, 'Ercilla'),
       (90, 'Estación Central'),
		 (91, 'Florida'),
       (92, 'Freire'),
       (93, 'Freirina'),
       (94, 'Fresia'),
       (95, 'Frutillar'),
       (96, 'Futaleufú'),
       (97, 'Futrono'),
		 (98, 'Galvarino'),
       (99, 'General Lagos'),
       (100, 'Gorbea'),
       (101, 'Graneros'),
       (102, 'Guaitecas'),
		 (103, 'Hijuelas'),
       (104, 'Hualaihué'),
       (105, 'Hualañé'),
       (106, 'Hualpén'),
       (107, 'Hualqui'),
       (108, 'Huara'),
       (109, 'Huasco'),
       (110, 'Huechuraba'),
		 (111, 'Illapel'),
       (112, 'Independencia'),
       (113, 'Iquique'),
       (114, 'Isla de Maipo'),
       (115, 'Isla de Pascua'),
		 (116, 'Juan Fernández'),
		 (117, 'La Cisterna'),
		 (118, 'La Cruz'),
		 (119, 'La Estrella'),
		 (120, 'La Florida'),
		 (121, 'Lago Ranco'),
		 (122, 'Lago Verde'),
		 (123, 'La Granja'),
		 (124, 'Laguna Blanca'),
		 (125, 'La Higuera'),
		 (126, 'Laja'),
		 (127, 'La Ligua'),
		 (128, 'Lampa'),
		 (129, 'Lanco'),
		 (130, 'La Pintana'),
		 (131, 'La Reina'),
		 (132, 'Las Cabras'),
		 (133, 'Las Condes'),
		 (134, 'La Serena'),
		 (135, 'La Unión'),
		 (136, 'Lautaro'),
		 (137, 'Lebu'),
		 (138, 'Licantén'),
		 (139, 'Limache'),
		 (140, 'Linares'),
		 (141, 'Litueche'),
		 (142, 'Llaillay'),
		 (143, 'Llanquihue'),
		 (144, 'Lo Barnechea'),
		 (145, 'Lo Espejo'),
		 (146, 'Lolol'),
		 (147, 'Loncoche'),
		 (148, 'Longaví'),
		 (149, 'Lonquimay'),
		 (150, 'Lo Prado'),
		 (151, 'Los Alamos'),
		 (152, 'Los Andes'),
		 (153, 'Los Angeles'),
		 (154, 'Los Lagos'),
		 (155, 'Los Muermos'),
		 (156, 'Los Sauces'),
		 (157, 'Los Vilos'),
		 (158, 'Lota'),
		 (159, 'Lumaco'),
		 (160, 'Machalí'),
		 (161, 'Macul'),
		 (162, 'Máfil'),
		 (163, 'Maipú'),
		 (164, 'Malloa'),
		 (165, 'Marchihue'),
		 (166, 'María Elena'),
		 (167, 'María Pinto'),
		 (168, 'Mariquina'),
		 (169, 'Maule'),
		 (170, 'Maullín'),
		 (171, 'Mejillones'),
		 (172, 'Melipeuco'),
		 (173, 'Melipilla'),
		 (174, 'Molina'),
		 (175, 'Monte Patria'),
		 (176, 'Mostazal'),
		 (177, 'Mulchén'),
		 (178, 'Nacimiento'),
		 (179, 'Nancagua'),
		 (180, 'Natales'),
		 (181, 'Navidad'),
		 (182, 'Negrete'),
		 (183, 'Ninhue'),
		 (184, 'Nogales'),
		 (185, 'Nueva Imperial'),
		 (186, 'Ñiquén'),
		 (187, 'Ñuñoa'),
		 (188, 'OHiggins'),
		 (189, 'Olivar'),
		 (190, 'Ollagüe'),
		 (191, 'Olmué'),
		 (192, 'Osorno'),
		 (193, 'Ovalle'),
		 (194, 'Padre Hurtado'),
		 (195, 'Padre Las Casas'),
		 (196, 'Paihuano'),
		 (197, 'Paillaco'),
		 (198, 'Paine'),
		 (199, 'Palena'),
		 (200, 'Palmilla'),
		 (201, 'Panguipulli'),
		 (202, 'Panquehue'),
		 (203, 'Papudo'),
		 (204, 'Paredones'),
		 (205, 'Parral'),
		 (206, 'Pedro Aguirre Cerda'),
		 (207, 'Pelarco'),
		 (208, 'Pelluhue'),
		 (209, 'Pemuco'),
		 (210, 'Peñaflor'),
		 (211, 'Peñalolén'),
		 (212, 'Pencahue'),
		 (213, 'Penco'),
		 (214, 'Peralillo'),
		 (215, 'Perquenco'),
		 (216, 'Petorca'),
		 (217, 'Peumo'),
		 (218, 'Pica'),
		 (219, 'Pichidegua'),
		 (220, 'Pichilemu'),
		 (221, 'Pinto'),
		 (222, 'Pirque'),
		 (223, 'Pitrufquén'),
		 (224, 'Placilla'),
		 (225, 'Portezuelo'),
		 (226, 'Porvenir'),
		 (227, 'Pozo Almonte'),
		 (228, 'Primavera'),
		 (229, 'Providencia'),
		 (230, 'Puchuncaví'),
		 (231, 'Pucón'),
		 (232, 'Pudahuel'),
		 (233, 'Puente Alto'),
		 (234, 'Puerto Montt'),
		 (235, 'Puerto Octay'),
		 (236, 'Puerto Varas'),
		 (237, 'Pumanque'),
		 (238, 'Punitaqui'),
		 (239, 'Punta Arenas'),
		 (240, 'Puqueldón'),
		 (241, 'Purén'),
		 (242, 'Purranque'),
		 (243, 'Putaendo'),
		 (244, 'Putre'),
		 (245, 'Puyehue'),
		 (246, 'Queilén'),
		 (247, 'Quellón'),
		 (248, 'Quemchi'),
		 (249, 'Quilaco'),
		 (250, 'Quilicura'),
		 (251, 'Quilleco'),
		 (252, 'Quillón'),
		 (253, 'Quillota'),
		 (254, 'Quilpué'),
		 (255, 'Quinchao'),
		 (256, 'Quinta de Tilcoco'),
		 (257, 'Quinta Normal'),
		 (258, 'Quintero'),
		 (259, 'Quirihue'),
		 (260, 'Rancagua'),
		 (261, 'Ránquil'),
		 (262, 'Rauco'),
		 (263, 'Recoleta'),
		 (264, 'Renaico'),
		 (265, 'Renca'),
		 (266, 'Rengo'),
		 (267, 'Requínoa'),
		 (268, 'Retiro'),
		 (269, 'Rinconada'),
		 (270, 'Río Bueno'),
		 (271, 'Río Claro'),
		 (272, 'Río Hurtado'),
		 (273, 'Río Ibáñez'),
		 (274, 'Río Negro'),
		 (275, 'Río Verde'),
		 (276, 'Romeral'),
		 (277, 'Saavedra'),
		(278, 'Sagrada Familia'),
		(279, 'Salamanca'),
		(280, 'San Antonio'),
		(281, 'San Bernardo'),
		(282, 'San Carlos'),
		(283, 'San Clemente'),
		(284, 'San Esteban'),
		(285, 'San Fabián'),
		(286, 'San Felipe'),
		(287, 'San Fernando'),
		(288, 'San Gregorio'),
		(289, 'San Ignacio'),
		(290, 'San Javier'),
		(291, 'San Joaquín'),
		(292, 'San José de Maipo'),
		(293, 'San Juan de la Costa'),
		(294, 'San Miguel'),
		(295, 'San Nicolás'),
		(296, 'San Pablo'),
		(297, 'San Pedro'),
		(298, 'San Pedro de Atacama'),
		(299, 'San Pedro de la Paz'),
		(300, 'San Rafael'),
		(301, 'San Ramón'),
		(302, 'San Rosendo'),
		(303, 'Santa Bárbara'),
		(304, 'Santa Cruz'),
		(305, 'Santa Juana'),
		(306, 'Santa María'),
		(307, 'Santiago'),
		(308, 'Santo Domingo'),
		(309, 'San Vicente'),
		(310, 'Sierra Gorda'),
		(311, 'Talagante'),
		(312, 'Talca'),
		(313, 'Talcahuano'),
		(314, 'Taltal'),
		(315, 'Temuco'),
		(316, 'Teno'),
		(317, 'Teodoro Schmidt'),
		(318, 'Tierra Amarilla'),
		(319, 'Tiltil'),
		(320, 'Timaukel'),
		(321, 'Tirúa'),
		(322, 'Tocopilla'),
		(323, 'Toltén'),
		(324, 'Tomé'),
		(325, 'Torres del Paine'),
		(326, 'Tortel'),
		(327, 'Traiguén'),
		(328, 'Treguaco'),
		(329, 'Tucapel'),
		(330, 'Valdivia'),
		(331, 'Vallenar'),
		(332, 'Valparaíso'),
		(333, 'Vichuquén'),
		(334, 'Victoria'),
		(335, 'Vicuña'),
		(336, 'Vilcún'),
		(337, 'Villa Alegre'),
		(338, 'Villa Alemana'),
		(339, 'Villarrica'),
		(340, 'Viña del Mar'),
		(341, 'Vitacura'),
		(342, 'Yerbas Buenas'),
		(343, 'Yumbel'),
		(344, 'Yungay'),
		(345, 'Zapallar');
GO
SET IDENTITY_INSERT [dbo].[ComunaOptions] OFF;