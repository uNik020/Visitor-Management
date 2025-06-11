--CREATE TABLE Visitors (
--    VisitorID INT IDENTITY(1,1) PRIMARY KEY,
--    FullName NVARCHAR(100) NOT NULL,
--    PhoneNumber NVARCHAR(20),
--    Email NVARCHAR(100),
--    PhotoPath NVARCHAR(255),
--    Purpose NVARCHAR(255),
--    CreatedAt DATETIME DEFAULT GETDATE()
--);


--CREATE TABLE Departments (
--    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
--    DepartmentName NVARCHAR(100) NOT NULL
--);


--CREATE TABLE Hosts (
--    HostID INT IDENTITY(1,1) PRIMARY KEY,
--    FullName NVARCHAR(100) NOT NULL,
--    Email NVARCHAR(100) NOT NULL,
--    PhoneNumber NVARCHAR(20),
--    DepartmentID INT,
--    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
--);



--CREATE TABLE Visits (
--    VisitID INT IDENTITY(1,1) PRIMARY KEY,
--    VisitorID INT NOT NULL,
--    HostID INT NOT NULL,
--    CheckInTime DATETIME DEFAULT GETDATE(),
--    CheckOutTime DATETIME NULL,
--    VisitStatus NVARCHAR(20) DEFAULT 'CheckedIn', -- or 'CheckedOut'
--    FOREIGN KEY (VisitorID) REFERENCES Visitors(VisitorID),
--    FOREIGN KEY (HostID) REFERENCES Hosts(HostID)
--);


--CREATE TABLE Admins (
--    AdminID INT IDENTITY(1,1) PRIMARY KEY,
--    Username NVARCHAR(50) NOT NULL UNIQUE,
--    PasswordHash NVARCHAR(255) NOT NULL,
--    Role NVARCHAR(50) DEFAULT 'Admin',
--    CreatedAt DATETIME DEFAULT GETDATE()
--);
