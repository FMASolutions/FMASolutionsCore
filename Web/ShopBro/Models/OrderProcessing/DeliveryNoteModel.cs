using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class DeliveryNoteModel : IModel, IDisposable
    {
        public DeliveryNoteModel(ICustomModelState modelState, IDeliveryNoteService deliveryNoteService)
        {
            _modelState = modelState;
            _service = deliveryNoteService;
        }
        public void Dispose()
        {           
            _service.Dispose();
            
        }

        private ICustomModelState _modelState;
        private IDeliveryNoteService _service;
        public ICustomModelState ModelState { get { return _modelState; } }

        public DisplayDeliveryNoteViewModel DeliverItems(int orderHeaderID)
        {            
            DisplayDeliveryNoteViewModel vmReturn = new DisplayDeliveryNoteViewModel();
            if(orderHeaderID > 0)
            {
                int deliveryNoteID = _service.DeliverOrderItems(orderHeaderID);
                if(deliveryNoteID > 0)
                    vmReturn = GetDeliveryNoteByDeliveryNoteID(deliveryNoteID);
                else
                    return null;
                return vmReturn;
            }
            else
                return null;
        }        
        public DisplayDeliveryNoteViewModel GetDeliveryNoteByDeliveryNoteID(int deliveryNoteID)
        {
            var notes = _service.GetDeliveryNoteByDeliveryNoteID(deliveryNoteID);
            DisplayDeliveryNoteViewModel returnVM = null;
            
            if(notes != null)
            {
                 returnVM = new DisplayDeliveryNoteViewModel();
                foreach(var note in notes)
                {
                    returnVM.DeliveryNoteItems.Add(note);
                }
            }
            return returnVM;
                   
        }        
    }
}