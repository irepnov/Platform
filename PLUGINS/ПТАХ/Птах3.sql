go
drop table rfcPriceCategory
go

CREATE TABLE [dbo].[rfcPriceCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	PriceMin decimal(18,2) NULL,
	PriceMax decimal(18,2) NULL,
 CONSTRAINT [PK_rfcPriceCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

go


insert into rfcPriceCategory(Name, PriceMin, PriceMax)values('до 1 т.р.', -1000, 999.99)
insert into rfcPriceCategory(Name, PriceMin, PriceMax)values('1-2 т.р.', 1000, 1999.99)
insert into rfcPriceCategory(Name, PriceMin, PriceMax)values('2-5 т.р.', 2000, 4999.99)
insert into rfcPriceCategory(Name, PriceMin, PriceMax)values('5-7 т.р.', 5000, 6999.99)
insert into rfcPriceCategory(Name, PriceMin, PriceMax)values('7-10 т.р.', 7000, 9999.99)
insert into rfcPriceCategory(Name, PriceMin, PriceMax)values('10-15 т.р.', 10000, 14999.99)
insert into rfcPriceCategory(Name, PriceMin, PriceMax)values('15-20 т.р.', 15000, 19999.99)
insert into rfcPriceCategory(Name, PriceMin, PriceMax)values('20-30 т.р.', 20000, 29999.99)
insert into rfcPriceCategory(Name, PriceMin, PriceMax)values('от 30 т.р.', 30000, 150000)

go




go
drop table rfcPostav
go

CREATE TABLE [dbo].[rfcPostav](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	isActive [bit] NULL,
 CONSTRAINT [PK_rfcPostav] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

go



drop table uchPostavNomen
go

CREATE TABLE [dbo].[uchPostavNomen](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
    
    RodnNomenRef int null,

	PostavRef int NULL,
	PricePostav decimal(18,2) NULL,
	DatePrice smalldatetime null,

	CreateDate smalldatetime null,
	CreateUser nvarchar(50) null,
	UpdateDate smalldatetime null,
	UpdateUser nvarchar(50) null,
 CONSTRAINT [PK_uchPostavNomen] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


go


create trigger [dbo].[trg_ADU_uchPostavNomen] on [dbo].uchPostavNomenfor insert, update AS

if not exists(select top 1 * from deleted) 
  begin
		update mt 
		set mt.CreateDate = GetDate(), mt.CreateUser = suser_sname()
		from uchPostavNomen mt 
		  inner join inserted ins on (ins.Id = mt.Id)
  end 
else
  begin
		if EXISTS(select top 1 * 
                	  from inserted ins
			    inner join deleted del on (ins.Id = del.Id)
			  where ins.UpdateDate < del.UpdateDate )   
      		  begin 
			if @@trancount > 0 rollback tran
			raiserror ('«апись в таблице uchPostavNomen изменена другим пользователем. ќбновите данные.', 16, 10)
		  end
		else 
		  begin
			update mt 
			set mt.UpdateDate = getdate(), mt.UpdateUser  = suser_sname() 
			from uchPostavNomen mt 
			  inner join inserted ins on (ins.Id = mt.Id)
		  end
  end
  
go
drop table tmpPostavNomen
go

CREATE TABLE [dbo].tmpPostavNomen(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	
	RodnName [nvarchar](200) NULL,
    
    RodnNomenRef int null,
    DatePrice smalldatetime null,

	PostavRef int NULL,
	PricePostav decimal(18,2) NULL,

 CONSTRAINT [PK_tmpPostavNomen] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

go




drop procedure uchPostavNomenc
go

create procedure uchPostavNomenc
	@IDOperation int,
	@ID int = null, 
	@Name nvarchar(200)= null, 
	@RodnName nvarchar(200)= null, 
	@RodnNomenRef int = null,
	
	@DatePrice smalldatetime = null,
		
	@PostavRef int = NULL,
	@PricePostav decimal(18,2) = NULL
as
begin
  set dateformat dmy
  
  if (@IDOperation = 1)
  begin
	INSERT INTO tmpPostavNomen
           (Name, RodnName, RodnNomenRef, DatePrice, PostavRef, PricePostav)
    values(@Name, @RodnName, @RodnNomenRef, @DatePrice, @PostavRef, @PricePostav)        
  end
  
  if (@IDOperation = 2)
  begin    
    update t  ---проставлю новые прив€зки
    set RodnNomenRef = u.id
    from tmpPostavNomen t
     inner join uchRodnNomen u on REPLACE(replace(t.RodnName, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
    where u.Name is not null
      and t.RodnName is not null
      and t.RodnNomenRef is null      
      and t.PostavRef = @PostavRef
      
    update t ---перенесу прошлые прив€зки 
    set RodnNomenRef = u.RodnNomenRef
    from tmpPostavNomen t
     inner join uchPostavNomen u on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
                                and t.PostavRef = u.PostavRef
    where u.RodnNomenRef is not null --ранее прив€зан
      and t.RodnName is not null --указано не правильное им€ эталонной номенкалутры
      and t.RodnNomenRef is null
      and t.PostavRef = @PostavRef    
      
    update t ---перенесу прошлые прив€зки 
    set RodnNomenRef = u.RodnNomenRef
    from tmpPostavNomen t
     inner join uchPostavNomen u on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
								and t.PostavRef = u.PostavRef
    where u.RodnNomenRef is not null --ранее прив€зан
      and t.RodnName is null  --вообзще не указано им€ эталонное
      and t.RodnNomenRef is null  
      and t.PostavRef = @PostavRef  
      
    update t
    set RodnName = e.name
    from tmpPostavNomen t
     inner join uchRodnNomen e on e.id = t.RodnNomenRef
    where t.RodnNomenRef is not null 
      and t.PostavRef = @PostavRef           
  end  
  
  if (@IDOperation = 3)
  begin
	delete from tmpPostavNomen      
  end  
  
  if (@IDOperation = 4)
  begin     
    select id, RodnName, Name
    from tmpPostavNomen
    where RodnNomenRef is null
      and Name is not null
      and PostavRef = @PostavRef
    order by Name 
  end 
    
  if (@IDOperation = 5)
  begin         
	update uchPostavNomen
	set Name = t.Name,
	    PricePostav = t.PricePostav,
	    DatePrice = t.DatePrice,
	    RodnNomenRef = t.RodnNomenRef,
	    PostavRef = t.PostavRef
	from uchPostavNomen r
	  inner join tmpPostavNomen t on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(r.Name, '-', ''), ' ', '')  
	                           and t.PostavRef = r.PostavRef 
	where t.RodnNomenRef is not null
	  and t.PostavRef = @PostavRef
	
	insert into uchPostavNomen(Name, PricePostav, DatePrice, RodnNomenRef, PostavRef)
	select t.Name, t.PricePostav, t.DatePrice, t.RodnNomenRef, t.PostavRef
	from tmpPostavNomen t
	  left join uchPostavNomen r on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(r.Name, '-', ''), ' ', '')  
	                            and t.PostavRef = r.PostavRef
	where t.RodnNomenRef is not null
	  and r.ID is null   
	  and t.PostavRef = @PostavRef                         
  end  
  
  if (@IDOperation = 6)  
  begin
	select distinct /*u.id,*/ u.name  ---отберу ту этулаонную номенклатуру котора€ содержит эти названи€
	from dbo.uchRodnNomen u
	inner join 
	(     
		select distinct v ---разберу наименование модели на слова
		from (
			  select cast('<r><c>'+replace(name, ' ', '</c><c>')+'</c></r>' as xml) s from tmpPostavNomen where RodnNomenRef is null and PostavRef = @PostavRef
			  ) t
			cross apply (select x.z.value('.', 'nvarchar(100)') v from s.nodes('/r/c') x(z)) tt 
	) f on u.name like '% ' + f.v + ''
		or u.name like '' + f.v + ' %'
		or u.name like '% ' + f.v + ' %'
  end
  
  if (@IDOperation = 7)
  begin
	update dbo.uchPostavNomen
	set Name = @Name,
	    PricePostav = @PricePostav,
		PostavRef = @PostavRef,
	    RodnNomenRef = @RodnNomenRef
	from uchPostavNomen r
	where r.ID = @ID
  end
end










go



drop table uchRodnNomen
go

CREATE TABLE [dbo].[uchRodnNomen](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	SebestFact decimal(18,2) NULL,
	RoznPrice decimal(18,2) NULL,
	
	[isAssort] bit default(0),
		
    CountOrder int null,
    
    EtalonNomenRef int null,

	PostavRef int NULL,
	PricePostav decimal(18,2) NULL,
	NacenkaPrice decimal(18,2) NULL,
	NacenkaProc decimal(18,2) NULL,
	PriceCategory nvarchar(10) NULL,

	CreateDate smalldatetime null,
	CreateUser nvarchar(50) null,
	UpdateDate smalldatetime null,
	UpdateUser nvarchar(50) null,
 CONSTRAINT [PK_uchRodnNomen] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


go


create trigger [dbo].[trg_ADU_uchRodnNomen] on [dbo].uchRodnNomenfor insert, update AS

if not exists(select top 1 * from deleted) 
  begin
		update mt 
		set mt.CreateDate = GetDate(), mt.CreateUser = suser_sname()
		from uchRodnNomen mt 
		  inner join inserted ins on (ins.Id = mt.Id)
  end 
else
  begin
		if EXISTS(select top 1 * 
                	  from inserted ins
			    inner join deleted del on (ins.Id = del.Id)
			  where ins.UpdateDate < del.UpdateDate )   
      		  begin 
			if @@trancount > 0 rollback tran
			raiserror ('«апись в таблице uchRodnNomen изменена другим пользователем. ќбновите данные.', 16, 10)
		  end
		else 
		  begin
			update mt 
			set mt.UpdateDate = getdate(), mt.UpdateUser  = suser_sname() 
			from uchRodnNomen mt 
			  inner join inserted ins on (ins.Id = mt.Id)
		  end
  end
  
go
drop table tmpRodnNomen
go

CREATE TABLE [dbo].tmpRodnNomen(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	SebestFact decimal(18,2) NULL,
	RoznPrice decimal(18,2) NULL,
	
	[isAssort] bit default(0),
		
    CountOrder int null,
    
    EtalonName [nvarchar](200) NULL,
    EtalonNomenRef int null,

	PostavRef int NULL,
	PricePostav decimal(18,2) NULL,
	NacenkaPrice decimal(18,2) NULL,
	NacenkaProc decimal(18,2) NULL,
	PriceCategory nvarchar(10) NULL,

	--CreateDate smalldatetime null,
	--CreateUser nvarchar(50) null,
	--UpdateDate smalldatetime null,
	--UpdateUser nvarchar(50) null,
 CONSTRAINT [PK_tmpRodnNomen] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


go


drop procedure uchRodnNomenc
go

create procedure uchRodnNomenc
	@IDOperation int,
	@ID int = null, 
	@Name nvarchar(200)= null, 
	@EtalonName nvarchar(200)= null, 
	@EtalonNomenRef int = null,
	@isAssort bit = 0, 
	@SebestFact decimal(18,2) = NULL,
	@RoznPrice decimal(18,2) = NULL,
	@CountOrder int = null,
		
	@PostavRef int = NULL,
	@PricePostav decimal(18,2) = NULL,

	@NacenkaPrice decimal(18,2) = NULL,
	@NacenkaProc decimal(18,2) = NULL,
	@PriceCategory nvarchar(10) = NULL
as
begin
  set dateformat dmy
  
  if (@IDOperation = 1)
  begin
	INSERT INTO tmpRodnNomen
           (Name, SebestFact, RoznPrice, isAssort, CountOrder, EtalonName, EtalonNomenRef, 
           PostavRef, PricePostav, NacenkaPrice, NacenkaProc, PriceCategory)
    values(@Name, @SebestFact, @RoznPrice, @isAssort, @CountOrder, @EtalonName, @EtalonNomenRef, 
           @PostavRef, @PricePostav, @NacenkaPrice, @NacenkaProc, @PriceCategory)        
  end
  
  if (@IDOperation = 2)
  begin    
    update t  ---проставлю новые прив€зки
    set EtalonNomenRef = u.id
    from tmpRodnNomen t
     inner join uchEtalonNomen u on REPLACE(replace(t.EtalonName, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
    where u.Name is not null
      and t.EtalonName is not null
      and t.EtalonNomenRef is null
      
    update t ---перенесу прошлые прив€зки 
    set EtalonNomenRef = u.EtalonNomenRef
    from tmpRodnNomen t
     inner join uchRodnNomen u on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
    where u.EtalonNomenRef is not null --ранее прив€зан
      and t.EtalonName is not null --указано не правильное им€ эталонной номенкалутры
      and t.EtalonNomenRef is null    
      
    update t ---перенесу прошлые прив€зки 
    set EtalonNomenRef = u.EtalonNomenRef
    from tmpRodnNomen t
     inner join uchRodnNomen u on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
    where u.EtalonNomenRef is not null --ранее прив€зан
      and t.EtalonName is null  --вообзще не указано им€ эталонное
      and t.EtalonNomenRef is null    
      
    update t
    set EtalonName = e.name
    from tmpRodnNomen t
     inner join uchEtalonNomen e on e.id = t.EtalonNomenRef
    where t.EtalonNomenRef is not null            
  end  
  
  if (@IDOperation = 3)
  begin
	delete from tmpRodnNomen      
  end  
  
  if (@IDOperation = 4)
  begin     
    select id, EtalonName, Name
    from tmpRodnNomen
    where EtalonNomenRef is null
      and Name is not null
    order by Name 
  end 
    
  if (@IDOperation = 5)
  begin   
    update dbo.uchRodnNomen
	set SebestFact = null,
	    RoznPrice = null,
	    isAssort = 0
        
	update dbo.uchRodnNomen
	set Name = t.Name,
	    SebestFact = t.SebestFact,
	    RoznPrice = t.RoznPrice,
	    isAssort = t.isAssort,
	    EtalonNomenRef = t.EtalonNomenRef
	from uchRodnNomen r
	  inner join tmpRodnNomen t on t.EtalonNomenRef = r.EtalonNomenRef 
	                           and REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(r.Name, '-', ''), ' ', '')   
	where t.EtalonNomenRef is not null
	
	insert into uchRodnNomen(Name, SebestFact, RoznPrice, isAssort, EtalonNomenRef)
	select t.Name, t.SebestFact, t.RoznPrice, t.isAssort, t.EtalonNomenRef
	from tmpRodnNomen t
	  left join uchRodnNomen r on t.EtalonNomenRef = r.EtalonNomenRef 
	                          and REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(r.Name, '-', ''), ' ', '')
	where t.EtalonNomenRef is not null
	  and r.ID is null                            
  end  
  
  if (@IDOperation = 6)  
  begin
	select distinct /*u.id,*/ u.name  ---отберу ту этулаонную номенклатуру котора€ содержит эти названи€
	from dbo.uchEtalonNomen u
	inner join 
	(     
		select distinct v ---разберу наименование модели на слова
		from (
			  select cast('<r><c>'+replace(name, ' ', '</c><c>')+'</c></r>' as xml) s from tmpRodnNomen where EtalonNomenRef is null
			  ) t
			cross apply (select x.z.value('.', 'nvarchar(100)') v from s.nodes('/r/c') x(z)) tt 
	) f on u.name like '% ' + f.v + ''
		or u.name like '' + f.v + ' %'
		or u.name like '% ' + f.v + ' %'
  end
  
  if (@IDOperation = 7)
  begin
	update dbo.uchRodnNomen
	set Name = @Name,
	    SebestFact = @SebestFact,
	    RoznPrice = @RoznPrice,
	    isAssort = @isAssort,
	    EtalonNomenRef = @EtalonNomenRef,
	    CountOrder = @CountOrder
	from uchRodnNomen r
	where r.ID = @ID
  end
  
  if (@IDOperation = 8)
  begin
	update dbo.uchRodnNomen
	set CountOrder = null
	from uchRodnNomen r
  end  
  
  if (@IDOperation = 9)
  begin
	update dbo.uchRodnNomen
	set PostavRef = null,
		PricePostav = null,
		NacenkaPrice = null,
		NacenkaProc = null,
		PriceCategory = null

	select p.RodnNomenRef, p.PostavRef, p.PricePostav
	into #minPrice
	from uchPostavNomen p
	 inner join (
				 select RodnNomenRef, MIN(PricePostav) as mPricePostav		--минимальна€ цена		 
				 from uchPostavNomen
				 where PostavRef in (select id from rfcPostav where isnull(isActive,0) = 1)  --среди актуальных поставщиков
				 group by RodnNomenRef
				 ) mp on mp.RodnNomenRef = p.RodnNomenRef
					 and mp.mPricePostav = p.PricePostav

	update r
	set PostavRef = m.PostavRef,--поставщик с мин. ценой
		PricePostav = m.PricePostav --минимальна€ цена
	from uchRodnNomen r
	 inner join (
				select RodnNomenRef, min(PostavRef) as mPostavRef
				from #minPrice
				group by RodnNomenRef
				) mp on r.id = mp.RodnNomenRef
	 inner join #minPrice m on m.RodnNomenRef = r.ID
	                       and m.PostavRef = mp.mPostavRef

	update dbo.uchRodnNomen
	set NacenkaPrice = RoznPrice - PricePostav,
		NacenkaProc = case when isnull(PricePostav,0) > 0 then (RoznPrice / PricePostav) - 1 else 0 end
	where PricePostav is not null

	update dbo.uchRodnNomen
	set PriceCategory = pp.name
	from uchRodnNomen r
	 inner join rfcPriceCategory pp on r.RoznPrice between pp.PriceMin and pp.PriceMax
  end  
  
  if (@IDOperation = 10)
  begin
	select pos.name as PostavName, p.name as PostavNomName, r.Name, r.CountOrder, r.PricePostav, r.RoznPrice
	from dbo.uchRodnNomen r
	 inner join rfcPostav pos on pos.ID = r.PostavRef
	 inner join uchPostavNomen p on p.RodnNomenRef = r.ID and p.PostavRef = r.PostavRef
	where isnull(r.CountOrder,0) > 0
	order by pos.name, p.name, r.Name, r.CountOrder
  end    
end




go
drop table uchEtalonNomen
go

CREATE TABLE [dbo].[uchEtalonNomen](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Proizv] [nvarchar](100) NULL,
	[Name] [nvarchar](200) NULL,
	[isActive] bit default(0),
	[TypeP] [nvarchar](50) NULL,
	[OS] [nvarchar](50) NULL,
	[DisplayType] [nvarchar](50) NULL,
	[DisplaySize] [nvarchar](50) NULL,
	[SDCARD] [nvarchar](50) NULL,
	[CameraBasic] nvarchar(50) NULL,
	[CameraSecond] nvarchar(50) NULL,
	[ROM] [nvarchar](20) NULL,
	[CPU] [nvarchar](30) NULL,
	[RAM] [nvarchar](20) NULL,
	[mp3_rad] [nvarchar](20) NULL,
	[java_mms] [nvarchar](20) NULL,
	[bluth_wifi] [nvarchar](20) NULL,
	[stand] [nvarchar](50) NULL,
	[navi] [nvarchar](50) NULL,
	[akum] int NULL,
	[rrc] int NULL,
	
	PostavRef int NULL,
	PricePostav decimal(18,2) NULL,
	SebestFact decimal(18,2) NULL,
	RoznPrice decimal(18,2) NULL,
	NacenkaPrice decimal(18,2) NULL,
	NacenkaProc decimal(18,2) NULL,
	ErrorRozn bit null,
	NewRozn decimal(18,2) NULL,
	PriceCategory nvarchar(10) NULL,
	
	con1 decimal(18,2) NULL,
	con2 decimal(18,2) NULL,
	con3 decimal(18,2) NULL,
	con4 decimal(18,2) NULL,
	con5 decimal(18,2) NULL,
	con6 decimal(18,2) NULL,
	con7 decimal(18,2) NULL,
	con8 decimal(18,2) NULL,
	con9 decimal(18,2) NULL,
	con10 decimal(18,2) NULL,
	
	
	CreateDate smalldatetime null,
	CreateUser nvarchar(50) null,
	UpdateDate smalldatetime null,
	UpdateUser nvarchar(50) null,
 CONSTRAINT [PK_uchEtalonNomen] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


go


create trigger [dbo].[trg_ADU_uchEtalonNom] on [dbo].uchEtalonNomenfor insert, update AS

if not exists(select top 1 * from deleted) 
  begin
		update mt 
		set mt.CreateDate = GetDate(), mt.CreateUser = suser_sname()
		from uchEtalonNomen mt 
		  inner join inserted ins on (ins.Id = mt.Id)
  end 
else
  begin
		if EXISTS(select top 1 * 
                	  from inserted ins
			    inner join deleted del on (ins.Id = del.Id)
			  where ins.UpdateDate < del.UpdateDate )   
      		  begin 
			if @@trancount > 0 rollback tran
			raiserror ('«апись в таблице uchEtalonNomen изменена другим пользователем. ќбновите данные.', 16, 10)
		  end
		else 
		  begin
			update mt 
			set mt.UpdateDate = getdate(), mt.UpdateUser  = suser_sname() 
			from uchEtalonNomen mt 
			  inner join inserted ins on (ins.Id = mt.Id)
		  end
  end
  
go
drop table tmpEtalonNomen
go

CREATE TABLE [dbo].[tmpEtalonNomen](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Proizv] [nvarchar](100) NULL,
	[Name] [nvarchar](200) NULL,
	[isActive] bit default(0),
	[TypeP] [nvarchar](50) NULL,
	[OS] [nvarchar](50) NULL,
	[DisplayType] [nvarchar](50) NULL,
	[DisplaySize] [nvarchar](50) NULL,
	[SDCARD] [nvarchar](50) NULL,
	[CameraBasic] nvarchar(50) NULL,
	[CameraSecond] nvarchar(50) NULL,
	[ROM] [nvarchar](20) NULL,
	[CPU] [nvarchar](30) NULL,
	[RAM] [nvarchar](20) NULL,
	[mp3_rad] [nvarchar](20) NULL,
	[java_mms] [nvarchar](20) NULL,
	[bluth_wifi] [nvarchar](20) NULL,
	[stand] [nvarchar](50) NULL,
	[navi] [nvarchar](50) NULL,
	[akum] int NULL,
	[rrc] int NULL,
	
	PostavRef int NULL,
	PricePostav decimal(18,2) NULL,
	SebestFact decimal(18,2) NULL,
	RoznPrice decimal(18,2) NULL,
	NacenkaPrice decimal(18,2) NULL,
	NacenkaProc decimal(18,2) NULL,
	ErrorRozn bit null,
	NewRozn decimal(18,2) NULL,
	PriceCategory nvarchar(10) NULL,
	
	con1 decimal(18,2) NULL,
	con2 decimal(18,2) NULL,
	con3 decimal(18,2) NULL,
	con4 decimal(18,2) NULL,
	con5 decimal(18,2) NULL,
	con6 decimal(18,2) NULL,
	con7 decimal(18,2) NULL,
	con8 decimal(18,2) NULL,
	con9 decimal(18,2) NULL,
	con10 decimal(18,2) NULL,	
 CONSTRAINT [PK_tmpEtalonNomen] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


go

drop procedure uchEtalonNomenc
go

create procedure uchEtalonNomenc
	@IDOperation int,
	@ID int = null, 
	@Proizv nvarchar(100)= null, 
	@Name nvarchar(200)= null, 
	@isActive bit = 0, 
	@TypeP nvarchar(50)= null, 
	@OS nvarchar(50)= null, 
	@DisplayType nvarchar(50)= null, 
	@DisplaySize nvarchar(50)= null, 
	@SDCARD nvarchar(50)= null, 
	@CameraBasic nvarchar(50)= null, 
	@CameraSecond nvarchar(50)= null, 
	@ROM nvarchar(20)= null, 
	@CPU nvarchar(30)= null, 
	@RAM nvarchar(20)= null, 
	@mp3_rad nvarchar(20)= null, 
	@java_mms nvarchar(20)= null, 
	@bluth_wifi nvarchar(20)= null, 
	@stand nvarchar(50)= null, 
	@navi nvarchar(50)= null, 
	@akum int = null, 
	@rrc int = null,
	
	@PostavRef int = NULL,
	@PricePostav decimal(18,2) = NULL,
	@SebestFact decimal(18,2) = NULL,
	@RoznPrice decimal(18,2) = NULL,
	@NacenkaPrice decimal(18,2) = NULL,
	@NacenkaProc decimal(18,2) = NULL,
	@ErrorRozn bit = null,
	@NewRozn decimal(18,2) = NULL,
	@PriceCategory nvarchar(10) = NULL,
	
	@con1 decimal(18,2) = NULL,
	@con2 decimal(18,2) = NULL,
	@con3 decimal(18,2) = NULL,
	@con4 decimal(18,2) = NULL,
	@con5 decimal(18,2) = NULL,
	@con6 decimal(18,2) = NULL,
	@con7 decimal(18,2) = NULL,
	@con8 decimal(18,2) = NULL,
	@con9 decimal(18,2) = NULL,
	@con10 decimal(18,2) = NULL
as
begin
  set dateformat dmy
  
  if (@IDOperation = 1)
  begin
	INSERT INTO tmpEtalonNomen
           (Proizv, Name, isActive, TypeP, OS, DisplayType, DisplaySize, SDCARD, 
            CameraBasic, CameraSecond, ROM, CPU, RAM, mp3_rad, java_mms, bluth_wifi, stand, navi, akum, rrc,
            con1,con2,con3,con4,con5,con6,con7,con8,con9,con10)
    values(@Proizv, @Name, @isActive, @TypeP, @OS, @DisplayType, @DisplaySize, @SDCARD, 
            @CameraBasic, @CameraSecond, @ROM, @CPU, @RAM, @mp3_rad, @java_mms, @bluth_wifi, @stand, @navi, @akum, @rrc,
            @con1,@con2,@con3,@con4,@con5,@con6,@con7,@con8,@con9,@con10)        
  end
  
  if (@IDOperation = 2)
  begin  
    insert into uchEtalonNomen
           (Proizv, Name, isActive, TypeP, OS, DisplayType, DisplaySize, SDCARD, 
           CameraBasic, CameraSecond, ROM, CPU, RAM, mp3_rad, java_mms, bluth_wifi, stand, navi, akum, rrc)
    select 
           t.Proizv, t.Name, t.isActive, t.TypeP, t.OS, t.DisplayType, t.DisplaySize, t.SDCARD, 
            t.CameraBasic, t.CameraSecond, t.ROM, t.CPU, t.RAM, t.mp3_rad, t.java_mms, t.bluth_wifi, t.stand, t.navi, t.akum, t.rrc
    from tmpEtalonNomen t
     left join uchEtalonNomen u on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
    where u.Name is null
      
    
    update uchEtalonNomen 
    set Proizv = case when t.Proizv is not null then t.Proizv  else u.Proizv end, 
        Name = case when t.Name is not null then t.Name  else u.Name end,  
        isActive = case when t.isActive is not null then t.isActive  else u.isActive end,  
        TypeP = case when t.TypeP is not null then t.TypeP  else u.TypeP end, 
        OS = case when t.OS is not null then t.OS  else u.OS end, 
        DisplayType = case when t.DisplayType is not null then t.DisplayType  else u.DisplayType end,
        DisplaySize = case when t.DisplaySize is not null then t.DisplaySize  else u.DisplaySize end, 
        SDCARD = case when t.SDCARD is not null then t.SDCARD  else u.SDCARD end,
        CameraBasic = case when t.CameraBasic is not null then t.CameraBasic  else u.CameraBasic end,
        CameraSecond = case when t.CameraSecond is not null then t.CameraSecond  else u.CameraSecond end, 
        ROM = case when t.ROM is not null then t.ROM  else u.ROM end,
        CPU = case when t.CPU is not null then t.CPU  else u.CPU end, 
        RAM = case when t.RAM is not null then t.RAM  else u.RAM end, 
        mp3_rad = case when t.mp3_rad is not null then t.mp3_rad  else u.mp3_rad end,
        java_mms = case when t.java_mms is not null then t.java_mms  else u.java_mms end,  
        bluth_wifi = case when t.bluth_wifi is not null then t.bluth_wifi  else u.bluth_wifi end,  
        stand = case when t.stand is not null then t.stand  else u.stand end,  
        navi = case when t.navi is not null then t.navi  else u.navi end,  
        akum = case when t.akum is not null then t.akum  else u.akum end,  
        rrc = case when t.rrc is not null then t.rrc  else u.rrc end
     from uchEtalonNomen u
      inner join tmpEtalonNomen t on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')  
  end  
  
  if (@IDOperation = 3)
  begin
	delete from tmpEtalonNomen      
  end  
  
  if (@IDOperation = 4)
  begin         
    update uchEtalonNomen 
    set Proizv = @Proizv, 
        Name = @Name,  
        isActive = @isActive,  
        TypeP = @TypeP, 
        OS = @OS, 
        DisplayType = @DisplayType,
        DisplaySize = @DisplaySize, 
        SDCARD = @SDCARD,
        CameraBasic = @CameraBasic,
        CameraSecond = @CameraSecond, 
        ROM = @ROM,
        CPU = @CPU, 
        RAM = @RAM, 
        mp3_rad = @mp3_rad,
        java_mms = @java_mms,  
        bluth_wifi = @bluth_wifi,  
        stand = @stand,  
        navi = @navi,  
        akum = @akum,  
        rrc = @rrc,
        NewRozn = @NewRozn
     from uchEtalonNomen u
     where u.id = @id
  end   
  
  if (@IDOperation = 5)
  begin
  ---обнуление
    update uchEtalonNomen
    set PostavRef = null,
		PricePostav = null,
		SebestFact = null,
		RoznPrice = null,
		NacenkaPrice = null,
		NacenkaProc = null,
		ErrorRozn = null,
		NewRozn = null,
		PriceCategory = null
    
	---средн€€ цена по поставщику
	select p.PostavRef
		   ,r.EtalonNomenRef
		   ,SUM(p.PricePostav) as _sum, COUNT(p.ID) as _count, AVG(p.PricePostav) as _avg
	into #avgPostav
	from dbo.uchPostavNomen p
	 inner join dbo.uchRodnNomen r on r.ID = p.RodnNomenRef     
	where r.EtalonNomenRef is not null
	  and p.PostavRef in (select id from rfcPostav where isnull(isActive,0) = 1)  --среди актуальных поставщиков
	group by p.PostavRef
			,r.EtalonNomenRef
	order by p.PostavRef

	---поставщик с минимальной средней ценой
	select p.PostavRef 
		  ,p.EtalonNomenRef
		  ,p._avg
	into #minPostav
	from #avgPostav p
	 inner join (
				select EtalonNomenRef, MIN(_avg) as m_avg
				from #avgPostav
				group by EtalonNomenRef
				) m on m.EtalonNomenRef = p.EtalonNomenRef
				   and m.m_avg = p._avg
	order by p.PostavRef 
		  ,p.EtalonNomenRef		

	--проставлю поставщика
	update e
	set PostavRef = m.PostavRef
	   ,PricePostav = m._avg
	from dbo.uchEtalonNomen e
	 inner join (
	            select EtalonNomenRef, min(PostavRef) as mPostavRef
	            from #minPostav
	            group by EtalonNomenRef
	            ) mp on e.id = mp.EtalonNomenRef
	 inner join #minPostav m on m.EtalonNomenRef = e.ID
	                        and m.PostavRef = mp.mPostavRef

	--средн€€ себестоимость
	update e
	set SebestFact = r.s_avg
	from uchEtalonNomen e
		inner join (
					select EtalonNomenRef, AVG(SebestFact) as s_avg
					from dbo.uchRodnNomen
					where EtalonNomenRef is not null
					group by EtalonNomenRef	
					) r on r.EtalonNomenRef = e.ID

	--средн€€ рознича€ цена
	update e
	set ErrorRozn = case when r.c_rp = 1 then 0 else 1 end
	   ,RoznPrice = r.r_avg
	from uchEtalonNomen e
		inner join (
					select EtalonNomenRef, AVG(RoznPrice) as r_avg, COUNT(distinct isnull(RoznPrice, -1)) as c_rp
					from dbo.uchRodnNomen
					where EtalonNomenRef is not null
					group by EtalonNomenRef	
					) r on r.EtalonNomenRef = e.ID  
					
	update dbo.uchEtalonNomen
	set NacenkaPrice = RoznPrice - SebestFact,
		NacenkaProc = case when isnull(SebestFact,0) > 0 then (RoznPrice / SebestFact) - 1 else 0 end
	where RoznPrice is not null

	update dbo.uchEtalonNomen
	set PriceCategory = pp.name
	from uchEtalonNomen r
	 inner join rfcPriceCategory pp on r.RoznPrice between pp.PriceMin and pp.PriceMax	
	 
	update uchEtalonNomen
	set isActive = 1 --признак актальности дл€ родного товара с себестоимостью > 0
	where id in (select EtalonNomenRef from uchRodnNomen where isnull(SebestFact, 0) > 0 and EtalonNomenRef is not null)			
  end
  
  if (@IDOperation = 6)
  begin
	update uchEtalonNomen 
    set con1 = null,con2 = null,
        con3 = null,con4 = null,
        con5 = null,con6 = null,
        con7 = null,con8 = null,
        con9 = null,con10 = null
  
	update uchEtalonNomen 
    set con1 = t.con1, 
        con2 = t.con2, 
        con3 = t.con3, 
        con4 = t.con4, 
        con5 = t.con5, 
        con6 = t.con6, 
        con7 = t.con7, 
        con8 = t.con8, 
        con9 = t.con9, 
        con10 = t.con10
     from uchEtalonNomen u
      inner join tmpEtalonNomen t on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
  end
  
end



go

--select * from uchEtalonNomen
--select * from tmpEtalonNomen
--{0BBE4453-E7D8-4D9C-AF0F-24086633C366}

GO


ALTER procedure [dbo].[uchReference]
                 @IDOperation int,
                 @ID int = null,
                 @Name nvarchar(100) = null,
                 @Comment nvarchar(300) = null,
                 @TypeRepositoryRef int = null,
                 @isMainRepository bit = null,
                 @RepositoryRef int = null,
                 @Fam nvarchar(30) = null, 
                 @Im nvarchar(20) = null, 
                 @Otch nvarchar(30) = null, 
                 @Phone_1 nvarchar(11) = null, 
                 @Phone_2 nvarchar(11) = null, 
                 @Phone_3 nvarchar(11) = null, 
                 @DocSeria nvarchar(5) = null, 
                 @DocNumber nvarchar(8) = null, 
                 @CodePoint nvarchar(100) = null,
                 @URAdres nvarchar(250)=null,
                  @URDom nvarchar(10)=null,
                  @URKorp nvarchar(50)=null,
                  @URKvart nvarchar(10)=null,
                  @URRegion int=null, 
                  @URRaion int=null, 
                  @URGorod int=null, 
                  @URNaspunkt int=null, 
                  @URStreet int=null,
                 @REAdres nvarchar(250)=null,
                  @REDom nvarchar(10)=null,
                  @REKorp nvarchar(50)=null,
                  @REKvart nvarchar(10)=null,
                  @RERegion int=null, 
                  @RERaion int=null, 
                  @REGorod int=null, 
                  @RENaspunkt int=null, 
                  @REStreet int=null,
                 @Analytics nvarchar(50)=null,
                 @LiabilityCuratorRef int = null,
                 @isActive bit = null


as
begin
  set ansi_warnings off
  set nocount on

  declare @NewIDAdresUR int
  declare @NewIDAdresRE int


------------select
  if @IDOperation = 1 
  begin
    select * from rfcTypeProduct order by Name
  end
-----------
  if @IDOperation = 2 
  begin
    select * from rfcProvider order by Name
  end
-----------
  if @IDOperation = 3 
  begin
    select r.*, t.Name as TypeR, isnull(l.fam, '') + ' ' + isnull(l.im, '') + ' ' + isnull(l.otch, '') as FIO 
    from rfcRepository r 
      left join rfcTypeRepository t on t.ID = r.TypeRepositoryRef
      left join rfcLiability l on l.ID = r.LiabilityCuratorRef
    order by t.Name, r.name
  end
-----------
  if @IDOperation = 4 
  begin
    select l.*, 
           r.Name as RepositoryName, 
           adrpr.Adres as AdresPR, adrpr.REGION as adrprREGION, adrpr.RAION as adrprRAION, adrpr.GOROD as adrprGOROD, adrpr.NASPUNKT as adrprNASPUNKT, adrpr.STREET as adrprSTREET, adrpr.DOM as adrprDOM, adrpr.KORP as adrprKORP, adrpr.KVART as adrprKVART, 
           adrreg.Adres as AdresREG, adrreg.REGION as adrregREGION, adrreg.RAION as adrregRAION, adrreg.GOROD as adrregGOROD, adrreg.NASPUNKT as adrregNASPUNKT, adrreg.STREET as adrregSTREET, adrreg.DOM as adrregDOM, adrreg.KORP as adrregKORP, adrreg.KVART as adrregKVART
     from rfcLiability l 
      left join rfcRepository r on r.ID = l.RepositoryRef
      left join kldAdres adrpr on adrpr.ID = l.AdresPRRef
      left join kldAdres adrreg on adrreg.ID = l.AdresREGRef
    order by r.name, l.Fam, l.Im, l.Otch
  end
-----------
  if @IDOperation = 5 
  begin
    select * from rfcTypeRepository order by Name
  end

  if @IDOperation = 6 
  begin
    select p.*, m.DateLastPrice as DateLastPrice 
    from rfcPostav p
		left join (select PostavRef, max(DatePrice) as DateLastPrice 
		           from uchPostavNomen group by PostavRef) m on m.PostavRef = p.id
	order by p.Name
  end
  
  
  
  
  

------------edit
  if @IDOperation = 16 
  begin
    update rfcTypeProduct set Name = @name where id = @ID
  end
-----------
  if @IDOperation = 17 
  begin
    update rfcProvider set Name = @name, Comment = @Comment where id = @ID
  end
-----------
  if @IDOperation = 18 
  begin
    update rfcRepository set Name = @name, Comment = @Comment, TypeRepositoryRef = @TypeRepositoryRef, isMainRepository = @isMainRepository, CodePoint = @CodePoint, Analytics = @Analytics, LiabilityCuratorRef = @LiabilityCuratorRef where id = @ID
  end
-----------
  if @IDOperation = 19 
  begin
    -- юридический адрес --
    set @NewIDAdresUR = null
    set @NewIDAdresUR = (select min(ID) from kldAdres where ((Region=@URRegion and
                                                              Raion=@URRaion and Gorod=@URGorod and 
                                                              Naspunkt=@URNaspunkt and Street=@URStreet and
                                                              Dom=@URDom and Korp=@URKorp and Kvart=@URKvart) or Adres=@URAdres))
    if @NewIDAdresUR is Null and @URAdres is not Null
    begin
      set @NewIDAdresUR = (select IsNull(max(ID), 0)+1 from kldAdres)
      insert into kldAdres (ID, Region, Raion, Gorod, Naspunkt,
                            Street, Dom, Korp, Kvart, Adres)
      values               (@NewIDAdresUR, @URRegion, @URRaion, @URGorod, @URNaspunkt,
                            @URStreet, @URDom, @URKorp, @URKvart, @URAdres)
    end 
    -- фактический адрес -- 
    set @NewIDAdresRE = null
    set @NewIDAdresRE = (select min(ID) from kldAdres where ((Region=@RERegion and
                                                              Raion=@RERaion and Gorod=@REGorod and 
                                                              Naspunkt=@RENaspunkt and Street=@REStreet and
                                                              Dom=@REDom and Korp=@REKorp and Kvart=@REKvart) or Adres=@REAdres))
    if @NewIDAdresRE is Null and @REAdres is not Null
    begin
      set @NewIDAdresRE = (select IsNull(max(ID), 0)+1 from kldAdres)
      insert into kldAdres (ID, Region, Raion, Gorod, Naspunkt,
                            Street, Dom, Korp, Kvart, Adres)
      values               (@NewIDAdresRE, @RERegion, @RERaion, @REGorod, @RENaspunkt,
                            @REStreet, @REDom, @REKorp, @REKvart, @REAdres)
    end

    update rfcLiability 
    set Fam = @Fam, Im = @Im, Otch = @Otch,
        Phone_1 = @Phone_1, Phone_2 = @Phone_2, Phone_3 = @Phone_3, 
        DocSeria = @DocSeria, DocNumber = @DocNumber,
        AdresPRRef = @NewIDAdresUR, AdresREGRef = @NewIDAdresRE
    where id = @ID
  end
-----------
  if @IDOperation = 110 
  begin
    update rfcTypeRepository set Name = @name where id = @ID
  end

  if @IDOperation = 1110 
  begin
    update rfcPostav set Name = @name, isActive = @isActive where id = @ID
  end




----------insert
  if @IDOperation = 111 
  begin
    if not exists(select id from rfcTypeProduct where name = @name) 
    begin
      insert into rfcTypeProduct(Name) values(@name)
    end
  end
-----------  
  if @IDOperation = 112 
  begin
    if not exists(select id from rfcProvider where name = @name) 
    begin
      insert into rfcProvider(Name, Comment) values(@name, @Comment)
    end
  end
-----------
  if @IDOperation = 113 
  begin
    if not exists(select id from rfcRepository where name = @name) 
    begin
      insert into rfcRepository(Name, Comment, TypeRepositoryRef, isMainRepository, CodePoint, Analytics, LiabilityCuratorRef) values(@name, @Comment, @TypeRepositoryRef, @isMainRepository, @CodePoint, @Analytics, @LiabilityCuratorRef)
    end
  end
-----------
  if @IDOperation = 114 
  begin
    if not exists(
                      select id 
                      from rfcLiability 
                      where (
                                 (Fam = @Fam and Im = @Im and Otch = @Otch) or (DocSeria = @DocSeria and DocNumber = @DocNumber)
                             )
                         and (RepositoryRef = @RepositoryRef)   
                 ) 
    begin
       -- юридический адрес --
      set @NewIDAdresUR = null
      set @NewIDAdresUR = (select min(ID) from kldAdres where ((Region=@URRegion and
                                                                Raion=@URRaion and Gorod=@URGorod and 
                                                                Naspunkt=@URNaspunkt and Street=@URStreet and
                                                                Dom=@URDom and Korp=@URKorp and Kvart=@URKvart) or Adres=@URAdres))
      if @NewIDAdresUR is Null and @URAdres is not Null
      begin
        set @NewIDAdresUR = (select IsNull(max(ID), 0)+1 from kldAdres)
        insert into kldAdres (ID, Region, Raion, Gorod, Naspunkt,
                              Street, Dom, Korp, Kvart, Adres)
        values               (@NewIDAdresUR, @URRegion, @URRaion, @URGorod, @URNaspunkt,
                              @URStreet, @URDom, @URKorp, @URKvart, @URAdres)
      end 
      -- фактический адрес -- 
      set @NewIDAdresRE = null
      set @NewIDAdresRE = (select min(ID) from kldAdres where ((Region=@RERegion and
                                                                Raion=@RERaion and Gorod=@REGorod and 
                                                                Naspunkt=@RENaspunkt and Street=@REStreet and
                                                                Dom=@REDom and Korp=@REKorp and Kvart=@REKvart) or Adres=@REAdres))
      if @NewIDAdresRE is Null and @REAdres is not Null
      begin
        set @NewIDAdresRE = (select IsNull(max(ID), 0)+1 from kldAdres)
        insert into kldAdres (ID, Region, Raion, Gorod, Naspunkt,
                              Street, Dom, Korp, Kvart, Adres)
        values               (@NewIDAdresRE, @RERegion, @RERaion, @REGorod, @RENaspunkt,
                              @REStreet, @REDom, @REKorp, @REKvart, @REAdres)
      end

      insert into rfcLiability(RepositoryRef, Fam, Im, Otch, Phone_1, Phone_2, Phone_3, DocSeria, DocNumber, AdresPRRef, AdresREGRef) values(@RepositoryRef, @Fam, @Im, @Otch, @Phone_1, @Phone_2, @Phone_3, @DocSeria, @DocNumber, @NewIDAdresUR, @NewIDAdresRE)
    end
  end
-----------
  if @IDOperation = 115 
  begin
    if not exists(select id from rfcTypeRepository where name = @name) 
    begin
      insert into rfcTypeRepository(Name) values(@name)
    end
  end
  
  if @IDOperation = 116 
  begin
    if not exists(select id from rfcPostav where name = @name) 
    begin
      insert into rfcPostav(Name, isActive) values(@name, @isActive)
    end
  end

end



