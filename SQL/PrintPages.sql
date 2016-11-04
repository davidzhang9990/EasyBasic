
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Print_Pages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) 
drop procedure [dbo].[Print_Pages] 
GO	
/*
********************************
*** exec Print_Pages
*** Pages
********************************
*/
CREATE PROC DBO.[Print_Pages]
As
begin
		Declare @Id varchar(50)
        Declare @Name nvarchar(500)
        Declare @ActionName nvarchar(max)
        Declare @ControlName nvarchar(50)
        Declare @Area nvarchar(50)
        Declare @DateAdded datetime
        Declare @DateUpdated datetime
        Declare @AddedBy varchar(50)
        Declare @UpdatedBy varchar(50)
		Declare curr_Resource Cursor For 
		select  [Id],[Name],[ActionName],[ControlName],[Area],[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy] FROM [dbo].[Pages]
		order by [DateAdded]
		Open curr_Resource 
		Fetch next From curr_Resource Into @Id,@Name,@ActionName,@ControlName,@Area,@DateAdded,@DateUpdated,@AddedBy,@UpdatedBy
		While @@fetch_status=0 
		Begin 

			print 'Insert Into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],[ShowPage],[ActiveFlag],[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy]) values('''+ @Id +''',N'''+ @Name +''',N'''+ @ActionName +''','''+ @ControlName +''','''+ @Area +''',''true'',1,GETDATE(),GETDATE(),'''+ @AddedBy +''','''+ @UpdatedBy +''')'
			
		Fetch Next From curr_Resource Into  @Id,@Name,@ActionName,@ControlName,@Area,@DateAdded,@DateUpdated,@AddedBy,@UpdatedBy
		End 
		Close curr_Resource 
		Deallocate curr_Resource
end
