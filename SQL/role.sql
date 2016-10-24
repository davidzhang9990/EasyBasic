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