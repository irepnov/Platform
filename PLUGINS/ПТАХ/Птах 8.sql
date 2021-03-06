USE [UCHSIM]
GO
/****** Object:  StoredProcedure [dbo].[uchPostavNomenc]    Script Date: 24.07.2017 18:30:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
    update t  ---проставлю новые привязки
    set RodnNomenRef = u.id
    from tmpPostavNomen t
     inner join uchRodnNomen u on REPLACE(replace(t.RodnName, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
    where u.Name is not null
      and t.RodnName is not null
      and t.RodnNomenRef is null      
      and t.PostavRef = @PostavRef
      
    update t ---перенесу прошлые привязки 
    set RodnNomenRef = u.RodnNomenRef
    from tmpPostavNomen t
     inner join uchPostavNomen u on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
                                and t.PostavRef = u.PostavRef
    where u.RodnNomenRef is not null --ранее привязан
      and t.RodnName is not null --указано не правильное имя эталонной номенкалутры
      and t.RodnNomenRef is null
      and t.PostavRef = @PostavRef    
      
    update t ---перенесу прошлые привязки 
    set RodnNomenRef = u.RodnNomenRef
    from tmpPostavNomen t
     inner join uchPostavNomen u on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
								and t.PostavRef = u.PostavRef
    where u.RodnNomenRef is not null --ранее привязан
      and t.RodnName is null  --вообзще не указано имя эталонное
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
    update uchPostavNomen set PricePostav = null where PostavRef = @PostavRef  --обнулю прошлый прайс

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
			select distinct /*u.id,*/ u.name  ---отберу ту этулаонную номенклатуру которая содержит эти названия
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
		union
			select '' as name
		order by u.name
	end
	if (@isIntellect = 0)
    begin
			select distinct /*u.id,*/ u.name  ---отберу ту этулаонную номенклатуру которая содержит эти названия
			from dbo.uchRodnNomen u
		union
			select '' as name
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
