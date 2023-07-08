create procedure SaveUser(@user xml) as
begin
	set nocount on

	declare @currentTransactionCount int
	set @currentTransactionCount = @@trancount
	if (@currentTransactionCount = 0)
	   begin transaction SaveUser

	begin try

		declare @id bigint = @user.value('/User[1]/@id', 'bigint')
		if ((isnull(@id, 0) = 0) or (@id < 0) or (not exists(select 1 from [User] where (Id = @id))))
		begin
			declare @result table(Id bigint)
			exec CreateNewUser @user

			select @id = Id from @result
		end
		else
		begin
			if (not exists(select 1 from [User] where (Id = @id)))
				throw 64406, 'Failed to find user to update', 16

			exec UpdateUser @user
		end

		if (@currentTransactionCount = 0)
			commit transaction SaveUser

		select @id as [@id] for xml path('Result'), type
	end try
	begin catch
		if ((xact_state() <> 0) and (@currentTransactionCount = 0))
			rollback transaction SaveUser;
		throw
	end catch
end
