create procedure CreateNewUser(@user xml) as
begin
	set nocount on

	declare @emailAddress nvarchar(100) = @user.value('/User[1]/@emailAddress', 'nvarchar(100)')
	if (exists(select 1 from [dbo].[User] where EmailAddress = @emailAddress))
		throw 62206, 'User already exists', 4

	declare @currentTransactionCount int
	set @currentTransactionCount = @@trancount
	if (@currentTransactionCount = 0)
	   begin transaction CreateUser

	begin try
		declare @result table(Id bigint);
		declare @id bigint;

		insert into [User](FirstName, LastName, EmailAddress, EmailAddressConfirmed, PhoneNumber, PhoneNumberConfirmed)

		output inserted.Id into @result
		select @user.value('(/User[1]/@firstName)', 'nvarchar(100)'), @user.value('(/User[1]/@lastName)', 'nvarchar(100)'),
			@user.value('/User[1]/@emailAddress', 'nvarchar(100)'), 0, @user.value('(/User[1]/@phoneNumber)', 'nvarchar(50)'),
			0
			
		select @id = Id from @result

		if (@currentTransactionCount = 0)
			commit transaction CreateUser

		select @id as [@id] for xml path('Result'), type
	end try
	begin catch
		if ((xact_state() <> 0) and (@currentTransactionCount = 0))
			rollback transaction CreateUser;
		throw
	end catch
end
