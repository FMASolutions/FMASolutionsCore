using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class OrderHeaderRepo : BaseRepository, IOrderHeaderRepo, IDisposable
    {
        public OrderHeaderRepo(IDbTransaction transaction)
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
        public bool Create(OrderHeaderEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO OrderHeaders(CustomerID,CustomerAddressID,OrderStatusID,OrderDate,DeliveryDate)
                VALUES (@CustomerID, @CustomerAddressID, @OrderStatusID, @OrderDate,@DeliveryDate)";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.Create Started for Customer ID: " + entity.CustomerID + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CustomerID = entity.CustomerID,
                    CustomerAddressID = entity.CustomerAddressID,
                    OrderStatusID = entity.OrderStatusID,
                    OrderDate = entity.OrderDate,
                    DeliveryDate = entity.DeliveryDate
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.Create: " + ex.Message, this);
                return false;
            }
        }
        public OrderHeaderEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT OrderHeaderID,CustomerID,CustomerAddressID,OrderStatusID,OrderDate,DeliveryDate
                FROM OrderHeaders
                WHERE OrderHeaderID = @OrderHeaderID
                ";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<OrderHeaderEntity>(query, new { OrderHeaderID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<OrderHeaderEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT OrderHeaderID,CustomerID,CustomerAddressID,OrderStatusID,OrderDate,DeliveryDate
                FROM OrderHeaders";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetAll Started: " + query);

                return _dbConnection.Query<OrderHeaderEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }
        public bool Update(OrderHeaderEntity entity)
        {
            try
            {
                string query = @"
                UPDATE OrderHeaders 
                SET CustomerID = @CustomerID
                , CustomerAddressID = @CustomerAddressID
                , OrderStatusID = @OrderStatusID
                , OrderDate = @OrderDate
                , DeliveryDate = @DeliveryDate
                WHERE OrderHeaderID = @OrderHeaderID";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.Update Started for ID: " + entity.OrderHeaderID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    CustomerID = entity.CustomerID,
                    CustomerAddressID = entity.CustomerAddressID,
                    OrderStatusID = entity.OrderStatusID,
                    OrderDate = entity.OrderDate,
                    DeliveryDate = entity.DeliveryDate,
                    OrderHeaderID = entity.OrderHeaderID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.Update: " + ex.Message, this);
                return false;
            }
        }
        public bool Delete(OrderHeaderEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IOrderHeaderRepo
        
        public IEnumerable<OrderPreviewDTO> GetAllOrderPreviews()
        {
            try
            {
                string query = @"
                SELECT ordHead.OrderHeaderID AS [OrderID], ohStatus.OrderstatusValue AS [OrderStatus], cust.CustomerName AS [Customer]
                    ,ordHead.OrderDate, ordHead.DeliveryDate AS [OrderDueDate]
                FROM OrderHeaders ordHead
                INNER JOIN OrderStatus ohStatus ON ohStatus.OrderStatusID = ordHead.OrderStatusID
                INNER JOIN Customers cust ON ordHead.CustomerID = cust.CustomerID
                ";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetAllOrderPreviews Started: " + query);
                return _dbConnection.Query<OrderPreviewDTO>(query, transaction: Transaction);             
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetAllOrderPreviews: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<OrderPreviewDTO> GetOrdersByCustomerCode(string customerCode)
        {
            try
            {
                string query = @"
                SELECT ordHead.OrderHeaderID AS [OrderID], ohStatus.OrderstatusValue AS [OrderStatus], cust.CustomerName AS [Customer]
                    ,ordHead.OrderDate, ordHead.DeliveryDate AS [OrderDueDate]
                FROM OrderHeaders ordHead
                INNER JOIN OrderStatus ohStatus ON ohStatus.OrderStatusID = ordHead.OrderStatusID
                INNER JOIN Customers cust ON ordHead.CustomerID = cust.CustomerID
                WHERE cust.CustomerCode = @CustomerCode
                ";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetOrdersByCustomerCode Started for code: " + customerCode + " full query := " + query);
                return _dbConnection.Query<OrderPreviewDTO>(query,new {CustomerCode = customerCode}, transaction: Transaction);             
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetOrdersByCustomerCode: " + ex.Message, this);
                return null;
            }
        }
        public OrderHeaderDetailedDTO GetOrderHeaderDetailed(int orderHeaderID)
        {
            try
            {
                string query = @"
                SELECT ordHead.OrderHeaderID AS [OrderID], ordHead.OrderDate, ordHead.DeliveryDate AS [OrderDueDate]
                    , ohStatus.OrderStatusID, ohStatus.OrderstatusValue,cust.CustomerID, cust.CustomerCode, cust.CustomerName, cust.CustomerContactNumber
                    , cust.CustomerEmailAddress, custAdd.CustomerAddressID, custAdd.CustomerAddressDescription, custAdd.IsDefaultAddress, addLoc.AddressLocationID
                    , addLoc.AddressLine1, addLoc.AddressLine2, addLoc.PostCode, ca.CityAreaID, ca.CityAreaCode, ca.CityAreaName
                    , cit.CityID, cit.CityCode, cit.CityName, c.CountryID, c.CountryCode, c.CountryName
                FROM OrderHeaders ordHead                
                INNER JOIN OrderStatus ohStatus ON ohStatus.OrderStatusID = ordHead.OrderStatusID                
                INNER JOIN Customers cust ON ordHead.CustomerID = cust.CustomerID
                INNER JOIN CustomerAddresses custAdd ON custAdd.CustomerAddressID = ordHead.CustomerAddressID
                INNER JOIN AddressLocations addLoc ON addLoc.AddressLocationID = custAdd.AddressLocationID
                INNER JOIN CityAreas ca ON ca.CityAreaID = addLoc.CityAreaID
                INNER JOIN Cities cit ON cit.CityID = ca.CityID
                INNER JOIN Countries c ON c.CountryID = cit.CountryID
                WHERE ordHead.OrderHeaderID = @OrderHeaderID
                ";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetOrderHeaderDetailed Started: " + query);
                return _dbConnection.QueryFirst<OrderHeaderDetailedDTO>(query, new { OrderHeaderID = orderHeaderID }, transaction: Transaction);             
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetOrderHeaderDetailed: " + ex.Message, this);
                return null;
            }
        }
        public OrderHeaderDTO GetOrderHeader(int orderHeaderID)
        {
            try
            {
                string query = @"
                SELECT ordHead.OrderHeaderID AS [OrderID],  ordHead.OrderDate, ordHead.DeliveryDate AS [OrderDueDate]
                , ohStatus.OrderStatusValue AS [OrderStatus], cust.CustomerName, addLoc.AddressLine1, addLoc.AddressLine2
                , ca.CityAreaName, cit.CityName, addLoc.PostCode, c.CountryName
                FROM OrderHeaders ordHead                
                INNER JOIN OrderStatus ohStatus ON ohStatus.OrderStatusID = ordHead.OrderStatusID                
                INNER JOIN Customers cust ON ordHead.CustomerID = cust.CustomerID
                INNER JOIN CustomerAddresses custAdd ON custAdd.CustomerAddressID = ordHead.CustomerAddressID
                INNER JOIN AddressLocations addLoc ON addLoc.AddressLocationID = custAdd.AddressLocationID
                INNER JOIN CityAreas ca ON ca.CityAreaID = addLoc.CityAreaID
                INNER JOIN Cities cit ON cit.CityID = ca.CityID
                INNER JOIN Countries c ON c.CountryID = cit.CountryID
                WHERE ordHead.OrderHeaderID = @OrderHeaderID
                ";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetOrderHeader Started: " + query);
                return _dbConnection.QueryFirst<OrderHeaderDTO>(query, new { OrderHeaderID = orderHeaderID }, transaction: Transaction);             
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetOrderHeader: " + ex.Message, this);
                return null;
            }
        }
        public OrderHeaderEntity GetLatestOrder()
        {
            try
            {
                string query = @"
                SELECT TOP 1 OrderHeaderID,CustomerID,CustomerAddressID,OrderStatusID,OrderDate,DeliveryDate
                FROM OrderHeaders
                ORDER BY OrderHeaderID desc
                ";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetLatestOrder Started: full query = " + query);

                return _dbConnection.QueryFirst<OrderHeaderEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetLatestOrder: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}
