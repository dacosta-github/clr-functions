CREATE FUNCTION dbo.EndOfWeek (
	@Date DATETIME	,
	-- Sun = 1, Mon = 2, Tue = 3, Wed = 4
	-- Thu = 5, Fri = 6, Sat = 7
	-- Default to Sunday
	@WeekStartDay INT = 1
	)
/*
Function: F_END_OF_WEEK
	Finds start of last day of week at 00:00:00.000
	for input datetime, @DAY, for a week that started
	on the day of week of @WeekStartDay.
	Returns a null if the end of week date would
	be after 9999-12-31.
*/
RETURNS DATETIME
AS
BEGIN
	DECLARE @EndOfWeekDate DATETIME
	DECLARE @FirstRow DATETIME
	DECLARE @LastEndOfWeek DATETIME

	-- Check for valid day of week, and return null if invalid
	IF NOT @WeekStartDay BETWEEN 1 AND 7
		RETURN NULL

	-- Find the last end of week for the passed day of week
	SELECT @LastEndOfWeek = convert(DATETIME, 2958457 + ((@WeekStartDay + 6) % 7))

	-- Return null if end of week for date passed is after 9999-12-31
	IF @Date > @LastEndOfWeek
		RETURN NULL

	-- Find the first valid beginning of week for the date passed.
	SELECT @FirstRow = convert(DATETIME, - 53690 + ((@WeekStartDay + 5) % 7))

	-- If date is before the first beginning of week for the passed day of week
	-- return the day before the first beginning of week
	IF @Date < @FirstRow
	BEGIN
		SET @EndOfWeekDate = dateadd(dd, - 1, @FirstRow)

		RETURN @EndOfWeekDate
	END

	-- Find end of week for the normal case as 6 days after the beginning of week
	SELECT @EndOfWeekDate = dateadd(dd, ((datediff(dd, @FirstRow, @Date) / 7) * 7) + 6, @FirstRow)

	RETURN @EndOfWeekDate
END
GO