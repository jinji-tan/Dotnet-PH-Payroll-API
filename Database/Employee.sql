-- 1. Create Employees Table
CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
    DateOfBirth DATE NOT NULL,
    DailyRate DECIMAL(18, 2) NOT NULL,
    WorkingDays NVARCHAR(20)
);

-- 2. Create Attendance Table
CREATE TABLE Attendance (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT NOT NULL,
    RecordDate DATE NOT NULL,
    IsPresent BIT NOT NULL,
    CONSTRAINT FK_Attendance_Employee FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);

-- 3. Create Payslips Table
CREATE TABLE Payslips (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT NOT NULL,
    PayPeriodStart DATE NOT NULL,
    PayPeriodEnd DATE NOT NULL,
    GrossPay DECIMAL(18, 2) NOT NULL,
    SSS_Deduction DECIMAL(18, 2) NOT NULL,
    PhilHealth_Deduction DECIMAL(18, 2) NOT NULL,
    PagIBIG_Deduction DECIMAL(18, 2) NOT NULL,
    Tax_Withheld DECIMAL(18, 2) NOT NULL,
    NetPay DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_Payslips_Employee FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);