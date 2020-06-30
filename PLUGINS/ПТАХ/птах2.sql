
Delete from [GGPlatform].ObjectsDescription where ObjectsRef not in (select id from [GGPlatform].[Objects])


alter table [GGPlatform].ObjectsDescription alter column FieldName nvarchar(100)



declare @key nvarchar(50); declare @keyID int

 set @key = 'ProductBills' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, 'Товарные накладные')

   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
	exec uchProducts @IDOperation = 7, @Income = {0}, @Move = {1}
   '
   ,objectExpressionSubQuery = null
   where objectname = @key
 end
 ---fields
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 delete from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID
 if not exists(select * from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID)
 begin	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ID', null, 'ID накладной', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('nakl', null, 'Вид накладной', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('NumberBill', null, 'Номер накладной', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('DateBill', null, 'Дата накладной', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Product', null, 'Товар', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Provider', null, 'Оператор', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('DateIncomeRepository', null, 'Дата поступления/ перемещения', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Sender', null, 'Склад отправитель', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Recipient', null, 'Склад получатель', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CountProduct', null, 'Кол-во товара', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Fam', null, 'Фамилия ответ. за склад', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Im', null, 'Имя ответ. за склад', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Otch', null, 'Отчество ответ. за склад', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Phone_1', null, 'Телефон ответ. за склад', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateDate', null, 'Создан', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateUser', null, 'Создал', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateDate', null, 'Изменен', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateUser', null, 'Изменил', 'C', 1, @keyID)
end





 --declare @key nvarchar(50); declare @keyID int
 set @key = 'Product' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, 'Работа с товаром')

   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
    set dateformat dmy
	select 1 as col, u.ID as ID, u.NumberPhone as NumberPhone, u.NumberCard as NumberCard, u.DateActive as DateActive, u.DateSale as DateSale, u.ProductName as ProductName,
		   u.DateInputRFA as DateInputRFA, u.FIOSeller as FIOSeller, u.NameStation as NameStation, u.IMEI as IMEI, u.FIOAbonent as FIOAbonent, u.SumAll as SumAll, u.SumPeriod as SumPeriod, 
		   u.BillIncomeRef as BillIncomeRef, u.BillMoveRef as BillMoveRef, u.CreateDate as CreateDate, u.CreateUser as CreateUser, u.UpdateDate as UpdateDate, u.UpdateUser as UpdateUser, 
		   u.DateRFAToProvider as DateRFAToProvider, u.UserRemote as UserRemote, u.DateRegisterRFA as DateRegisterRFA, u.isActivePeriod as isActivePeriod, t.name as product, p.name as provider, 
		   i.DateIncomeRepository, u.PriceProduct, rep.name as Repository, m.DateMove, 
		   (case when u.isActivePeriod = 1 then 1 else 0 end) as isActivePeriodI, (case when u.SumAll is not null or u.SumPeriod is not null then ''Да'' else ''Нет'' end) as PrVozn, 
		   (isnull(u.SumAll, 0) + isnull(u.SumPeriod, 0)) as SumI, convert(smalldatetime, convert(nvarchar(10), u.dateactive, 104), 104) as dateactivedate, right(convert(nvarchar(19), u.dateactive, 20), 8) as dateactivetime, 
		   year(u.dateactive) as dateactiveyear, month(u.dateactive) as dateactivemonth
	from uchProduct u 
        inner join uchBillIncome i on i.ID = u.BillIncomeRef 
        inner join rfcTypeProduct t on t.ID = u.TypeProductRef 
        inner join rfcProvider p on p.ID = i.ProviderRef 
        inner join rfcRepository rep on rep.ID = i.RepositoryRecipientRef 
        left join uchBillMove m on m.ID = u.BillMoveRef 
   '
   ,objectExpressionSubQuery = null
   where objectname = @key
 end
 ---fields
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 delete from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID
 if not exists(select * from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID)
 begin	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('1', 'col', 'Кол-во', 'N', 1, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.ID', 'ID', 'ID товара', 'N', 0, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('t.name', 'product', 'Товар', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.ProductName', 'ProductName', 'Продукт', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('p.name', 'provider', 'Оператор', 'C', 1, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.PriceProduct', 'PriceProduct', 'Цена ед.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.NumberPhone', 'NumberPhone', 'Номер телефона', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.NumberCard', 'NumberCard', 'Номер ICC', 'C', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.IMEI', 'IMEI', 'IMEI', 'C', 1, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.DateIncomeRepository', 'DateIncomeRepository', 'Дата поступ. на склад', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.DateMove', 'DateMove', 'Дата перемещения', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.DateSale', 'DateSale', 'Дата продажи', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.DateInputRFA', 'DateInputRFA', 'Дата приема РФА', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('year(u.dateactive)', 'dateactiveyear', 'Год активации', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('month(u.dateactive)', 'dateactivemonth', 'Месяц активации', 'N', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('convert(smalldatetime, convert(nvarchar(10), u.dateactive, 104), 104)', 'dateactivedate', 'Дата активации', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('right(convert(nvarchar(19), u.dateactive, 20), 8)', 'dateactivetime', 'Время активации', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.DateRegisterRFA', 'DateRegisterRFA', 'Дата регистрации РФА', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.DateRFAToProvider', 'DateRFAToProvider', 'Дата сдачи РФА оператору', 'D', 1, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.FIOSeller', 'FIOSeller', 'ФИО продавца', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.FIOAbonent', 'FIOAbonent', 'ФИО абонента', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('(case when u.isActivePeriod = 1 then 1 else 0 end)', 'isActivePeriodI', 'Актив. в отч. периоде', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('(case when u.SumAll is not null or u.SumPeriod is not null then ''Да'' else ''Нет'' end)', 'PrVozn', 'Налич. вознагр.', 'C', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.NameStation', 'NameStation', 'Станция', 'C', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('(isnull(u.SumAll, 0) + isnull(u.SumPeriod, 0))', 'SumI', 'Сумма вознагр. (всего)', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.SumAll', 'SumAll', 'Сумма вознагр. (до посл. периода)', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.SumPeriod', 'SumPeriod', 'Сумма вознагр. (период)', 'N', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.BillIncomeRef', 'BillIncomeRef', 'ID прих. накл.', 'C', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.BillMoveRef', 'BillMoveRef', 'ID накл. перем.', 'C', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.UserRemote', 'UserRemote', 'Удаленный пользователь', 'C', 1, @keyID)
	
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('rep.name', 'Repository', 'Склад отправитель', 'C', 1, @keyID)	
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('repr.name', 'RepositoryRecipient', 'Склад получатель', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Fam', 'Fam', 'Фамилия ответ.', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Im', 'Im', 'Имя ответ.', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Otch', 'Otch', 'Отчество ответ.', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Phone_1', 'Phone_1', 'Телефон ответ.', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('rept.name', 'repType', 'Тип склада получателя', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('c.Fam', 'cFam', 'Фамилия куратора склада получателя ', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('c.Im', 'cIm', 'Имя куратора склада получателя', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('c.Otch', 'cOtch', 'Отчество куратора склада получателя', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('repr.Analytics', 'Analytics', 'Анализ склада получателя', 'C', 1, @keyID)	
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.CreateDate', 'CreateDate', 'Создан', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.CreateUser', 'CreateUser', 'Создал', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.UpdateDate', 'UpdateDate', 'Изменен', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.UpdateUser', 'UpdateUser', 'Изменил', 'C', 1, @keyID)
 end


 --declare @key nvarchar(50); declare @keyID int
 set @key = 'BillMove' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, 'Перемещение товара между складами')

   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
	select b.ID, b.NumberBill, b.DateMove, b.RepositorySenderRef, b.RepositoryRecipientRef, 
		   b.LiabilityRef, b.TypeProductRef, b.CountProduct, b.CreateDate, b.CreateUser, b.UpdateDate, b.UpdateUser,
		   s.Name as Sender, r.Name as Recipient, p.Name as Product, l.Fam as Fam, l.Im as Im, l.Otch as Otch, l.Phone_1 as Phone_1 
	from uchBillMove b
		inner join rfcRepository s on s.ID = b.RepositorySenderRef 
		inner join rfcRepository r on r.ID = b.RepositoryRecipientRef 
		inner join rfcTypeProduct p on p.ID = b.TypeProductRef 
		left join rfcLiability l on l.ID = b.LiabilityRef
   '
   ,objectExpressionSubQuery = 
   '
	select b.ID 
	from uchBillMove b
		inner join rfcRepository s on s.ID = b.RepositorySenderRef 
		inner join rfcRepository r on r.ID = b.RepositoryRecipientRef 
		inner join rfcTypeProduct p on p.ID = b.TypeProductRef 
		left join rfcLiability l on l.ID = b.LiabilityRef
   '
   where objectname = @key
 end
 ---fields
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 delete from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID
 if not exists(select * from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID)
 begin
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ID', 'ID', 'ID накладной', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberBill', 'NumberBill', 'Номер накладной', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateMove', 'DateMove', 'Дата перемещения', 'D', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.RepositorySenderRef',            'RepositorySenderRef', 'ID склада-отправителя', 'N', 0, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.RepositoryRecipientRef', 'RepositoryRecipientRef', 'ID склада-получателя', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.LiabilityRef',   'LiabilityRef', 'ID ответственного', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.TypeProductRef',         'TypeProductRef', 'ID товара', 'N', 0, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.CountProduct', 'CountProduct', 'Кол-во ед.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateDate', null, 'Создан', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateUser', null, 'Создал', 'C', 1, @keyID)		
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateUser', null, 'Изменил', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateDate', null, 'Изменен', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('s.Name', 'Sender', 'Склад отправитель', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('r.Name', 'Recipient', 'Склад получатель', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('p.Name', 'Product', 'Товар', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Fam', 'Fam', 'Фамилия ответсвенного', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Im', 'Im', 'Имя ответсвенного', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Otch', 'Otch', 'Отчество ответственного', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Phone_1', 'Phone_1', 'Телефон ответственного', 'C', 1, @keyID)
 end




 --declare @key nvarchar(50); declare @keyID int
 set @key = 'uchProductBillMove' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, 'Перемещенный товар по накладной')

   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
	select b.ID, b.NumberPhone, b.NumberCard, b.DateActive, b.DateSale, b.DateInputRFA, b.FIOAbonent, b.BillIncomeRef,
	       m.NumberBill as BillMove, m.DateMove, r.Name as repName,
		   i.NumberBill as BillIncome, i.DateBill as DateIncome, b.PriceProduct as Price, pp.Name as Provider, pt.Name as TypeProductName
	from uchProduct b 
	 inner join uchBillIncome i on i.ID = b.BillIncomeRef
	 inner join rfcProvider pp on pp.ID = i.ProviderRef
	 inner join rfcTypeProduct pt on pt.ID = b.TypeProductRef
	 left join uchBillMove m on m.ID = b.BillMoveRef
	 left join rfcRepository r on r.ID = m.RepositoryRecipientRef
   '
   ,objectExpressionSubQuery = 
   '
	select distinct b.BillMoveRef
	from uchProduct b 
	 inner join uchBillIncome i on i.ID = b.BillIncomeRef
	 inner join rfcProvider pp on pp.ID = i.ProviderRef
	 left join uchBillMove m on m.ID = b.BillMoveRef
	 left join rfcRepository r on r.ID = m.RepositoryRecipientRef
   '
   where objectname = @key
 end
 ---fields
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 delete from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID
 if not exists(select * from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID)
 begin
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ID', 'ID', 'ID товара', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberPhone', 'NumberPhone', 'Номер телефона', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberCard', 'NumberCard', 'Номер ICC', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('pt.name', 'TypeProductName', 'Товар', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('pp.Name', 'Provider', 'Оператор', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.PriceProduct', 'Price', 'Цена ед.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.NumberBill', 'BillIncome', '№ приходной накл.', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.DateBill', 'DateIncome', 'Дата поступления', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateActive', 'DateActive', 'Дата активации', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateSale', 'DateSale', 'Дата продажи', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateInputRFA', 'DateInputRFA', 'Дата РФА', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.FIOAbonent', 'FIOAbonent', 'ФИО абонента', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.NumberBill', 'BillMove', '№ накл. перемещения', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.DateMove', 'DateMove', 'Дата перемещения', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('r.Name', 'repName', 'Склад получатель', 'C', 1, @keyID)	
 end

 --фильтрация по вложенным данным
 set @key = 'BillMove'
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef, ObjectsRefOrSubRef)
 values
 (
 'b.ID', 'ID', 'Перемещенный товар', 'Q', 0, @keyID
 ,(select id from [GGPlatform].[Objects] where objectname = 'uchProductBillMove')
 )   










 --declare @key nvarchar(50); declare @keyID int

 set @key = 'BillIncome' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, 'Приход товара на склад')

   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
	select b.ID, b.NumberBill, b.DateBill, b.ProviderRef, b.DateIncomeRepository, b.RepositoryRecipientRef, 
		   b.CountProduct, b.CreateDate, b.CreateUser, b.UpdateDate, b.UpdateUser, 
	       prov.Name as Provider, repos.Name as Repository 
	from uchBillIncome b 
		left join rfcProvider prov on prov.ID = b.ProviderRef 
		left join rfcRepository repos on repos.ID = b.RepositoryRecipientRef
   '
   ,objectExpressionSubQuery = 
   '
	select b.ID
	from uchBillIncome b 
		left join rfcProvider prov on prov.ID = b.ProviderRef 
		left join rfcRepository repos on repos.ID = b.RepositoryRecipientRef
   '
   where objectname = @key
 end
 ---fields
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 delete from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID
 if not exists(select * from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID)
 begin
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ID', 'ID', 'ID накладной', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberBill', 'NumberBill', 'Номер накладной', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateBill', 'DateBill', 'Дата накладной', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateIncomeRepository',   'DateIncomeRepository', 'Дата постановки на склад ', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ProviderRef',            'ProviderRef', 'ID оператора', 'N', 0, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.RepositoryRecipientRef', 'RepositoryRecipientRef', 'ID склада', 'N', 0, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.TypeProductRef',         'TypeProductRef', 'ID товара', 'N', 0, @keyID)	
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.PriceProduct', 'PriceProduct', 'Цена ед.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.CountProduct', 'CountProduct', 'Кол-во ед.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('prov.Name', 'Provider', 'Оператор', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('prod.Name', 'Product', 'Товар', 'C', 1, @keyID)
    --insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ProductName', 'ProductName', 'Продукт', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('repos.Name', 'Repository', 'Склад', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateUser', null, 'Изменил', 'C', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateDate', null, 'Создан', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateUser', null, 'Создал', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateDate', null, 'Изменен', 'D', 1, @keyID)
 end


--  declare @key nvarchar(50); declare @keyID int
 set @key = 'uchProductBillIncome' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, 'Поступивший товар по накладной')

   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
	select b.ID, b.NumberPhone, b.NumberCard, b.DateActive, b.DateSale, b.DateInputRFA, b.FIOAbonent, b.BillIncomeRef,
	       m.NumberBill as BillMove, m.DateMove, r.Name as repName, i.DateIncomeRepository as DateIncomeRepository,
		   i.NumberBill as BillIncome, i.DateBill as DateIncome, b.PriceProduct as Price, pp.Name as Provider, tp.name as TypeProductName
	from uchProduct b 
	 inner join uchBillIncome i on i.ID = b.BillIncomeRef
	 inner join rfcProvider pp on pp.ID = i.ProviderRef
	 inner join rfcTypeProduct tp on tp.id = b.TypeProductRef
	 left join uchBillMove m on m.ID = b.BillMoveRef
	 left join rfcRepository r on r.ID = m.RepositoryRecipientRef
   '
   ,objectExpressionSubQuery = 
   '
	select distinct b.BillIncomeRef
	from uchProduct b 
	 inner join uchBillIncome i on i.ID = b.BillIncomeRef
	 inner join rfcProvider pp on pp.ID = i.ProviderRef
	 inner join rfcTypeProduct tp on tp.id = b.TypeProductRef
	 left join uchBillMove m on m.ID = b.BillMoveRef
	 left join rfcRepository r on r.ID = m.RepositoryRecipientRef
   '
   where objectname = @key
 end
 ---fields
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 delete from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID
 if not exists(select * from [GGPlatform].ObjectsDescription where ObjectsRef = @keyID)
 begin
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ID', 'ID', 'ID товара', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberPhone', 'NumberPhone', 'Номер телефона', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberCard', 'NumberCard', 'Номер ICC', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('tp.name', 'TypeProductName', 'Товар', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('pp.Name', 'Provider', 'Оператор', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.PriceProduct', 'Price', 'Цена ед.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.NumberBill', 'BillIncome', '№ приходной накл.', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.DateBill', 'DateIncome', 'Дата приходной накл.', 'C', 1, @keyID)
    insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.DateIncomeRepository',   'DateIncomeRepository', 'Дата постановки на склад ', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateActive', 'DateActive', 'Дата активации', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateSale', 'DateSale', 'Дата продажи', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateInputRFA', 'DateInputRFA', 'Дата РФА', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.FIOAbonent', 'FIOAbonent', 'ФИО абонента', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.NumberBill', 'BillMove', '№ накл. перемещения', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.DateMove', 'DateMove', 'Дата перемещения', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('r.Name', 'repName', 'Склад получатель', 'C', 1, @keyID)
 end

 --фильтрация по вложенным данным
 set @key = 'BillIncome'
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef, ObjectsRefOrSubRef)
 values
 (
 'b.ID', 'ID', 'Поступивший товар', 'Q', 0, @keyID
 ,(select id from [GGPlatform].[Objects] where objectname = 'uchProductBillIncome')
 )   



 --select id, name from rfcOKOPF

 /*
 select * from uchProduct b where BillIncomeref = 19 and b.BillMoveRef = 80
 select * from uchBillmove where id = 80
 select * from uchBillIncome where id = 19
 */

--select min(numbercard) as icc1, max(numbercard) as icc2 from tmpMoveProduct

--select RepositoryRecipientRef, TypeProductRef 
--    from uchProduct p
--      inner join uchBillIncome i on i.id = p.BillIncomeRef
--    where BillMoveRef is null 
--	group by RepositoryRecipientRef, TypeProductRef 

--	select * from rfcRepository  08-Основной склад (Краснодар)
--	select id, name from rfcTypeProduct order by name КП Tele2 - 3G MB Интернет для устройств ПОДАРОК (1 р.)






--select 0 as id, '' as name union select id, name from rfcTypeProduct order by name

--	select top(1) /*@ProductID =*/ p.id --174288
--    from uchProduct p
--      inner join uchBillIncome i on i.id = p.BillIncomeRef
--    where BillMoveRef is null  
--      and i.RepositoryRecipientRef = 5
--      and i.TypeProductRef = 72 
    
--    insert into tmpMoveProduct(NumberCard, NumberPhone)
--     select  NumberCard, NumberPhone 
--     from uchProduct p
--       inner join uchBillIncome i on i.id = p.BillIncomeRef
--     where BillMoveRef is null and p.id >= 174288--@ProductID 
--       and i.RepositoryRecipientRef = 5
--       and i.TypeProductRef = 72 




 --if not exists(select * from [GGPlatform].[Objects] where objectname = 'BillIncome')
 --begin
 --  insert into [GGPlatform].[Objects](objectname, objectcaption) values('BillIncome', 'Приход товара на склад')

 --  update [GGPlatform].[Objects]
 --  set 
 --  objectExpression = 
 --  '
	--	select b.ID, b.NumberBill, b.DateBill, b.ProviderRef, b.DateIncomeRepository, b.RepositoryRecipientRef, b.TypeProductRef, b.PriceProduct, b.CountProduct, b.CreateDate, b.CreateUser, b.UpdateDate, b.UpdateUser, 
	--		   prov.Name as Provider, prod.Name as Product, repos.Name as Repository 
	--	from uchBillIncome b 
	--		left join rfcProvider prov on prov.ID = b.ProviderRef 
	--		left join rfcTypeProduct prod on prod.ID = b.TypeProductRef 
	--		left join rfcRepository repos on repos.ID = b.RepositoryRecipientRef
 --  '
 --  ,objectExpressionSubQuery = 
 --  '
	--	select b.ID
	--	from uchBillIncome b 
	--		left join rfcProvider prov on prov.ID = b.ProviderRef 
	--		left join rfcTypeProduct prod on prod.ID = b.TypeProductRef 
	--		left join rfcRepository repos on repos.ID = b.RepositoryRecipientRef
 --  '
 --  where objectname = 'BillIncome'
 --end