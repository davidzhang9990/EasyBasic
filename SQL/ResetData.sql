
/*

SELECT TOP 1000 [Id],[Name],[Value],[Culture],[Type],[Group],[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy] FROM [Exam].[dbo].[Resource]

Declare @Id varchar(50) 
Declare @Name varchar(50) 
declare @Value nvarchar(1000)
declare @Culture varchar(50)
declare @Type varchar(50)
declare @Group varchar(50)
declare @DateAdded datetime
declare @DateUpdated datetime
Declare @AddedBy varchar(50) 
Declare @UpdatedBy varchar(50) 
Declare Cur Cursor For 
select  [Id],[Name],[Value],[Culture],[Type],[Group],[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy] FROM [Exam].[dbo].[Resource]
Open Cur 
Fetch next From Cur Into @Id,@Name,@Value,@Culture,@Type,@Group,@DateAdded,@DateUpdated,@AddedBy,@UpdatedBy
While @@fetch_status=0 
Begin 

print 'insert into [dbo].[Resource] values('''+ @Id +''','''+ @Name +''','''+ @Value +''','''+ @Culture +''','''+ @Type +''','''+ @Group +''','''+ cast(@DateAdded as varchar(100)) +''','''+ cast(@DateUpdated as varchar(100)) +''','''+ @AddedBy +''','''+ @UpdatedBy +''')'

Fetch Next From Cur Into @Id,@Name,@Value,@Culture,@Type,@Group,@DateAdded,@DateUpdated,@AddedBy,@UpdatedBy
End 
Close Cur 
Deallocate Cur 

select count(name) from sysobjects where xtype='u'

SELECT TOP 1000  u1.* FROM [Exam].[dbo].[UserRole] r1 
inner join [Exam].[dbo].[Role] r2 on r2.Id=r1.RoleId
inner join [Exam].[dbo].[Users] u1 on u1.Id=r1.UserId
where r2.Name='Teacher' and u1.FirstName is not null

select * from [Exam_Copy].[dbo].[Resource]
select * from [Exam_Copy].[dbo].[Users]
select * from [Exam_Copy].[dbo].[Account]

System Account has been removed, can''t be logined

select name from syscolumns where id=(select max(id) from sysobjects where xtype='u' and name='Users')

use Exam_Copy
go
exec clear_data
*/
alter proc dbo.[clear_data]
As
begin
		drop table dbo.Email
		drop table dbo.EmailTemplate
		drop table dbo.[Log]
		drop table dbo.[Pages]
		drop table dbo.[Profile]
		drop table dbo.[Resource]
		drop table dbo.RolePage
		drop table dbo.UserClaim
		drop table dbo.UserLog
		drop table dbo.UserLogin
		drop table dbo.UserRole
		drop table dbo.[Role]
		drop table dbo.Users

		--add role
		insert into [Role] values('4CCC16EC-1A21-4467-B4D5-37CF6D71227D','System')
		insert into [Role] values('A1EFDF60-410A-4902-B926-CADBBB3C4F85','SystemAdmin')
		insert into [Role] values('06EA0F9C-86AC-49EB-8746-1A50B034E8B8','SystemMember')
		
		insert into [dbo].[Users]([Id],[Email],[EmailConfirmed],[PasswordHash],[SecurityStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEndDateUtc],[LockoutEnabled],[AccessFailedCount],[UserName],[TrueName],[TwoPassword],[Sex],[CardNo],[BirthDay],[Address],[ParentNumber],[RecNumber],[AgentNumber],[KeyString],[IsApproved],[AllCoin],[BuyCoin],[CurrLen],[RecLen],[DateApproved],[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Discriminator])
		values('40d76804-597c-450f-96da-814bc4cbaf3d','kklovegg@126.com',0,'111111','8280eaf2-bb22-4d8c-932f-6713449a4930','18795679990',0,0,NULL,1,0,'system','david','111111',0,'320323','1987-08-14','1','1','1','1','1',0,0,0,0,0,'1900-01-01','2016-10-26 14:28:50.510','2016-10-26 14:28:50.510','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',0,'User')

		insert into [dbo].[UserRole]( [UserId],[RoleId])
		values('40d76804-597c-450f-96da-814bc4cbaf3d','4CCC16EC-1A21-4467-B4D5-37CF6D71227D')

		--Pages
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('E9522F2B-5FF3-468A-A063-5A40C4607197','Dashboard Page','Index','Dashboard','Teacher','true','2014-12-24 08:02:15.620','2014-12-24 08:02:15.620','F06CAB40-A098-4324-8961-9A39E6055E9E','F06CAB40-A098-4324-8961-9A39E6055E9E',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('8C3CE106-2F40-45AA-91F8-0C324132FF01','Content List Page','Index','Question','Teacher','true','2014-12-24 08:03:32.563','2014-12-24 08:03:32.563','F06CAB40-A098-4324-8961-9A39E6055E9E','F06CAB40-A098-4324-8961-9A39E6055E9E',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('2C4326E3-9E14-4CDA-8373-5989C59B7D80','Deck List Page','Index','Deck','Teacher','true','2014-12-24 08:03:49.730','2014-12-24 08:03:49.730','F06CAB40-A098-4324-8961-9A39E6055E9E','F06CAB40-A098-4324-8961-9A39E6055E9E',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('CC7B1940-7C47-47DC-AACD-AD2BC94FE8B8','Session List Page','Index','Session','Teacher','true','2014-12-24 08:03:58.793','2014-12-24 08:03:58.793','F06CAB40-A098-4324-8961-9A39E6055E9E','F06CAB40-A098-4324-8961-9A39E6055E9E',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('54EC60B7-9BF0-4F39-A44A-BFC8E5D06CAB','Group List Page','Index','Group','Teacher','true','2014-12-24 08:04:17.483','2014-12-24 08:04:17.483','F06CAB40-A098-4324-8961-9A39E6055E9E','F06CAB40-A098-4324-8961-9A39E6055E9E',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('B4809BF8-2D52-4D50-8224-7B03D188FE96','Announcement List Page','Index','Announcement','Teacher','true','2014-12-24 08:04:55.210','2014-12-24 08:04:55.210','F06CAB40-A098-4324-8961-9A39E6055E9E','F06CAB40-A098-4324-8961-9A39E6055E9E',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('40B2B18A-C35A-4C96-8253-E9D051D5B7F4','Teacher List Page','Index','Teacher','Teacher','true','2014-12-24 08:05:39.957','2014-12-24 08:05:39.957','9D534620-365D-4D4A-82FB-B511A7D21502','9D534620-365D-4D4A-82FB-B511A7D21502',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('A49855B5-02CF-4FB4-B9E8-5B76837EA58C','School List Page','Index','School','Admin','true','2014-12-24 08:06:16.153','2014-12-24 08:06:16.153','2E036B87-3C67-4651-9980-6A4FCE8A1363','2E036B87-3C67-4651-9980-6A4FCE8A1363',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('3DEA12C7-EED3-464D-8627-D35DB7298E33','Student List Page','Index','Student','Teacher','true','2014-12-24 08:19:53.877','2014-12-24 08:19:53.877','9D534620-365D-4D4A-82FB-B511A7D21502','9D534620-365D-4D4A-82FB-B511A7D21502',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('8AAE5115-16C6-472B-A754-21C24AAF2D81','Teacher Class List Page','Index','TeacherClass','Teacher','true','2014-12-24 08:20:56.183','2014-12-24 08:20:56.183','9D534620-365D-4D4A-82FB-B511A7D21502','9D534620-365D-4D4A-82FB-B511A7D21502',1,NULL)
		insert into [dbo].[Pages]([Id],[Name],[ActionName],[ControlName],[Area],showpage,[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy],[ActiveFlag],[Description]) values('610668DE-0558-4B75-932A-4061E8BC25BD','Book List Page','Index','Book','','true','2016-04-28 09:33:32.860','2016-04-28 09:33:32.860','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',1,'')
		
end
