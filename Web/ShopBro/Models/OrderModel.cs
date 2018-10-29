using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class OrderModel : IModel, IDisposable
    {
        public OrderModel(ICustomModelState modelState, IOrderService orderService)
        {
            _modelState = modelState;
            _service = orderService;
        }
        public void Dispose()
        {           
            _service.Dispose();
        }

        private ICustomModelState _modelState;
        private IOrderService _service;
        public ICustomModelState ModelState { get { return _modelState; } }
        

    }
}