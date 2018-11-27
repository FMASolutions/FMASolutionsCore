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

        public DeliveryNoteViewModel DeliverItems(int orderHeaderID)
        {            
            DeliveryNoteViewModel vmReturn = new DeliveryNoteViewModel();

            if(orderHeaderID > 0){
                DeliveryNote deliveryNote = _service.DeliverOrderItems(orderHeaderID);
                if(deliveryNote != null)
                    vmReturn = ConvertDeliveryModelToViewModel(deliveryNote);
                else
                    return null;
                return vmReturn;
            }
            else
                return null;
        }        
        public List<DeliveryNoteViewModel> GetDeliveryNoteByOrder(int orderID)
        {
            List<DeliveryNoteViewModel> deliveryNotes = new List<DeliveryNoteViewModel>();
            List<DeliveryNote> searchResult = _service.GetDeliveryNotesForOrder(orderID);

            foreach(var item in searchResult)
            {
                DeliveryNoteViewModel current = ConvertDeliveryModelToViewModel(item);
                deliveryNotes.Add(current);
            }
            return deliveryNotes;            
        }
        public DeliveryNoteViewModel GetDeliveryNoteByDeliveryNoteID(int deliveryNoteID)
        {
            var note = _service.GetDeliveryNoteByID(deliveryNoteID);
            if(note != null)
                return ConvertDeliveryModelToViewModel(note);
            return null;
                   
        }

        private DeliveryNoteViewModel ConvertDeliveryModelToViewModel(DeliveryNote model)
        {            
            DeliveryNoteViewModel vmReturn = new DeliveryNoteViewModel();
            List<DeliveryNoteItemViewModel> vmItems = new List<DeliveryNoteItemViewModel>();
            Order orderModel = _service.GetOrder(model.OrderHeaderID);
            vmReturn.DeliveryDateTime = model.DeliveryDate;
            vmReturn.DeliveryNoteID = model.DeliveryNoteID;
            vmReturn.orderHeaderID = model.OrderHeaderID;
            foreach(var item in model.Items)
            {
                DeliveryNoteItemViewModel current = new DeliveryNoteItemViewModel();
                
                current.DeliveryNoteItemID = item.DeliveryNoteItemID;
                current.ItemDeliveryDate = model.DeliveryDate;
                current.OrderItemID = item.OrderItemID;
                current.DeliveryNoteID = item.DeliveryNoteID;                

                OrderItem currentOrderItem = orderModel.OrderItems.Find(e => e.OrderItemID == current.OrderItemID);
                current.ItemID = currentOrderItem.ItemID;
                current.ItemQty = currentOrderItem.OrderItemQty;
                current.ItemDescription = currentOrderItem.OrderItemDescription;
                vmItems.Add(current);
            }
            vmReturn.Items = vmItems;
            return vmReturn;
        }
        
    }
}