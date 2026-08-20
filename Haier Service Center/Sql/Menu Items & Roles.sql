
CREATE TABLE MenuItems (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(255) NOT NULL,
    Url NVARCHAR(255),
    ParentId INT,
    FOREIGN KEY (ParentId) REFERENCES MenuItems(Id)
);


CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY,
    Role NVARCHAR(50) NOT NULL
);

CREATE TABLE MenuItemRoleMapping (
    MenuItemId INT,
    RoleId INT,
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(Id),
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);


-- Sample Roles
INSERT INTO Roles (Role) VALUES ('Admin');
INSERT INTO Roles (Role) VALUES ('User');

INSERT INTO MenuItems (Title, Url, ParentId) VALUES ('Home', '/', NULL);
INSERT INTO MenuItems (Title, Url, ParentId) VALUES ('About', '/About', NULL);
INSERT INTO MenuItems (Title, Url, ParentId) VALUES ('Contact', '/Contact', NULL);
INSERT INTO MenuItems (Title, Url, ParentId) VALUES ('Admin Dashboard', '/Admin', NULL);

INSERT INTO MenuItems (Title, Url, ParentId) VALUES ('Products', '/Products', NULL);
INSERT INTO MenuItems (Title, Url, ParentId) VALUES ('Services', '/Services', NULL);

INSERT INTO MenuItems (Title, Url, ParentId) VALUES ('Manage Users', '/Admin/Users', 4);
INSERT INTO MenuItems (Title, Url, ParentId) VALUES ('Manage Products', '/Admin/Products', 4);

-- Assign Roles to Menu Items
INSERT INTO MenuItemRoleMapping (MenuItemId, RoleId) VALUES (5, 1); -- Products can be accessed by Admin
INSERT INTO MenuItemRoleMapping (MenuItemId, RoleId) VALUES (6, 2); -- Services can be accessed by User
INSERT INTO MenuItemRoleMapping (MenuItemId, RoleId) VALUES (7, 1); -- Manage Users can be accessed by Admin
INSERT INTO MenuItemRoleMapping (MenuItemId, RoleId) VALUES (8, 1); -- Manage Products can be accessed by Admin



