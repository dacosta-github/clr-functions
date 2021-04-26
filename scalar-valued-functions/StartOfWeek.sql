CREATE FUNCTION dbo.StartOfWeek (
	@Date DATETIME,
	-- Sun = 1, Mon = 2, Tue = 3, Wed = 4
	-- Thu = 5, Fri = 6, Sat = 7
	-- Default to Sunday
	@WeekStartDay INT = 1
	)
/*
Find the fisrt date on or before @Date that matches 
day of week of @WeekStartDay.
*/
RETURNS DATETIME
AS
BEGIN
	DECLARE @StartOfWeekDate DATETIME
	DECLARE @FirstRow DATETIME

	-- Check for valid day of week
	IF @WeekStartDay BETWEEN 1
			AND 7
	BEGIN
		-- Find first day on or after 1753/1/1 (-53690)
		-- matching day of week of @WeekStartDay
		-- 1753/1/1 is earliest possible SQL Server date.
		SELECT @FirstRow = convert(DATETIME, - 53690 + ((@WeekStartDay + 5) % 7))

		-- Verify beginning of week not before 1753/1/1
		IF @DATE >= @FirstRow
		BEGIN
			SELECT @StartOfWeekDate = dateadd(dd, (datediff(dd, @FirstRow, @Date) / 7) * 7, @FirstRow)
		END
	END

	RETURN @StartOfWeekDate
END
GO