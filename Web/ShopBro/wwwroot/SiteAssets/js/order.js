var ExistingItemList = [];
var ColumnHeadings = ["Item Description","Qty","Price"];
var AvailableItemList = [];
document.getElementById("AddItemInputButton").onclick = function(){
    window.alert("Adding item...");
    var qtyValue = $("#QtyInput").val();
    var descValue =  $("[id*='SelectedItem'] :selected")[0].text
    var priceValue = $("#UnitPriceInput").val();
    var itemID = $("#SelectedItem")[0].value;
    addRow(descValue, qtyValue, priceValue,itemID);
}
document.addEventListener('DOMContentLoaded', function() {
    var table = document.getElementById("ExistingItemsTable");
    var rows = table.rows;

    var i = 0;
    while(i < rows.length)
    {
        var row = rows[i];
        console.log("row info: ")
        console.log(row);

        if(i>0) //Ignore first row as it contains headers
        {
            ExistingItemList.push({
                "ID" : rows[i].id.replace("ItemRow",""),
                "IDText" : rows[i].id,
                "Description" : row.cells[0].textContent,
                "Qty" : row.cells[1].textContent,
                "Price" : row.cells[2].textContent
            })
        }
        i++;
    }
 }, false);

 function addRow(itemDescription, qty, price, itemID)
 {
     ExistingItemList.push({
        "ID" : itemID,
        "IDText" : "ItemRow" + itemID,
        "Description" : itemDescription,
        "Qty" : qty,
        "Price" : price
    });
    var htmlNewRow = '<tr id=ItemRow' + itemID + '>';
    htmlNewRow += '<td><span class="form-control">' + itemDescription + '</span></td>';
    htmlNewRow += '<td><span class="form-control">' + qty + '</span></td>';
    htmlNewRow += '<td><span class="form-control">' + price + '</span></td>';
    htmlNewRow += '<td><span class="form-control" onClick=deleteExistingRow("ItemRow' + itemID +'")>Remove</span></td>';
    htmlNewRow += '</tr>'

    var test = $("#ExistingItemsTable").find("tbody").append(htmlNewRow);
 }

 function deleteExistingRow(rowID)
 {
     $("#"+rowID).remove();
 }
