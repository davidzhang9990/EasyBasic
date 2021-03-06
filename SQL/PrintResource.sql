
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Print_Resource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) 
drop procedure [dbo].[Print_Resource] 
GO	
/*
********************************
*** exec Print_Resource
*** 输出Resource
********************************
*/
CREATE PROC DBO.[Print_Resource]
As
begin
		Declare @Id varchar(50)
        Declare @Name nvarchar(500)
        Declare @Value nvarchar(max)
        Declare @Culture nvarchar(50)
        Declare @Type nvarchar(50)
        Declare @Group nvarchar(50)
        Declare @DateAdded datetime
        Declare @DateUpdated datetime
        Declare @AddedBy varchar(50)
        Declare @UpdatedBy varchar(50)
		Declare curr_Resource Cursor For 
		select  [Id],[Name],[Value],[Culture],[Type],[Group],[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy] FROM [dbo].[Resource] order by [Culture]
		Open curr_Resource 
		Fetch next From curr_Resource Into @Id,@Name,@Value,@Culture,@Type,@Group,@DateAdded,@DateUpdated,@AddedBy,@UpdatedBy
		While @@fetch_status=0 
		Begin 

			print 'Insert Into [dbo].[Resource] values('''+ @Id +''','''+ @Name +''',N'''+ @Value +''','''+ @Culture +''','''+ @Type +''','''+ @Group +''',GETDATE(),GETDATE(),'''+ @AddedBy +''','''+ @UpdatedBy +''')'
			
		Fetch Next From curr_Resource Into @Id,@Name,@Value,@Culture,@Type,@Group,@DateAdded,@DateUpdated,@AddedBy,@UpdatedBy
		End 
		Close curr_Resource 
		Deallocate curr_Resource
end
