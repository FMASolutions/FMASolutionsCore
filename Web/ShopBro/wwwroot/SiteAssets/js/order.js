var ExistingItemList = [];
var ColumnHeadings = ["Item Description", "Qty", "Price"];
var AvailableItemList = [];

$(document).ready(function() {
    document.getElementById("AddItemInputButton").addEventListener("click",addRow);
    setupGlobals();
});


function setupGlobals(){
    var table = document.getElementById("ExistingItemsTable");
    var rows = table.rows;

    var i = 0;
    while (i < rows.length) {
        var row = rows[i];

        if (i > 0) //Ignore first row as it contains headers
        {
            ExistingItemList.push({
                "ID": rows[i].id.replace("ItemRow", ""),
                "IDText": rows[i].id,
                "Description": row.cells[0].textContent,
                "Qty": row.cells[1].textContent,
                "Price": row.cells[2].textContent
            })
        }
        i++;
    }
}

function addRow() {

    var qty = $("#QtyInput").val();
    var itemDescription = $("[id*='SelectedItem'] :selected")[0].text
    var price = $("#UnitPriceInput").val();
    var itemID = $("#SelectedItem")[0].value;

    

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