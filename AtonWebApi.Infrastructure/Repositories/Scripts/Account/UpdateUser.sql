UPDATE Users SET
                 Login = @Login,
                 Password = @Password,
                 Name = @Name,
                 Gender = @Gender,
                 Birthday = @Birthday,
                 Admin = @Admin,
                 ModifiedOn = @ModifiedOn,
                 ModifiedBy = @ModifiedBy,
                 RevokedOn = @RevokedOn,
                 RevokedBy = @RevokedBy
WHERE Guid = @Guid;