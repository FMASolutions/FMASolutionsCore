function toggleVisability(controlElementID, toggleElementID, oppositeElementID){
    var chkBox = document.getElementById(controlElementID);
    var visToggle = "none";
    var visOpposite = "block";

    if(chkBox.checked)
    {
        visToggle = "block";
        visOpposite = "none";        
    }
    console.log("vis is currently: " + visToggle)
    document.getElementById(toggleElementID).style.display = visToggle;
    document.getElementById(oppositeElementID).style.display = visOpposite;
}