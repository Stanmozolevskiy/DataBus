create procedure UpdateUser(@user xml) as
begin
	set nocount on

	declare @id bigint = @user.value('/User[1]/@id', 'bigint')
	if (isnull(@id, 0) = 0)
		throw 64504, 'User must be specified', 16
	if (not exists(select 1 from [User] where (Id = @id)))
		throw 64507, 'Failed to find user to update', 16

	declare @currentTransactionCount int
	set @currentTransactionCount = @@trancount
	if (@currentTransactionCount = 0)
	   begin transaction UpdateUser

	begin try
		;with C(Id, FirstName, LastName, EmailAddress, PhoneNumber, PhoneNumberConfirmed) as
		(
			select
				@id,
				t.c.value('FirstName[1]', 'nvarchar(100)'),
				t.c.value('LastName[1]', 'nvarchar(100)'),
				t.c.value('@emailAddress', 'nvarchar(100)'),
				t.c.value('PhoneNumber[1]', 'nvarchar(50)'),
				t.c.value('PhoneNumberConfirmed[1]', 'bit')
			from @user.nodes('/User') t(c)
		)
		update u set
			u.FirstName = C.FirstName,
			u.LastName = C.LastName,
			u.EmailAddress = C.EmailAddress,
			u.PhoneNumber = isnull(C.PhoneNumber, u.PhoneNumber),
			u.PhoneNumberConfirmed = isnull(C.PhoneNumberConfirmed, u.PhoneNumberConfirmed)
		from [User] u
			inner join C on C.Id = u.Id

		if (@currentTransactionCount = 0)
			commit transaction UpdateUser

		select @id as [@id] for xml path('Result'), type
	end try
	begin catch
		if ((xact_state() <> 0) and (@currentTransactionCount = 0))
			rollback transaction UpdateUser;
		throw
	end catch
end
