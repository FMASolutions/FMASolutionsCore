var ExistingItemList = [];
var ColumnHeadings = ["Item Description", "Qty", "Price"];
var AvailableItemList = [];

$(document).ready(function() {
    $('#DDAddItemButton').on("click",addRowFromDD);
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
            "ItemImageLocation": $(this).children('span.ItemImageFilenameRaw')[0].innerText
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

function deleteExistingRow(rowID) {
    $("#" + rowID).remove();
}