


function autocomplete(inp, arr) {
    var currentFocus;

    inp.addEventListener("input", function(e) {
        var a, b, i, val = this.value;
        var numHits = Number(0);
        var hitArray = [];
        
        closeAllLists();
        if (!val) { return false;}
        currentFocus = -1;

        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items text-white text-dark");
        this.parentNode.appendChild(a);

        for (i = 0; i < arr.length; i++) {
          var searchIndex = Number(arr[i].toUpperCase().indexOf(val.toUpperCase()));
          if(searchIndex > -1)
          {
              numHits = numHits + 1;
              b = document.createElement("DIV");
              if(searchIndex == 0 )
              {
                b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[i].substr(val.length);                  
              }
              else
              {
                b.innerHTML = arr[i].substr(0, searchIndex);
                b.innerHTML += "<strong>" + arr[i].substr(searchIndex, val.length) + "</strong>"
                if(arr[i].length > searchIndex + val.length)
                {
                  b.innerHTML += arr[i].substr(searchIndex + val.length);
                }
                
              } 
              b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
              b.addEventListener("click", function(e) {
                inp.value = this.getElementsByTagName("input")[0].value;
                closeAllLists();
              });
              hitArray.push({
                "DIVElement" : b,
                "searchIndex" : searchIndex 
              })
              
              if(numHits >= 10)
              {
                break;
              } 
          }
        }
        hitArray.sort((a,b) => (a.searchIndex > b.searchIndex) ? 1 : ((b.searchIndex > a.searchIndex) ? -1 : 0));
        hitArray.forEach(function(arrayItem){
          a.appendChild(arrayItem.DIVElement);
        });
    });
    /*execute a function presses a key on the keyboard:*/
    inp.addEventListener("keydown", function(e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
          /*If the arrow DOWN key is pressed,
          increase the currentFocus variable:*/
          currentFocus++;
          /*and and make the current item more visible:*/
          addActive(x);
        } else if (e.keyCode == 38) { //up
          /*If the arrow UP key is pressed,
          decrease the currentFocus variable:*/
          currentFocus--;
          /*and and make the current item more visible:*/
          addActive(x);
        } else if (e.keyCode == 13) {
          /*If the ENTER key is pressed, prevent the form from being submitted,*/
          e.preventDefault();
          if (currentFocus > -1) {
            /*and simulate a click on the "active" item:*/
            if (x) x[currentFocus].click();
          }
        }
    });
    function addActive(x) {
      /*a function to classify an item as "active":*/
      if (!x) return false;
      /*start by removing the "active" class on all items:*/
      removeActive(x);
      if (currentFocus >= x.length) currentFocus = 0;
      if (currentFocus < 0) currentFocus = (x.length - 1);
      /*add class "autocomplete-active":*/
      x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {
      /*a function to remove the "active" class from all autocomplete items:*/
      for (var i = 0; i < x.length; i++) {
        x[i].classList.remove("autocomplete-active");
      }
    }
    function closeAllLists(elmnt) {
      /*close all autocomplete lists in the document,
      except the one passed as an argument:*/
      var x = document.getElementsByClassName("autocomplete-items");
      for (var i = 0; i < x.length; i++) {
        if (elmnt != x[i] && elmnt != inp) {
          x[i].parentNode.removeChild(x[i]);
        }
      }
    }
    /*execute a function when someone clicks in the document:*/
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
  }
  