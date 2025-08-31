
-- add all roles in the system 
INSERT INTO Roles (Name) VALUES
('SuperAdmin'),
('Admin'),
('Doctor'),
('LabTechnical'),
('Receptionist'),
('Patient');

--------------------
-- Inser the intial user for the system

---Insert the person data
INSERT INTO People ( FirstName, SecondName, ThirdName, LastName,  Gender,  Phone, Address)

VALUES ('superadmin', 'superadmin', 'superadmin', 'superadmin',  1, '123456789', 'Omduramn');

declare @SuperAdminPersonID int = SCOPE_IDENTITY();;

--- Insert the user data
INSERT INTO Users (PersonID, Email,UserName, Password)
VALUES (@SuperAdminPersonID, 'superadmin@superadmin.com','superadmin', 'AQAAAAIAAYagAAAAEHq67vxuKN0thnUaZL/YVyi8V24jH3d07ihwd4aAchpFefll1oUAs5iEplqVJRWA/A==');

declare @UserId int = SCOPE_IDENTITY();

--- Insert the userrole data
INSERT INTO UserRoles(RoleId, UserId,IsActive)
VALUES (1, @UserId,1);

