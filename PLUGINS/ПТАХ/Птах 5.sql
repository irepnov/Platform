--select * from GGPlatform.Assemblys

update GGPlatform.Assemblys set Name = '–одна€ номенклатура (1C)' where AssemblyName = 'simrodnn.dll'

go


USE [GGPlatform2]
GO
/****** Object:  StoredProcedure [dbo].[uchRodnNomenc]    Script Date: 06/21/2017 10:54:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[uchRodnNomenc]
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
	@PriceCategory nvarchar(10) = NULL,
	@isIntellect bit = 0
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
	where EtalonNomenRef in (select EtalonNomenRef from tmpRodnNomen)
        
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
    if (@isIntellect = 1)
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
	if (@isIntellect = 0)
    begin
		select distinct /*u.id,*/ u.name  ---отберу всю этулаонную номенклатуру
		from dbo.uchEtalonNomen u
	end
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
		NacenkaProc = case when isnull(PricePostav,0) > 0 then ((RoznPrice / PricePostav) - 1)*100.00 else 0 end
	where PricePostav is not null
	  and RoznPrice is not null

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


GO

ALTER procedure [dbo].[uchPostavNomenc]
	@IDOperation int,
	@ID int = null, 
	@Name nvarchar(200)= null, 
	@RodnName nvarchar(200)= null, 
	@RodnNomenRef int = null,
	
	@DatePrice smalldatetime = null,
		
	@PostavRef int = NULL,
	@PricePostav decimal(18,2) = NULL,
	@isIntellect bit = 0
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
	if (@isIntellect = 1)
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
	if (@isIntellect = 0)
    begin
		select distinct /*u.id,*/ u.name  ---отберу ту этулаонную номенклатуру котора€ содержит эти названи€
		from dbo.uchRodnNomen u
		order by u.name
    end
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
