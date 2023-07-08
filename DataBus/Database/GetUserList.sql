create procedure [dbo].[GetUserList] as
begin
	set nocount on;

		declare @result xml;
		select
			@result = (
				select Id as [@id], EmailAddress as [@emailAddress], IsActive as [@isActive], FullName as [@name],
						FirstName, LastName, PhoneNumber
				from [dbo].[User] u with(nolock)
				for xml path('User'), root('Users'), type
		)

	if (@result is null)
		set @result = N'<Users/>';

	
	select @result;
end
