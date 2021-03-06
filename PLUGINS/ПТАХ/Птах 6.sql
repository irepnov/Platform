
if not exists(select * from sysobjects t where t.name like 'uchEtalonNomen' and id in (select id from syscolumns where name like 'NacenkaNewRozn')) 
begin
	alter table uchEtalonNomen add NacenkaNewRozn decimal(18,2)
	alter table uchEtalonNomen add NacenkaProcNewRozn decimal(18,2)
end


go


 declare @key nvarchar(50); declare @keyID int
 set @key = 'EtalonNomen' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, 'Эталонная номенклатура товара')

   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
    set dateformat dmy
	select 
	e.ID as id, e.Proizv as Proizv, e.Name as Name, isnull(e.isActive,0) as isActive, e.TypeP as TypeP, e.OS as os, 
	e.DisplayType as DisplayType, e.DisplaySize as DisplaySize, e.SDCARD as SDCARD, e.CameraBasic as CameraBasic, e.CameraSecond as CameraSecond, 
	e.ROM as ROM, e.CPU as CPU, e.RAM as RAM, e.mp3_rad as mp3_rad, e.java_mms as java_mms, e.bluth_wifi as bluth_wifi, e.stand as stand, e.navi as navi, e.akum as akum, e.rrc as rrc, 
	e.PostavRef as PostavRef, p.name as postname, e.PricePostav as PricePostav, e.SebestFact as SebestFact, e.RoznPrice as RoznPrice, e.NacenkaPrice as NacenkaPrice, e.NacenkaProc as NacenkaProc, 
	isnull(e.ErrorRozn,0) as ErrorRozn, e.NewRozn as NewRozn, e.PriceCategory as PriceCategory, e.CreateDate as CreateDate, e.CreateUser as CreateUser, e.UpdateDate as UpdateDate, e.UpdateUser as UpdateUser,
	e.con1 as con1, e.con2 as con2, e.con3 as con3, e.con4 as con4, e.con5 as con5,
	e.con6 as con6, e.con7 as con7, e.con8 as con8, e.con9 as con9, e.con10 as con10,
	e.NacenkaNewRozn as NacenkaNewRozn, e.NacenkaProcNewRozn as NacenkaProcNewRozn
from uchEtalonNomen e
 left join rfcPostav p on p.ID = e.PostavRef 
   '
   ,objectExpressionSubQuery = null
   where objectname = @key
 end
 ---fields
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 delete from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID
 if not exists(select * from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID)
 begin	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.ID', 'ID', 'ID товара', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.Proizv', 'Proizv', 'Производитель', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.Name', 'Name', 'Наименование', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('isnull(e.isActive,0)', 'isActive', 'Актуальный', 'L', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.TypeP', 'TypeP', 'Тип устройства', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.OS', 'OS', 'Операционная система', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.DisplayType', 'DisplayType', 'Тип дисплея/ размер', 'C', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.DisplaySize', 'DisplaySize', 'Разрешение дисплея', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.SDCARD', 'SDCARD', 'Слот под карту памяти', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CameraBasic', 'CameraBasic', 'Камера основная', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CameraSecond', 'CameraSecond', 'Камера фронтальная', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.ROM', 'ROM', 'Встроенная память', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CPU', 'CPU', 'Процессор', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.RAM', 'RAM', 'Оперативная память', 'C', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.mp3_rad', 'mp3_rad', 'МР3/ FM-радио', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.java_mms', 'java_mms', 'Java/ MMS', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.bluth_wifi', 'bluth_wifi', 'Bluetooth/ Wi-Fi', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.stand', 'stand', 'Стандарт', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.navi', 'navi', 'Навигация', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.akum', 'akum', 'АКБ', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.rrc', 'rrc', 'РРЦ производителя', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.PostavRef', 'PostavRef', 'ID поставщика', 'C', 0, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('p.name', 'postname', 'Поставщик', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.PricePostav', 'PricePostav', 'Цена поставщика', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.SebestFact', 'SebestFact', 'Себестоимость факт', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.RoznPrice', 'RoznPrice', 'Розничная цена', 'N', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.NacenkaPrice', 'NacenkaPrice', 'Наценка', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.NacenkaProc', 'NacenkaProc', 'Наценка %', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('isnull(e.ErrorRozn,0)', 'ErrorRozn', 'Ошибка РЦ', 'L', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.NewRozn', 'NewRozn', 'Новая РЦ', 'N', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.NacenkaNewRozn', 'NacenkaNewRozn', 'Наценка Новая РЦ', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.NacenkaProcNewRozn', 'NacenkaProcNewRozn', 'Наценка Новая РЦ %', 'N', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.PriceCategory', 'PriceCategory', 'Ценовая категория', 'C', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con1', 'con1', 'Конкурент 1', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con2', 'con2', 'Конкурент 2', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con3', 'con3', 'Конкурент 3', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con4', 'con4', 'Конкурент 4', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con5', 'con5', 'Конкурент 5', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con6', 'con6', 'Конкурент 6', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con7', 'con7', 'Конкурент 7', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con8', 'con8', 'Конкурент 8', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con9', 'con9', 'Конкурент 9', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.con10', 'con10', 'Конкурент 10', 'N', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CreateDate', 'CreateDate', 'Создан', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CreateUser', 'CreateUser', 'Создал', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.UpdateDate', 'UpdateDate', 'Изменен', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.UpdateUser', 'UpdateUser', 'Изменил', 'C', 1, @keyID)
 end




GO
/****** Object:  StoredProcedure [dbo].[uchEtalonNomenc]    Script Date: 06/22/2017 11:06:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[uchEtalonNomenc]
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
	@con10 decimal(18,2) = NULL,
	
	@TypeNacenka int = 2 --    1-цена поставщика   2-себестоимость
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
     
     if (@TypeNacenka = 1)
	 begin				
		update dbo.uchEtalonNomen
		set NacenkaNewRozn = NewRozn - PricePostav,
			NacenkaProcNewRozn = case when isnull(PricePostav,0) > 0 then ((NewRozn / PricePostav) - 1)*100.00 else 0 end
		where NewRozn is not null
		  and PricePostav is not null
		  and id = @id
    end

	if (@TypeNacenka = 2)
	begin				
		update dbo.uchEtalonNomen
		set NacenkaNewRozn = NewRozn - SebestFact,
			NacenkaProcNewRozn = case when isnull(SebestFact,0) > 0 then ((NewRozn / SebestFact) - 1)*100.00 else 0 end
		where NewRozn is not null
		  and SebestFact is not null
		  and id = @id
    end
     
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
		NacenkaNewRozn = null,
		NacenkaProcNewRozn = null,
		PriceCategory = null,
		isActive = 0
    
	---средняя цена по поставщику
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

	--средняя себестоимость
	update e
	set SebestFact = r.s_avg
	from uchEtalonNomen e
		inner join (
					select EtalonNomenRef, AVG(SebestFact) as s_avg
					from dbo.uchRodnNomen
					where EtalonNomenRef is not null
					group by EtalonNomenRef	
					) r on r.EtalonNomenRef = e.ID

	--средняя розничая цена
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


	if (@TypeNacenka = 1)
	begin				
		update dbo.uchEtalonNomen
		set NacenkaPrice = RoznPrice - PricePostav,
			NacenkaProc = case when isnull(PricePostav,0) > 0 then ((RoznPrice / PricePostav) - 1)*100.00 else 0 end
		where RoznPrice is not null
		  and PricePostav is not null
    end

	if (@TypeNacenka = 2)
	begin				
		update dbo.uchEtalonNomen
		set NacenkaPrice = RoznPrice - SebestFact,
			NacenkaProc = case when isnull(SebestFact,0) > 0 then ((RoznPrice / SebestFact) - 1)*100.00 else 0 end
		where RoznPrice is not null
		  and SebestFact is not null
    end




	update dbo.uchEtalonNomen
	set PriceCategory = pp.name
	from uchEtalonNomen r
	 inner join rfcPriceCategory pp on r.RoznPrice between pp.PriceMin and pp.PriceMax	
	 
	update uchEtalonNomen
	set isActive = 1 --признак актальности для родного товара с себестоимостью > 0
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






GO
/****** Object:  StoredProcedure [dbo].[uchRodnNomenc]    Script Date: 06/22/2017 10:09:57 ******/
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
    update t  ---проставлю новые привязки
    set EtalonNomenRef = u.id
    from tmpRodnNomen t
     inner join uchEtalonNomen u on REPLACE(replace(t.EtalonName, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
    where u.Name is not null
      and t.EtalonName is not null
      and t.EtalonNomenRef is null
      
    update t ---перенесу прошлые привязки 
    set EtalonNomenRef = u.EtalonNomenRef
    from tmpRodnNomen t
     inner join uchRodnNomen u on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
    where u.EtalonNomenRef is not null --ранее привязан
      and t.EtalonName is not null --указано не правильное имя эталонной номенкалутры
      and t.EtalonNomenRef is null    
      
    update t ---перенесу прошлые привязки 
    set EtalonNomenRef = u.EtalonNomenRef
    from tmpRodnNomen t
     inner join uchRodnNomen u on REPLACE(replace(t.Name, '-', ''), ' ', '') = REPLACE(replace(u.Name, '-', ''), ' ', '')
    where u.EtalonNomenRef is not null --ранее привязан
      and t.EtalonName is null  --вообзще не указано имя эталонное
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
	---where EtalonNomenRef in (select EtalonNomenRef from tmpRodnNomen)
        
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
			select distinct /*u.id,*/ u.name  ---отберу ту этулаонную номенклатуру которая содержит эти названия
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
		union
			select '' as name
		order by u.name
	end
	if (@isIntellect = 0)
    begin
			select distinct /*u.id,*/ u.name  ---отберу всю этулаонную номенклатуру
			from dbo.uchEtalonNomen u
		union
			select '' as name
		order by u.name
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
				 select RodnNomenRef, MIN(PricePostav) as mPricePostav		--минимальная цена		 
				 from uchPostavNomen
				 where PostavRef in (select id from rfcPostav where isnull(isActive,0) = 1)  --среди актуальных поставщиков
				 group by RodnNomenRef
				 ) mp on mp.RodnNomenRef = p.RodnNomenRef
					 and mp.mPricePostav = p.PricePostav

	update r
	set PostavRef = m.PostavRef,--поставщик с мин. ценой
		PricePostav = m.PricePostav --минимальная цена
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
/****** Object:  StoredProcedure [dbo].[uchPostavNomenc]    Script Date: 06/22/2017 10:14:42 ******/
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
