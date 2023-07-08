create procedure [dbo].[DeleteUser]( @user xml) as
begin
	set nocount on


	declare @userId bigint = @user.value('/User[1]/@id', 'bigint')
	if (isnull(@userId, 0) = 0)
		throw 73114, 'User must be specified', 16

	if (not exists(select 1 from [User] where (Id = @userId) ))
		throw 73115, 'User may not exist', 16

	declare @currentTransactionCount int
	set @currentTransactionCount = @@trancount
	if (@currentTransactionCount = 0)
	   begin transaction DeleteUser

	begin try
		-- Deleting a user requires deletion from the following tables:
		--  User

		delete from [User] where Id = @userId

		if (@currentTransactionCount = 0)
			commit transaction DeleteUser
		
		if (exists(select 1 from [User] where (Id = @userId)))
			throw 73118, 'User could not be deleted.', 16

		select @userId as [@id] for xml path('Result'), type
	end try
	begin catch
		if ((xact_state() <> 0) and (@currentTransactionCount = 0))
			rollback transaction DeleteUser;
		throw
	end catch
end