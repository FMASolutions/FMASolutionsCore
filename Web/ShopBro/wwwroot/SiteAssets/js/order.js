var ExistingItemList = [];
var ColumnHeadings = ["Item Description", "Qty", "Price"];
var AvailableItemList = [];

$(document).ready(function() {
    $('#DDAddItemButton').on("click",addRowFromDD);
    $('.ProductHeading').on('click', ToggleProduct);
    $('.SubHeading').on('click',ToggleSub);
    $('#AddItemButtonSearch').on('click',AddSearchItem);
    $('.AddAccordItemButton').on('click',AddAccordItem);
    setupGlobals();
});

function setupGlobals(){
    $('#OrderItemsRaw').children().each(function(){
        ExistingItemList.push({
            "ID": $(this).children('span.OrderItemItemRowIDRaw')[0].innerText,
            "IDText": "ItemRow" + $(this).children('span.OrderItemItemRowIDRaw')[0].innerText,
            "Description":  $(this).children('span.OrderItemDescriptionRaw')[0].innerText,
            "Qty":  $(this).children('span.OrderItemQtyRaw')[0].innerText,
            "Price":  $(this).children('span.OrderItemUnitPriceRaw')[0].innerText,
            "ItemID": $(this).children('span.OrderItemItemIDRaw')[0].innerText
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
    AddItemToExistingList(qty, itemDescription, price, itemID);    
}

function AddItemToExistingList(qty, itemDescription, price, itemID){

    var itemSearch = AvailableItemList.find(function(inputItem){ return inputItem.ItemID == this;}, itemID);
    if(itemSearch)
    {
        if(itemSearch.ItemMinPrice <= price)
        {
            console.log(itemSearch);
            ExistingItemList.push({
                "ID": itemID,
                "IDText": "ItemRow" + itemID,
                "Description": itemDescription,
                "Qty": qty,
                "Price": price
            });
            var htmlNewRow = '<tr id=ItemRow' + itemID + '>';
            htmlNewRow += '<td><span class="form-control">' + itemDescription + '</span></td>';
            htmlNewRow += '<td><span class="form-control">' + qty + '</span></td>';
            htmlNewRow += '<td><span class="form-control">' + price + '</span></td>';
            htmlNewRow += '<td><span class="form-control" onClick=deleteExistingRow("ItemRow' + itemID + '")>Remove</span></td>';
            htmlNewRow += '</tr>'

            $("#ExistingItemsTable").find("tbody").append(htmlNewRow);

        }
        else
            window.alert('price can\'t be less than the minimum sale value of: ' + itemSearch.ItemMinPrice);
    }
    else
        window.alert('invalid item');
}

function deleteExistingRow(rowID) {
    $("#" + rowID).remove();
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

function AddSearchItem(){    
    var Code = $('#CodeInputSearch')[0].value
    var Qty = $('#QtyInputSearch')[0].value
    var Price = $('#PriceInputSearch')[0].value

    if(!Code)
        window.alert('Code value must be populated');
    else if(!Qty)
        window.alert('Qty value must be populated');  
    else if(!Price)
        window.alert('Price value must be populated');   
    
    var itemSearch = AvailableItemList.find(function(inputItem){ return inputItem.ItemCode == this;}, Code);

    if(itemSearch)
        AddItemToExistingList(Qty, itemSearch.ItemDescription, Price, itemSearch.ItemID);    
    
}

function AddAccordItem(){
    var ItemID = $(this).parent().siblings('span.ItemID')[0].textContent;
    var MinimumPrice = $(this).parent().siblings('span.MinimumPrice')[0].textContent;
    var Qty = $(this).parent().siblings('span').children('.QtyInput')[0].value;
    var InputPrice = $(this).parent().siblings('span').children('.PriceInput')[0].value;  
    var ItemDescription = $(this).parent().siblings('span').children('.ItemDescritpion').value; 
    
    AddItemToExistingList(Qty, ItemDescription, InputPrice, ItemID);
}