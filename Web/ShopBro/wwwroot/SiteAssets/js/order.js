var ExistingItemList = [];
var ColumnHeadings = ["Item Description", "Qty", "Price"];
var AvailableItemList = [];
var NewItemID = 0;

$(document).ready(function() {
    setupGlobals();
    $('#DDAddItemButton').on("click",addRowFromDD);
    $('.ProductHeading').on('click', ToggleProduct);
    $('.SubHeading').on('click',ToggleSub);
    $('#AddItemButtonSearch').on('click',AddSearchItem);
    $('.AddAccordItemButton').on('click',AddAccordItem);    
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

function AddSearchItem(){    
    var Code = $('#CodeInputSearch')[0].value
    var Qty = $('#QtyInputSearch')[0].value
    var Price = $('#PriceInputSearch')[0].value

    if(!Code)
        window.alert('Code value must be populated');
    
    var itemSearch = AvailableItemList.find(function(inputItem){ return inputItem.ItemCode == this;}, Code);    
        
    if(itemSearch)        
        AddItemToExistingList(Qty, itemSearch.ItemDescription, Price, itemSearch.ItemID);
    else
        window.alert('Item with code: ' + Code + ' doesn\'t exist');
}

function AddAccordItem(){
    var ItemID = $(this).parent().siblings('span.ItemID')[0].textContent;
    var MinimumPrice = $(this).parent().siblings('span.MinimumPrice')[0].textContent;
    var Qty = $(this).parent().siblings('.row').children().children().children('.QtyInput')[0].value;
    var InputPrice = $(this).parent().siblings('.row').children().children().children('.PriceInput')[0].value;  
    var ItemDescription = $(this).parent().siblings('span.ItemDescritpion')[0].textContent; 
    
    AddItemToExistingList(Qty, ItemDescription, InputPrice, ItemID);
}

function AddItemToExistingList(qty, itemDescription, price, itemID){

    var itemSearch = AvailableItemList.find(function(inputItem){ return inputItem.ItemID == this;}, itemID);
    if(itemSearch)
    {
        if(itemSearch.ItemMinPrice > price)
            window.alert('price can\'t be less than the minimum sale value of: ' + itemSearch.ItemMinPrice);
        else if(Number(itemSearch.ItemAvailQty) < Number(qty))
            window.alert('Items current stock level is: ' + itemSearch.ItemAvailQty);
        else
        {
            NewItemID = NewItemID+1;

            ExistingItemList.push({
                "ID": NewItemID,
                "IDText": "ItemRowNew" + NewItemID,
                "Description": itemDescription,
                "Qty": qty,
                "Price": price,
                "ItemID" : itemID
            });

            var htmlNewRow = GenerateHTMLForTableRow(itemID, itemDescription, qty, price);
            $("#ExistingItemsTable").find("tbody").append(htmlNewRow);
        }   
    }
    else
        window.alert('invalid item');
}
function GenerateHTMLForTableRow(itemID, itemDescription, qty, price)
{
    var indexValue = ExistingItemList.length-1;

    var newHTML = '<tr id=ItemRowNew' + itemID + '>';
        newHTML += '<td><span class="form-control">'; 
            newHTML += itemDescription;
            newHTML += '<input id="ExistingItems_' + indexValue + '__ItemDescription" name="ExistingItems[' + indexValue + '].ItemDescription" type="hidden" value="' + itemDescription + '">';
        newHTML += '</span></td>';
        newHTML += '<td><span class="form-control">';
            newHTML += qty;
            newHTML += '<input data-val="true" data-val-number="The field Qty must be a number." data-val-required="The Qty field is required." id="ExistingItems_' + indexValue + '__Qty" name="ExistingItems[' + indexValue + '].Qty" type="hidden" value="' + qty + '">';
        newHTML += '</span></td>';
        newHTML += '<td><span class="form-control">';
            newHTML += price; 
            newHTML += '<input data-val="true" data-val-number="The field UnitPrice must be a number." data-val-required="The UnitPrice field is required." id="ExistingItems_' + indexValue + '__UnitPrice" name="ExistingItems[' + indexValue + '].UnitPrice" type="hidden" value="' + price + '">';
        newHTML += '</span></td>';
        newHTML += '<td><span class="form-control">'; 
            newHTML += '<input data-val="true" data-val-number="The field ItemID must be a number." data-val-required="The ItemID field is required." id="ExistingItems_' + indexValue + '__ItemID" name="ExistingItems[' + indexValue + '].ItemID" type="hidden" value="' + itemID + '">';
        newHTML += '</span></td>';
    newHTML += '</tr>'
    return newHTML;
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
