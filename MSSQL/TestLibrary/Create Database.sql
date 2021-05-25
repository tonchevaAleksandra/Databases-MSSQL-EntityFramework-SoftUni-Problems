use TestLibrary

CREATE table Role
(
    Id int primary key  identity ,
    Name nvarchar(20) not null
)
create table Address
(
    Id   int primary key identity ,
    Country nvarchar(30) not null  ,
    City nvarchar(50) not null,
    Street nvarchar(50) not null,
    StreetNumber int not null ,
    BuildingNumber int,
    ApartmentNumber int,
    AdditionalInfo nvarchar(100)
)

create table ApplicationUser
(
    Id int primary key  identity ,
    UserName nvarchar(25) not null unique ,
    Email nvarchar(50) not null unique ,
    FirstName nvarchar(60) not null ,
    LastName nvarchar(50) not null ,
    PhoneNumber nvarchar(12) not null ,
    Password nchar(10) CHECK (Len(Password)=10),
    AddressId int foreign key references Address
)

create  table Author
(
    Id int primary key  identity ,
    Name nvarchar(100) not null
)

create table Genre
(
    Id int primary key identity ,
    Name nvarchar(50) not null ,
    Description nvarchar(200)
)

create table Book
(
    Id int primary key identity ,
    Title nvarchar(100) not null ,
    Description nvarchar(max) not null ,
    AuthorId int foreign key references Author,
    CoverImageUrl nvarchar(max) not null ,
    Quantity int not null ,
    GenreId int foreign key references Genre

)

alter table ApplicationUser
add IsActive bit default (0)

create table SubscriptionRequest
(
    Id int primary key identity,
    ApplicationUserId int not null foreign key references ApplicationUser
)



