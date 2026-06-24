-- CREAR BASE DE DATOS SOLO SI NO EXISTE
IF DB_ID('BibliotecaDB') IS NULL
BEGIN
    CREATE DATABASE BibliotecaDB;
END
GO

USE BibliotecaDB;
GO

-- TABLAS PRINCIPALES
CREATE TABLE Autores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Nacionalidad NVARCHAR(100)
);

CREATE TABLE Temas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Tema NVARCHAR(100) NOT NULL,
    Area_Conocimiento NVARCHAR(100)
);

CREATE TABLE Editoriales (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Editorial NVARCHAR(100) NOT NULL,
    Sitio_Web NVARCHAR(200)
);

CREATE TABLE Paises (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Pais NVARCHAR(100) NOT NULL,
    Region NVARCHAR(100)
);

CREATE TABLE Tipos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Tipo NVARCHAR(100) NOT NULL
);

CREATE TABLE Libros (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Editorial INT NOT NULL FOREIGN KEY REFERENCES Editoriales(Id),
    Pais INT NOT NULL FOREIGN KEY REFERENCES Paises(Id),
    Tipo INT NOT NULL FOREIGN KEY REFERENCES Tipos(Id),
    Isbn NVARCHAR(20) UNIQUE NOT NULL,     -- el ISBN es obligatorio
    Titulo NVARCHAR(200) NOT NULL,
    Edicion NVARCHAR(50) NULL,
    Fecha_Lanzamiento DATE NULL
);

CREATE TABLE LibrosAutores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Libro INT NOT NULL FOREIGN KEY REFERENCES Libros(Id),
    Autor INT NOT NULL FOREIGN KEY REFERENCES Autores(Id)
);

CREATE TABLE LibrosTemas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Libro INT NOT NULL FOREIGN KEY REFERENCES Libros(Id),
    Tema INT NOT NULL FOREIGN KEY REFERENCES Temas(Id)
);

CREATE TABLE Existencias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Libro INT NOT NULL FOREIGN KEY REFERENCES Libros(Id),
    Ejemplares INT NOT NULL CHECK (Ejemplares >= 0) -- no puede ser negativo
);

CREATE TABLE Estados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Estado NVARCHAR(100) NOT NULL
);

CREATE TABLE EstadosExistencias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Existencia INT FOREIGN KEY REFERENCES Existencias(Id) NOT NULL,
    Estado INT FOREIGN KEY REFERENCES Estados(Id) NOT NULL,
    Fecha_Cambio DATETIME DEFAULT GETDATE()
);

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Documento NVARCHAR(50) NOT NULL UNIQUE,
    Direccion NVARCHAR(200) NULL,
    Telefono NVARCHAR(20) NULL,
    Correo NVARCHAR(150) NOT NULL UNIQUE,
    Contraseña NVARCHAR(255) NOT NULL,
    Fecha_Nacimiento DATE NULL,
    Rol NVARCHAR(50) NOT NULL DEFAULT 'usuario'
);

CREATE TABLE TiposPrestamos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(100) NOT NULL
);

CREATE TABLE Prestamos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Usuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    Tipo_Prestamo INT NOT NULL FOREIGN KEY REFERENCES TiposPrestamos(Id),
    Existencia INT NOT NULL FOREIGN KEY REFERENCES Existencias(Id),
    Fecha_Prestamo DATE NOT NULL,
    Fecha_Devolucion DATE NULL,    -- planeada
    Fecha_Entrega_Real DATE NULL   -- real
);

CREATE TABLE Sanciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Usuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    Descripcion NVARCHAR(MAX) NOT NULL,
    Fecha_Inicio DATE NOT NULL,
    Fecha_Fin DATE NULL
);

-- AUTORES
INSERT INTO Autores (Nombre, Nacionalidad) VALUES
('Gabriel García Márquez', 'Colombiano'),
('Isabel Allende', 'Chilena'),
('Mario Vargas Llosa', 'Peruano'),
('Julio Cortázar', 'Argentino'),
('J. K. Rowling', 'Británica');

-- TEMAS
INSERT INTO Temas (Nombre_Tema, Area_Conocimiento) VALUES
('Realismo Mágico', 'Literatura Latinoamericana'),
('Fantasía', 'Literatura Universal'),
('Historia', 'Ciencias Sociales'),
('Ciencia Ficción', 'Ciencias Exactas'),
('Literatura Infantil', 'Educación');

-- EDITORIALES
INSERT INTO Editoriales (Nombre_Editorial, Sitio_Web) VALUES
('Editorial Sudamericana', 'http://sudamericana.com'),
('Alfaguara', 'http://alfaguara.com'),
('Planeta', 'http://planeta.com'),
('Penguin Random House', 'http://penguinrandomhouse.com'),
('Seix Barral', 'http://seixbarral.com');

-- PAISES
INSERT INTO Paises (Nombre_Pais, Region) VALUES
('Colombia', 'América del Sur'),
('Chile', 'América del Sur'),
('Perú', 'América del Sur'),
('Argentina', 'América del Sur'),
('Reino Unido', 'Europa');

-- TIPOS
INSERT INTO Tipos (Nombre_Tipo) VALUES
('Novela'),
('Cuento'),
('Ensayo'),
('Poesía'),
('Biografía');

-- LIBROS
INSERT INTO Libros (Editorial, Pais, Tipo, Isbn, Titulo, Edicion, Fecha_Lanzamiento) VALUES
(1, 1, 1, '9780307474728', 'Cien Años de Soledad', '1ra', '1967-05-30'),
(2, 2, 1, '9789505116024', 'La Casa de los Espíritus', '2da', '1982-01-01'),
(3, 3, 1, '9788432227764', 'La Ciudad y los Perros', '3ra', '1963-01-01'),
(4, 4, 2, '9788497592208', 'Bestiario', '1ra', '1951-01-01'),
(5, 5, 2, '9780747532743', 'Harry Potter y la Piedra Filosofal', '1ra', '1997-06-26');

-- LIBROSAUTORES
INSERT INTO LibrosAutores (Libro, Autor) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

-- LIBROSTEMAS
INSERT INTO LibrosTemas (Libro, Tema) VALUES
(1, 1),
(2, 1),
(3, 3),
(4, 4),
(5, 2);

-- EXISTENCIAS
INSERT INTO Existencias (Libro, Ejemplares) VALUES
(1, 10),
(2, 7),
(3, 5),
(4, 8),
(5, 12);

-- ESTADOS
INSERT INTO Estados (Nombre_Estado) VALUES
('Disponible'),
('Prestado'),
('En reparación'),
('Perdido'),
('Reservado');

-- ESTADOSEXISTENCIAS
INSERT INTO EstadosExistencias (Existencia, Estado) VALUES
(1, 1),
(2, 2),
(3, 1),
(4, 3),
(5, 1);

-- USUARIOS
INSERT INTO Usuarios (Nombre, Documento, Direccion, Telefono, Correo, Contraseña, Fecha_Nacimiento, Rol) VALUES
('Administrador General', '999999999', 'N/A', '0000000000', 'admin@biblioteca.com', 'admin123', '1980-01-01', 'admin'),
('María Gómez', '200300400', 'Carrera 45 #12', '3019876543', 'maria.gomez@mail.com', 'hash2', '1985-11-10', 'usuario'),
('Carlos Ramírez', '300400500', 'Av. 7 #89', '3024567890', 'carlos.ramirez@mail.com', 'hash3', '1992-02-15', 'usuario'),
('Ana Torres', '400500600', 'Calle 9 #32', '3036549871', 'ana.torres@mail.com', 'hash4', '2000-09-25', 'usuario'),
('Luis Fernández', '500600700', 'Transv. 21 #14', '3047412589', 'luis.fernandez@mail.com', 'hash5', '1995-03-30', 'usuario');

-- TIPOSPRESTAMOS
INSERT INTO TiposPrestamos (Descripcion) VALUES
('Consulta en sala'),
('Préstamo a domicilio'),
('Préstamo interbibliotecario'),
('Reserva especial'),
('Consulta restringida');

-- PRESTAMOS
INSERT INTO Prestamos (Usuario, Tipo_Prestamo, Existencia, Fecha_Prestamo, Fecha_Devolucion, Fecha_Entrega_Real) VALUES
(1, 2, 1, '2025-09-01', '2025-09-15', NULL),
(2, 1, 2, '2025-09-05', '2025-09-05', '2025-09-05'),
(3, 2, 3, '2025-08-20', '2025-09-03', '2025-09-02'),
(4, 3, 4, '2025-09-10', '2025-09-24', NULL),
(5, 4, 5, '2025-09-12', '2025-09-26', NULL);

-- SANCIONES
INSERT INTO Sanciones (Usuario, Descripcion, Fecha_Inicio, Fecha_Fin) VALUES
(1, 'Retraso en la devolución de "Cien Años de Soledad"', '2025-09-16', '2025-09-23'),
(2, 'Mal estado del ejemplar "La Casa de los Espíritus"', '2025-09-07', '2025-09-14'),
(3, 'Devolución tardía de "Harry Potter y la Piedra Filosofal"', '2025-09-04', '2025-09-11'),
(4, 'Pérdida del ejemplar "Bestiario"', '2025-09-01', '2025-09-30'),
(5, 'Incumplimiento de reglas de consulta en sala', '2025-08-25', '2025-09-05');


/*

-- CREAR BASE DE DATOS SOLO SI NO EXISTE
IF DB_ID('BibliotecaDB') IS NULL
BEGIN
    CREATE DATABASE BibliotecaDB;
END
GO

USE BibliotecaDB;
GO

-- TABLAS PRINCIPALES
CREATE TABLE Autores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Nacionalidad NVARCHAR(100)
);

CREATE TABLE Temas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Tema NVARCHAR(100) NOT NULL,
    Area_Conocimiento NVARCHAR(100)
);

CREATE TABLE Editoriales (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Editorial NVARCHAR(100) NOT NULL,
    Sitio_Web NVARCHAR(200)
);

CREATE TABLE Paises (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Pais NVARCHAR(100) NOT NULL,
    Region NVARCHAR(100)
);

CREATE TABLE Tipos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Tipo NVARCHAR(100) NOT NULL
);

CREATE TABLE Libros (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Editorial INT NOT NULL FOREIGN KEY REFERENCES Editoriales(Id),
    Pais INT NOT NULL FOREIGN KEY REFERENCES Paises(Id),
    Tipo INT NOT NULL FOREIGN KEY REFERENCES Tipos(Id),
    Isbn NVARCHAR(20) UNIQUE NOT NULL,     -- el ISBN es obligatorio
    Titulo NVARCHAR(200) NOT NULL,
    Edicion NVARCHAR(50) NULL,
    Fecha_Lanzamiento DATE NULL
);

CREATE TABLE LibrosAutores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Libro INT NOT NULL FOREIGN KEY REFERENCES Libros(Id),
    Autor INT NOT NULL FOREIGN KEY REFERENCES Autores(Id)
);

CREATE TABLE LibrosTemas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Libro INT NOT NULL FOREIGN KEY REFERENCES Libros(Id),
    Tema INT NOT NULL FOREIGN KEY REFERENCES Temas(Id)
);

CREATE TABLE Existencias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Libro INT NOT NULL FOREIGN KEY REFERENCES Libros(Id),
    Ejemplares INT NOT NULL CHECK (Ejemplares >= 0) -- no puede ser negativo
);

CREATE TABLE Estados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Estado NVARCHAR(100) NOT NULL
);

CREATE TABLE EstadosExistencias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Existencia INT FOREIGN KEY REFERENCES Existencias(Id) NOT NULL,
    Estado INT FOREIGN KEY REFERENCES Estados(Id) NOT NULL,
    Fecha_Cambio DATETIME DEFAULT GETDATE()
);

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Documento NVARCHAR(50) NOT NULL UNIQUE,
    Direccion NVARCHAR(200) NULL,
    Telefono NVARCHAR(20) NULL,
    Correo NVARCHAR(150) NOT NULL UNIQUE,
    Contraseña NVARCHAR(255) NOT NULL, 
    Fecha_Nacimiento DATE NULL
);

CREATE TABLE TiposPrestamos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(100) NOT NULL
);

CREATE TABLE Prestamos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Usuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    Tipo_Prestamo INT NOT NULL FOREIGN KEY REFERENCES TiposPrestamos(Id),
    Existencia INT NOT NULL FOREIGN KEY REFERENCES Existencias(Id),
    Fecha_Prestamo DATE NOT NULL,
    Fecha_Devolucion DATE NULL,    -- planeada
    Fecha_Entrega_Real DATE NULL   -- real
);

CREATE TABLE Sanciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Usuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    Descripcion NVARCHAR(MAX) NOT NULL,
    Fecha_Inicio DATE NOT NULL,
    Fecha_Fin DATE NULL
);

-- AUTORES
INSERT INTO Autores (Nombre, Nacionalidad) VALUES
('Gabriel García Márquez', 'Colombiano'),
('Isabel Allende', 'Chilena'),
('Mario Vargas Llosa', 'Peruano'),
('Julio Cortázar', 'Argentino'),
('J. K. Rowling', 'Británica'),
('Pablo Neruda', 'Chileno'),
('Jorge Luis Borges', 'Argentino'),
('Margaret Atwood', 'Canadiense'),
('Haruki Murakami', 'Japonés'),
('Victoria Ocampo', 'Argentina'),
('Carlos Fuentes', 'Mexicano'),
('Julio Ramón Ribeyro', 'Peruano'),
('Nadine Gordimer', 'Sudafricana'),
('Orson Scott Card', 'Estadounidense'),
('Toni Morrison', 'Estadounidense');

-- TEMAS
INSERT INTO Temas (Nombre_Tema, Area_Conocimiento) VALUES
('Realismo Mágico', 'Literatura Latinoamericana'),
('Fantasía', 'Literatura Universal'),
('Historia', 'Ciencias Sociales'),
('Ciencia Ficción', 'Ciencias Exactas'),
('Literatura Infantil', 'Educación'),
('Poesía', 'Literatura Universal'),
('Realismo', 'Literatura Latinoamericana'),
('Feminismo', 'Ciencias Sociales'),
('Psicología', 'Ciencias Sociales'),
('Amor', 'Literatura Universal'),
('Ecosistemas', 'Ciencias Ambientales'),
('Filosofía', 'Humanidades'),
('Futuro Distópico', 'Ciencia Ficción'),
('Teoría Literaria', 'Literatura'),
('Sociología', 'Ciencias Sociales');

-- EDITORIALES
INSERT INTO Editoriales (Nombre_Editorial, Sitio_Web) VALUES
('Editorial Sudamericana', 'http://sudamericana.com'),
('Alfaguara', 'http://alfaguara.com'),
('Planeta', 'http://planeta.com'),
('Penguin Random House', 'http://penguinrandomhouse.com'),
('Seix Barral', 'http://seixbarral.com'),
('Editorial Planeta', 'http://planetaeditorial.com'),
('Random House Mondadori', 'http://randomhousemondadori.com'),
('Ediciones B', 'http://edicionesb.com'),
('Anagrama', 'http://anagrama-ediciones.com'),
('RBA Libros', 'http://rbalibros.com'),
('Tusquets Editores', 'http://tusquets.es'),
('Salamandra', 'http://salamandra.net'),
('Suma de Letras', 'http://sumadeletras.com'),
('Lumen', 'http://lumen.com'),
('Círculo de Lectores', 'http://circulodelectores.com');

-- PAISES
INSERT INTO Paises (Nombre_Pais, Region) VALUES
('Colombia', 'América del Sur'),
('Chile', 'América del Sur'),
('Perú', 'América del Sur'),
('Argentina', 'América del Sur'),
('Reino Unido', 'Europa'),
('España', 'Europa'),
('Estados Unidos', 'América del Norte'),
('México', 'América del Norte'),
('Francia', 'Europa'),
('Alemania', 'Europa'),
('Italia', 'Europa'),
('Brasil', 'América del Sur'),
('Canadá', 'América del Norte'),
('India', 'Asia'),
('Australia', 'Oceanía');

-- TIPOS
INSERT INTO Tipos (Nombre_Tipo) VALUES
('Novela'),
('Cuento'),
('Ensayo'),
('Poesía'),
('Biografía'),
('Tragedia'),
('Fábula'),
('Crónica'),
('Diario'),
('Teatro'),
('Manga'),
('Antología'),
('Carta'),
('Relato'),
('Historia');

-- LIBROS
INSERT INTO Libros (Editorial, Pais, Tipo, Isbn, Titulo, Edicion, Fecha_Lanzamiento) VALUES
(1, 1, 1, '9780307474728', 'Cien Años de Soledad', '1ra', '1967-05-30'),
(2, 2, 1, '9789505116024', 'La Casa de los Espíritus', '2da', '1982-01-01'),
(3, 3, 1, '9788432227764', 'La Ciudad y los Perros', '3ra', '1963-01-01'),
(4, 4, 2, '9788497592208', 'Bestiario', '1ra', '1951-01-01'),
(5, 5, 2, '9780747532743', 'Harry Potter y la Piedra Filosofal', '1ra', '1997-06-26'),
(6, 6, 1, '9788433981397', 'El Amor en los Tiempos del Cólera', '1ra', '1985-04-01'),
(7, 7, 3, '9788497593143', 'El Aleph', '1ra', '1945-05-01'),
(8, 8, 1, '9780871406757', 'Crónica de una Muerte Anunciada', '1ra', '1981-06-06'),
(9, 9, 2, '9780785793549', 'Rayuela', '1ra', '1963-02-10'),
(10, 10, 2, '9780399563933', 'Kafka en la Orilla', '1ra', '2002-09-01'),
(11, 11, 1, '9780525435024', 'El Cuento de la Criada', '1ra', '1985-10-01'),
(12, 12, 3, '9780151010264', 'Matar a un Ruiseñor', '1ra', '1960-07-11'),
(13, 13, 1, '9780735213406', 'La Sombra del Viento', '1ra', '2001-04-11'),
(14, 14, 2, '9780439064873', 'Harry Potter y la Cámara Secreta', '1ra', '1998-07-02'),
(15, 15, 2, '9780061122415', 'Los Juegos del Hambre', '1ra', '2008-09-14');

-- LIBROSAUTORES
INSERT INTO LibrosAutores (Libro, Autor) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10),
(11, 11),
(12, 12),
(13, 13),
(14, 14),
(15, 15);

-- LIBROSTEMAS
INSERT INTO LibrosTemas (Libro, Tema) VALUES
(1, 1),
(2, 1),
(3, 3),
(4, 4),
(5, 2),
(6, 1),
(7, 5),
(8, 3),
(9, 4),
(10, 2),
(11, 1),
(12, 3),
(13, 4),
(14, 2),
(15, 5);

-- EXISTENCIAS
INSERT INTO Existencias (Libro, Ejemplares) VALUES
(1, 10),
(2, 7),
(3, 5),
(4, 8),
(5, 12),
(6, 9),
(7, 14),
(8, 10),
(9, 11),
(10, 13),
(11, 15),
(12, 16),
(13, 9),
(14, 8),
(15, 7);

-- ESTADOS
INSERT INTO Estados (Nombre_Estado) VALUES
('Disponible'),
('Prestado'),
('En reparación'),
('Perdido'),
('Reservado');

-- ESTADOSEXISTENCIAS
INSERT INTO EstadosExistencias (Existencia, Estado) VALUES
(1, 1),
(2, 2),
(3, 1),
(4, 3),
(5, 1),
(6, 2),
(7, 4),
(8, 5),
(9, 1),
(10, 2),
(11, 1),
(12, 4),
(13, 2),
(14, 3),
(15, 5);

-- USUARIOS
INSERT INTO Usuarios (Nombre, Documento, Direccion, Telefono, Correo, Contraseña, Fecha_Nacimiento) VALUES
('Juan Pérez', '100200300', 'Calle Falsa 123', '3001234567', 'juan.perez@mail.com', 'hash1', '1990-05-20'),
('María Gómez', '200300400', 'Carrera 45 #12', '3019876543', 'maria.gomez@mail.com', 'hash2', '1985-11-10'),
('Carlos Ramírez', '300400500', 'Av. 7 #89', '3024567890', 'carlos.ramirez@mail.com', 'hash3', '1992-02-15'),
('Ana Torres', '400500600', 'Calle 9 #32', '3036549871', 'ana.torres@mail.com', 'hash4', '2000-09-25'),
('Luis Fernández', '500600700', 'Transv. 21 #14', '3047412589', 'luis.fernandez@mail.com', 'hash5', '1995-03-30'),
('Martín Sánchez', '600700800', 'Calle 5 #50', '3051234567', 'martin.sanchez@mail.com', 'hash6', '1988-01-12'),
('Sofía López', '700800900', 'Calle 8 #22', '3062345678', 'sofia.lopez@mail.com', 'hash7', '1991-07-18'),
('Ricardo Pérez', '800900100', 'Calle 12 #34', '3073456789', 'ricardo.perez@mail.com', 'hash8', '1983-02-25'),
('Laura Martínez', '900100200', 'Calle 2 #80', '3084567890', 'laura.martinez@mail.com', 'hash9', '1987-05-03'),
('Pedro González', '100200301', 'Calle 3 #60', '3095678901', 'pedro.gonzalez@mail.com', 'hash10', '1994-10-09'), -- Cambié el documento duplicado
('Felipe Ruiz', '110210310', 'Calle 4 #40', '3106789012', 'felipe.ruiz@mail.com', 'hash11', '1990-12-11'),
('Marta Sánchez', '120220420', 'Calle 6 #90', '3117890123', 'marta.sanchez@mail.com', 'hash12', '1992-08-01'),
('José Álvarez', '130230530', 'Calle 7 #70', '3128901234', 'jose.alvarez@mail.com', 'hash13', '1986-04-06'),
('Sara Gómez', '140240640', 'Calle 9 #100', '3139012345', 'sara.gomez@mail.com', 'hash14', '1993-06-13');

-- TIPOSPRESTAMOS
INSERT INTO TiposPrestamos (Descripcion) VALUES
('Consulta en sala'),
('Préstamo a domicilio'),
('Préstamo interbibliotecario'),
('Reserva especial'),
('Consulta restringida'),
('Préstamo de equipo'),
('Préstamo para investigación'),
('Préstamo en línea'),
('Préstamo exprés'),
('Préstamo con registro'),
('Préstamo de material audiovisual'),
('Préstamo de documentos raros'),
('Préstamo de libros antiguos'),
('Préstamo de revistas'),
('Préstamo por temporadas');

-- PRESTAMOS
INSERT INTO Prestamos (Usuario, Tipo_Prestamo, Existencia, Fecha_Prestamo, Fecha_Devolucion, Fecha_Entrega_Real) VALUES
(1, 2, 1, '2025-09-01', '2025-09-15', NULL),   -- Usuario con Id = 1 (Juan Pérez)
(2, 1, 2, '2025-09-05', '2025-09-05', '2025-09-05'),   -- Usuario con Id = 2 (María Gómez)
(3, 2, 3, '2025-08-20', '2025-09-03', '2025-09-02'),   -- Usuario con Id = 3 (Carlos Ramírez)
(4, 3, 4, '2025-09-10', '2025-09-24', NULL),   -- Usuario con Id = 4 (Ana Torres)
(5, 4, 5, '2025-09-12', '2025-09-26', NULL),   -- Usuario con Id = 5 (Luis Fernández)
(6, 2, 1, '2025-09-15', '2025-09-30', NULL),   -- Usuario con Id = 6 (Martín Sánchez)
(7, 1, 2, '2025-09-17', '2025-09-25', NULL),   -- Usuario con Id = 7 (Sofía López)
(8, 3, 3, '2025-09-18', '2025-09-28', NULL),   -- Usuario con Id = 8 (Ricardo Pérez)
(9, 4, 4, '2025-09-20', '2025-10-04', NULL),   -- Usuario con Id = 9 (Laura Martínez)
(10, 5, 5, '2025-09-22', '2025-10-06', NULL),  -- Usuario con Id = 10 (Pedro González) -- Actualizado el ID
(11, 1, 1, '2025-09-25', '2025-10-10', NULL),  -- Usuario con Id = 11 (Felipe Ruiz)
(12, 2, 2, '2025-09-26', '2025-10-11', NULL),  -- Usuario con Id = 12 (Marta Sánchez)
(13, 3, 3, '2025-09-27', '2025-10-12', NULL),  -- Usuario con Id = 13 (José Álvarez)
(14, 4, 4, '2025-09-30', '2025-10-14', NULL),  -- Usuario con Id = 14 (Sara Gómez)
(15, 5, 5, '2025-10-01', '2025-10-15', NULL);  -- Usuario con Id = 15 (Juan Pérez)

-- SANCIONES
INSERT INTO Sanciones (Usuario, Descripcion, Fecha_Inicio, Fecha_Fin) VALUES
(1, 'Retraso en la devolución de "Cien Años de Soledad"', '2025-09-16', '2025-09-23'),
(2, 'Mal estado del ejemplar "La Casa de los Espíritus"', '2025-09-07', '2025-09-14'),
(3, 'Devolución tardía de "Harry Potter y la Piedra Filosofal"', '2025-09-04', '2025-09-11'),
(4, 'Pérdida del ejemplar "Bestiario"', '2025-09-01', '2025-09-30'),
(5, 'Incumplimiento de reglas de consulta en sala', '2025-08-25', '2025-09-05'),
(6, 'Retraso en la devolución de "Kafka en la Orilla"', '2025-09-28', '2025-10-05'),
(7, 'Mal estado del ejemplar "Rayuela"', '2025-09-18', '2025-09-25'),
(8, 'Devolución tardía de "La Casa de los Espíritus"', '2025-09-14', '2025-09-20'),
(9, 'Pérdida del ejemplar "Crónica de una Muerte Anunciada"', '2025-09-10', '2025-09-20');

--SELECT Id, Nombre FROM Usuarios;

USE BibliotecaDB;
GO

SELECT 
    t.name AS Table_Name,
    c.name AS Column_Name,
    c.column_id AS Column_ID,
    tp.name AS Data_Type,
    c.max_length AS Max_Length,
    c.is_nullable AS Is_Nullable
FROM 
    sys.tables AS t
INNER JOIN 
    sys.columns AS c ON t.object_id = c.object_id
INNER JOIN 
    sys.types AS tp ON c.user_type_id = tp.user_type_id
WHERE 
    t.name IN ('Autores', 'Temas', 'Editoriales', 'Paises', 'Tipos', 'Libros', 
               'LibrosAutores', 'LibrosTemas', 'Existencias', 'Estados', 'EstadosExistencias', 
               'Usuarios', 'TiposPrestamos', 'Prestamos', 'Sanciones')
ORDER BY 
    t.name, c.column_id;

*/