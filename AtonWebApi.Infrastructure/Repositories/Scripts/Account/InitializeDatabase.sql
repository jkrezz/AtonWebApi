CREATE TABLE IF NOT EXISTS Users (
    Guid UUID PRIMARY KEY,
    Login VARCHAR(50) UNIQUE NOT NULL CHECK (Login ~ '^[a-zA-Z0-9]+$'),
    Password VARCHAR(100) NOT NULL CHECK (Password ~ '^[a-zA-Z0-9]+$'),
    Name VARCHAR(100) NOT NULL CHECK (Name ~ '^[a-zA-Zа-яА-ЯёЁ]+$'),
    Gender INT NOT NULL CHECK (Gender BETWEEN 0 AND 2),
    Birthday DATE,
    Admin BOOLEAN NOT NULL,
    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy VARCHAR(50) NOT NULL,
    ModifiedOn TIMESTAMP,
    ModifiedBy VARCHAR(50),
    RevokedOn TIMESTAMP,
    RevokedBy VARCHAR(50)
    );

INSERT INTO Users (
    Guid, Login, Password, Name, Gender, Admin, CreatedBy
) VALUES (
          '00000000-0000-0000-0000-000000000000',
          'admin',
          '{hashedPassword}',
          'Administrator',
          1,
          true,
          'System'
         ) ON CONFLICT (Guid) DO NOTHING;