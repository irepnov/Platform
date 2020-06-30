
dbserv
excelmanag
uchMonitoringPrice.xlt
simetalonn
simrfc
simrodn
adminman


usercontrol
querybuilder
gridcells





   update [GGPlatform].[Objects]
   set 
   objectExpression = 
   '
    set dateformat dmy
	select 1 as col, u.ID as ID, u.NumberPhone as NumberPhone, u.NumberCard as NumberCard, u.DateActive as DateActive, u.DateSale as DateSale, u.ProductName as ProductName,
		   u.DateInputRFA as DateInputRFA, u.FIOSeller as FIOSeller, u.NameStation as NameStation, u.IMEI as IMEI, u.FIOAbonent as FIOAbonent, u.SumAll as SumAll, u.SumPeriod as SumPeriod, 
		   u.BillIncomeRef as BillIncomeRef, u.BillMoveRef as BillMoveRef, u.CreateDate as CreateDate, u.CreateUser as CreateUser, u.UpdateDate as UpdateDate, u.UpdateUser as UpdateUser, 
		   u.DateRFAToProvider as DateRFAToProvider, u.UserRemote as UserRemote, u.DateRegisterRFA as DateRegisterRFA, u.isActivePeriod as isActivePeriod, t.name as product, p.name as provider, 
		   i.DateIncomeRepository, u.PriceProduct, rep.name as Repository, m.DateMove, u.TarifPlan as TarifPlan,
		   (case when u.isActivePeriod = 1 then 1 else 0 end) as isActivePeriodI, (case when u.SumAll is not null or u.SumPeriod is not null then ''Да'' else ''Нет'' end) as PrVozn, 
		   (isnull(u.SumAll, 0) + isnull(u.SumPeriod, 0)) as SumI, convert(smalldatetime, convert(nvarchar(10), u.dateactive, 104), 104) as dateactivedate, right(convert(nvarchar(19), u.dateactive, 20), 8) as dateactivetime, 
		   year(u.dateactive) as dateactiveyear, month(u.dateactive) as dateactivemonth, repr.name as RepositoryRecipientt
	from uchProduct u 
        inner join uchBillIncome i on i.ID = u.BillIncomeRef 
        inner join rfcTypeProduct t on t.ID = u.TypeProductRef 
        inner join rfcProvider p on p.ID = i.ProviderRef 
        inner join rfcRepository rep on rep.ID = i.RepositoryRecipientRef 
        left join uchBillMove m on m.ID = u.BillMoveRef 
        left join rfcRepository repr on (  (repr.ID = m.RepositoryRecipientRef and u.BillMoveRef is not null) 
                                        or (repr.ID = i.RepositoryRecipientRef and u.BillMoveRef is null) 
                                        )
   '
   ,objectExpressionSubQuery = null
   where objectname = 'Product'

go


if not exists(select * from [GGPlatform].ObjectsDescription where ObjectsRef = 10 and FieldAlias = 'TarifPlan')
begin
   insert into [GGPlatform].ObjectsDescription(FieldName, FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef)values('u.TarifPlan', 'TarifPlan', 'Тарифный план', 'C', 1, 10)
end

go





if not exists(select * from sysobjects t where t.name like 'rfcPostav' and id in (select id from syscolumns where name like 'PriorityID')) 
begin
	alter table rfcPostav add PriorityID int
end

go

update rfcPostav set PriorityID = ID


go


  ----автоматическое присвоение прав
    declare @name nvarchar(100), @type nvarchar(10)
	DECLARE CT1 CURSOR FOR 
	                       SELECT s.name + '.' + o.name as name, o.type
						   FROM sys.objects o
						    inner join sys.schemas s on s.schema_id = o.schema_id
						   WHERE o.type in ('U', 'P', /*'TR',*/ 'V', 'FN') 
						   order by o.type					
	OPEN CT1
	WHILE 1 = 1 
	BEGIN
		FETCH FROM CT1 INTO @name, @type
		IF @@fetch_status = -1 BREAK
		IF @@fetch_status = -2 CONTINUE

		if (@type = 'FN') or (@type = 'P')
		begin
			exec('GRANT EXECUTE ON ' + @name + ' TO sysUsers')
			exec('GRANT EXECUTE ON ' + @name + ' TO sysAdmins')
		end
		if (@type = 'V') or (@type = 'U')
		begin
			exec('GRANT SELECT, INSERT, UPDATE, DELETE ON ' + @name + ' TO sysUsers')
			exec('GRANT SELECT, INSERT, UPDATE, DELETE ON ' + @name + ' TO sysAdmins')
		end
	END
	CLOSE CT1
	DEALLOCATE CT1
	


GO
/****** Object:  StoredProcedure [dbo].[uchReference]    Script Date: 06/27/2017 20:46:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
      declare @pr int
      
      select @pr = MAX(PriorityID) from rfcPostav
    
      insert into rfcPostav(Name, isActive, PriorityID) values(@name, @isActive, @pr)
    end
  end

end



go



GO
/****** Object:  StoredProcedure [dbo].[uchEtalonNomenc]    Script Date: 06/27/2017 21:02:37 ******/
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
	--update e
	--set PostavRef = m.PostavRef
	--   ,PricePostav = m._avg
	--from dbo.uchEtalonNomen e
	-- inner join (
	--            select EtalonNomenRef, min(PostavRef) as mPostavRef
	--            from #minPostav
	--            group by EtalonNomenRef
	--            ) mp on e.id = mp.EtalonNomenRef
	-- inner join #minPostav m on m.EtalonNomenRef = e.ID
	--                        and m.PostavRef = mp.mPostavRef
	                        	
	update e
	set PostavRef = m.PostavRef
	   ,PricePostav = m._avg
	from dbo.uchEtalonNomen e
	 inner join (
	            select o.EtalonNomenRef, min(r.PriorityID) as mPriority
	            from #minPostav o
	              inner join rfcPostav r on r.ID = o.PostavRef
	            group by o.EtalonNomenRef
	            ) mp on e.id = mp.EtalonNomenRef
	 inner join rfcPostav pos on pos.PriorityID = mp.mPriority     	 	            
	 inner join #minPostav m on m.EtalonNomenRef = e.ID 
	                        and m.PostavRef = pos.ID

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




go



GO
/****** Object:  StoredProcedure [dbo].[uchRodnNomenc]    Script Date: 06/27/2017 21:36:32 ******/
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

	--update r
	--set PostavRef = m.PostavRef,--поставщик с мин. ценой
	--	PricePostav = m.PricePostav --минимальная цена
	--from uchRodnNomen r
	-- inner join (
	--			select RodnNomenRef, min(PostavRef) as mPostavRef
	--			from #minPrice
	--			group by RodnNomenRef
	--			) mp on r.id = mp.RodnNomenRef
	-- inner join #minPrice m on m.RodnNomenRef = r.ID
	--                       and m.PostavRef = mp.mPostavRef
	
	update r
	set PostavRef = m.PostavRef,--поставщик с мин. ценой
		PricePostav = m.PricePostav --минимальная цена
	from uchRodnNomen r
	 inner join (
				select RodnNomenRef, min(r.PriorityID) as mPriority
				from #minPrice o
				 inner join rfcPostav r on r.ID = o.PostavRef
				group by RodnNomenRef
				) mp on r.id = mp.RodnNomenRef
	 inner join rfcPostav pos on pos.PriorityID = mp.mPriority
	 inner join #minPrice m on m.RodnNomenRef = r.ID
	                       and m.PostavRef = pos.ID	

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
/****** Object:  StoredProcedure [GGPlatform].[usp_WorkplacesManager]    Script Date: 05.07.2017 12:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*

exec [GGPlatform].[usp_UsersManager] @TypeQuery = 5, @ObjectID = 6

exec [GGPlatform].[usp_UsersManager] @TypeQuery = 4, @Login = 'gg', 
       @Fam = 'gg', @Im = 'hh', @Otch = 'hhh', 
	   @PostRef = 0, @SectionRef = 0, 
	   @isWindowsUser = 0, @isAdmin = 1, 
	   @Workplaces = '1;2;', @ReportGroups = '1;'

	   select u.Name
		from GGPlatform.Workplaces g
		 inner join GGPlatform.MatrixWorkplaceAssemblys m on m.WorkplaceRef = g.ID
		 inner join GGPlatform.Assemblys u on m.AssemblysRef = u.ID
		where g.ID = 1
		order by u.Name

*/


-- =============================================
ALTER PROCEDURE [GGPlatform].[usp_WorkplacesManager] 
                @TypeQuery int,
				@ObjectID int = null,

                @Name nvarchar(100) = NULL,
                @Description nvarchar(250) = NULL,

				@Assemblys varchar(8000) = null,
				@Users varchar(8000) = null
AS
BEGIN
	begin tran
	declare @ErrMsg nvarchar(200)
	declare @WorkplaceID int
	declare @Result int
	declare @maxOrder int

	select @WorkplaceID = -1
	select @Result = -1

	if (@TypeQuery = 1)
	begin
		select u.ID
		from GGPlatform.Workplaces g
		 inner join GGPlatform.MatrixUsersWorkplace m on m.WorkplaceRef = g.ID
		 inner join GGPlatform.Users u on m.UsersRef = u.ID
		where g.ID = @ObjectID
		order by u.Fam, u.Im, u.Otch
	end--пользователи

	if (@TypeQuery = 2)
	begin
		select u.ID
		from GGPlatform.Workplaces g
		 inner join GGPlatform.MatrixWorkplaceAssemblys m on m.WorkplaceRef = g.ID
		 inner join GGPlatform.Assemblys u on m.AssemblysRef = u.ID
		where g.ID = @ObjectID
		order by u.Name
	end--интерфейсы

	if (@TypeQuery = 3)
    begin
	   --begin tran

       if @ObjectID is null 
       begin
         select @ErrMsg = 'Ќе указан идентификатор рабочего места.'
         goto ErrExit                              
       end

	   if exists(select id from GGPlatform.Workplaces where Upper(Name) = Upper(Rtrim(Ltrim(@Name))) and ID <> @ObjectID)
       begin
         select @ErrMsg = 'ђабочее место ' + @Name + ' уже зарегистрировано в системе.'
         goto ErrExit          
       end

       update GGPlatform.Workplaces 
	   set  Name = @Name, 
			Description = @Description
       where ID  = @ObjectID

	   exec GGPlatform.usp_SetObjectArray @TypeQuery = 3, @ObjectID = @ObjectID,  @Array = @Users
	   exec GGPlatform.usp_SetObjectArray @TypeQuery = 4, @ObjectID = @ObjectID,  @Array = @Assemblys

	   select @WorkplaceID = @ObjectID 
	   goto GoodExit
    end--изменить рабочее место

	if (@TypeQuery = 4)
	begin
	  if exists(select id from GGPlatform.Workplaces where Upper(Name) = Upper(Rtrim(Ltrim(@Name))))
      begin
        select @ErrMsg = 'ђабочее место ' + @Name + ' уже зарегистрирован в системе.'
        goto ErrExit          
      end
           
	  select @maxOrder = max(OrderID) from GGPlatform.Workplaces 
		     
      insert into GGPlatform.Workplaces(Name, Description, OrderID)         
      values(@Name, @Description, @maxOrder + 1)
 
	  select @WorkplaceID = @@IDENTITY

      if ((@WorkplaceID = -1) or (@WorkplaceID is null))
      begin
        select @ErrMsg = 'Ћшибка добавлениЯ рабочего места в системные таблицы.'
        goto ErrExit          
      end
        
	  exec GGPlatform.usp_SetObjectArray @TypeQuery = 3, @ObjectID = @WorkplaceID,  @Array = @Users
      exec GGPlatform.usp_SetObjectArray @TypeQuery = 4, @ObjectID = @WorkplaceID,  @Array = @Assemblys

	  goto GoodExit
    end--добавить 

	if (@TypeQuery = 5)
	begin	  
	  select @Name = Name from GGPlatform.Workplaces where Id = @ObjectID
        
      if @Name is null
      begin
        select @ErrMsg = 'ђабочее место с идентификатором ' + cast(@ObjectID as nvarchar(10)) + ' не найдено.'
        goto ErrExit                              
      end

	  delete from GGPlatform.MatrixWorkplaceAssemblys where WorkplaceRef = @ObjectID
	  delete from GGPlatform.MatrixUsersWorkplace where WorkplaceRef = @ObjectID
      delete from GGPlatform.Workplaces where Id = @ObjectID

	  select @WorkplaceID = @ObjectID
	  goto GoodExit
	end--удалить

	
	GoodExit:
		if @@trancount > 0 commit transaction
		select @WorkplaceID as WorkplaceID
		return @WorkplaceID

	ErrExit:
		if @@trancount > 0 rollback transaction
		raiserror(@ErrMsg, 18, 1)
		select -1 as WorkplaceID
		return -1

END



go




GO
/****** Object:  StoredProcedure [GGPlatform].[usp_UsersManager]    Script Date: 05.07.2017 15:48:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*

exec [GGPlatform].[usp_UsersManager] @TypeQuery = 5, @ObjectID = 6

exec [GGPlatform].[usp_UsersManager] @TypeQuery = 4, @Login = 'gg', 
       @Fam = 'gg', @Im = 'hh', @Otch = 'hhh', 
	   @PostRef = 0, @SectionRef = 0, 
	   @isWindowsUser = 0, @isAdmin = 1, 
	   @Workplaces = '1;2;', @ReportGroups = '1;'

*/


-- =============================================
ALTER PROCEDURE [GGPlatform].[usp_UsersManager] 
                @TypeQuery int,
				@ObjectID int = null,

				@Login    sysname = NULL,
                @Password nvarchar(50) = NULL,
                @Fam      nvarchar(40) = NULL,
                @Im       nvarchar(30) = NULL,
                @Otch     nvarchar(40) = NULL,
                @PostRef    int = NULL, 
				@SectionRef int = null,
                @isWindowsUser bit = NULL, 
                @isAdmin   bit = 0,
				@isDropped bit = 0,
				@isAudit bit = 0,

				@Workplaces varchar(8000) = null,
				@ReportGroups varchar(8000) = null
AS
BEGIN
	--begin tran
	declare @ErrMsg nvarchar(200)
	declare @UserID int
	declare @Result int

	select @UserID = -1
	select @Result = -1

	if (@TypeQuery = 1)
	begin
		select g.ID
		from GGPlatform.Workplaces g
		 inner join GGPlatform.MatrixUsersWorkplace m on m.WorkplaceRef = g.ID
		 inner join GGPlatform.Users u on m.UsersRef = u.ID
		where u.ID = @ObjectID
		order by g.ID
	end--рабочие места

	if (@TypeQuery = 2)
	begin
		select g.ID
		from GGPlatform.ReportGroups g
		 inner join GGPlatform.MatrixUsersReportGroup m on m.ReportGroupRef = g.ID
		 inner join GGPlatform.Users u on m.UsersRef = u.ID
		where u.ID = @ObjectID
		order by g.ID
	end--группы отчетов

	if (@TypeQuery = 3)
    begin
	   begin tran

       if @ObjectID is null 
       begin
         select @ErrMsg = 'Ќе указан идентификатор пользователЯ.'
         goto ErrExit                              
       end

       update GGPlatform.Users 
	   set  Fam = @Fam, 
			Im = @Im, 
			Otch = @Otch, 
			PostRef = @PostRef, 
			SectionRef = @SectionRef, 
			isAdmin = @isAdmin,
			isAudit = @isAudit
       where ID  = @ObjectID

	   exec GGPlatform.usp_SetObjectArray @TypeQuery = 1, @ObjectID = @ObjectID,  @Array = @Workplaces
	   exec GGPlatform.usp_SetObjectArray @TypeQuery = 2, @ObjectID = @ObjectID,  @Array = @ReportGroups

	   select @UserID = @ObjectID 
	   goto GoodExit
    end--изменить пользователЯ

	if (@TypeQuery = 4)
	begin
	  if exists(select id from GGPlatform.Users where Upper(Login) = Upper(Rtrim(Ltrim(@Login))))
      begin
        select @ErrMsg = 'Џользователь ' + @Login + ' уже зарегистрирован в системе.'
        goto ErrExit          
      end
             
      insert into GGPlatform.Users(Login, Fam, Im, Otch, PostRef, SectionRef, isAdmin, isDropped, isWindowsUser, isAudit)         
      values(@Login, @Fam, @Im, @Otch, @PostRef, @SectionRef, @isAdmin, 0, @isWindowsUser, @isAudit)
 
	  select @UserID = @@IDENTITY

      if ((@UserID = -1) or (@UserID is null))
      begin
        select @ErrMsg = 'Ћшибка добавлениЯ пользователЯ в системные таблицы.'
        goto ErrExit          
      end
        
      if @isWindowsUser = 0  -- аутентификациЯ SQL Server
      begin --добавлЯю пользователЯ ‘Љ‹
        exec @Result = sp_addlogin  @loginame = @Login, @passwd = @Password, @defdb = 'master', @deflanguage = 'russian' 

        if @Result = 1
        begin
          select @ErrMsg = 'Ћшибка добавлениЯ имени пользователЯ в Ѓ„ (sp_addlogin).'
          delete from GGPlatform.Users where id = @UserID
          goto ErrExit                              
        end                  
      end
        else                        -- аутентификациЯ Windows
      begin
        exec @Result = sp_grantlogin @loginame = @Login

        if @Result = 1
        begin
          select @ErrMsg = 'Ћшибка добавлениЯ имени пользователЯ в Ѓ„ (sp_grantlogin).'
          delete from GGPlatform.Users where id = @UserID
          goto ErrExit                              
        end      
      end

      exec @Result = sp_grantdbaccess @loginame = @Login     ---получить доступ к базе                                               
   
      if @Result = 1
      begin
        select @ErrMsg = 'Ћшибка добавлениЯ имени пользователЯ в Ѓ„ (sp_grantdbaccess).'
        delete from GGPlatform.Users where id = @UserID              
        
		if @isWindowsUser = 1
           exec sp_revokelogin @login   ---- удалЯем пользователЯ ‚инды
        else 
           exec sp_droplogin @login     ---- удалЯем пользователЯ ‘Љ‹

        goto ErrExit                              
      end

      if @isAdmin = 0
	  begin
        execute @Result = sp_addrolemember @rolename = 'sysUsers' , @membername = @Login ----включаем в роль
	  end
       else 
	  begin
        execute @Result = sp_addrolemember @rolename = 'db_owner' , @membername = @Login 
        execute @Result = sp_addsrvrolemember @loginame = @Login, @rolename = 'sysAdmins'   ----включаем в стандартную роль сисадмин
      end

      if @Result = 1
      begin
        select @ErrMsg = 'Ћшибка добавлениЯ пользователЯ в системную группу.'
        delete from GGPlatform.Users where id = @UserID              

        if @isWindowsUser = 1
           exec sp_revokelogin @login
        else 
           exec sp_droplogin @login

        goto ErrExit                              
      end

	  exec GGPlatform.usp_SetObjectArray @TypeQuery = 1, @ObjectID = @UserID,  @Array = @Workplaces
	  exec GGPlatform.usp_SetObjectArray @TypeQuery = 2, @ObjectID = @UserID,  @Array = @ReportGroups

	  goto GoodExit
    end--добавить пользователЯ

	if (@TypeQuery = 5)
	begin	  
	  select @Login = Login, @isWindowsUser = isWindowsUser, @isDropped = isDropped from GGPlatform.Users where Id = @ObjectID
        
      if @Login is null
      begin
        select @ErrMsg = 'Џользователь с идентификатором ' + cast(@ObjectID as nvarchar(10)) + ' не найден.'
        goto ErrExit                              
      end

      if @isDropped = 0
         exec @Result = sp_revokedbaccess @Login

      if @isWindowsUser = 1 
         exec @Result = sp_revokelogin @Login
      else 
         exec @Result = master..sp_droplogin @Login

	 -- begin tran

	  delete from GGPlatform.MatrixUsersWorkplace where UsersRef = @ObjectID
	  delete from GGPlatform.MatrixUsersReportGroup where UsersRef = @ObjectID

      if @Result = 1
      begin
        update GGPlatform.Users set isDropped = 1 where Id = @ObjectID

        select @ErrMsg = 'Ќе удалось удалить пользователЯ ' + @login + ' (' +case when @isWindowsUser = 1 then 'sp_revokelogin' else 'sp_droplogin' end + ' ).'
        goto ErrExit                              
      end
	  else
	    delete from GGPlatform.Users where Id = @ObjectID

	  select @UserID = @ObjectID
	  goto GoodExit
	end--удалить

	
	GoodExit:
		if @@trancount > 0 commit transaction
		select @UserID as UserID
		return @UserID

	ErrExit:
		if @@trancount > 0 rollback transaction
		raiserror(@ErrMsg, 18, 1)
		select -1 as UserID
		return -1

END


