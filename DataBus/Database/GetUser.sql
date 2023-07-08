
create procedure GetUser(@id bigint) as
begin
	set nocount on

	declare @result xml;

	if (@id is null)
		throw 76101, 'User id is not provided', 16
			
		

	select @result =
		(
			select 
			u.Id [@id], 
			u.FirstName as [@firstName],
			u.LastName as [@lastName],	
			u.EmailAddress as [@emailAddress], 
			u.IsActive as [@isActive]

			
			from [dbo].[User] u with(nolock)
			where u.Id = @id 
			for xml path('User'), type
		)

	
	if (@result is null)
		set @result = '<User xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true" tenantId="0" id="0" emailAddress="" />'
	
	select @result
end
