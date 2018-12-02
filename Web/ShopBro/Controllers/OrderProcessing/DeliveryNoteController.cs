using FMASolutionsCore.BusinessServices.ControllerTemplate;
using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using Microsoft.AspNetCore.Authorization;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class DeliveryNoteController : BaseController
    {
        public DeliveryNoteController(IDeliveryNoteService service)
        {
            _service = service;
        }
        private IDeliveryNoteService _service;

        [Authorize(Policy = "Admin")]
        public IActionResult DeliverItems(int id=0)
        {
            if(id>0)
            {
                DeliveryNoteModel model = GetNewModel();
                DisplayDeliveryNoteViewModel vmReturn = model.DeliverItems(id);
                if(vmReturn != null)
                    return View("DisplayDeliveryNote",vmReturn);
                else
                    return null;
            }
            else
                return null;
        }

        [Authorize(Policy = "Admin")]
        public IActionResult ViewDeliveryNote(int id=0) //id = deliverynoteID
        {            
            DeliveryNoteModel model = GetNewModel();
            DisplayDeliveryNoteViewModel deliveryNote = model.GetDeliveryNoteByDeliveryNoteID(id);
            if(deliveryNote != null)
            {
                return View("DisplayDeliveryNote", deliveryNote);
            }
            else
                return null;
        }

        private DeliveryNoteModel GetNewModel()
        {
            return new DeliveryNoteModel(new ModelStateConverter(this).Convert(), _service);
        }
    
       
    }
}