var ExistingItemList = [];
var ColumnHeadings = ["Item Description", "Qty", "Price"];
var AvailableItemList = [];
var NewItemID = 0;

$(document).ready(function() {
    setupGlobals();
    document.onkeypress = processKeyStroke;
    $('#DDAddItemButton').on("click",addRowFromDD);
    $('.ProductHeading').on('click', ToggleProduct);
    $('.SubHeading').on('click',ToggleSub);
    $('#AddItemButtonSearch').on('click',AddSearchItem);
    $('.AddAccordItemButton').on('click',AddAccordItem);    
    $('.ExistingItemsRemovalButton').on('click',RemoveItemFromExistingItemsList );
});

function setupGlobals(){
    $('#OrderItemsRaw').children().each(function(){
        ExistingItemList.push({
            "ID": $(this).children('span.OrderItemItemRowIDRaw')[0].innerText,
            "IDText": "ItemRow" + $(this).children('span.OrderItemItemRowIDRaw')[0].innerText,
            "Description":  $(this).children('span.OrderItemDescriptionRaw')[0].innerText,
            "Qty":  $(this).children('span.OrderItemQtyRaw')[0].innerText,
            "Price":  $(this).children('span.OrderItemUnitPriceRaw')[0].innerText,
            "ItemID": $(this).children('span.OrderItemItemIDRaw')[0].innerText,
            "ItemStatus" : $(this).children('span.OrderItemStatusRaw')[0].innerText,
            "CurrentIndex": ExistingItemList.length,
            "OrderItemRowID" : $(this).children('span.OrderItemItemRowIDRaw')[0].innerText
        });        
    });

    $('#ItemsRaw').children().each(function(){
        AvailableItemList.push({
            "ItemID": $(this).children('span.ItemIDRaw')[0].innerText,
            "SubGroupID": $(this).children('span.SubGroupIDRaw')[0].innerText,
            "ItemName": $(this).children('span.ItemNameRaw')[0].innerText,
            "ItemDescription": $(this).children('span.ItemDescriptionRaw')[0].innerText,
            "ItemUnitPrice": $(this).children('span.ItemUnitPriceRaw')[0].innerText,
            "ItemMinPrice": $(this).children('span.ItemUnitPriceWithMaxDiscountRaw')[0].innerText,
            "ItemAvailQty": $(this).children('span.ItemAvailableQtyRaw')[0].innerText,
            "ItemImageLocation": $(this).children('span.ItemImageFilenameRaw')[0].innerText,
            "ItemCode": $(this).children('span.ItemCode')[0].innerText
        });
    });
}

function addRowFromDD() {
    var qty = $("#QtyInput").val();
    var itemDescription = $("[id*='SelectedItem'] :selected")[0].text
    var price = $("#UnitPriceInput").val();
    var itemID = $("#SelectedItem")[0].value;    

    if(AddItemToExistingList(qty, itemDescription, price, itemID))
    {
        $("#QtyInput")[0].value = "";
        $("#UnitPriceInput")[0].value = "";
    }
}

function AddSearchItem(){    
    var Code = $('#CodeInputSearch')[0].value
    var Qty = $('#QtyInputSearch')[0].value
    var Price = $('#PriceInputSearch')[0].value

    if(!Code)
        window.alert('Code value must be populated');
    
    var itemSearch = AvailableItemList.find(function(inputItem){ return inputItem.ItemCode == this;}, Code);    
        
    if(itemSearch)        
    {
        if(AddItemToExistingList(Qty, itemSearch.ItemDescription, Price, itemSearch.ItemID))
        {
            $('#CodeInputSearch')[0].value = "";
            $('#QtyInputSearch')[0].value = "";
            $('#PriceInputSearch')[0].value = "";
        }
    }
    else
        window.alert('Item with code: ' + Code + ' doesn\'t exist');
}

function AddAccordItem(){
    var ItemID = $(this).parent().siblings('span.ItemID')[0].textContent;
    var MinimumPrice = $(this).parent().siblings('span.MinimumPrice')[0].textContent;
    var Qty = $(this).parent().siblings('.row').children().children().children('.QtyInput')[0].value;
    var InputPrice = $(this).parent().siblings('.row').children().children().children('.PriceInput')[0].value;  
    var ItemDescription = $(this).parent().siblings('span.ItemDescritpion')[0].textContent; 
    
    if(AddItemToExistingList(Qty, ItemDescription, InputPrice, ItemID))
    {
        $(this).parent().siblings('.row').children().children().children('.QtyInput')[0].value = "";
        $(this).parent().siblings('.row').children().children().children('.PriceInput')[0].value = "";
    }

}
function AddAccordItemFromEnterKey(node){
    
    var ItemID = $(node).parent().parent().parent().siblings('span.ItemID')[0].textContent;
    var MinimumPrice = $(node).parent().parent().parent().siblings('span.MinimumPrice')[0].textContent;
    var Qty = $(node).parent().parent().parent().children().children().children('.QtyInput')[0].value;
    var InputPrice = $(node).parent().parent().parent().children().children().children('.PriceInput')[0].value;  
    var ItemDescription = $(node).parent().parent().parent().siblings('span.ItemDescritpion')[0].textContent; 
    
    if(AddItemToExistingList(Qty, ItemDescription, InputPrice, ItemID))
    {
        $(node).parent().parent().parent().children().children().children('.QtyInput')[0].value = "";
        $(node).parent().parent().parent().children().children().children('.PriceInput')[0].value = "";
    }
    

}

function AddItemToExistingList(qty, itemDescription, price, itemID){

    var itemSearch = AvailableItemList.find(function(inputItem){ return inputItem.ItemID == this;}, itemID);
    var success = false;
    if(itemSearch)
    {
        var convertedPrice = parseFloat(price);
        var convertedMinPrice = parseFloat(itemSearch.ItemMinPrice);
        var convertedQty = parseInt(qty);
        var convertedAvailQty = parseInt(itemSearch.ItemAvailQty);

        if(convertedMinPrice > convertedPrice)
            window.alert('price can\'t be less than the minimum sale value of: ' + itemSearch.ItemMinPrice);
        else if(convertedAvailQty < convertedQty)
            window.alert('Items current stock level is: ' + itemSearch.ItemAvailQty);
        else
        {
            if(convertedPrice >= 0.01 && Number(convertedQty) >= 1)
            {
                ID = ++NewItemID;
                var indexValue = ExistingItemList.length;
                ExistingItemList.push({
                    "ID": ID,
                    "IDText": "ItemRowNew" + ID,
                    "Description": itemDescription,
                    "Qty": qty,
                    "Price": price,
                    "ItemID" : itemID,
                    "ItemStatus" : "Estimate",
                    "CurrentIndex" : indexValue,
                    "OrderItemRowID" : 0
                }); 
                    
                GenerateNewTableBody();
                success = true;
                alert("Item Added Successfully")
            }
            else
            {
                alert("Price and Qty must be greater than 0")
            }

        }   
    }
    else
        window.alert('invalid item');
    return Boolean(success);
}
function GenerateNewTableBody(){
    var newTableBody = '<tbody>'
    var i = 0;
    ExistingItemList.forEach(function(arrayItem){        
            newTableBody += GenerateItemHTML(arrayItem.ItemID, arrayItem.Description, arrayItem.ItemStatus, arrayItem.Qty, arrayItem.Price, i, arrayItem.OrderItemRowID,arrayItem.IDText);        
            i++;
    });
    newTableBody += '</tbody>'

    $("#ExistingItemsTable").find("tbody").remove();
    $("#ExistingItemsTable").append(newTableBody);
    $('.ExistingItemsRemovalButton').on('click',RemoveItemFromExistingItemsList );
    
}
function GenerateItemHTML(itemID, itemDescription, itemStatus, qty, price, indexValue, orderItemRowID, tableRowID){
    var newHTML = '<tr id=' + tableRowID + '>';
        newHTML += '<td><span>'; 
            newHTML += itemDescription;
            newHTML += '<input id="ItemDetails_' + indexValue + '__OrderItemDescription" name="ItemDetails[' + indexValue + '].OrderItemDescription" type="hidden" value="' + itemDescription + '">';
        newHTML += '</span></td>';
        newHTML += '<td class="text-center"><span>';
            newHTML += qty;
            newHTML += '<input data-val="true" data-val-number="The field Qty must be a number." data-val-required="The Qty field is required." id="ItemDetails_' + indexValue + '__OrderItemQty" name="ItemDetails[' + indexValue + '].OrderItemQty" type="hidden" value="' + qty + '">';
        newHTML += '</span></td>';
        newHTML += '<td class="text-center"><span>';
            newHTML += price; 
            newHTML += '<input data-val="true" data-val-number="The field UnitPrice must be a number." data-val-required="The UnitPrice field is required." id="ItemDetails_' + indexValue + '__OrderItemUnitPriceAfterDiscount" name="ItemDetails[' + indexValue + '].OrderItemUnitPriceAfterDiscount" type="hidden" value="' + Number(Math.round(price+'e2')+'e-2').toFixed(2) + '">';
        newHTML += '</span></td>';

        if(itemStatus.indexOf("Estimate") >= 0){
            newHTML += '<td class="ExistingItemsRemovalButton text-center">';             
                newHTML += ' <img class="icon" src="/SiteAssets/images/Icons/delete.svg" width="20">';
            newHTML += '</td>';
        }
        else
        {
            newHTML += '<td class="text-center"><span>';
                newHTML += '';
            newHTML += '</span></td>';
        }
   
        newHTML += '<input data-val="true" data-val-required="The ItemID field is required." id="ItemDetails_' + indexValue + '__ItemID" name="ItemDetails[' + indexValue + '].ItemID" type="hidden" value="' + itemID + '">';
        if(orderItemRowID > 0)
        {
            newHTML += '<input data-val="true" data-val-required="The OrderItemRowID field is required." id="ItemDetails_' + indexValue + '__OrderItemID" name="ItemDetails[' + indexValue + '].OrderItemID" type="hidden" value="' + orderItemRowID + '">'
        }
    newHTML += '</tr>'
    return newHTML;
}

function RemoveItemFromExistingItemsList(){    
    var RowIdText = $(this).parent()[0].id;
    console.log(RowIdText);  
    var itemSearch = ExistingItemList.find(function(inputItem){ return inputItem.IDText == this;}, RowIdText);
    if(itemSearch)
    {
        ExistingItemList.splice(itemSearch.CurrentIndex, 1);        
        for(i = 0; i < ExistingItemList.length; i++)
            ExistingItemList[i].CurrentIndex = i;
        
        GenerateNewTableBody();
    }
}

function ToggleProduct(){    
    var visibility = $(this).siblings().children().css('display');
    if(visibility === "block")
        $(this).siblings().children().hide();
    else    
        $(this).siblings().children().show();
}
function ToggleSub(){
    $(this).siblings('ul').toggle();
}

function processKeyStroke(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    if ((evt.keyCode == 13)) 
    {
        var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
        var classList = $(node).attr('class').split(/\s+/);
        for (var i = 0; i < classList.length; i++) {
            if (classList[i] === 'AccordItemControl') {
                AddAccordItemFromEnterKey(node);
                return false;
            }
            else if(classList[i] === 'SelectorItemControl'){
                addRowFromDD();
                return false;
            }
            else if(classList[i] === 'SearchItemControl'){
                AddSearchItem();
                return false;
            }
        }
        return false; 
    }
}
