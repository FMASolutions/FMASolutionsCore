using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

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
                SELECT OrderItemID,OrderHeaderID,ItemID,OrderItemStatusID,,OrderItemUnitPrice,OrderItemUnitPriceAfterDiscount,OrderItemQty,OrderItemDescription
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

        public IEnumerable<OrderItemEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT OrderItemID,OrderHeaderID,ItemID,OrderItemStatusID, OrderItemUnitPrice,OrderItemUnitPriceAfterDiscount,OrderItemQty,OrderItemDescription
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
                INSERT INTO OrderItems(OrderHeaderID,ItemID,OrderItemStatusID,OrderItemUnitPrice,OrderItemUnitPriceAfterDiscount,OrderItemQty,OrderItemDescription)
                VALUES (@OrderHeaderID,@ItemID,@OrderItemStatusID, @OrderItemUnitPrice,@OrderItemUnitPriceAfterDiscount,@OrderItemQty,@OrderItemDescription)";

                Helper.logger.WriteToProcessLog("OrderItemRepo.Create Started for OrderHeader ID: " + entity.OrderHeaderID + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    OrderHeaderID = entity.OrderHeaderID,
                    ItemID = entity.ItemID,
                    OrderItemStatusID = entity.OrderItemStatusID,
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
                , OrderItemStatusID = @OrderItemStatusID
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
                    OrderItemStatusID = entity.OrderItemStatusID,                
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
        public IEnumerable<DTOOrderItemDetailed> GetOrderItemsDetailedForOrder(int orderHeaderID)
        {
            try
            {
                string query = @"
                SELECT ordHead.OrderHeaderID, ordItm.OrderItemID, ordItm.OrderItemDescription, ordItm.OrderItemQty
                , ordItm.OrderItemUnitPrice, ordItm.OrderItemUnitPriceAfterDiscount, oiStatus.OrderStatusID AS [OrderItemStatusID]
                , oiStatus.OrderstatusValue AS [OrderItemStatusValue], itm.ItemID, itm.ItemCode, itm.ItemImageFilename, sub.SubGroupID, sub.SubGroupCode, sub.SubGroupName
                , sub.SubGroupDescription, prod.ProductGroupID, prod.ProductGroupCode, prod.ProductGroupName, prod.ProductGroupDescription
                FROM OrderHeaders ordHead
                INNER JOIN OrderItems ordItm ON ordHead.OrderHeaderID = ordItm.OrderHeaderID                
                INNER JOIN OrderStatus oiStatus ON oiStatus.OrderStatusID = ordItm.OrderItemStatusID                
                INNER JOIN Items itm ON itm.ItemID = ordItm.ItemID
                INNER JOIN SubGroups sub ON itm.SubGroupID = sub.SubGroupID
                INNER JOIN ProductGroups prod ON sub.ProductGroupID = prod.ProductGroupID                
                WHERE ordHead.OrderHeaderID = @OrderHeaderID 
                ORDER BY ordItm.OrderItemID ASC
                ";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetAmendOrderItemsDTO Started: " + query);
                return _dbConnection.Query<DTOOrderItemDetailed>(query, new { OrderHeaderID = orderHeaderID }, transaction: Transaction);             
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetAmendOrderItemsDTO: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<OrderItemEntity> GetOrderItemsForOrder(int orderHeaderID)
        {
            try
            {
                string query = @"
                SELECT OrderItemID,OrderHeaderID,ItemID,OrderItemStatusID,,OrderItemUnitPrice,OrderItemUnitPriceAfterDiscount,OrderItemQty,OrderItemDescription
                FROM OrderItems
                WHERE OrderHeaderID = @OrderHeaderID
                ";

                Helper.logger.WriteToProcessLog("OrderItemRepo.GetByID Started for ID: " + orderHeaderID.ToString() + " full query = " + query);

                return _dbConnection.Query<OrderItemEntity>(query, new { OrderHeaderID = orderHeaderID }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderItemRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
    }    
}