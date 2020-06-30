
GO
/****** Object:  StoredProcedure [GGPlatform].[usp_UsersManager]    Script Date: 05.07.2017 15:48:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*

exec [GGPlatform].[usp_UsersManager] @TypeQuery = 5, @ObjectID = 6

exec [GGPlatform].[usp_UsersManager] @TypeQuery = 4, @Login = 'gg', 
       @Fam = 'gg', @Im = 'hh', @Otch = 'hhh', 
	   @PostRef = 0, @SectionRef = 0, 
	   @isWindowsUser = 0, @isAdmin = 1, 
	   @Workplaces = '1;2;', @ReportGroups = '1;'

*/


-- =============================================
ALTER PROCEDURE [GGPlatform].[usp_UsersManager] 
                @TypeQuery int,
				@ObjectID int = null,

				@Login    sysname = NULL,
                @Password nvarchar(50) = NULL,
                @Fam      nvarchar(40) = NULL,
                @Im       nvarchar(30) = NULL,
                @Otch     nvarchar(40) = NULL,
                @PostRef    int = NULL, 
				@SectionRef int = null,
                @isWindowsUser bit = NULL, 
                @isAdmin   bit = 0,
				@isDropped bit = 0,
				@isAudit bit = 0,

				@Workplaces varchar(8000) = null,
				@ReportGroups varchar(8000) = null
AS
BEGIN
	--begin tran
	declare @ErrMsg nvarchar(200)
	declare @UserID int
	declare @Result int

	select @UserID = -1
	select @Result = -1

	if (@TypeQuery = 1)
	begin
		select g.ID
		from GGPlatform.Workplaces g
		 inner join GGPlatform.MatrixUsersWorkplace m on m.WorkplaceRef = g.ID
		 inner join GGPlatform.Users u on m.UsersRef = u.ID
		where u.ID = @ObjectID
		order by g.ID
	end--рабочие места

	if (@TypeQuery = 2)
	begin
		select g.ID
		from GGPlatform.ReportGroups g
		 inner join GGPlatform.MatrixUsersReportGroup m on m.ReportGroupRef = g.ID
		 inner join GGPlatform.Users u on m.UsersRef = u.ID
		where u.ID = @ObjectID
		order by g.ID
	end--группы отчетов

	if (@TypeQuery = 3)
    begin
	   begin tran

       if @ObjectID is null 
       begin
         select @ErrMsg = 'Не указан идентификатор пользователя.'
         goto ErrExit                              
       end

       update GGPlatform.Users 
	   set  Fam = @Fam, 
			Im = @Im, 
			Otch = @Otch, 
			PostRef = @PostRef, 
			SectionRef = @SectionRef, 
			isAdmin = @isAdmin,
			isAudit = @isAudit
       where ID  = @ObjectID

	   exec GGPlatform.usp_SetObjectArray @TypeQuery = 1, @ObjectID = @ObjectID,  @Array = @Workplaces
	   exec GGPlatform.usp_SetObjectArray @TypeQuery = 2, @ObjectID = @ObjectID,  @Array = @ReportGroups

	   select @UserID = @ObjectID 
	   goto GoodExit
    end--изменить пользователя

	if (@TypeQuery = 4)
	begin
	  if exists(select id from GGPlatform.Users where Upper(Login) = Upper(Rtrim(Ltrim(@Login))))
      begin
        select @ErrMsg = 'Пользователь ' + @Login + ' уже зарегистрирован в системе.'
        goto ErrExit          
      end
             
      insert into GGPlatform.Users(Login, Fam, Im, Otch, PostRef, SectionRef, isAdmin, isDropped, isWindowsUser, isAudit)         
      values(@Login, @Fam, @Im, @Otch, @PostRef, @SectionRef, @isAdmin, 0, @isWindowsUser, @isAudit)
 
	  select @UserID = @@IDENTITY

      if ((@UserID = -1) or (@UserID is null))
      begin
        select @ErrMsg = 'Ошибка добавления пользователя в системные таблицы.'
        goto ErrExit          
      end
        
      if @isWindowsUser = 0  -- аутентификация SQL Server
      begin --добавляю пользователя СКЛ
        exec @Result = sp_addlogin  @loginame = @Login, @passwd = @Password, @defdb = 'master', @deflanguage = 'russian' 

        if @Result = 1
        begin
          select @ErrMsg = 'Ошибка добавления имени пользователя в БД (sp_addlogin).'
          delete from GGPlatform.Users where id = @UserID
          goto ErrExit                              
        end                  
      end
        else                        -- аутентификация Windows
      begin
        exec @Result = sp_grantlogin @loginame = @Login

        if @Result = 1
        begin
          select @ErrMsg = 'Ошибка добавления имени пользователя в БД (sp_grantlogin).'
          delete from GGPlatform.Users where id = @UserID
          goto ErrExit                              
        end      
      end

      exec @Result = sp_grantdbaccess @loginame = @Login     ---получить доступ к базе                                               
   
      if @Result = 1
      begin
        select @ErrMsg = 'Ошибка добавления имени пользователя в БД (sp_grantdbaccess).'
        delete from GGPlatform.Users where id = @UserID              
        
		if @isWindowsUser = 1
           exec sp_revokelogin @login   ---- удаляем пользователя Винды
        else 
           exec sp_droplogin @login     ---- удаляем пользователя СКЛ

        goto ErrExit                              
      end

      if @isAdmin = 0
	  begin
        execute @Result = sp_addrolemember @rolename = 'sysUsers' , @membername = @Login ----включаем в роль
	  end
       else 
	  begin
        execute @Result = sp_addrolemember @rolename = 'db_owner' , @membername = @Login 
        execute @Result = sp_addsrvrolemember @loginame = @Login, @rolename = 'sysAdmins'   ----включаем в стандартную роль сисадмин
      end

      if @Result = 1
      begin
        select @ErrMsg = 'Ошибка добавления пользователя в системную группу.'
        delete from GGPlatform.Users where id = @UserID              

        if @isWindowsUser = 1
           exec sp_revokelogin @login
        else 
           exec sp_droplogin @login

        goto ErrExit                              
      end

	  exec GGPlatform.usp_SetObjectArray @TypeQuery = 1, @ObjectID = @UserID,  @Array = @Workplaces
	  exec GGPlatform.usp_SetObjectArray @TypeQuery = 2, @ObjectID = @UserID,  @Array = @ReportGroups

	  goto GoodExit
    end--добавить пользователя

	if (@TypeQuery = 5)
	begin	  
	  select @Login = Login, @isWindowsUser = isWindowsUser, @isDropped = isDropped from GGPlatform.Users where Id = @ObjectID
        
      if @Login is null
      begin
        select @ErrMsg = 'Пользователь с идентификатором ' + cast(@ObjectID as nvarchar(10)) + ' не найден.'
        goto ErrExit                              
      end

      if @isDropped = 0
         exec @Result = sp_revokedbaccess @Login

      if @isWindowsUser = 1 
         exec @Result = sp_revokelogin @Login
      else 
         exec @Result = master..sp_droplogin @Login

	 -- begin tran

	  delete from GGPlatform.MatrixUsersWorkplace where UsersRef = @ObjectID
	  delete from GGPlatform.MatrixUsersReportGroup where UsersRef = @ObjectID

      if @Result = 1
      begin
        update GGPlatform.Users set isDropped = 1 where Id = @ObjectID

        select @ErrMsg = 'Не удалось удалить пользователя ' + @login + ' (' +case when @isWindowsUser = 1 then 'sp_revokelogin' else 'sp_droplogin' end + ' ).'
        goto ErrExit                              
      end
	  else
	    delete from GGPlatform.Users where Id = @ObjectID

	  select @UserID = @ObjectID
	  goto GoodExit
	end--удалить

	
	GoodExit:
		if @@trancount > 0 commit transaction
		select @UserID as UserID
		return @UserID

	ErrExit:
		if @@trancount > 0 rollback transaction
		raiserror(@ErrMsg, 18, 1)
		select -1 as UserID
		return -1

END
