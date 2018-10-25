using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class OrderModel : IModel, IDisposable
    {
        public OrderModel(ICustomModelState modelState, IItemService itemService)
        {
            _modelState = modelState;
            _service = itemService;
        }
        public void Dispose()
        {
            _service.Dispose();
        }

        private ICustomModelState _modelState;
        private IItemService _service;
        public ICustomModelState ModelState { get { return _modelState; } }
        

    }
}