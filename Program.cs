using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Fetch1
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "GENESIS_ONE\\SQLEXPRESS",
                InitialCatalog = "Fetchdata",
                UserID = "sa",
                Password = "nCC1701"
            };
            string connectionString = builder.ConnectionString;

            //////////user table
            string filePath = @"C:\Fetch\users.json\usersbk.json";
            List<User> users = new List<User>();
            try
            {


                try
                {
                    // Read the entire JSON file as an array
                    string jsonText = File.ReadAllText(filePath);

                    // Deserialize JSON directly into a JArray
                    JArray jsonArray = JArray.Parse(jsonText);

                    foreach (JObject obj in jsonArray) // Iterate over each object
                    {
                        User user = new User
                        {

                            Id = Guid.NewGuid(),
                            userid = obj["_id"]?["$oid"]?.ToString() ?? "Unknown", // Handles missing `_id`
                            Active = obj["active"]?.ToObject<bool>() ?? false, // Defaults to `false`
                            CreatedDate = obj["createdDate"] != null ? ConvertMongoDate(obj["createdDate"]) : new DateTime(1753, 1, 1),
                            LastLogin = obj["lastLogin"] != null ? ConvertMongoDate(obj["lastLogin"]) : new DateTime(1753, 1, 1),
                            Role = obj["role"]?.ToString() ?? "Unknown",
                            SignUpSource = obj["signUpSource"]?.ToString() ?? "Unknown",
                            State = obj["state"]?.ToString() ?? "NY"
                        };

                        users.Add(user);
                    }

                    Console.WriteLine($"Loaded {users.Count} users from JSON.");
                    // Insert data into the database
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            Console.WriteLine("Database connection opened successfully.");

                            string query = @"
                            insert into [dbo].[Users] (
                                _id,
                                userId,
                                active,
                                createdDate,
                                lastLogin,
                                role,
                                signUpSource,
                                state
                            )
                            VALUES (
                                @_id,
                                @userId,
                                @active,
                                @createdDate,
                                @lastLogin,
                                @role,
                                @signUpSource,
                                @state
                            );";


                            foreach (var user in users)
                            {
                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@_id", user.Id);
                                    command.Parameters.AddWithValue("@userId", user.userid);
                                    command.Parameters.AddWithValue("@active", user.Active);
                                    command.Parameters.AddWithValue("@createdDate", user.CreatedDate);
                                    command.Parameters.AddWithValue("@lastLogin", user.LastLogin);
                                    command.Parameters.AddWithValue("@role", user.Role);
                                    command.Parameters.AddWithValue("@signUpSource", user.SignUpSource);
                                    command.Parameters.AddWithValue("@state", user.State);

                                    command.ExecuteNonQuery();
                                }
                            }

                            Console.WriteLine("Data inserted successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Database error: {ex.Message}");
                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading JSON: {ex.Message}");
                    return;
                }
            }
            catch (Exception)
            {

                throw;
            }


            //////////brand table

            string filePathBrand = @"C:\Fetch\brands.json\brandsArray.json";
            // List<Brand> brands = new List<Brand>();
            try
            {


                try
                {

                    string jsonText = File.ReadAllText(filePathBrand);


                    JArray jsonArray = JArray.Parse(jsonText);

                    foreach (JObject obj in jsonArray)
                    {
                        Brand brand = new Brand();
                       
                        brand.Id = Guid.NewGuid();
                        brand._id = (string)obj["_id"]["$oid"];//?.ToString() ?? "Unknown",
                        brand.barcode = (string)obj["barcode"]?.ToString() ?? "Unknown";
                        brand.brandCode = obj["brandCode"]?.ToString() ?? "Unknown";
                        brand.category = obj["category"]?.ToString() ?? "Unknown";
                        brand.categoryCode = obj["categoryCode"]?.ToString() ?? "Unknown";
                        brand.cpg = obj["cpg"]["$id"]["$oid"]?.ToString() ?? "Unknown";
                        brand._ref = obj["cpg"]?["$ref"]?.ToString() ?? "Unknown";
                        brand.name = obj["name"]?.ToString() ?? "Unknown";
                        brand.topBrand = obj["topBrand"]?.ToObject<bool>() ?? false;

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();

                                string query = @"
                            insert into [dbo].[Brands] (
                                _id,
                                brandId,
                                barcode,
                                brandCode,
                                
                                category,
                                categoryCode,
                                cpg,
                                _ref,
                                name,
                                topBrand
                            )
                            values (
                                @Id,
                                @_id,
                                @barcode,
                                @brandId,
                                
                                @category,
                                @categoryCode,
                                @cpg,
                                @cpg_ref,
                                @name,
                                @topBrand
                            );";

                                // foreach (var brand in brands)
                                //  {
                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@Id", brand.Id);
                                    command.Parameters.AddWithValue("@_id", brand._id);
                                    command.Parameters.AddWithValue("@brandId", brand.brandCode ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@barcode", brand.barcode ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@category", brand.category ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@categoryCode", brand.categoryCode ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@cpg", brand.cpg ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@cpg_ref", brand.cpg ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@name", brand.name ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@topBrand", brand.topBrand);

                                    command.ExecuteNonQuery();
                                }
                                //     }

                                Console.WriteLine("Data inserted successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Database error: {ex.Message}");
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading JSON: {ex.Message}");
                    return;
                }
                // Console.WriteLine($"Loaded {brands.Count} users from JSON.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON: {ex.Message}");
                return;
            }


            ////////////reciepte table

            string filePathReciept = @"C:\Fetch\receipts.json\receipts.json";
            // List<Brand> brands = new List<Brand>();
            try
            {


                try
                {

                    string jsonText = File.ReadAllText(filePathReciept);


                    JArray jsonArray = JArray.Parse(jsonText);


                    List<Receipts> rootObjects = new List<Receipts>();

                    foreach (JObject obj in jsonArray)
                    {

                        Receipts root = new Receipts
                        {
                            Id = Guid.NewGuid(),

                            rec_Id = (string)obj["_id"]["$oid"],
                            bonusPointsEarned = obj["bonusPointsEarned"]?.ToString() ?? "Unknown",
                            bonusPointsEarnedReason = obj["bonusPointsEarnedReason"]?.ToString() ?? "Unknown",
                            CreateDate = obj["createDate"] != null ? ConvertMongoDate(obj["createDate"]) : DateTime.MinValue,
                            dateScanned = obj["dateScanned"] != null ? ConvertMongoDate(obj["dateScanned"]) : DateTime.MinValue,
                            finishedDate = obj["finishedDate"] != null ? ConvertMongoDate(obj["finishedDate"]) : DateTime.MinValue,
                            modifyDate = obj["modifyDate"] != null ? ConvertMongoDate(obj["modifyDate"]) : DateTime.MinValue,
                            pointsAwardedDate = obj["pointsAwardedDate"] != null ? ConvertMongoDate(obj["pointsAwardedDate"]) : DateTime.MinValue,
                            pointsEarned = obj["pointsEarned"]?.ToString() ?? "Unknown",
                            purchaseDate = obj["purchaseDate"] != null ? ConvertMongoDate(obj["purchaseDate"]) : DateTime.MinValue,
                            purchasedItemCount = obj["purchasedItemCount"]?.ToString() ?? "Unknown",
                            rewardsReceiptItemList = obj["rewardsReceiptItemList"]?.ToString() ?? "Unknown",
                            rewardsReceiptStatus = obj["rewardsReceiptStatus"]?.ToString() ?? "Unknown",
                            totalSpent = obj["totalSpent"]?.ToString() ?? "Unknown",
                            userId = obj["userId"]?.ToString() ?? "Unknown"
                        };



                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();

                                string query = @"
                            insert into [dbo].[Receipts] (
                                _id,
                                rec_Id,
                                bonusPointsEarned,
                                bonusPointsEarnedReason,
                                createDate_date,
                                dateScanned_date,
                                finishedDate_date,
                                modifyDate_date,
                                pointsAwardedDate_date,
                                pointsEarned,
                                purchaseDate_date,
                                purchasedItemCount,
                                rewardsReceiptItemList,
                                rewardsReceiptStatus,
                                totalSpent,
                                userId
                            )
                            values (
                                @Id,
                                @rec_Id,    
                                @bonusPointsEarned,
                                @bonusPointsEarnedReason,
                                @createDate,
                                @dateScanned,
                                @finishedDate,
                                @modifyDate,
                                @pointsAwardedDate,
                                @pointsEarned,
                                @purchaseDate,
                                @purchasedItemCount,
                                @rewardsReceiptItemList,
                                @rewardsReceiptStatus,
                                @totalSpent,
                                @userId

                            );";


                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@Id", root.Id);
                                    command.Parameters.AddWithValue("@rec_Id", root.rec_Id);
                                    command.Parameters.AddWithValue("@bonusPointsEarned", root.bonusPointsEarned ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@bonusPointsEarnedReason", root.bonusPointsEarnedReason ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@createDate", root.CreateDate);
                                    command.Parameters.AddWithValue("@dateScanned", root.dateScanned);
                                    command.Parameters.AddWithValue("@finishedDate", root.dateScanned);
                                    command.Parameters.AddWithValue("@modifyDate", root.modifyDate);
                                    command.Parameters.AddWithValue("@pointsAwardedDate", root.modifyDate);

                                    command.Parameters.AddWithValue("@pointsEarned", root.pointsEarned ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@purchaseDate", root.purchaseDate);
                                    command.Parameters.AddWithValue("@purchasedItemCount", root.purchasedItemCount ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@rewardsReceiptItemList", root.rewardsReceiptStatus ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@rewardsReceiptStatus", root.rewardsReceiptStatus ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@totalSpent", root.totalSpent ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@userId", root.userId ?? (object)DBNull.Value);



                                    command.ExecuteNonQuery();
                                }

                                if (obj["rewardsReceiptItemList"] is JArray itemsArray)
                                {
                                    foreach (JObject item in itemsArray)
                                    {

                                        string rec_Id = (string)obj["_id"]["$oid"];
                                        string barcode = item["barcode"]?.ToString() ?? "Unknown";
                                        string description = item["description"]?.ToString() ?? "Unknown";
                                        string finalPrice = item["finalPrice"]?.ToString() ?? "Unknown";

                                        string itemPrice = item["itemPrice"]?.ToString() ?? "Unknown";
                                        string needsFetchReview = item["needsFetchReview"]?.ToString() ?? "Unknown";
                                        string partnerItemId = item["partnerItemId"]?.ToString() ?? "Unknown";
                                        string preventTargetGapPoints = item["preventTargetGapPoints"]?.ToString() ?? "Unknown";
                                        string quantityPurchased = item["quantityPurchased"]?.ToString() ?? "Unknown";
                                        string userFlaggedBarcode = item["userFlaggedBarcode"]?.ToString() ?? "Unknown";
                                      
                                        string userFlaggedNewItem = item["userFlaggedNewItem"]?.ToString() ?? "Unknown";
                                        string userFlaggedPrice = item["userFlaggedPrice"]?.ToString() ?? "Unknown";
                                        string userFlaggedQuantity = item["userFlaggedQuantity"]?.ToString() ?? "Unknown";


                                      //
                                        
                                        string competitiveProduct = item["competitiveProduct"]?.ToString() ?? "Unknown";
                                        string discountedItemPrice = item["discountedItemPrice"]?.ToString() ?? "Unknown";
                                        string itemNumber = item["itemNumber "]?.ToString() ?? "Unknown";
                                        string originalMetaBriteBarcode = item["originalMetaBriteBarcode"]?.ToString() ?? "Unknown";
                                        string originalReceiptItemText = item["originalReceiptItemText"]?.ToString() ?? "Unknown";
                                        //string partnerItemId = item["partnerItemId"]?.ToString() ?? "Unknown";
                                       // string quantityPurchased = item["quantityPurchased"]?.ToString() ?? "Unknown";
                                        string rewardsGroup = item["rewardsGroup"]?.ToString() ?? "Unknown";
                                        string rewardsProductPartnerId = item["rewardsProductPartnerId"]?.ToString() ?? "Unknown";
                                        string pointsEarned = item["pointsEarned"]?.ToString() ?? "Unknown";
                                        string pointsPayerId = item["pointsPayerId"]?.ToString() ?? "Unknown";
                                        //

                                     

                                        string query1 = @"
                                        insert into [dbo].[rewardsReceiptItemList] (
                                            _id,
                                            rec_Id,
                                            userId,
                                            barcode,
                                            competitiveProduct,
                                            description,
                                            discountedItemPrice,
                                            finalPrice,
                                            itemNumber,
                                            itemPrice,
                                            originalMetaBriteBarcode,
                                            originalReceiptItemText,
                                            partnerItemId,
                                            quantityPurchased,
                                            needsFetchReview,
                                            RewardsGroup,
                                            
                                            RewardsProductPartnerId,
                                            PointsEarned,
                                            PointsPayerId,

                                            userFlaggedNewItem,
                                            userFlaggedPrice,
                                            userFlaggedQuantity
                                            
                                        )
                                        values (
                                            @Id,
                                            @rec_Id,
                                            @userId,
                                            @barcode,
                                            @CompetitiveProduct,
                                            @description,    
                                            @DiscountedItemPrice,
                                            @finalPrice,
                                            @itemNumber,
                                            @itemPrice,
                                            @originalMetaBriteBarcode,
                                            @originalReceiptItemText,
                                            @partnerItemId,
                                            @quantityPurchased,
                                            @needsFetchReview,
                                            @RewardsGroup,
                                            
                                            @RewardsProductPartnerId,
                                            @PointsEarned,
                                            @PointsPayerId,

                                            @userFlaggedNewItem,
                                            @userFlaggedPrice,
                                            @userFlaggedQuantity
                                            
                                            
                                            
                                           

                        

                                        );";
                                        using (SqlCommand command = new SqlCommand(query1, connection))
                                        {

                                            command.Parameters.AddWithValue("@Id", root.Id);
                                            command.Parameters.AddWithValue("@rec_Id", root.rec_Id);
                                            command.Parameters.AddWithValue("@userId", root.userId ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@barcode", barcode ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@CompetitiveProduct", competitiveProduct ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@description", description ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@DiscountedItemPrice", discountedItemPrice ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@finalPrice", finalPrice ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@ItemNumber", itemNumber ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@itemPrice", itemPrice ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@OriginalMetaBriteBarcode", originalMetaBriteBarcode ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@OriginalReceiptItemText", originalReceiptItemText ?? (object)DBNull.Value);


                                          
                                            command.Parameters.AddWithValue("@partnerItemId", partnerItemId ?? (object)DBNull.Value);
                                          
                                            command.Parameters.AddWithValue("@quantityPurchased", quantityPurchased ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@needsFetchReview", needsFetchReview ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@RewardsGroup", rewardsGroup ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@RewardsProductPartnerId", rewardsProductPartnerId ?? (object)DBNull.Value);


                                            command.Parameters.AddWithValue("@PointsEarned", pointsEarned ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@PointsPayerId", pointsPayerId ?? (object)DBNull.Value);

									
											command.Parameters.AddWithValue("@userFlaggedNewItem", userFlaggedNewItem ?? (object)DBNull.Value);
											command.Parameters.AddWithValue("@userFlaggedPrice", userFlaggedPrice ?? (object)DBNull.Value);
											command.Parameters.AddWithValue("@userFlaggedQuantity", userFlaggedQuantity ?? (object)DBNull.Value);

                                            //command.Parameters.AddWithValue("@userFlaggedBarcode", userFlaggedBarcode ?? (object)DBNull.Value);



                                            //    command.Parameters.AddWithValue("@preventTargetGapPoints", preventTargetGapPoints ?? (object)DBNull.Value);







                                            command.ExecuteNonQuery();
                                            //command.Parameters.AddWithValue("@Id", root.Id);
                                            //command.Parameters.AddWithValue("@userId", root.userId ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@barcode", barcode ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@description", description ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@finalPrice", finalPrice ?? (object)DBNull.Value);

                                            //command.Parameters.AddWithValue("@itemPrice", itemPrice ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@needsFetchReview", needsFetchReview ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@partnerItemId", partnerItemId ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@preventTargetGapPoints", preventTargetGapPoints ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@quantityPurchased", quantityPurchased ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@userFlaggedBarcode", userFlaggedBarcode ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@userFlaggedNewItem", userFlaggedNewItem ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@userFlaggedPrice", userFlaggedPrice ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@userFlaggedQuantity", userFlaggedQuantity ?? (object)DBNull.Value);


                                            //command.Parameters.AddWithValue("@CompetitiveProduct", competitiveProduct ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@DiscountedItemPrice", discountedItemPrice ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@ItemNumber", itemNumber ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@OriginalMetaBriteBarcode", originalMetaBriteBarcode ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@OriginalReceiptItemText", originalReceiptItemText ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@RewardsGroup", rewardsGroup ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@RewardsProductPartnerId", rewardsProductPartnerId ?? (object)DBNull.Value);

                                            //command.Parameters.AddWithValue("@PointsEarned", pointsEarned ?? (object)DBNull.Value);
                                            //command.Parameters.AddWithValue("@PointsPayerId", pointsPayerId ?? (object)DBNull.Value);



                                            //command.ExecuteNonQuery();
                                        }
                                        Console.WriteLine($"Barcode: {barcode}, Description: {description}, Final Price: {finalPrice}");
                                    }
                                }



                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Database error: {ex.Message}");
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading JSON: {ex.Message}");
                    return;
                }
                // Console.WriteLine($"Loaded {brands.Count} users from JSON.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON: {ex.Message}");
                return;
            }

        }




        // Helper method to convert MongoDB $date format to DateTime
        static DateTime ConvertMongoDate(JToken token)
        {
            if (token != null && token["$date"] != null)
            {
                long unixTimestamp = token["$date"].ToObject<long>();
                return DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp).DateTime;
            }
            return DateTime.MinValue; // Default if null
        }
    }


}
