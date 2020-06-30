
Delete from [GGPlatform].ObjectsDescription where ObjectsRef not in (select id from [GGPlatform].[Objects])


alter table [GGPlatform].ObjectsDescription alter column FieldName nvarchar(100)



declare @key nvarchar(50); declare @keyID int

 set @key = 'ProductBills' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, '�������� ���������')

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
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('ID', null, 'ID ���������', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('nakl', null, '��� ���������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('NumberBill', null, '����� ���������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('DateBill', null, '���� ���������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Product', null, '�����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Provider', null, '��������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('DateIncomeRepository', null, '���� �����������/ �����������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Sender', null, '����� �����������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Recipient', null, '����� ����������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CountProduct', null, '���-�� ������', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Fam', null, '������� �����. �� �����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Im', null, '��� �����. �� �����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Otch', null, '�������� �����. �� �����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('Phone_1', null, '������� �����. �� �����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateDate', null, '������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateUser', null, '������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateDate', null, '�������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateUser', null, '�������', 'C', 1, @keyID)
end





 --declare @key nvarchar(50); declare @keyID int
 set @key = 'Product' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, '������ � �������')

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
		   (case when u.isActivePeriod = 1 then 1 else 0 end) as isActivePeriodI, (case when u.SumAll is not null or u.SumPeriod is not null then ''��'' else ''���'' end) as PrVozn, 
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
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('1', 'col', '���-��', 'N', 1, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.ID', 'ID', 'ID ������', 'N', 0, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('t.name', 'product', '�����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.ProductName', 'ProductName', '�������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('p.name', 'provider', '��������', 'C', 1, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.PriceProduct', 'PriceProduct', '���� ��.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.NumberPhone', 'NumberPhone', '����� ��������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.NumberCard', 'NumberCard', '����� ICC', 'C', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.IMEI', 'IMEI', 'IMEI', 'C', 1, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.DateIncomeRepository', 'DateIncomeRepository', '���� ������. �� �����', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.DateMove', 'DateMove', '���� �����������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.DateSale', 'DateSale', '���� �������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.DateInputRFA', 'DateInputRFA', '���� ������ ���', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('year(u.dateactive)', 'dateactiveyear', '��� ���������', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('month(u.dateactive)', 'dateactivemonth', '����� ���������', 'N', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('convert(smalldatetime, convert(nvarchar(10), u.dateactive, 104), 104)', 'dateactivedate', '���� ���������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('right(convert(nvarchar(19), u.dateactive, 20), 8)', 'dateactivetime', '����� ���������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.DateRegisterRFA', 'DateRegisterRFA', '���� ����������� ���', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.DateRFAToProvider', 'DateRFAToProvider', '���� ����� ��� ���������', 'D', 1, @keyID)

	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.FIOSeller', 'FIOSeller', '��� ��������', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.FIOAbonent', 'FIOAbonent', '��� ��������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('(case when u.isActivePeriod = 1 then 1 else 0 end)', 'isActivePeriodI', '�����. � ���. �������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('(case when u.SumAll is not null or u.SumPeriod is not null then ''��'' else ''���'' end)', 'PrVozn', '�����. �������.', 'C', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.NameStation', 'NameStation', '�������', 'C', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('(isnull(u.SumAll, 0) + isnull(u.SumPeriod, 0))', 'SumI', '����� �������. (�����)', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.SumAll', 'SumAll', '����� �������. (�� ����. �������)', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.SumPeriod', 'SumPeriod', '����� �������. (������)', 'N', 1, @keyID)
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.BillIncomeRef', 'BillIncomeRef', 'ID ����. ����.', 'C', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.BillMoveRef', 'BillMoveRef', 'ID ����. �����.', 'C', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.UserRemote', 'UserRemote', '��������� ������������', 'C', 1, @keyID)
	
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('rep.name', 'Repository', '����� �����������', 'C', 1, @keyID)	
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('repr.name', 'RepositoryRecipient', '����� ����������', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Fam', 'Fam', '������� �����.', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Im', 'Im', '��� �����.', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Otch', 'Otch', '�������� �����.', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Phone_1', 'Phone_1', '������� �����.', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('rept.name', 'repType', '��� ������ ����������', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('c.Fam', 'cFam', '������� �������� ������ ���������� ', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('c.Im', 'cIm', '��� �������� ������ ����������', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('c.Otch', 'cOtch', '�������� �������� ������ ����������', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('repr.Analytics', 'Analytics', '������ ������ ����������', 'C', 1, @keyID)	
	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.CreateDate', 'CreateDate', '������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.CreateUser', 'CreateUser', '������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.UpdateDate', 'UpdateDate', '�������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.UpdateUser', 'UpdateUser', '�������', 'C', 1, @keyID)
 end


 --declare @key nvarchar(50); declare @keyID int
 set @key = 'BillMove' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, '����������� ������ ����� ��������')

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
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ID', 'ID', 'ID ���������', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberBill', 'NumberBill', '����� ���������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateMove', 'DateMove', '���� �����������', 'D', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.RepositorySenderRef',            'RepositorySenderRef', 'ID ������-�����������', 'N', 0, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.RepositoryRecipientRef', 'RepositoryRecipientRef', 'ID ������-����������', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.LiabilityRef',   'LiabilityRef', 'ID ��������������', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.TypeProductRef',         'TypeProductRef', 'ID ������', 'N', 0, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.CountProduct', 'CountProduct', '���-�� ��.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateDate', null, '������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateUser', null, '������', 'C', 1, @keyID)		
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateUser', null, '�������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateDate', null, '�������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('s.Name', 'Sender', '����� �����������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('r.Name', 'Recipient', '����� ����������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('p.Name', 'Product', '�����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Fam', 'Fam', '������� �������������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Im', 'Im', '��� �������������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Otch', 'Otch', '�������� ��������������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('l.Phone_1', 'Phone_1', '������� ��������������', 'C', 1, @keyID)
 end




 --declare @key nvarchar(50); declare @keyID int
 set @key = 'uchProductBillMove' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, '������������ ����� �� ���������')

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
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ID', 'ID', 'ID ������', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberPhone', 'NumberPhone', '����� ��������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberCard', 'NumberCard', '����� ICC', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('pt.name', 'TypeProductName', '�����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('pp.Name', 'Provider', '��������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.PriceProduct', 'Price', '���� ��.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.NumberBill', 'BillIncome', '� ��������� ����.', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.DateBill', 'DateIncome', '���� �����������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateActive', 'DateActive', '���� ���������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateSale', 'DateSale', '���� �������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateInputRFA', 'DateInputRFA', '���� ���', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.FIOAbonent', 'FIOAbonent', '��� ��������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.NumberBill', 'BillMove', '� ����. �����������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.DateMove', 'DateMove', '���� �����������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('r.Name', 'repName', '����� ����������', 'C', 1, @keyID)	
 end

 --���������� �� ��������� ������
 set @key = 'BillMove'
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef, ObjectsRefOrSubRef)
 values
 (
 'b.ID', 'ID', '������������ �����', 'Q', 0, @keyID
 ,(select id from [GGPlatform].[Objects] where objectname = 'uchProductBillMove')
 )   










 --declare @key nvarchar(50); declare @keyID int

 set @key = 'BillIncome' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, '������ ������ �� �����')

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
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ID', 'ID', 'ID ���������', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberBill', 'NumberBill', '����� ���������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateBill', 'DateBill', '���� ���������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateIncomeRepository',   'DateIncomeRepository', '���� ���������� �� ����� ', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ProviderRef',            'ProviderRef', 'ID ���������', 'N', 0, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.RepositoryRecipientRef', 'RepositoryRecipientRef', 'ID ������', 'N', 0, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.TypeProductRef',         'TypeProductRef', 'ID ������', 'N', 0, @keyID)	
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.PriceProduct', 'PriceProduct', '���� ��.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.CountProduct', 'CountProduct', '���-�� ��.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('prov.Name', 'Provider', '��������', 'C', 1, @keyID)
	--insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('prod.Name', 'Product', '�����', 'C', 1, @keyID)
    --insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ProductName', 'ProductName', '�������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('repos.Name', 'Repository', '�����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateUser', null, '�������', 'C', 1, @keyID)	
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateDate', null, '������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('CreateUser', null, '������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('UpdateDate', null, '�������', 'D', 1, @keyID)
 end


--  declare @key nvarchar(50); declare @keyID int
 set @key = 'uchProductBillIncome' 
 delete from [GGPlatform].[Objects] where objectname = @key
 if not exists(select * from [GGPlatform].[Objects] where objectname = @key)
 begin
   insert into [GGPlatform].[Objects](objectname, objectcaption) values(@key, '����������� ����� �� ���������')

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
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.ID', 'ID', 'ID ������', 'N', 0, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberPhone', 'NumberPhone', '����� ��������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.NumberCard', 'NumberCard', '����� ICC', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('tp.name', 'TypeProductName', '�����', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('pp.Name', 'Provider', '��������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.PriceProduct', 'Price', '���� ��.', 'N', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.NumberBill', 'BillIncome', '� ��������� ����.', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.DateBill', 'DateIncome', '���� ��������� ����.', 'C', 1, @keyID)
    insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('i.DateIncomeRepository',   'DateIncomeRepository', '���� ���������� �� ����� ', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateActive', 'DateActive', '���� ���������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateSale', 'DateSale', '���� �������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.DateInputRFA', 'DateInputRFA', '���� ���', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('b.FIOAbonent', 'FIOAbonent', '��� ��������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.NumberBill', 'BillMove', '� ����. �����������', 'C', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('m.DateMove', 'DateMove', '���� �����������', 'D', 1, @keyID)
	insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('r.Name', 'repName', '����� ����������', 'C', 1, @keyID)
 end

 --���������� �� ��������� ������
 set @key = 'BillIncome'
 select @keyID = id from [GGPlatform].[Objects] where objectname = @key
 insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef, ObjectsRefOrSubRef)
 values
 (
 'b.ID', 'ID', '����������� �����', 'Q', 0, @keyID
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

--	select * from rfcRepository  08-�������� ����� (���������)
--	select id, name from rfcTypeProduct order by name �� Tele2 - 3G MB �������� ��� ��������� ������� (1 �.)






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
 --  insert into [GGPlatform].[Objects](objectname, objectcaption) values('BillIncome', '������ ������ �� �����')

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