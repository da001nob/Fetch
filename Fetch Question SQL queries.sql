
-- What are the top 5 brands by receipts scanned for most recent month?
select top 5 
    b.brandCode
from [FetchData].[dbo].[Receipts] r
inner join [FetchData].[dbo].[rewardsReceiptItemList] rl   ON r.userId = rl.userId
inner join [FetchData].[dbo].Brands b  on rl.RewardsProductPartnerId = b.cpg
where r.dateScanned_date >'2021-01-31 00:00:00.000' and  r.dateScanned_date <= '2021-02-28 00:00:00.000'
group by b.brandCode
order by COUNT(*) DESC;

--Which brand has the most spend among users who were created within the past 6 months?
select b.brandCode ,sum(r.totalspent)as totalSpent,r.userId
from [FetchData].[dbo].[Receipts] r
inner join [FetchData].[dbo].[rewardsReceiptItemList] rl   ON r.userId = rl.userId
inner join [FetchData].[dbo].Brands b  on rl.RewardsProductPartnerId = b.cpg
where r.createDate_date <'2021-02-28 00:00:00.000' and  r.createDate_date >= '2020-09-01 00:00:00.000'
group by b.brandCode,r.userId
order by totalSpent desc

--Which brand has the most transactions among users who were created within the past 6 months?
select  r.rec_Id,count(*) AS TransactionCount ,b.name 
from [FetchData].[dbo].[Receipts] r
inner join [FetchData].[dbo].[rewardsReceiptItemList] rl   ON r.userId = rl.userId
inner join [FetchData].[dbo].Brands b  on rl.RewardsProductPartnerId = b.cpg
where r.userId IN (select userId
from [FetchData].[dbo].[Users]
where createDate_date < '2021-02-28 00:00:00.000' AND createDate_date >= '2020-09-01 00:00:00.000')
group by  r.rec_Id ,b.name  
order by TransactionCount desc;


