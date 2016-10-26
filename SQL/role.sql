use EasyProduct
go

drop table dbo.__MigrationHistory
drop table dbo.UserClaim
drop table dbo.UserLogin
drop table dbo.UserRole
drop table dbo.[Role]
drop table dbo.[Users]


insert into [Role] values(newid(),'System')
insert into [Role] values(newid(),'SystemAdmin')
insert into [Role] values(newid(),'SystemMember')


select NEWID()

  Update [test].[dbo].[Resource] set Value='Area'
  where Id='C65A93CC-65E4-4AB1-8FF1-F82B8BF0A6E8'

  select * from [test].[dbo].[Resource]

  insert into [test].[dbo].[Resource] values(NEWID(),'UI_LangArea','Area','en-us','String','UI','2016-01-26 05:23:11.483','2016-01-27 02:59:08.773','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
  insert into [test].[dbo].[Resource] values(NEWID(),'UI_LangArea','Area','zh-Hans','String','UI','2016-01-26 05:23:11.483','2016-01-27 02:59:08.773','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')

  insert into [test].[dbo].[Resource] values(NEWID(),'UI_Edit','Edit','en-us','String','UI','2016-01-26 05:23:11.483','2016-01-27 02:59:08.773','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
  insert into [test].[dbo].[Resource] values(NEWID(),'UI_Edit','Edit','zh-Hans','String','UI','2016-01-26 05:23:11.483','2016-01-27 02:59:08.773','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')

  insert into [test].[dbo].[Resource] values(NEWID(),'UI_Delete','Delete','en-us','String','UI','2016-01-26 05:23:11.483','2016-01-27 02:59:08.773','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
  insert into [test].[dbo].[Resource] values(NEWID(),'UI_Delete','Delete','zh-Hans','String','UI','2016-01-26 05:23:11.483','2016-01-27 02:59:08.773','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000')
