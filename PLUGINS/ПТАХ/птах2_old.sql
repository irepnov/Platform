USE [GGPlatform2]
GO
/****** Object:  StoredProcedure [dbo].[uchMoveBill]    Script Date: 22.05.2017 8:39:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
select * from uchBillMove order by id desc
select * from [dbo].[tmpProduct]
89701205765300050000
89701205765300050091
truncate table tmpProduct

select t.OrderID, t.NumberPhone, p.NumberCard 
from tmpProduct t 
	left join uchProduct p on (p.NumberPhone = t.NumberPhone and t.NumberPhone is not null) 
	                       or (p.NumberCard = t.NumberCard and t.NumberCard is not null) 
order by t.OrderID




*/



if not exists(select * from sysobjects t where t.name like 'tmpIncomeProduct' and id in (select id from syscolumns where name like 'ProductName')) 
begin
  --alter table uchBillIncome add ProductName nvarchar(300)
  alter table tmpIncomeProduct add NumberBillIncome nvarchar(10)
  alter table tmpIncomeProduct add DateBillIncome smalldatetime
  
  alter table tmpIncomeProduct add TypeProductName nvarchar(100)--буду использовать для разноски товара
  alter table tmpIncomeProduct add PriceProduct decimal(18,2)--буду использовать для разноски товара
  alter table tmpIncomeProduct add ProductName nvarchar(300)--буду использовать для разноски товара

  /*переношу эти поля из реквизитов накладной, на товар*/
  alter table tmpProduct add TypeProductName nvarchar(100)
  alter table tmpProduct add PriceProduct decimal(18,2)
  alter table tmpProduct add ProductName nvarchar(300)
  alter table uchProduct add TypeProductRef int
  alter table uchProduct add PriceProduct decimal(18,2)
  alter table uchProduct add ProductName nvarchar(300)
  /*переношу эти поля из реквизитов накладной, на товар*/

  alter table uchProduct add TarifPlan nvarchar(300)
  alter table tmpProduct add TarifPlan nvarchar(300)

  select id, sumall, SumPeriod into _oldSummProduct from uchProduct
  update uchProduct set sumall = round(sumall, 2), SumPeriod = round(SumPeriod, 2)
  alter table uchProduct alter column SumAll decimal(18,2)
  alter table uchProduct alter column SumPeriod decimal(18,2)
  alter table tmpProduct alter column SumAll decimal(18,2)
  alter table tmpProduct alter column SumPeriod decimal(18,2)
end


go

GO

/*
select top 10000 * from uchProduct where numberphone = '79528759990' order by DateActive desc
*/

GO
/****** Object:  StoredProcedure [dbo].[uchProducts]    Script Date: 29.05.2017 16:19:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[uchProducts]
                 @IDOperation int,
                 @ID int = null,
                 @NumberCard nvarchar(20) = null,
                 @NumberPhone nvarchar(11) = null,
                 @DateInputRFA smalldatetime = null,
                 @DateActive smalldatetime = null,
                 @IMEI nvarchar(16) = null,
                 @NameStation nvarchar(50) = null,
                 @DateSale smalldatetime = null,
                 @FIOAbonent nvarchar(70) = null,
                 @SumAll decimal(18, 2) = null,
                 @SumPeriod decimal(18, 2) = null,
                 @Income int = null,
                 @Move int = null,
                 @DateRFAToProvider smalldatetime = null,
                 @DateRegisterRFA smalldatetime = null,
                 @UserRemote nvarchar(50) = null,
                
                 @DS smalldatetime = null,
                 @DE smalldatetime = null,
                 @Provider int = null,
                 @TypeProduct nvarchar(240) = null,
                 
                 @isMTS int = null,
                 @isActivePeriod bit = 0,
                 
                 @OrderID int = null,
				 @TarifPlan nvarchar(300) = null
as
begin
  set dateformat dmy
  set ansi_warnings off
  set nocount on 

--truncate table tmpProduct

  if @IDOperation = 1 
  begin
    delete from tmpProduct
  end

  if @IDOperation = 2 
  begin
    insert into tmpProduct(NumberCard, NumberPhone, DateInputRFA, DateActive, IMEI, NameStation, DateSale, FIOAbonent, SumAll, SumPeriod, DateRFAToProvider, UserRemote, DateRegisterRFA, isActivePeriod, OrderID, TarifPlan) 
    values(@NumberCard, @NumberPhone, @DateInputRFA, @DateActive, @IMEI, @NameStation, @DateSale, @FIOAbonent, @SumAll, @SumPeriod, @DateRFAToProvider, @UserRemote, @DateRegisterRFA, @isActivePeriod, @OrderID, @TarifPlan)
  end

  if @IDOperation = 3
  begin
    update uchProduct  --по номеру исс
     set DateInputRFA = t.DateInputRFA,
         FIOSeller = t.FIOSeller
    from uchProduct u
     inner join tmpProduct t on t.NumberCard = u.NumberCard
    where u.DateInputRFA is null

    update uchProduct  --по номеру телефона
     set DateInputRFA = t.DateInputRFA,
         FIOSeller = t.FIOSeller
    from uchProduct u
     inner join tmpProduct t on t.NumberPhone = u.NumberPhone
    where u.DateInputRFA is null

    delete from tmpProduct
  end

  if @IDOperation = 4
  begin
    update uchProduct
     set DateActive = t.DateActive,
         IMEI = t.IMEI,
         NameStation = t.NameStation,
		 TarifPlan = t.TarifPlan
    from uchProduct u
     inner join tmpProduct t on t.NumberCard = u.NumberCard
    where u.DateActive is null
      and t.DateActive is not null

    select 'Птах' as DLR_NAME, t.NumberPhone, t.DateActive, t.NumberCard, t.IMEI, t.NameStation, 'Номер телефона не найден в БД' as STAT 
    from tmpProduct t
     left join uchProduct u on t.NumberCard = u.NumberCard
    where u.NumberPhone is null
      and t.DateActive is not null
  end

  if @IDOperation = 5
  begin
    update uchProduct
     set DateSale = t.DateSale,
         FIOAbonent = t.FIOAbonent,
         DateRegisterRFA = t.DateRegisterRFA,
         UserRemote = t.UserRemote,
         DateActive = t.DateActive
    from uchProduct u
     inner join tmpProduct t on t.NumberPhone = u.NumberPhone
    where t.DateActive is not null     ----МТС  обновляю Дату активации  




    ---для тех у которых номера не пустые 20.02.2015
    update uchProduct
     set DateSale = t.DateSale,
         FIOAbonent = t.FIOAbonent,
         DateRegisterRFA = t.DateRegisterRFA,
         UserRemote = t.UserRemote
    from uchProduct u
     inner join tmpProduct t on --t.NumberPhone = u.NumberPhone  раньше так было
                                t.NumberCard = u.NumberCard   --теперь по ИСС  29.03.2014     
    where t.DateActive is null     ----ТЕЛЕ  НЕ обновляю Дату активации и не загружаю из файла
      and isnull(u.NumberPhone, '0') <> '0'
      
    --если номер пустой, то возьму его из файла   20.02.2015
    update uchProduct
     set DateSale = t.DateSale,
         FIOAbonent = t.FIOAbonent,
         DateRegisterRFA = t.DateRegisterRFA,
         UserRemote = t.UserRemote,
         NumberPhone = t.NumberPhone
    from uchProduct u
     inner join tmpProduct t on --t.NumberPhone = u.NumberPhone  раньше так было
                                t.NumberCard = u.NumberCard   --теперь по ИСС  29.03.2014     
    where t.DateActive is null     ----ТЕЛЕ  НЕ обновляю Дату активации и не загружаю из файла
      and isnull(u.NumberPhone, '0') = '0'


    delete from tmpProduct
  end

  if @IDOperation = 6
  begin
    if @isMTS = 0 
    begin 
	  update uchProduct
	   set SumAll = t.SumAll,
	   	   SumPeriod = t.SumPeriod,
           isActivePeriod = t.isActivePeriod
	  from uchProduct u
	   inner join tmpProduct t on t.NumberCard = u.NumberCard
    end

   -- if @isMTS = 1 ---в файле от МТС суммы за прошлые периоды НЕТу
   -- begin 
   --     ----------вознаграждение БЫЛО ранее
	  --update uchProduct
	  -- set SumAll = isnull(u.SumAll, 0) + u.SumPeriod ---К предыдущему периоды прибавляю ТП
	  --from uchProduct u
   --    inner join uchBillIncome i on i.ID = u.BillIncomeRef and i.ProviderRef = 5  ---только МТС
   --   where isnull(u.SumPeriod, 0) <> 0  ---вознаграждений было

	  --update uchProduct
	  -- set SumPeriod = 0  -- обнуляю прошлые периоды, т.к. я их уже приплюсовал
	  --from uchProduct u
   --    inner join uchBillIncome i on i.ID = u.BillIncomeRef and i.ProviderRef = 5  ---только МТС
   --   where isnull(u.SumPeriod, 0) <> 0  ---вознаграждений было

   --   -----------поступила активность в отчетном периоде
	  --update uchProduct
	  -- set SumPeriod = t.SumPeriod,
   --        isActivePeriod = t.isActivePeriod
	  --from uchProduct u
	  -- inner join tmpProduct t on t.NumberPhone = u.NumberPhone

	  --update uchProduct
	  -- set DateActive = t.DateActive
	  --from uchProduct u
	  -- inner join tmpProduct t on t.NumberPhone = u.NumberPhone
   --   where t.DateActive is not null
   --     and u.DateActive is null
   -- end

    delete from tmpProduct
  end
  
  if @IDOperation = 7
  begin

  ----добавить информация по складам и ответственным
	select i.ID, 'приход' as nakl, i.NumberBill, i.DateBill, tp.name as Product, p.Name as Provider, 
	       i.DateIncomeRepository, rep.Name as Sender, null as Recipient, i.PriceProduct, i.CountProduct, 
		   i.CreateDate, i.CreateUser, i.UpdateDate, i.UpdateUser,
		   l.Fam, l.Im, l.Otch, l.Phone_1
	from uchBillIncome i
	 inner join rfcTypeProduct tp on tp.ID = i.TypeProductRef
	 inner join rfcProvider p on p.ID = i.ProviderRef
	 inner join rfcRepository rep on rep.ID = i.RepositoryRecipientRef
	 left join rfcLiability l on l.ID = rep.LiabilityCuratorRef
	where i.ID = @Income
	union
	select m.ID, 'перемещение' as nakl, m.NumberBill, m.DateMove, tp.name as Product, null as Provider, 
		   m.DateMove, rep.Name as Sender, repr.name as Recipient, null as PriceProduct, m.CountProduct, 
		   m.CreateDate, m.CreateUser, m.UpdateDate, m.UpdateUser,
		   l.Fam, l.Im, l.Otch, l.Phone_1
	from uchBillMove m
	 inner join rfcTypeProduct tp on tp.ID = m.TypeProductRef
	 inner join rfcRepository rep on rep.ID = m.RepositorySenderRef
	 inner join rfcRepository repr on repr.ID = m.RepositoryRecipientRef
	 left join rfcLiability l on l.ID = repr.LiabilityCuratorRef
	where m.ID = @Move
	order by CreateDate
  end

  if @IDOperation = 8
  begin
    select 'РФА принята ранее  ' as Status, u.NumberPhone, u.DateInputRFA, u.IMEI, u.NumberCard, u.NameStation
    from tmpProduct t
     inner join uchProduct u on (t.NumberCard = u.NumberCard) or (t.NumberPhone = u.NumberPhone)
    where u.DateInputRFA is not null
  union
    select 'ICC не найдена в БД' as Status, t.NumberPhone, t.DateInputRFA, t.IMEI, t.NumberCard, t.NameStation
    from tmpProduct t
     left join uchProduct u on (t.NumberCard = u.NumberCard) or (t.NumberPhone = u.NumberPhone)
    where u.NumberPhone is null
  end

  if @IDOperation = 9
  begin
    update tmpProduct
     set BillMoveRef = u.BillMoveRef
    from tmpProduct t
     inner join uchProduct u on t.NumberCard = u.NumberCard
    where t.BillMoveRef is null  --потом по ICC    
     
    update tmpProduct
     set BillMoveRef = u.BillMoveRef
    from tmpProduct t
     inner join uchProduct u on t.NumberPhone = u.NumberPhone
    where t.BillMoveRef is null  --сначало по номеру телефона

    select t.NumberPhone, t.NumberCard, r.Name
--,t.id, m.id, r.id
    from tmpProduct t
     left join uchBillMove m on m.ID = t.BillMoveRef
     left join rfcRepository r on r.ID = m.RepositoryRecipientRef  --результат на экран  Сортируя по порядку загрузки
    order by t.id
  end


--select* from tmpProduct

  if @IDOperation = 10
  begin
    declare @dtss nvarchar(10),
            @dtee nvarchar(10)

    set @dtss = convert(nvarchar(10), @ds, 104)
    set @dtee = convert(nvarchar(10), @de, 104)


    exec ('create table #mas(OrderR int null, RepositoryRecipientID int null, RepositoryRecipient nvarchar(100) null, 
                             OrderL int null, Liability nvarchar(40) null, 
                             Curator nvarchar(40) null, 
                             ProductID int, Product nvarchar(100) null,
                             Sale int null, NotSale int null)

    set dateformat dmy
    insert into #mas
    select 1 as OrderR, r.ID as RepositoryRecipientID, r.Name as RepositoryRecipient, 
		   2 as OrderL, isnull(l.fam, '''') + '' '' + left(l.Im, 1) + ''. '' + left(l.Otch, 1) + ''.'' as Liability,
		   isnull(c.fam, '''') + '' '' + left(c.Im, 1) + ''. '' + left(c.Otch, 1) + ''.'' as Curator, 
		   tp.ID as ProductID, tp.Name as Product,
		   sum(case when p.DateActive between '''+@dtss+''' and '''+@dtee+''' then 1 else 0 end) Sale,
		   sum(case when p.DateActive is not null and p.DateInputRFA is not null and p.DateActive <= '''+@dtee+''' and p.DateInputRFA <= '''+@dtee+''' then 1 else 0 end) NotSale
	from uchProduct p
	 inner join uchBillMove m on m.ID = p.BillMoveRef
	 inner join rfcRepository r on r.ID = m.RepositoryRecipientRef
	 inner join uchBillIncome i on i.ID = p.BillIncomeRef
	 inner join rfcTypeProduct tp on tp.ID = m.TypeProductRef
	 left join rfcLiability l on l.ID = m.LiabilityRef   
     left join rfcRepository repr on repr.ID = m.RepositoryRecipientRef 
     left join rfcLiability c on repr.LiabilityCuratorRef = c.ID 
 	where 
		  ( ('+@Provider+' = 0) or ('+@Provider+' <> 0 and i.ProviderRef = '+@Provider+') ) 
	  and ( ('''+@TypeProduct+''' = ''-1'') or ('''+@TypeProduct+''' <> ''-1'' and m.TypeProductRef in ('+@TypeProduct+')) )
	group by r.ID, r.Name, 
		   isnull(l.fam, '''') + '' '' + left(l.Im, 1) + ''. '' + left(l.Otch, 1) + ''.'',
		   isnull(c.fam, '''') + '' '' + left(c.Im, 1) + ''. '' + left(c.Otch, 1) + ''.'', 
		   tp.ID, tp.Name


    --итого по складу
    insert into #mas (OrderR, RepositoryRecipientID, RepositoryRecipient, OrderL, Liability, Curator, ProductID, Product, Sale, NotSale)
    select OrderR, RepositoryRecipientID, RepositoryRecipient, OrderL, Liability, Curator, 999999 as ProductID, ''ИТОГО'' as Product, sum(Sale), sum(NotSale) from #mas group by OrderR, RepositoryRecipientID, RepositoryRecipient, OrderL, Liability, Curator

    --итог по складу и товару 
	insert into #mas (OrderR, RepositoryRecipientID, RepositoryRecipient, OrderL, Curator, ProductID, Product, Sale, NotSale)
	select 1 as OrderR, RepositoryRecipientID, RepositoryRecipient, 1 as OrderL, Curator, ProductID, Product, sum(Sale), sum(NotSale) from #mas group by RepositoryRecipientID, RepositoryRecipient, Curator, ProductID, Product

    --итог всего
	insert into #mas (OrderR, RepositoryRecipient, OrderL, ProductID, Product, Sale, NotSale)
	select 2 as OrderR, ''   ИТОГО:'' as RepositoryRecipient, 3 as OrderL, ProductID, Product, sum(Sale), sum(NotSale) from #mas where OrderR = 1 and OrderL = 1 group by ProductID, Product


	select * from #mas-- order by OrderR, RepositoryRecipientID, OrderL
    ')
  end
  
  if @IDOperation = 11
  begin
    update uchProduct
     set DateActive = t.DateActive, ---перезаписываем дату активации
         IMEI = t.IMEI,
         NameStation = t.NameStation,
		 TarifPlan = t.TarifPlan
    from uchProduct u
     inner join tmpProduct t on t.NumberCard = u.NumberCard
    where t.DateActive is not null

    select 'Птах' as DLR_NAME, t.NumberPhone, t.DateActive, t.NumberCard, t.IMEI, t.NameStation, 'Номер телефона не найден в БД' as STAT  
    from tmpProduct t
     left join uchProduct u on t.NumberCard = u.NumberCard
    where u.NumberPhone is null
      and t.DateActive is not null
  end  


  if @IDOperation = 12
  begin
    update uchProduct
     set DateRFAToProvider = t.DateRFAToProvider
    from uchProduct u
     inner join tmpProduct t on t.NumberCard = u.NumberCard
    where u.DateRFAToProvider is null 

    delete from tmpProduct
  end

end



go




ALTER procedure [dbo].[uchIncomeBill]
                 @IDOperation int,
                 @ID int = null,
                 @NumberCard nvarchar(20) = null,
                 @NumberPhone nvarchar(11) = null,

                 @NumberBill nvarchar(10) = null,
                 @DateBill smalldatetime = null,
                 @ProviderRef int = null,
                 @DateIncomeRepository smalldatetime = null,
                 @RepositoryRecipientRef int = null,
                 @TypeProductRef int = null,
                 @PriceProduct decimal(18,2) = null,
                 @CountProduct int = null,

				 @ProductName nvarchar(300) = null,
				 @TypeProductName nvarchar(100)	= null		
as
begin
  set ansi_warnings off
  set nocount on

  declare @BillID int 

  if @IDOperation = 1 
  begin
    delete from tmpIncomeProduct
  end

  if @IDOperation = 2 
  begin
    insert into tmpIncomeProduct(NumberCard, NumberPhone) values(@NumberCard, @NumberPhone)
  end

  if @IDOperation = 3
  begin
    begin tran    
      if not exists(select id from uchBillIncome where NumberBill = @NumberBill and DateBill = @DateBill) 
      begin
        insert into uchBillIncome(NumberBill, DateBill, ProviderRef, DateIncomeRepository, RepositoryRecipientRef, TypeProductRef, PriceProduct, CountProduct, ProductName)
                    values(@NumberBill, @DateBill, @ProviderRef, @DateIncomeRepository, @RepositoryRecipientRef, @TypeProductRef, @PriceProduct, @CountProduct, @ProductName)
        select @BillID = @@IDENTITY 

        insert into uchProduct(NumberPhone, NumberCard, BillIncomeRef) 
                    select NumberPhone, NumberCard, @BillID from tmpIncomeProduct

        delete from tmpIncomeProduct
      end
    commit tran
  end

  if @IDOperation = 4
  begin
    if not exists (select id from uchProduct where BillIncomeRef = @ID and BillMoveRef is not null)
    begin
      begin tran    
        delete from uchProduct where BillIncomeRef = @ID
        delete from uchBillIncome where ID = @ID
      commit tran
    end else
    begin
      raiserror('Товар данной накладной был перемещен на другой склад', 18, 1) 
    end
  end

  if @IDOperation = 5
  begin
    update uchBillIncome
     set NumberBill = @NumberBill, 
         DateBill = @DateBill, 
         ProviderRef = @ProviderRef, 
         DateIncomeRepository = @DateIncomeRepository, 
         RepositoryRecipientRef = @RepositoryRecipientRef, 
         TypeProductRef = @TypeProductRef, 
         PriceProduct = @PriceProduct, 
         CountProduct = @CountProduct,
		 ProductName = @ProductName
    where ID = @ID 
  end
 
  if @IDOperation = 6 
  begin
    insert into tmpIncomeProduct(NumberCard, NumberPhone, ProductName, NumberBillIncome, DateBillIncome, TypeProductName, PriceProduct) 
	                      values(@NumberCard, @NumberPhone, @ProductName, @NumberBill, @DateBill, @TypeProductName, @PriceProduct)
  end

  if @IDOperation = 7
  begin
    select t.ProductName, t.NumberBillIncome, t.DateBillIncome, p.id as TypeProductRef, t.PriceProduct, count(t.ID) as CountProd
	from tmpIncomeProduct t
	  left join rfcTypeProduct p on replace(replace(Upper(p.name), '-', ''), ' ', '') = replace(replace(Upper(t.TypeProductName), '-', ''), ' ', '')
	group by t.ProductName, t.NumberBillIncome, t.DateBillIncome, p.id, t.PriceProduct
---	<> 1  -- error
  end

end

go

ALTER procedure [dbo].[uchMoveBill]
                 @IDOperation int,
                 @CountProduct int = null,
                 @FirstICC nvarchar(20) = null,
                 @LastICC nvarchar(20) = null,
                 @TypeProductRef int = null,
                 @RepositorySenderRef int = null,
                 @NumberBill nvarchar(10) = null,
                 @DateMove smalldatetime = null,
                 @RepositoryRecipientRef int = null,
                 @LiabilityRef int = null,
                 @ID int = null,
				 @NumberCard nvarchar(20) = null,
                 @NumberPhone nvarchar(11) = null
as
begin
  set ansi_warnings off
  set nocount on

  declare @ProductID int,
          @BillID int 

  if @IDOperation = 1 
  begin
    delete from tmpMoveProduct
  end

  if @IDOperation = 2
  begin
    select top(1) @ProductID = p.id 
    from uchProduct p
      inner join uchBillIncome i on i.id = p.BillIncomeRef
    where BillMoveRef is null  
      and i.RepositoryRecipientRef = @RepositorySenderRef
      and i.TypeProductRef = @TypeProductRef 
    
    insert into tmpMoveProduct(NumberCard, NumberPhone)
     select top(@CountProduct) NumberCard, NumberPhone 
     from uchProduct p
       inner join uchBillIncome i on i.id = p.BillIncomeRef
     where BillMoveRef is null and p.id >= @ProductID 
       and i.RepositoryRecipientRef = @RepositorySenderRef
       and i.TypeProductRef = @TypeProductRef      
  end

  if @IDOperation = 3
  begin  
    insert into tmpMoveProduct(NumberCard, NumberPhone)
     select NumberCard, NumberPhone 
     from uchProduct p
       inner join uchBillIncome i on i.id = p.BillIncomeRef 
     where BillMoveRef is null and NumberCard between @FirstICC and @LastICC 
       and i.RepositoryRecipientRef = @RepositorySenderRef
       and i.TypeProductRef = @TypeProductRef          
     order by NumberCard
  end

  if @IDOperation = 4
  begin  
    insert into uchBillMove(NumberBill, DateMove, RepositorySenderRef, RepositoryRecipientRef, LiabilityRef, TypeProductRef, CountProduct)
     values(@NumberBill, @DateMove, @RepositorySenderRef, @RepositoryRecipientRef, @LiabilityRef, @TypeProductRef, @CountProduct)
  
    set @BillID = @@IDENTITY

    update uchProduct
    set BillMoveRef = @BillID
    from uchProduct p
      inner join uchBillIncome i on i.id = p.BillIncomeRef 
    where --p.BillMoveRef is null 
    --  and 
    p.NumberCard in (select NumberCard from tmpMoveProduct)
    --  and i.RepositoryRecipientRef = @RepositorySenderRef
      and i.TypeProductRef = @TypeProductRef
  end

  if @IDOperation = 5
  begin   
	select pt.Name as Product, u.NumberCard, u.NumberPhone, 
		   i.PriceProduct as Price,
		   m.NumberBill as BillMoveNumber, m.DateMove as DateMove,
           sender.Name as Sender, recip.Name as Recipient, recip.Comment as RecipientComment
	from uchProduct u
	  inner join uchBillIncome i on i.ID = u.BillIncomeRef
	  inner join rfcTypeProduct pt on pt.ID = i.TypeProductRef
	  inner join uchBillMove m on m.ID = u.BillMoveRef
	  inner join rfcRepository sender on sender.ID = m.RepositorySenderRef
	  inner join rfcRepository recip on recip.ID = m.RepositoryRecipientRef
	where BillMoveRef = @ID
	order by u.NumberCard
  end

  if @IDOperation = 6
  begin   
	select m.NumberBill as BillMoveNumber, m.DateMove as DateMove,
           sender.Name as Sender, recip.Name as Recipient, recip.Comment as RecipientComment, pt.Name as Product,
           count(u.ID) as col, min(i.PriceProduct) as Price, dbo.RubPhrase(count(u.ID) * min(i.PriceProduct)) as summ 
	from uchProduct u
	  inner join uchBillIncome i on i.ID = u.BillIncomeRef
	  inner join rfcTypeProduct pt on pt.ID = i.TypeProductRef
	  inner join uchBillMove m on m.ID = u.BillMoveRef
	  inner join rfcRepository sender on sender.ID = m.RepositorySenderRef
	  inner join rfcRepository recip on recip.ID = m.RepositoryRecipientRef
	where BillMoveRef = @ID
    group by m.NumberBill, m.DateMove, sender.Name, recip.Name, recip.Comment, pt.Name
  end

  if @IDOperation = 7
  begin  
    update uchBillMove
    set NumberBill = @NumberBill,
        DateMove = @DateMove,
        RepositorySenderRef = @RepositorySenderRef,
        RepositoryRecipientRef = @RepositoryRecipientRef,
        LiabilityRef = @LiabilityRef,
        TypeProductRef = @TypeProductRef,
        CountProduct = @CountProduct
    where ID = @ID
  end

  if @IDOperation = 8
  begin
    if not exists (select id from uchProduct where BillMoveRef = @ID and DateActive is not null)
    begin
      begin tran    
        update uchProduct set BillMoveRef = null where BillMoveRef = @ID
        delete from uchBillMove where ID = @ID
      commit tran 
    end else
    begin
      raiserror('У данной накладной имеется активированный товар', 18, 1) 
    end
  end

  if @IDOperation = 9
  begin 
    if not exists(select * from tmpMoveProduct where NumberCard = @NumberCard and NumberPhone = @NumberPhone) 
	begin
	  insert into tmpMoveProduct(NumberCard, NumberPhone) values(@NumberCard, @NumberPhone)
	end
  end

  if @IDOperation = 10
  begin 
    delete from tmpMoveProduct where NumberCard = @NumberCard and NumberPhone = @NumberPhone 
  end

  if @IDOperation = 11
  begin 
    if not exists(select * from tmpMoveProduct where NumberCard = @NumberCard) 
	begin
	  insert into tmpMoveProduct(NumberCard) values(@NumberCard)
	end
  end

end

