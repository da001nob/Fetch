using System;

using System.IO;

namespace Fetch1
{
	public class User
	{
		public Guid Id { get; set; }
		public string userid { get; set; }
		public bool Active { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastLogin { get; set; }
		public string Role { get; set; }
		public string SignUpSource { get; set; }
		public string State { get; set; }

	}

	public class Brand
	{
		public Guid Id { get; set; }
		public string _id { get; set; }
		public string barcode { get; set; }
		public string brandCode { get; set; }

		public string category { get; set; }
		public string categoryCode { get; set; }
		public string cpg { get; set; }
		public string _ref { get; set; }
		public string name { get; set; }
		public bool topBrand { get; set; }
	}

	public class Receipts
	{

		public Guid Id { get; set; }
		public string rec_Id { get; set; }
		public string bonusPointsEarned { get; set; }
		public string bonusPointsEarnedReason { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime dateScanned { get; set; }

		public DateTime finishedDate { get; set; }
		public DateTime modifyDate { get; set; }
		public DateTime pointsAwardedDate { get; set; }
		public string pointsEarned { get; set; }
		public DateTime purchaseDate { get; set; }

		public string purchasedItemCount { get; set; }
		public string rewardsReceiptItemList { get; set; }
		public string rewardsReceiptStatus { get; set; }
		public string totalSpent { get; set; }

		public string userId { get; set; }

		public string barcode { get; set; }
		public string description { get; set; }

		public string finalPrice { get; set; }
		public string itemPrice { get; set; }
		public bool needsFetchReview { get; set; }
		public string partnerItemId { get; set; }
		public bool preventTargetGapPoints { get; set; }

		public string quantityPurchased { get; set; }

		public string userFlaggedBarcode { get; set; }
		public bool userFlaggedNewItem { get; set; }

		public string userFlaggedPrice { get; set; }
		public string userFlaggedQuantity { get; set; }




		///////////
		public string CompetitiveProduct { get; set; }

		public string DiscountedItemPrice { get; set; }
		public string ItemNumber { get; set; }

		public string OriginalMetaBriteBarcode { get; set; }
		public string OriginalReceiptItemText { get; set; }
		public bool PartnerItemId { get; set; }
		public string QuantityPurchased { get; set; }
		public bool RewardsGroup { get; set; }

		public string RewardsProductPartnerId { get; set; }

		public string PointsEarned { get; set; }
		public bool PointsPayerId { get; set; }

		


	}


	//public class Rootobject
	//{
	//	public _Id _id { get; set; }
	//	public string barcode { get; set; }
	//	public string brandCode { get; set; }
	//	public string category { get; set; }
	//	public string categoryCode { get; set; }
	//	public Cpg cpg { get; set; }
	//	public string name { get; set; }
	//	public bool topBrand { get; set; }
	//}

	//public class _Id
	//{
	//	public string oid { get; set; }
	//}

	//public class Cpg
	//{
	//	public Id id { get; set; }
	//	public string _ref { get; set; }
	//}

	//public class Id
	//{
	//	public string oid { get; set; }
	//}

	///////////////





}

