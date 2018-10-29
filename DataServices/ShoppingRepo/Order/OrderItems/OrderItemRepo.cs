using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class OrderItemRepo : BaseRepository, IOrderItemRepo, IDisposable
    {
        public OrderItemRepo(IDbTransaction transaction)
           : base(transaction)
        {
            _dbConnection = Connection;
        }
        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private IDbConnection _dbConnection;

        #region IDataRepository
        public OrderItemEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT OrderItemID,OrderHeaderID,ItemID,OrderItemUnitPrice,OrderItemUnitPriceAfterDiscount,OrderItemQty,OrderItemDescription
                FROM OrderItems
                WHERE OrderItemID = @OrderItemID
                ";

                Helper.logger.WriteToProcessLog("OrderItemRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<OrderItemEntity>(query, new { OrderItemID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderItemRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public int GetLatestOrderItemByOrder(int orderHeaderID)
        {
            try
            {
                string query = @"
                SELECT top 1 OrderItemID
                FROM OrderItems
                WHERE OrderHeaderID = @OrderHeaderID
                ORDER BY OrderItemID DESC
                ";

                Helper.logger.WriteToProcessLog("OrderItemRepo.GetLatestOrderItemByOrder Started for Order Header ID: " + orderHeaderID.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<int>(query, new { OrderHeaderID = orderHeaderID }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderItemRepo.GetLatestOrderItemByOrder: " + ex.Message, this);
                return -1;
            }
        }

        public IEnumerable<OrderItemEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT OrderItemID,OrderHeaderID,ItemID,OrderItemUnitPrice,OrderItemUnitPriceAfterDiscount,OrderItemQty,OrderItemDescription
                FROM OrderItems";

                Helper.logger.WriteToProcessLog("OrderItemRepo.GetAll Started: " + query);

                return _dbConnection.Query<OrderItemEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderItemRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(OrderItemEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO OrderItems(OrderHeaderID,ItemID,OrderItemUnitPrice,OrderItemUnitPriceAfterDiscount,OrderItemQty,OrderItemDescription)
                VALUES (@OrderHeaderID,@ItemID,@OrderItemUnitPrice,@OrderItemUnitPriceAfterDiscount,@OrderItemQty,@OrderItemDescription)";

                Helper.logger.WriteToProcessLog("OrderItemRepo.Create Started for OrderHeader ID: " + entity.OrderHeaderID + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    OrderHeaderID = entity.OrderHeaderID,
                    ItemID = entity.ItemID,
                    OrderItemUnitPrice = entity.OrderItemUnitPrice,
                    OrderItemUnitPriceAfterDiscount = entity.OrderItemUnitPriceAfterDiscount,
                    OrderItemQty = entity.OrderItemQty,
                    OrderItemDescription = entity.OrderItemDescription
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderItemRepo.Create: " + ex.Message, this);
                return false;
            }
        }
        public bool Update(OrderItemEntity entity)
        {
            try
            {
                string query = @"
                UPDATE OrderItems 
                SET OrderHeaderID = @OrderHeaderID
                , ItemID = @ItemID
                , OrderItemUnitPrice = @OrderItemUnitPrice
                , OrderItemUnitPriceAfterDiscount = @OrderItemUnitPriceAfterDiscount
                , OrderItemQty = @OrderItemQty
                , OrderItemDescription = @OrderItemDescription
                WHERE OrderItemID = @OrderItemID";

                Helper.logger.WriteToProcessLog("OrderItemRepo.Update Started for ID: " + entity.OrderItemID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    OrderHeaderID = entity.OrderHeaderID,
                    ItemID = entity.ItemID,
                    OrderItemUnitPrice = entity.OrderItemUnitPrice,
                    OrderItemUnitPriceAfterDiscount = entity.OrderItemUnitPriceAfterDiscount,
                    OrderItemQty = entity.OrderItemQty,                    
                    OrderItemDescription = entity.OrderItemDescription,
                    OrderItemID = entity.OrderItemID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderItemRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(OrderItemEntity entity)
        {
            try
            {
                string query = @"
                DELETE FROM OrderItems
                WHERE OrderItemID = @OrderItemID
                ";

                Helper.logger.WriteToProcessLog("OrderItemRepo.Delete Started for ID: " + entity.OrderItemID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new { OrderItemID = entity.OrderItemID}, transaction: Transaction);
                if(i >= 1)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderItemRepo.Delete: " + ex.Message, this);
                return false;
            }
        }
        #endregion
    }    
}