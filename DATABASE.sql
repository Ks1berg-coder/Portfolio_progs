USE [PGIC]
GO
/****** Object:  Table [dbo].[building]    Script Date: 20.06.2023 11:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[building](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Building_Address] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventory]    Script Date: 20.06.2023 11:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inventory](
	[inv_num] [nvarchar](20) NOT NULL,
	[name] [nvarchar](50) NULL,
	[description] [nvarchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[inv_num] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventory_arrival]    Script Date: 20.06.2023 11:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inventory_arrival](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NULL,
	[login] [nvarchar](32) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventory_movement_log]    Script Date: 20.06.2023 11:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inventory_movement_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NULL,
	[login] [nvarchar](32) NULL,
	[inventory_num] [nvarchar](20) NULL,
	[room_from] [int] NULL,
	[room_to] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[list_of_arrival]    Script Date: 20.06.2023 11:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[list_of_arrival](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[inventory_num] [nvarchar](20) NULL,
	[arrival_id] [int] NULL,
	[room_id] [int] NULL,
	[price] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[roles]    Script Date: 20.06.2023 11:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[room]    Script Date: 20.06.2023 11:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[room](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[level] [int] NULL,
	[type_of_room] [int] NULL,
	[id_building] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[type_of_room]    Script Date: 20.06.2023 11:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[type_of_room](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 20.06.2023 11:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[login] [nvarchar](32) NOT NULL,
	[password] [nvarchar](32) NULL,
	[Surname] [nvarchar](30) NULL,
	[Name] [nvarchar](30) NULL,
	[Middle_name] [nvarchar](30) NULL,
	[id_role] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[building] ON 
GO
INSERT [dbo].[building] ([id], [Building_Address]) VALUES (1, N'ул. Газеты Звезда, 18')
GO
INSERT [dbo].[building] ([id], [Building_Address]) VALUES (2, N'Советская ул., 102')
GO
SET IDENTITY_INSERT [dbo].[building] OFF
GO
INSERT [dbo].[inventory] ([inv_num], [name], [description]) VALUES (N'А77212', N'Компьютер', N'Моноблок, i3 3700, gt550, 16gb ram, 1366x768')
GO
INSERT [dbo].[inventory] ([inv_num], [name], [description]) VALUES (N'Б32145', N'Стол', N'100х100 круглый, из дерева')
GO
INSERT [dbo].[inventory] ([inv_num], [name], [description]) VALUES (N'В77154', N'Шторы', N'Плотные, тёмно-зеленый цвет, 5м')
GO
INSERT [dbo].[inventory] ([inv_num], [name], [description]) VALUES (N'К88213', N'Кресло', N'КожЗам, бежевый цвет, 100х50, со спинкой')
GO
SET IDENTITY_INSERT [dbo].[inventory_arrival] ON 
GO
INSERT [dbo].[inventory_arrival] ([id], [date], [login]) VALUES (1, CAST(N'2023-03-11T00:00:00.000' AS DateTime), N'klad')
GO
INSERT [dbo].[inventory_arrival] ([id], [date], [login]) VALUES (2, CAST(N'2023-04-01T00:00:00.000' AS DateTime), N'klad')
GO
SET IDENTITY_INSERT [dbo].[inventory_arrival] OFF
GO
SET IDENTITY_INSERT [dbo].[inventory_movement_log] ON 
GO
INSERT [dbo].[inventory_movement_log] ([id], [date], [login], [inventory_num], [room_from], [room_to]) VALUES (3, CAST(N'2023-03-11T00:00:00.000' AS DateTime), N'klad', N'А77212', NULL, 4)
GO
INSERT [dbo].[inventory_movement_log] ([id], [date], [login], [inventory_num], [room_from], [room_to]) VALUES (4, CAST(N'2023-04-01T00:00:00.000' AS DateTime), N'klad', N'А77212', NULL, 9)
GO
INSERT [dbo].[inventory_movement_log] ([id], [date], [login], [inventory_num], [room_from], [room_to]) VALUES (6, CAST(N'2023-03-12T00:00:00.000' AS DateTime), N'manager', N'А77212', 4, 1)
GO
INSERT [dbo].[inventory_movement_log] ([id], [date], [login], [inventory_num], [room_from], [room_to]) VALUES (7, CAST(N'2023-04-05T00:00:00.000' AS DateTime), N'manager', N'А77212', 9, 8)
GO
INSERT [dbo].[inventory_movement_log] ([id], [date], [login], [inventory_num], [room_from], [room_to]) VALUES (8, CAST(N'2023-04-03T00:00:00.000' AS DateTime), N'manager', N'К88213', NULL, 4)
GO
INSERT [dbo].[inventory_movement_log] ([id], [date], [login], [inventory_num], [room_from], [room_to]) VALUES (9, CAST(N'2023-04-04T00:00:00.000' AS DateTime), N'manager', N'К88213', 4, 5)
GO
INSERT [dbo].[inventory_movement_log] ([id], [date], [login], [inventory_num], [room_from], [room_to]) VALUES (10, CAST(N'2023-06-10T23:25:24.000' AS DateTime), N'admin', N'К88213', 5, 4)
GO
INSERT [dbo].[inventory_movement_log] ([id], [date], [login], [inventory_num], [room_from], [room_to]) VALUES (16, CAST(N'2014-07-30T00:00:00.000' AS DateTime), N'manager', N'Б32145', NULL, 5)
GO
SET IDENTITY_INSERT [dbo].[inventory_movement_log] OFF
GO
SET IDENTITY_INSERT [dbo].[list_of_arrival] ON 
GO
INSERT [dbo].[list_of_arrival] ([id], [inventory_num], [arrival_id], [room_id], [price]) VALUES (1, N'А77212', 1, 4, CAST(32000.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[list_of_arrival] ([id], [inventory_num], [arrival_id], [room_id], [price]) VALUES (2, N'К88213', 1, 4, CAST(30000.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[list_of_arrival] ([id], [inventory_num], [arrival_id], [room_id], [price]) VALUES (3, N'Б32145', 1, 4, CAST(24840.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[list_of_arrival] ([id], [inventory_num], [arrival_id], [room_id], [price]) VALUES (4, N'В77154', 1, 4, CAST(15000.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[list_of_arrival] ([id], [inventory_num], [arrival_id], [room_id], [price]) VALUES (5, N'Б32145', 2, 9, CAST(40000.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[list_of_arrival] ([id], [inventory_num], [arrival_id], [room_id], [price]) VALUES (6, N'А77212', 2, 9, CAST(96000.00 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [dbo].[list_of_arrival] OFF
GO
SET IDENTITY_INSERT [dbo].[roles] ON 
GO
INSERT [dbo].[roles] ([id], [name]) VALUES (1, N'Администратор')
GO
INSERT [dbo].[roles] ([id], [name]) VALUES (2, N'Менеджер')
GO
INSERT [dbo].[roles] ([id], [name]) VALUES (3, N'Кладовщик')
GO
SET IDENTITY_INSERT [dbo].[roles] OFF
GO
SET IDENTITY_INSERT [dbo].[room] ON 
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (1, N'Аудитория №111', 1, 1, 1)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (2, N'Восточной коридор', 1, 4, 1)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (3, N'Актовый зал №1', 1, 3, 1)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (4, N'Склад №1', 2, 2, 1)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (5, N'Аудитория №315', 3, 1, 1)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (6, N'Северный коридор', 1, 2, 2)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (7, N'Западный коридор', 1, 2, 2)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (8, N'Аудитория №211', 2, 1, 2)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (9, N'Склад №1', 1, 2, 2)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (10, N'Актовый зал №1', 2, 3, 2)
GO
INSERT [dbo].[room] ([id], [name], [level], [type_of_room], [id_building]) VALUES (11, N'Приход', NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[room] OFF
GO
SET IDENTITY_INSERT [dbo].[type_of_room] ON 
GO
INSERT [dbo].[type_of_room] ([id], [name]) VALUES (1, N'Кабинет')
GO
INSERT [dbo].[type_of_room] ([id], [name]) VALUES (2, N'Склад')
GO
INSERT [dbo].[type_of_room] ([id], [name]) VALUES (3, N'Актовый зал')
GO
INSERT [dbo].[type_of_room] ([id], [name]) VALUES (4, N'Коридор')
GO
INSERT [dbo].[type_of_room] ([id], [name]) VALUES (5, N'000')
GO
SET IDENTITY_INSERT [dbo].[type_of_room] OFF
GO
INSERT [dbo].[users] ([login], [password], [Surname], [Name], [Middle_name], [id_role]) VALUES (N'admin', N'admin', N'Большаков', N'Игорь', N'Алексеевич', 1)
GO
INSERT [dbo].[users] ([login], [password], [Surname], [Name], [Middle_name], [id_role]) VALUES (N'klad', N'klad', N'Верещагин', N'Олег', N'Александрович', 3)
GO
INSERT [dbo].[users] ([login], [password], [Surname], [Name], [Middle_name], [id_role]) VALUES (N'manager', N'manager', N'Орлов', N'Алексей', N'Вячеславович', 2)
GO
ALTER TABLE [dbo].[inventory_arrival]  WITH CHECK ADD  CONSTRAINT [FK_ariival_to_users] FOREIGN KEY([login])
REFERENCES [dbo].[users] ([login])
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[inventory_arrival] CHECK CONSTRAINT [FK_ariival_to_users]
GO
ALTER TABLE [dbo].[inventory_movement_log]  WITH CHECK ADD  CONSTRAINT [FK_moveLog_to_inventory] FOREIGN KEY([inventory_num])
REFERENCES [dbo].[inventory] ([inv_num])
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[inventory_movement_log] CHECK CONSTRAINT [FK_moveLog_to_inventory]
GO
ALTER TABLE [dbo].[inventory_movement_log]  WITH CHECK ADD  CONSTRAINT [FK_moveLog_to_roomFrom] FOREIGN KEY([room_from])
REFERENCES [dbo].[room] ([id])
GO
ALTER TABLE [dbo].[inventory_movement_log] CHECK CONSTRAINT [FK_moveLog_to_roomFrom]
GO
ALTER TABLE [dbo].[inventory_movement_log]  WITH CHECK ADD  CONSTRAINT [FK_moveLog_to_roomTo] FOREIGN KEY([room_to])
REFERENCES [dbo].[room] ([id])
GO
ALTER TABLE [dbo].[inventory_movement_log] CHECK CONSTRAINT [FK_moveLog_to_roomTo]
GO
ALTER TABLE [dbo].[inventory_movement_log]  WITH CHECK ADD  CONSTRAINT [FK_moveLog_to_users] FOREIGN KEY([login])
REFERENCES [dbo].[users] ([login])
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[inventory_movement_log] CHECK CONSTRAINT [FK_moveLog_to_users]
GO
ALTER TABLE [dbo].[list_of_arrival]  WITH CHECK ADD  CONSTRAINT [FK_listArrival_to_arrival] FOREIGN KEY([arrival_id])
REFERENCES [dbo].[inventory_arrival] ([id])
GO
ALTER TABLE [dbo].[list_of_arrival] CHECK CONSTRAINT [FK_listArrival_to_arrival]
GO
ALTER TABLE [dbo].[list_of_arrival]  WITH CHECK ADD  CONSTRAINT [FK_listArrival_to_inventory] FOREIGN KEY([inventory_num])
REFERENCES [dbo].[inventory] ([inv_num])
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[list_of_arrival] CHECK CONSTRAINT [FK_listArrival_to_inventory]
GO
ALTER TABLE [dbo].[list_of_arrival]  WITH CHECK ADD  CONSTRAINT [FK_listArrival_to_room] FOREIGN KEY([room_id])
REFERENCES [dbo].[room] ([id])
GO
ALTER TABLE [dbo].[list_of_arrival] CHECK CONSTRAINT [FK_listArrival_to_room]
GO
ALTER TABLE [dbo].[room]  WITH CHECK ADD  CONSTRAINT [FK_room_to_building] FOREIGN KEY([id_building])
REFERENCES [dbo].[building] ([id])
GO
ALTER TABLE [dbo].[room] CHECK CONSTRAINT [FK_room_to_building]
GO
ALTER TABLE [dbo].[room]  WITH CHECK ADD  CONSTRAINT [FK_room_to_type] FOREIGN KEY([type_of_room])
REFERENCES [dbo].[type_of_room] ([id])
GO
ALTER TABLE [dbo].[room] CHECK CONSTRAINT [FK_room_to_type]
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [FK_users_to_roles] FOREIGN KEY([id_role])
REFERENCES [dbo].[roles] ([id])
GO
ALTER TABLE [dbo].[users] CHECK CONSTRAINT [FK_users_to_roles]
GO
