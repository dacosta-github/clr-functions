CREATE FUNCTION [dbo].[Split]
(
    @SearchString NVARCHAR(MAX),
    @Delimiter NCHAR(1)
)
RETURNS 
	@tab TABLE (ID INT IDENTITY, Data VARCHAR(100) COLLATE SQL_Latin1_General_Cp1253_CI_AI) 
AS
BEGIN
	DECLARE @i1 INT;
	DECLARE @i2 INT;
	DECLARE @Word VARCHAR(100);

	SELECT @SearchString = UPPER(@Delimiter + @SearchString  + @Delimiter COLLATE SQL_Latin1_General_CP1253_CI_AI);

	SET @i1 = 1;
	WHILE (@i1 != 0)
	BEGIN
		SET @i2=CHARINDEX(@Delimiter, @SearchString, @i1+1)

		IF (@i2 != 0)
		BEGIN
			SELECT @Word = REPLACE(RTRIM(LTRIM(SUBSTRING(@SearchString, @i1+1, @i2-@i1))), @Delimiter, '')
			IF @Word != '' 
					INSERT INTO @tab(Data) SELECT @Word
		END

		SET @i1 = @i2
	END

	IF (SELECT COUNT(ID) FROM @tab) = 0
		INSERT INTO @tab(Data) VALUES ('%')

	RETURN;
END
