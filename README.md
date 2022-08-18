# ADO.NET and Entity Framework Core on ASP.NET Core 6 (MVC)

### General:
This is a test project to differentiate the syntaxs, structure, use cases, design methods, ... between 2 popular frameworks in .NET environment.
Techstack:
- Microsoft SQL Server as database provider
- ASP.NET Core 6 MVC as front-end web application

### Database:
The simple database is the same for both application:

![databasediagram](https://user-images.githubusercontent.com/92220850/185288940-d45e663b-b7da-42b9-8be1-3cf49982e634.PNG)

You will need to setup the database locally for the applications to work. The database script [HERE](https://github.com/khanhnguyen02311/Test-ADONET-EFCore-on-ASPNETCore6/blob/master/SQLSERVER-databasescript.sql) contains all the tables, procedures, triggers and test datas for the database.

You may need to change the server name on the connection string of both application's appsetting.json to your local SQLServer name:

![cnnstr](https://user-images.githubusercontent.com/92220850/185293365-a8cee837-634e-4073-9ef3-3d92cbb682e2.PNG)

### User interface:
Both applications have pretty much the same UI/UX for the CRUD operations:

**_Student Page - Index_**
- Show student list and option buttons
- Sorting and searching on list

![studentpage_index](https://user-images.githubusercontent.com/92220850/185291515-830bba46-0443-45e3-89f7-c709b0555d64.PNG)

**_Student Page - Create and Edit_**
- Create new student, or edit current student's informations
- The information box will be filled with selected student's information if you are editing a student

![studentpage_create](https://user-images.githubusercontent.com/92220850/185291556-f50a80e5-64ce-4f57-a0d4-ab6097347ae8.PNG)
![studentpage_edit](https://user-images.githubusercontent.com/92220850/185291551-da884e7b-f70a-4767-983e-98f4e708d9ea.PNG)

**_Class Page - Index_**
- Show class list and option buttons
- Sorting and searching on list

![classpage_index](https://user-images.githubusercontent.com/92220850/185291598-c1df83f5-a33a-48fa-b800-cbd31fab2c3f.PNG)

**_Class Page - Create_**
- Create new class

![classpage_create](https://user-images.githubusercontent.com/92220850/185291614-f9f2a416-7c09-4187-8013-308877c97869.PNG)

**_Class Page - Edit_**
- Change class name
- Add student or remove student out of class

![classpage_edit](https://user-images.githubusercontent.com/92220850/185291631-74b15bdb-6cbd-4d4c-a00c-d2d27952627a.PNG)

### ADO.NET:
### Entity Framework (Database First):
