using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System.Collections.Generic;
using System;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class Order
    {
        public Order()
        {
            _items = new List<OrderItem>();
        }
        public OrderHeader Header { get { return _header; } set { _header = value; } }

        public List<OrderItem> OrderItems { get { return _items; } set { _items = value; } }

        private List<OrderItem> _items;
        private OrderHeader _header;
    }
}