SET IDENTITY_INSERT Restaurants ON;

DECLARE @i INT = 1;
DECLARE @id INT = 90000; -- Starting value for Id
DECLARE @randomString NVARCHAR(MAX);
DECLARE @randomBit BIT;
DECLARE @randomNumber NVARCHAR(MAX);
DECLARE @randomCity NVARCHAR(MAX);
DECLARE @randomStreet NVARCHAR(MAX);
DECLARE @randomPostalCode NVARCHAR(MAX);

WHILE @i <= 5000
BEGIN
    SET @randomString = CONVERT(nvarchar(max), NEWID()); -- Random string
    SET @randomBit = ABS(CHECKSUM(NewId())) % 2; -- Random bit (0 or 1)
    SET @randomNumber = CONVERT(nvarchar(max), ABS(CHECKSUM(NewId())) % 10000000000); -- Random number
    SET @randomCity = 'City' + CONVERT(nvarchar(max), ABS(CHECKSUM(NewId())) % 1000); -- Random city
    SET @randomStreet = 'Street' + CONVERT(nvarchar(max), ABS(CHECKSUM(NewId())) % 1000); -- Random street
    SET @randomPostalCode = CONVERT(nvarchar(max), ABS(CHECKSUM(NewId())) % 100000); -- Random postal code

    INSERT INTO Restaurants (Id, Name, Description, Category, HasDelivery, ContactEmail, ContactNumber, Address_City, Address_Street, Address_PostalCode, OwnerId)
    VALUES (
        @id, -- Sequential Id
        @randomString, -- Random name
        @randomString, -- Random description
        @randomString, -- Random category
        @randomBit, -- Random delivery availability
        @randomString + '@test.com', -- Random email
        @randomNumber, -- Random contact number
        @randomCity, -- Random city
        @randomStreet, -- Random street
        @randomPostalCode, -- Random postal code
        'c1c18a9c-caeb-4ba2-8ae0-9052ec39eb0d' -- OwnerId
    );

    SET @i = @i + 1;
    SET @id = @id + 1; -- Increment Id
END

SET IDENTITY_INSERT Restaurants OFF;