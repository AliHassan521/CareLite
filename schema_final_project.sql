-- use careLite_6598

If OBJECT_ID('Roles','U') is not null drop table Roles;
Create table Roles(
    RoleId int primary key identity,
	RoleName nvarchar(50) unique not null
);

If OBJECT_ID('Users','U') is not null drop table Users;
Create table Users(
    UserId int primary key identity,
	Username nvarchar(50) unique not null,
	PasswordHash nvarchar(255) not null,
	FullName nvarchar(100) not null,
	Email nvarchar(100) unique not null,
	Phone nvarchar(100),
	RoleId int not null foreign key references Roles(RoleId),
	IsActive bit default 1,
	CreatedAt datetime default getdate(),
	UpdatedAt datetime null
);

If OBJECT_ID('AuditLogs','U') is not null drop table AuditLogs;
Create table AuditLogs(
    AuditId int primary key identity,
	CorrelationId uniqueidentifier not null,
	UserId int foreign key references Users(UserId),
	Action nvarchar(50) not null,
	Description nvarchar(300),
	CreatedAt datetime default getdate()
);




