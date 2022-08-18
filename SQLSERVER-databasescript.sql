-- CTRL-A + Execute
use master
GO
drop database if exists GeneralDB
GO
create database GeneralDB
GO
use GeneralDB
GO

-- GENERATE TABLES AND CONSTRAINTS--
--drop table if exists Student
create table Student
(
	ID int not null identity(1,1) primary key,
	Firstname nvarchar(50),
	Lastname nvarchar(50) not null,
	Birthdate smalldatetime,
	ClassID int,
)
GO
--drop table if exists Class
create table Class
(
	ID int not null identity(1,1) primary key,
	Classname nvarchar(50),
	NumStudent int default 0,
)
GO
alter table Student add constraint FK_ClassID foreign key (ClassID) references Class(ID)
GO

-- GENERATE PROCEDURES FOR ADO.NET'S METHODS --
--drop procedure GETALLSTUDENT
create procedure GETALLSTUDENT
as
begin
	select s.ID as StudentID, Firstname, Lastname, s.ClassID, Classname, Birthdate from Student s left outer join Class c on s.ClassID = c.ID
end
GO
--get by id--
--drop procedure GETSTUDENTBYID
create procedure GETSTUDENTBYID @input int
as
begin
	select s.ID as StudentID, Firstname, Lastname, s.ClassID, Classname, Birthdate from Student s left outer join Class c on s.ClassID = c.ID where s.ID = @input
end
GO
--drop procedure GETSTUDENTBYCLASS
create procedure GETSTUDENTBYCLASS @input int
as
begin
	if (@input is null) select ID as StudentID, Firstname, Lastname, Birthdate from Student where ClassID is null
	else select ID as StudentID, Firstname, Lastname, Birthdate from Student where ClassID = @input
end
GO
--drop procedure DELETESTUDENTBYID
create procedure DELETESTUDENTBYID @input int
as 
begin
	delete from Student where ID = @input
end
GO
--drop procedure UPDATESTUDENT
create procedure UPDATESTUDENT @input int, @FN nvarchar(50), @LN nvarchar(50), @C int
as
begin
	update Student set Firstname = @FN, Lastname = @LN, ClassID = @C where ID = @input
end
GO
create procedure UPDATESTUDENTCLASS @id int, @classid int
as
begin
	update Student set ClassID = @classid where ID = @id
end
GO
--drop procedure ADDSTUDENT
create procedure ADDSTUDENT @FN nvarchar(50), @LN nvarchar(50), @C int, @BD smalldatetime
as
begin
	INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES (@FN, @LN, @C, @BD)
end
GO
--drop procedure GETCLASSBYID
create procedure GETCLASSBYID @input int
as 
begin
	select * from Class where ID = @input
end
GO
--drop procedure ADDCLASS
create procedure ADDCLASS @input nvarchar(50)
as
begin
	insert into Class (Classname) values (@input)
end
GO
--drop procedure UPDATECLASS
create procedure UPDATECLASS @input int, @name nvarchar(50)
as
begin
	update Class set Classname = @name where ID = @input
end
GO
--drop procedure DELETECLASSBYID
create procedure DELETECLASSBYID @input int
as
begin
	update Student set ClassID = null where ClassID = @input
	delete from Class where ID = @input
end
GO

-- GENERATE DATABASE TRIGGERS
--drop trigger StudentUpdate_StudentTrigger
create trigger StudentUpdate_StudentTrigger ON Student
FOR UPDATE AS
	DECLARE @LNNew nvarchar(50), @C int, @CNew int
	SELECT @LNNew = Lastname FROM inserted
	SELECT @C = ClassID from deleted
	SELECT @CNew = ClassID from inserted

	IF (@LNNew = '')
	BEGIN 
		ROLLBACK TRAN
		RAISERROR ('Lastname khong duoc de trong', 16, 1)
		return
	END
	if (@C is null and @CNew is not null) 
	begin
		update Class set NumStudent = NumStudent + 1 where ID = @CNew
		print('Da update lai si so lop')
	end
	else if (@C is not null and @CNew is null)
	begin
		update Class set NumStudent = NumStudent - 1 where ID = @C
		print('Da update lai si so lop')
	end
	else if (@C != @CNew)
	begin
		update Class set NumStudent = NumStudent - 1 where ID = @C
		update Class set NumStudent = NumStudent + 1 where ID = @CNew
		print('Da update lai si so lop')
	end
GO
--drop trigger StudentCreate_StudentTrigger
create trigger StudentCreate_StudentTrigger on Student 
for insert as
	declare @ClassID int, @LN nvarchar(50)
	select @ClassID = ClassID from inserted
	select @LN = Lastname from inserted

	if (@LN = '')
	begin
		ROLLBACK TRAN
		RAISERROR ('Lastname khong duoc de trong', 16, 1)
		return
	end
	if (@ClassID is not null) 
	begin
		update Class set NumStudent = NumStudent + 1 where ID = @ClassID
		print('Da update si so lop tuong ung')
	end
GO
--drop trigger StudentDelete_StudentTrigger
create trigger StudentDelete_StudentTrigger on Student
for delete as
	declare @C int
	select @C = ClassID from deleted
	if (@C is not null)
	begin
		update Class set NumStudent = NumStudent - 1 where ID = @C
		print('Da update lai si so lop')
	end
GO
--drop trigger ClassCreate_ClassTrigger
create trigger ClassCreateUpdate_ClassTrigger on Class
after insert, update as
	declare @CN nvarchar(50), @Count int
	select @CN = Classname from inserted
	select @Count = count(*) from Class where Classname = @CN
	
	if (@Count > 1)
	begin
		ROLLBACK TRAN
		RAISERROR ('Classname bi trung', 16, 1)
		return
	end
GO

-- TEST ROWS
INSERT INTO Class (Classname) VALUES ('PMCL01')
INSERT INTO Class (Classname) VALUES ('PMCL02')
INSERT INTO Class (Classname) VALUES ('PMCL03')
GO
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Tran Trung', 'Hieu', 1, '2000-06-05')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Luong Thanh', 'Tu', 1, '2000-02-25')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Tran Ngoc', 'Duyen', 1, '2000-09-01')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Nguyen Tuan', 'Khoi', 1, '2000-11-11')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Nguyen Thanh', 'Phong', 1, '2000-01-20')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Hoang Trung', 'Kien', 1, '2000-3-31')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Trinh', 'Tuan', 1, '2000-04-04')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Nguyen Cong', 'Vinh', 2, '2000-08-06')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Truong Thi', 'Tram', 2, '2000-09-15')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Luong Cong', 'Tuan', 2, '2000-03-03')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Tran', 'Vinh', 2, '2000-12-21')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Trieu Vinh', 'Thai', 2, '2000-06-29')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Dang Hoang Tuan', 'Khai', 2, '2000-11-01')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Nguyen Minh', 'Hoang', 2, '2000-04-13')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Luong Minh', 'Tuan', 2, '2000-02-20')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Vu Viet', 'Huy', null, '2000-09-12')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Nguyen Tran Bao', 'Thy', null, '2000-11-30')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Truong Dinh Gia', 'Kien', null, '2000-12-01')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Vo Gia', 'Bao', null, '2000-05-02')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Tran Minh', 'Kiet', 3, '2000-01-09')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Luong Cong', 'Quy', 3, '2000-04-02')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Hoang Ngoc', 'Tuan',3, '2000-06-13')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Tran Nam', 'Vy', 3, '2000-05-27')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Hoang Hoang', 'Linh', 3, '2000-09-09')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Dang Lan', 'Tuan', 3, '2000-02-10')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Hoang Trieu', 'Quan', 3, '2000-05-02')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Truong Hoang', 'Dieu', 3, '2000-11-19')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Tran Minh', 'Quy', 3, '2000-08-21')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Dang Nam', 'Trung',3,'2000-10-09')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Do Tan', 'Vinh', 3, '2000-06-13')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Phan Trong', 'Dieu', 3, '2000-12-20')
INSERT INTO Student (Firstname, Lastname, ClassID, Birthdate) VALUES ('Phan Khanh', 'Tuan', 3, '2000-10-29')
GO
-----------------------------
