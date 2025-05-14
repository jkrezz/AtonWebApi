INSERT INTO Users (
    Guid, Login, Password, Name, Gender, Birthday,
    Admin, CreatedOn, CreatedBy
) VALUES ( @Guid, @Login, @Password, @Name, @Gender, @Birthday, @Admin, @CreatedOn, @CreatedBy);