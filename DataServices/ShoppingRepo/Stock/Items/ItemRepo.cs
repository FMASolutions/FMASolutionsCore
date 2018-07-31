using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.AppLoggerExtension;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class ItemRepo : IItemRepo, IDisposable
    {
        public ItemRepo(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
        private IDbConnection _dbConnection;        

        #region IDataRepository
        public ItemEntity GetByID(int id)
        {
            try
            {                   
                string query = @"
                SELECT [ItemID],[ItemCode],[SubGroupID],[ItemName],[ItemDescription],[ItemUnitPrice],[ItemUnitPriceWithMaxDiscount],[ItemAvailableQty],[ItemReorderQtyReminder],[ItemImageFilename]
                FROM Items
                WHERE ItemID = @ItemID
                ";

                Helper.logger.WriteToProcessLog("ItemRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<ItemEntity>(query, new { ItemID = id });
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ItemRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<ItemEntity> GetAll()
        {
            try
            {                
                string query = @"
                SELECT [ItemID],[ItemCode],[SubGroupID],[ItemName],[ItemDescription],[ItemUnitPrice],[ItemUnitPriceWithMaxDiscount],[ItemAvailableQty],[ItemReorderQtyReminder],[ItemImageFilename]
                FROM Items";

                Helper.logger.WriteToProcessLog("ItemRepo.GetAll Running Query: " + query);

                return _dbConnection.Query<ItemEntity>(query);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ItemRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(ItemEntity entity)
        {
            try
            {                
                string query = @"
                INSERT INTO Items([ItemCode],[SubGroupID],[ItemName],[ItemDescription],[ItemUnitPrice],[ItemUnitPriceWithMaxDiscount],[ItemAvailableQty],[ItemReorderQtyReminder],[ItemImageFilename])
                VALUES (@ItemCode,@SubGroupID,@ItemName,@ItemDescription,@ItemUnitPrice,@ItemUnitPriceWithMaxDiscount,@ItemAvailableQty,@ItemReorderQtyReminder,@ItemImageFilename)";
                
                Helper.logger.WriteToProcessLog("ItemRepo.Create Started for code: " + entity.ItemCode + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    ItemCode = entity.ItemCode,
                    SubGroupID = entity.SubGroupID,
                    ItemName = entity.ItemName,
                    ItemDescription = entity.ItemDescription,
                    ItemUnitPrice = entity.ItemUnitPrice,
                    ItemUnitPriceWithMaxDiscount = entity.ItemUnitPriceWithMaxDiscount,
                    ItemAvailableQty = entity.ItemAvailableQty,
                    ItemReorderQtyReminder = entity.ItemReorderQtyReminder,
                    ItemImageFilename = entity.ItemImageFilename
                });
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex) 
            {
                Helper.logger.WriteToErrorLog("Error in ItemRepo.Create: " + ex.Message, this);
                return false;
            }
        }

        public bool Update(ItemEntity entity)
        {
            try
            {                
                string query = @"
                UPDATE Items 
                SET [ItemCode] = @ItemCode
                ,[SubGroupID] = @SubGroupID
                ,[ItemName] = @ItemName
                ,[ItemDescription] = @ItemDescription
                ,[ItemUnitPrice] = @ItemUnitPrice
                ,[ItemUnitPriceWithMaxDiscount] = @ItemUnitPriceWithMaxDiscount
                ,[ItemAvailableQty] = @ItemAvailableQty
                ,[ItemReorderQtyReminder] = @ItemReorderQtyReminder
                ,[ItemImageFilename] = @ItemImageFilename
                WHERE ItemID = @ItemID";

                Helper.logger.WriteToProcessLog("ItemRepo.Update Started for ID: " + entity.ItemID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    ItemID = entity.ItemID
                    ,ItemCode = entity.ItemCode
                    ,SubGroupID = entity.SubGroupID
                    ,ItemName = entity.ItemName
                    ,ItemDescription = entity.ItemDescription
                    ,ItemUnitPrice = entity.ItemUnitPrice
                    ,ItemUnitPriceWithMaxDiscount = entity.ItemUnitPriceWithMaxDiscount
                    ,ItemAvailableQty = entity.ItemAvailableQty
                    ,ItemReorderQtyReminder = entity.ItemReorderQtyReminder
                    ,ItemImageFilename = entity.ItemImageFilename
                });
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ItemRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(ItemEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IItemRepo
        public Int32 GetNextAvailableID()
        {
            try
            {
                Helper.logger.WriteToProcessLog("ItemRepo.GetNextAvailableID Started");

                return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(ItemID),0)+1 FROM Items");
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ItemRepo.GetNextAvailableID: " + ex.Message, this);
                return -1;
            }
        }
        public ItemEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [ItemID],[ItemCode],[SubGroupID],[ItemName],[ItemDescription],[ItemUnitPrice],[ItemUnitPriceWithMaxDiscount],[ItemAvailableQty],[ItemReorderQtyReminder],[ItemImageFilename]
                FROM Items
                WHERE ItemCode = @ItemCode
                ";

                Helper.logger.WriteToProcessLog("ItemRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<ItemEntity>(query, new { ItemCode = code });
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ItemRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion

    }
}