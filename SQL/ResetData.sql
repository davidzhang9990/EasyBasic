
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
select  [Id],[Name],[Value],[Culture],[Type],[Group],[DateAdded],[DateUpdated],[AddedBy],[UpdatedBy] FROM [dbo].[Resource]
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

		Insert Into [dbo].[Resource] values('4890A90B-71CA-4012-84BE-014AB46104AE','UI_Add_Pages','添加页面','zh-CN','String','UI','10 26 2016 10:38:58:937AM','10 26 2016 10:38:58:937AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('965C420A-A388-4B6D-8AB4-01E051605A0F','UI_Pages_Manage','Pages Manage','en-US','String','UI','10 26 2016 10:37:55:183AM','10 26 2016 10:37:55:183AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('7497BF7B-BC99-4E27-A547-0525FF131696','UI_Resource_Manage','资源管理','zh-CN','String','UI','10 25 2016 12:14:36:667PM','10 25 2016 12:14:36:667PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('918E9AF9-7B45-4984-B68C-0885AFABE031','UI_Canel','重置','zh-CN','String','UI','10 25 2016 12:50:33:427PM','10 25 2016 12:50:33:427PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('40F6BBDB-F41F-438F-9E1B-160FB070A283','UI_Edit_Pages','修改页面','zh-CN','String','UI','10 26 2016  1:40:13:943PM','10 26 2016  1:40:13:943PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('D6F16594-BF61-4F66-A9CC-18DCC7CDA4D5','UI_Delete','Delete','en-US','String','UI','10 25 2016 10:51:00:567AM','10 25 2016 10:51:00:567AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('33A212A2-9A12-406F-BA07-1A0D023EAEBD','UI_Resource_Value','资源值','zh-CN','String','UI','10 25 2016 12:42:34:793PM','10 25 2016 12:42:34:793PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('36DED4B9-46CB-4B12-ACF6-1C3C6FB067FD','UI_LogOut','注销','zh-CN','String','UI','10 25 2016  1:10:22:603PM','10 25 2016  1:10:22:603PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('E6E0F4F8-2DA3-4232-AF6B-1C97400583C4','UI_ActionName','ActionName','en-US','String','UI','10 26 2016 11:08:57:180AM','10 26 2016 11:08:57:180AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('943413E6-578D-45DD-B29D-1D5CF34DD542','UI_Resource_Value','Resource Value','en-US','String','UI','10 25 2016 12:42:20:237PM','10 25 2016 12:42:20:237PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('663FE75E-858F-476F-8432-1EBAC888BF59','UI_UpdatedTime','修改时间','zh-CN','String','UI','10 25 2016 12:10:52:573PM','10 25 2016 12:38:15:697PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('E5AF09DE-D9F2-4983-BD49-1EFE5085364C','UI_Edit','修改','zh-CN','String','UI','10 25 2016 10:50:22:867AM','10 25 2016 10:50:22:867AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('62E5F4A2-CEA8-4EA7-B25B-30DC498E9129','UI_Import','Import','en-US','String','UI','10 25 2016  1:20:14:000PM','10 25 2016  1:20:14:000PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('CE70E61A-E2BB-4993-B1AA-3FD27F89D7CC','UI_Resource_Group','Group','en-US','String','UI','10 25 2016 12:47:38:607PM','10 25 2016 12:47:38:607PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('3FEC178D-48EF-4D25-BC31-3FDEF414EC9E','UI_ControlName','ControlName','en-US','String','UI','10 26 2016 11:09:34:510AM','10 26 2016 11:09:34:510AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('789F7460-3778-4017-AD79-486B7C07CD23','UI_Culture','语言','zh-CN','String','UI','10 25 2016 12:43:37:307PM','10 25 2016 12:43:37:307PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('E7477B54-D116-42AD-B5A7-4BBAAA39CA6C','UI_Export','Export','en-US','String','UI','10 25 2016  1:20:43:783PM','10 25 2016  1:20:43:783PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('7ADC1BC2-2A95-4A57-9024-504D65CE38D2','UI_Canel','Canel','en-US','String','UI','10 25 2016 12:50:15:767PM','10 25 2016 12:50:15:767PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('2C7FC7B8-16BA-4870-AB9F-51F1EBA0D64B','UI_Account','账户','zh-CN','String','UI','10 25 2016  1:12:30:393PM','10 25 2016  1:12:30:393PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('998376FC-126F-4BAF-BEA3-541390F89C51','UI_Resource_Manage','Resource Manage','en-US','String','UI','10 25 2016 12:14:14:537PM','10 25 2016 12:14:14:537PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('ACC28B6D-CF99-4A3E-8131-6906C65C5899','UI_Account','My Account','en-US','String','UI','10 25 2016  1:12:18:587PM','10 25 2016  1:12:18:587PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('E56AF120-C4F9-415D-A1CC-6FCE7D78EB53','UI_Edit_Resource','Edit Resource','en-US','String','UI','10 25 2016 12:30:25:837PM','10 25 2016 12:30:25:837PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('C5814C66-4A93-4776-A665-700614EAD932','UI_Home','首页','zh-CN','String','UI','10 25 2016 12:21:39:223PM','10 25 2016 12:21:39:223PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('937E5143-0519-4992-8C1E-74B910B262D4','UI_Import','导入','zh-CN','String','UI','10 25 2016  1:20:24:540PM','10 25 2016  1:20:24:540PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('2E4F09E2-2F9A-438C-B896-77CC4E914EEA','UI_ShowPage','ShowPage','en-US','String','UI','10 26 2016 11:10:54:167AM','10 26 2016 11:10:54:167AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('F3DBB4E9-1606-4497-80D7-781D64C45844','UI_Settings','设置','zh-CN','String','UI','10 25 2016  1:08:56:103PM','10 25 2016  1:08:56:103PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('5A48DF01-4324-4E4F-B030-7B2BC24DF68B','UI_Resource_Name','Resource Name','en-US','String','UI','10 25 2016 12:41:09:087PM','10 25 2016 12:41:09:087PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('59EB885C-830D-4518-9BFC-7B8E810C55BC','UI_Save','Save','en-US','String','UI','10 25 2016 12:49:38:107PM','10 25 2016 12:49:38:107PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('1D5F1819-D775-458D-A6A8-7F51D54BC4F4','UI_LogOut','Log Out','en-US','String','UI','10 25 2016  1:10:08:167PM','10 25 2016  1:10:08:167PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('5D92CA5A-F7A6-481E-99F9-84D5461A4182','UI_Edit','Edit','en-US','String','UI','10 25 2016 10:50:04:443AM','10 25 2016 10:50:04:443AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('EA2D421D-615D-47EE-80A3-85768147BA3E','UI_ControlName','控制器','zh-CN','String','UI','10 26 2016 11:09:50:367AM','10 26 2016 11:09:50:367AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('731A9420-C001-4520-BA52-8989C8C47096','UI_SiteTitle','你的','zh-CN','String','UI','10 25 2016  1:15:07:357PM','10 25 2016  1:15:07:357PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('F45F9D67-B9F4-431B-A8D2-8A6409AC1783','UI_ShowPage','是否页面','zh-CN','String','UI','10 26 2016 11:11:09:527AM','10 26 2016 11:11:09:527AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('5B715A8B-6D17-4C7F-8D1E-8ABEB29B80E4','UI_Resource_Group','类型','zh-CN','String','UI','10 25 2016 12:47:55:823PM','10 25 2016 12:47:55:823PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('A74323DB-01E1-4832-BFC0-8C7DBF6F9E4A','UI_Delete','删除','zh-CN','String','UI','10 25 2016 10:51:13:267AM','10 25 2016 10:51:13:267AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('BED41F87-FA5D-44B7-A576-8CB9C7E8959F','UI_SiteName','Company','en-US','String','UI','10 25 2016  1:15:28:410PM','10 25 2016  1:15:28:410PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('BFF8E124-FECA-495E-803E-9A86D596F692','UI_Area','区域','zh-CN','String','UI','10 26 2016 11:10:32:327AM','10 26 2016 11:10:32:327AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('43A1EADB-B3EA-4569-9AB7-9BCDB4CCB011','UI_SiteName','公司','zh-CN','String','UI','10 25 2016  1:15:37:007PM','10 25 2016  1:15:37:007PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('AC8CF4AD-D469-465B-9191-9FCA284CF043','UI_UpdatedTime','UpdatedTime','en-US','String','UI','10 25 2016 12:10:38:107PM','10 25 2016 12:10:38:107PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('A9720FD7-FDFA-4F74-B329-AB214DAF2017','UI_Pages_Name','页面名称','zh-CN','String','UI','10 26 2016 11:08:13:750AM','10 26 2016 11:08:13:750AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('46CF0B60-E183-455C-9775-B021865704E4','UI_Export','导出','zh-CN','String','UI','10 25 2016  1:20:57:020PM','10 25 2016  1:20:57:020PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('B0191D82-9D2E-4881-80EA-B90D7AD50E69','UI_Add_Resource','Add Resource','en-US','String','UI','10 25 2016 12:19:31:750PM','10 25 2016 12:19:31:750PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('3B491322-A9D8-4DB5-8969-B91F23F58261','UI_Culture','Culture','en-US','String','UI','10 25 2016 12:43:16:103PM','10 25 2016 12:43:16:103PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('41DCE4AC-A245-407C-B96C-BFFE357DF22C','UI_Home','Home','en-US','String','UI','10 25 2016 12:21:30:350PM','10 25 2016 12:21:30:350PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('4EA2A77C-BE5D-461E-9E13-C8A08A26E99F','UI_Resource_Name','资源名称','zh-CN','String','UI','10 25 2016 12:41:53:297PM','10 25 2016 12:41:53:297PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('1181FEDC-F4EA-4D15-8E40-CA7CF27E2047','UI_Pages_Name','Pages Name','en-US','String','UI','10 26 2016 11:08:26:167AM','10 26 2016 11:08:26:167AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('C9D83408-9A32-4C91-9107-CDE75FBE0A9E','UI_Add_Resource','添加资源','zh-CN','String','UI','10 25 2016 12:19:54:430PM','10 25 2016 12:19:54:430PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('D0286E8F-2CF5-42AA-8410-D801B312E4F6','UI_SiteTitle','Your','en-US','String','UI','10 25 2016  1:14:52:597PM','10 25 2016  1:14:52:597PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('A32F3463-D914-459E-976A-DC8A0CA62C68','UI_Save','保存','zh-CN','String','UI','10 25 2016 12:49:49:460PM','10 25 2016 12:49:49:460PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('43D266F9-675C-4785-B660-E33C29846E81','UI_Edit_Pages','Edit Pages','en-US','String','UI','10 26 2016  1:39:59:750PM','10 26 2016  1:39:59:750PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('710E73A8-E8D4-4AB0-8706-E8F058DF09C2','UI_Add_Pages','Add Pages','en-US','String','UI','10 26 2016 10:38:44:520AM','10 26 2016 10:38:44:520AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('76EC1F70-4026-4C59-B84B-EEA46769BA66','UI_Edit_Resource','修改资源','zh-CN','String','UI','10 25 2016 12:30:36:510PM','10 25 2016 12:30:36:510PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('761305F5-4BE3-4C7F-8341-EFE3B7C6725A','UI_Pages_Manage','页面管理','zh-CN','String','UI','10 26 2016 10:38:17:983AM','10 26 2016 10:38:17:983AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('6F6CB10D-7C23-482A-8C38-F59228444AFE','UI_Area','Area','en-US','String','UI','10 26 2016 11:10:19:510AM','10 26 2016 11:10:19:510AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('01B87ED6-9D65-409E-8878-F7EAB516CDCE','11','11','en-US','String','en-us','10 25 2016 10:46:44:507AM','10 25 2016 10:46:44:507AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('4A54A74B-CD43-403D-8D18-F8EFCF05C001','UI_ActionName','方法名','zh-CN','String','UI','10 26 2016 11:09:11:680AM','10 26 2016 11:09:11:680AM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
		Insert Into [dbo].[Resource] values('8F7DC4E7-8CC3-42E4-88C1-FB5A4B5EF805','UI_Settings','Settings','en-US','String','UI','10 25 2016  1:08:46:913PM','10 25 2016  1:08:46:913PM','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')

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
