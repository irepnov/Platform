
Delete from [GGPlatform].ObjectsDescription where ObjectsRef not in (select id from [GGPlatform].[Objects])

go

alter table [GGPlatform].ObjectsDescription alter column FieldName nvarchar(100)

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


--declare @key nvarchar(50); declare @keyID int
 set @key = 'RodnNomen' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, 'Родная номенклатура товара')

   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
    set dateformat dmy
	select 
	e.ID as id, e.Name as Name, isnull(e.isAssort,0) as isAssort, e.EtalonNomenRef as EtalonNomenRef, e.CountOrder as CountOrder, ee.Proizv as Proizv, ee.Name as EtalonName, isnull(ee.isActive,0) as isActive, ee.TypeP as TypeP, ee.OS as os, 
	ee.DisplayType as DisplayType, ee.DisplaySize as DisplaySize, ee.SDCARD as SDCARD, ee.CameraBasic as CameraBasic, ee.CameraSecond as CameraSecond, 
	ee.ROM as ROM, ee.CPU as CPU, ee.RAM as RAM, ee.mp3_rad as mp3_rad, ee.java_mms as java_mms, ee.bluth_wifi as bluth_wifi, ee.stand as stand, ee.navi as navi, ee.akum as akum, ee.rrc as rrc, 
	e.PostavRef as PostavRef, p.name as postname, e.PricePostav as PricePostav, e.SebestFact as SebestFact, e.RoznPrice as RoznPrice, e.NacenkaPrice as NacenkaPrice, e.NacenkaProc as NacenkaProc, 
	e.PriceCategory as PriceCategory, e.CreateDate as CreateDate, e.CreateUser as CreateUser, e.UpdateDate as UpdateDate, e.UpdateUser as UpdateUser,
	pos.name as PostNomName
from uchRodnNomen e
 left join uchEtalonNomen ee on ee.ID = e.EtalonNomenRef
 left join rfcPostav p on p.ID = e.PostavRef 
 left join uchPostavNomen pos on pos.RodnNomenRef = e.ID and pos.PostavRef = e.PostavRef
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
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.EtalonNomenRef', 'EtalonNomenRef', 'ID эталона', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.Name', 'Name', 'Родное наименование', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('isnull(e.isAssort,0)', 'isAssort', 'Ассорт. матрица', 'L', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CountOrder', 'CountOrder', 'Заказ', 'N', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.Proizv', 'Proizv', 'Производитель', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.Name', 'EtalonName', 'Эталонное наименование', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('isnull(ee.isActive,0)', 'isActive', 'Актуальный', 'L', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.TypeP', 'TypeP', 'Тип устройства', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.OS', 'OS', 'Операционная система', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.DisplayType', 'DisplayType', 'Тип дисплея/ размер', 'C', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.DisplaySize', 'DisplaySize', 'Разрешение дисплея', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.SDCARD', 'SDCARD', 'Слот под карту памяти', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.CameraBasic', 'CameraBasic', 'Камера основная', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.CameraSecond', 'CameraSecond', 'Камера фронтальная', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.ROM', 'ROM', 'Встроенная память', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.CPU', 'CPU', 'Процессор', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.RAM', 'RAM', 'Оперативная память', 'C', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.mp3_rad', 'mp3_rad', 'МР3/ FM-радио', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.java_mms', 'java_mms', 'Java/ MMS', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.bluth_wifi', 'bluth_wifi', 'Bluetooth/ Wi-Fi', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.stand', 'stand', 'Стандарт', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.navi', 'navi', 'Навигация', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.akum', 'akum', 'АКБ', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.rrc', 'rrc', 'РРЦ производителя', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.PostavRef', 'PostavRef', 'ID поставщика', 'C', 0, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('p.name', 'postname', 'Поставщик', 'C', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('pos.name', 'PostNomName', 'Наименование товара поставщика', 'C', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.PricePostav', 'PricePostav', 'Цена поставщика', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.SebestFact', 'SebestFact', 'Себестоимость факт', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.RoznPrice', 'RoznPrice', 'Розничная цена', 'N', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.NacenkaPrice', 'NacenkaPrice', 'Наценка', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.NacenkaProc', 'NacenkaProc', 'Наценка %', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.PriceCategory', 'PriceCategory', 'Ценовая категория', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CreateDate', 'CreateDate', 'Создан', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CreateUser', 'CreateUser', 'Создал', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.UpdateDate', 'UpdateDate', 'Изменен', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.UpdateUser', 'UpdateUser', 'Изменил', 'C', 1, @keyID)
 end









--declare @key nvarchar(50); declare @keyID int
 set @key = 'PostavNomen' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, 'Номенклатура поставщиков')

   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
	set dateformat dmy
	select 
		e.ID as id, e.Name as Name, e.RodnNomenRef as RodnNomenRef, r.Name as RodnName, ee.Name as EtalonName, ee.Proizv as Proizv, 
		isnull(r.isAssort,0) as isAssort, e.PostavRef as PostavRef, p.name as postname, e.PricePostav as PricePostav, e.DatePrice as DatePrice,
		e.CreateDate as CreateDate, e.CreateUser as CreateUser, e.UpdateDate as UpdateDate, e.UpdateUser as UpdateUser
	from uchPostavNomen e
	 left join dbo.uchRodnNomen r on r.ID = e.RodnNomenRef
	 left join uchEtalonNomen ee on ee.ID = r.EtalonNomenRef
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
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.Name', 'Name', 'Наименование товара поставщика', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.RodnNomenRef', 'RodnNomenRef', 'ID родн номенклатуры', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('r.Name', 'RodnName', 'Родное наименование', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.Name', 'EtalonName', 'Эталонное наименование', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ee.Proizv', 'Proizv', 'Производитель', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('isnull(r.isAssort,0)', 'isAssort', 'Ассорт. матрица', 'L', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.PostavRef', 'PostavRef', 'ID поставщика', 'C', 0, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('p.name', 'postname', 'Поставщик', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.PricePostav', 'PricePostav', 'Цена поставщика', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.DatePrice', 'DatePrice', 'Дата прайса', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CreateDate', 'CreateDate', 'Создан', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.CreateUser', 'CreateUser', 'Создал', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.UpdateDate', 'UpdateDate', 'Изменен', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('e.UpdateUser', 'UpdateUser', 'Изменил', 'C', 1, @keyID)
 end

