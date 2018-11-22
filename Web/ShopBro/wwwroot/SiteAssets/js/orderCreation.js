 var G_UseExistingAddress = true;
 var G_AvailableAddresses = [];

$(document).ready(function() {    

    $('#UseExistingAddressDisplay').change(UseExistingAddressChanged);
    SetupGlobals();

});

function SetupGlobals()
{
    G_UseExistingAddress = $('#UseExistingAddress')[0].value    
}

function UseExistingAddressChanged()
{
    G_UseExistingAddress = $(this).prop('checked');
    $('#UseExistingAddress')[0].value = G_UseExistingAddress;
    $('#NewAddressWrapper').toggle();
    $('#DDListWrapper').toggle();    
    console.log(G_UseExistingAddress);
    console.log($('#UseExistingAddress')[0].value);
}