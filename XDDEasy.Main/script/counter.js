ProductList=new Array();var totalCollectedBV=0.00;function Product(id,price,bv,ct,rowid){this.id=id;this.price=price;this.bv=bv;this.ct=ct;this.rowid=rowid;}
function addProduct(id,price,bv,ct,rowid){ProductList[ProductList.length]=new Product(id,price,bv,ct,rowid);}
function init(){var thisform=document.salesform;var qty=0;for(var i=0;i<ProductList.length;i++){var obj=eval("thisform.Qty_"+ProductList[i].ct+"_"+ ProductList[i].id);if(obj!=null)
qty=obj.value;if(qty>0){var total=eval(document.getElementById("Amt_"+ProductList[i].ct+"_"+ ProductList[i].id));total.innerHTML=qty==0?"":decFormat(ProductList[i].price*qty);invokeCart(i,ProductList[i].rowid,obj);}}
calcGrandTotal();}
function calcUnit(idx,qty){var unit=qty.value;if(isNaN(unit)){alert("Quantity must be numeric number.");qty.value="";qty.focus();}
var total=eval(document.getElementById("Amt_"+ProductList[idx].ct+"_"+ ProductList[idx].id));total.innerHTML=isNaN(unit)||unit==0?"":decFormat(ProductList[idx].price*unit);qty.value=isNaN(unit)||unit==0?"":unit*1;calcGrandTotal();}
function calcFocUnit(idx,qty){var unit=qty.value;if(isNaN(unit)){alert("Quantity must be numeric number.");qty.value="";qty.focus();return;}}
function calcAdminFee(thisObj){thisObj.value=decFormat(thisObj.value*1);calcGrandTotal();}
function calcDelivery(thisObj){thisObj.value=decFormat(thisObj.value*1);calcGrandTotal();}
function decFormat(amount){var i=parseFloat(amount);if(isNaN(i)){i=0.00;}
var minus='';if(i<0){minus='-';}
i=Math.abs(i);i=parseInt((i+.005)*100);i=i/100;s=new String(i);if(s.indexOf('.')<0){s+='.00';}
if(s.indexOf('.')==(s.length- 2)){s+='0';}
s=minus+ s;return s;}
function calcGrandTotal(){var thisform=document.salesform;selectedUnit=0;total=0;bv=0;qty=0;for(var i=0;i<ProductList.length;i++){var obj=eval("thisform.Qty_"+ProductList[i].ct+"_"+ ProductList[i].id);if(obj!=null)
qty=obj.value;total+=ProductList[i].price*qty;bv+=ProductList[i].bv*qty;selectedUnit+=qty;}
if(document.getElementById("TotalPv")!=null){if(navigator.appName=="Microsoft Internet Explorer"){TotalPv.innerText=bv==0?"0.00":decFormat(bv);}else{document.getElementById("TotalPv").textContent=bv==0?"0.00":decFormat(bv);}}
if(navigator.appName=="Microsoft Internet Explorer"){Total.innerText=total==0?"0.00":decFormat(total);}else{document.getElementById("Total").textContent=total==0?"0.00":decFormat(total);}
if(total==0){disableSubmitBtn(true);}else{disableSubmitBtn(false);}
if(document.getElementById("Adjustmentratio")!=null){if(navigator.appName=="Microsoft Internet Explorer"){var adjAmt=-total*Adjustmentratio.innerText/100;total+=adjAmt;if(document.getElementById("Adjustmentamount")!=null)
Adjustmentamount.innerText=adjAmt==0?"0.00":decFormat(adjAmt);}else{var adjAmt=-total*Adjustmentratio.textContent/100;total+=adjAmt;if(document.getElementById("Adjustmentamount")!=null)
document.getElementById("Adjustmentamount").textContent=adjAmt==0?"0.00":decFormat(adjAmt);}}
if(thisform.AdminFee!=null)
total+=(thisform.AdminFee.value*1);if(thisform.DeliveryFee!=null)
total+=(thisform.DeliveryFee.value*1);if(document.getElementById("Grandtotal")!=null){if(navigator.appName=="Microsoft Internet Explorer"){Grandtotal.innerText=total==0?"0.00":decFormat(total);}else{document.getElementById("Grandtotal").textContent=total==0?"0.00":decFormat(total);}}
totalCollectedBV=bv;}
function disableSubmitBtn(truefalse){var thisform=document.salesform;thisform.btnSubmit.disabled=truefalse;}
function calcAmountPaid(_this){var thisform=_this.form;paid=0;if(document.getElementById("Grandtotal").innerHTML.length==0||document.getElementById("Grandtotal").innerHTML<=0){alert('Grand total cannot be EMPTY and value must greater than ZERO.');_this.value='';return false;}else if(isNaN(document.getElementById("PayAmt").value)){alert("Amount must be in numeric number.");_this.value="";_this.focus();return false;}else{if(thisform.PayAmt.length==null){paid+=thisform.PayAmt.value*1;}else{for(var i=0;i<thisform.PayAmt.length;i++){paid+=(thisform.PayAmt[i].value*1);}}
if(document.getElementById("amountPaid")!=null)
document.getElementById("amountPaid").innerHTML=paid==0?"0.00":decFormat(paid);if(document.getElementById("balance")!=null)
document.getElementById("balance").innerHTML=decFormat(document.getElementById("Grandtotal").innerHTML- paid);}}
function calcTopupAmountPaid(_this){var thisform=_this.form;amt=0;paid=0;rwalletpaid=0;ewalletpaid=0;cusdwalletpaid=0;sellingprice=(document.getElementById("SellingPrice").value||document.getElementById("SellingPrice").innerHTML);if(document.getElementById("Amount").value.length==0||document.getElementById("Amount").value<=0){alert('Topup amount is empty.');_this.value='';return false;}else if(isNaN(document.getElementById("Amount").value)||isNaN(document.getElementById("RwalletPayAmt").value)||isNaN(document.getElementById("EwalletPayAmt").value)||isNaN(document.getElementById("CusdwalletPayAmt").value)){alert("Amount must be in numeric number.");_this.value="";_this.focus();return false;}else{if(thisform.Amount.length==null){amt+=thisform.Amount.value*1;}else{for(var i=0;i<thisform.Amount.length;i++){amt+=(thisform.Amount[i].value*1);}}
if(thisform.RwalletPayAmt.length==null){paid+=thisform.RwalletPayAmt.value*1;rwalletpaid+=thisform.RwalletPayAmt.value*1;}else{for(var i=0;i<thisform.RwalletPayAmt.length;i++){paid+=(thisform.RwalletPayAmt[i].value*1);rwalletpaid+=(thisform.RwalletPayAmt[i].value*1);}}
if(thisform.EwalletPayAmt.length==null){paid+=thisform.EwalletPayAmt.value*1;ewalletpaid+=thisform.EwalletPayAmt.value*1;}else{for(var i=0;i<thisform.EwalletPayAmt.length;i++){paid+=(thisform.EwalletPayAmt[i].value*1);ewalletpaid+=(thisform.EwalletPayAmt[i].value*1);}}
if(thisform.CusdwalletPayAmt.length==null){paid+=thisform.CusdwalletPayAmt.value*1;cusdwalletpaid+=thisform.CusdwalletPayAmt.value*1;}else{for(var i=0;i<thisform.CusdwalletPayAmt.length;i++){paid+=(thisform.CusdwalletPayAmt[i].value*1);cusdwalletpaid+=(thisform.CusdwalletPayAmt[i].value*1);}}
if(document.getElementById("totalAmt")!=null)
document.getElementById("totalAmt").innerHTML=amt==0?"0.00":decFormat(amt*sellingprice);if(document.getElementById("balance")!=null)
document.getElementById("balance").innerHTML=amt==0?"0.00":decFormat(amt*sellingprice- paid);if(document.getElementById("amountPaid")!=null)
document.getElementById("amountPaid").innerHTML=paid==0?"0.00":decFormat(paid);if(document.getElementById("balance")!=null)
document.getElementById("balance").innerHTML=decFormat(document.getElementById("totalAmt").innerHTML- paid);if(document.getElementById("balanceRwallet")!=null)
document.getElementById("balanceRwallet").innerHTML=decFormat(document.getElementById("totalRwalletBal").value- rwalletpaid);if(document.getElementById("balanceEwallet")!=null)
document.getElementById("balanceEwallet").innerHTML=decFormat(document.getElementById("totalEwalletBal").value- ewalletpaid);if(document.getElementById("balanceCusdwallet")!=null)
document.getElementById("balanceCusdwallet").innerHTML=decFormat(document.getElementById("totalCusdwalletBal").value- cusdwalletpaid);}}
function calcTopupTotalAmt(_this){var thisform=_this.form;amt=0;paid=0;rwalletpaid=0;ewalletpaid=0;cusdwalletpaid=0;if(isNaN(document.getElementById("Amount").value)){alert("Amount must be in numeric number.");_this.value="";_this.focus();return false;}else{sellingprice=(document.getElementById("SellingPrice").value||document.getElementById("SellingPrice").innerHTML);if(thisform.Amount.length==null){amt+=thisform.Amount.value*1;}else{for(var i=0;i<thisform.Amount.length;i++){amt+=(thisform.Amount[i].value*1);}}
if(thisform.RwalletPayAmt.length==null){paid+=thisform.RwalletPayAmt.value*1;rwalletpaid+=thisform.RwalletPayAmt.value*1;}else{for(var i=0;i<thisform.RwalletPayAmt.length;i++){paid+=(thisform.RwalletPayAmt[i].value*1);rwalletpaid+=(thisform.RwalletPayAmt[i].value*1);}}
if(thisform.EwalletPayAmt.length==null){paid+=thisform.EwalletPayAmt.value*1;ewalletpaid+=thisform.EwalletPayAmt.value*1;}else{for(var i=0;i<thisform.EwalletPayAmt.length;i++){paid+=(thisform.EwalletPayAmt[i].value*1);ewalletpaid+=(thisform.EwalletPayAmt[i].value*1);}}
if(thisform.CusdwalletPayAmt.length==null){paid+=thisform.CusdwalletPayAmt.value*1;cusdwalletpaid+=thisform.CusdwalletPayAmt.value*1;}else{for(var i=0;i<thisform.CusdwalletPayAmt.length;i++){paid+=(thisform.CusdwalletPayAmt[i].value*1);cusdwalletpaid+=(thisform.CusdwalletPayAmt[i].value*1);}}
if(document.getElementById("totalAmt")!=null)
document.getElementById("totalAmt").innerHTML=amt==0?"0.00":decFormat(amt*sellingprice);if(document.getElementById("balance")!=null)
document.getElementById("balance").innerHTML=amt==0?"0.00":decFormat(amt*sellingprice- paid);if(document.getElementById("balanceRwallet")!=null)
document.getElementById("balanceRwallet").innerHTML=decFormat(document.getElementById("totalRwalletBal").value- rwalletpaid);if(document.getElementById("balanceEwallet")!=null)
document.getElementById("balanceEwallet").innerHTML=decFormat(document.getElementById("totalEwalletBal").value- ewalletpaid);if(document.getElementById("balanceCusdwallet")!=null)
document.getElementById("balanceCusdwallet").innerHTML=decFormat(document.getElementById("totalCusdwalletBal").value- cusdwalletpaid);}}
function calcAmountPaid2(_this){var thisform=_this.form;amt=document.getElementById("totalAmt").innerHTML;paid=0;rwalletpaid=0;ewalletpaid=0;cusdwalletpaid=0;if(isNaN(document.getElementById("RwalletPayAmt").value)||isNaN(document.getElementById("EwalletPayAmt").value)||isNaN(document.getElementById("CusdwalletPayAmt").value)){alert("Amount must be in numeric number.");_this.value="";_this.focus();return false;}else{if(thisform.RwalletPayAmt.length==null){paid+=thisform.RwalletPayAmt.value*1;rwalletpaid+=thisform.RwalletPayAmt.value*1;}else{for(var i=0;i<thisform.RwalletPayAmt.length;i++){paid+=(thisform.RwalletPayAmt[i].value*1);rwalletpaid+=(thisform.RwalletPayAmt[i].value*1);}}
if(thisform.EwalletPayAmt.length==null){paid+=thisform.EwalletPayAmt.value*1;ewalletpaid+=thisform.EwalletPayAmt.value*1;}else{for(var i=0;i<thisform.EwalletPayAmt.length;i++){paid+=(thisform.EwalletPayAmt[i].value*1);ewalletpaid+=(thisform.EwalletPayAmt[i].value*1);}}
if(thisform.CusdwalletPayAmt.length==null){paid+=thisform.CusdwalletPayAmt.value*1;cusdwalletpaid+=thisform.CusdwalletPayAmt.value*1;}else{for(var i=0;i<thisform.CusdwalletPayAmt.length;i++){paid+=(thisform.CusdwalletPayAmt[i].value*1);cusdwalletpaid+=(thisform.CusdwalletPayAmt[i].value*1);}}
if(document.getElementById("balance")!=null)
document.getElementById("balance").innerHTML=amt==0?"0.00":decFormat(amt- paid);if(document.getElementById("amountPaid")!=null)
document.getElementById("amountPaid").innerHTML=paid==0?"0.00":decFormat(paid);if(document.getElementById("balance")!=null)
document.getElementById("balance").innerHTML=decFormat(amt- paid);if(document.getElementById("balanceRwallet")!=null)
document.getElementById("balanceRwallet").innerHTML=decFormat(document.getElementById("totalRwalletBal").value- rwalletpaid);if(document.getElementById("balanceEwallet")!=null)
document.getElementById("balanceEwallet").innerHTML=decFormat(document.getElementById("totalEwalletBal").value- ewalletpaid);if(document.getElementById("balanceCusdwallet")!=null)
document.getElementById("balanceCusdwallet").innerHTML=decFormat(document.getElementById("totalCusdwalletBal").value- cusdwalletpaid);}}
var cartTable="Cart";var emptyMessage="No Purchase Item";var noOfItemInCart=0;function addToCart(tidx,data){offMessage();var table=document.getElementById(cartTable);var header=table.rows[0];var rowSize=table.rows.length;var colSize=header.cells.length;var thisRow=table.insertRow(-1);thisRow.setAttribute("id","t"+ tidx);thisRow.className=rowSize%2==0?"even":"odd";for(var i=0;i<colSize;i++){cell=thisRow.insertCell(i);if(i==0)
cell.innerHTML=rowSize+".";else{cell.innerHTML=data[i-1];}
cell.align=header.cells[i].align;}
noOfItemInCart+=1;}
function deleteFromCart(tidx){var rowidx=getCart(tidx);var table=document.getElementById(cartTable);var thisRow=table.rows[rowidx];if(thisRow!=null){table.deleteRow(rowidx);noOfItemInCart-=1;}
refreshCart();}
function updateCart(rowidx,data){var table=document.getElementById(cartTable);var thisRow=table.rows[rowidx];if(thisRow!=null){var colSize=thisRow.cells.length;for(var i=1;i<colSize;i++){cell=thisRow.cells[i];cell.innerHTML=data[i-1];}}}
function offMessage(){if(noOfItemInCart==0){var table=document.getElementById(cartTable);var thisRow=table.rows[1];if(thisRow!=null){table.deleteRow(1);}}}
function onMessage(){if(noOfItemInCart==0){var table=document.getElementById(cartTable);var rowSize=table.rows.length;if(rowSize==1){var thisRow=table.insertRow();var cell=thisRow.insertCell(0);cell.innerHTML=emptyMessage;cell.align="center";cell.colSpan="10";}}}
function refreshCart(){var table=document.getElementById(cartTable);var rowSize=table.rows.length;for(var i=1;noOfItemInCart>0&&i<rowSize;i++){var thisRow=table.rows[i];var cell=thisRow.cells[0];thisRow.className=i%2==0?"even":"odd";cell.innerHTML=i+".";}
onMessage();}
function getCart(tidx){var table=document.getElementById(cartTable);var rowSize=table.rows.length;var rowidx=-1;var tmptidx="t"+ tidx;for(var i=0;i<rowSize;i++){var thisRow=table.rows[i];var thisId=thisRow.getAttribute("id");if(thisId==tmptidx){rowidx=i;break;}}
return rowidx;}
function notifyCart(tidx,data){var rowidx=getCart(tidx);if(rowidx<0){addToCart(tidx,data);}else{updateCart(rowidx,data);}}
function notifyCartCC(tidx,data){var keyindex=(data[4]+data[5]+data[3]);var rowidx=getCart(keyindex);if(rowidx<0){addToCartCC(keyindex,data);}else{updateCartCC(rowidx,keyindex,data);}}
function addToCartCC(keyindex,data){offMessage();var table=document.getElementById(cartTable);var header=table.rows[0];var rowSize=table.rows.length;var colSize=header.cells.length;var thisRow=table.insertRow();thisRow.setAttribute("id","t"+(keyindex));thisRow.setAttribute("amtcc",(data[2]));thisRow.className=rowSize%2==0?"even":"odd";for(var i=0;i<colSize;i++){cell=thisRow.insertCell(i);if(i==0){cell.innerHTML=rowSize+".";}
else if(i==colSize-1){cell.innerHTML="<input type=\"button\" onclick=\"deleteFromCartCC('"+ keyindex+"')\" value=Remove align='center'><input type=hidden name='PayIdCC' value='"+ data[4]+"'><input type=hidden name='BnkCC' value='"+ data[5]+"'><input type=hidden name='PayAmntCC' value='"+ data[2]+"'><input type=hidden name='PayRefnCC' value='"+ data[3]+"'>";}
else{cell.innerHTML=data[i-1];}
cell.align=header.cells[i].align;}
noOfItemInCart+=1;}
function deleteFromCartCC(keyindex){var rowidx=getCart(keyindex);var table=document.getElementById(cartTable);var thisRow=table.rows[rowidx];if(thisRow!=null){table.deleteRow(rowidx);noOfItemInCart-=1;}
refreshCartCC();calcAmountPaidCC();}
function updateCartCC(rowidx,keyindex,data){var table=document.getElementById(cartTable);var thisRow=table.rows[rowidx];if(thisRow!=null){thisRow.setAttribute("amtcc",(data[2]));var colSize=thisRow.cells.length;for(var i=1;i<colSize;i++){cell=thisRow.cells[i];if(i==0){cell.innerHTML=rowSize+".";}
else if(i==colSize-1){cell.innerHTML="<input type=\"button\" onclick=\"deleteFromCartCC('"+ keyindex+"')\" value=Remove align='center'><input type=hidden name='PayIdCC' value='"+ data[4]+"'><input type=hidden name='BnkCC' value='"+ data[5]+"'><input type=hidden name='PayAmntCC' value='"+ data[2]+"'><input type=hidden name='PayRefnCC' value='"+ data[3]+"'>";}
else{cell.innerHTML=data[i-1];}}}}
function refreshCartCC(){var table=document.getElementById(cartTable);var rowSize=table.rows.length;for(var i=1;noOfItemInCart>0&&i<rowSize;i++){var thisRow=table.rows[i];var cell=thisRow.cells[0];thisRow.className=i%2==0?"even":"odd";cell.innerHTML=i+".";}
onMessageCC();}
function onMessageCC(){if(noOfItemInCart==0){var table=document.getElementById(cartTable);var rowSize=table.rows.length;if(rowSize==1){var thisRow=table.insertRow();var cell=thisRow.insertCell(0);cell.innerHTML="";cell.align="center";cell.colSpan="10";}}}
function calcAmountPaidCC(){var thisform=document.salesform;paid=0;if(Grandtotal.innerHTML.length==0||Grandtotal.innerHTML<=0){alert('Grand total cannot be EMPTY and value must greater than ZERO.');thisform.PayAmtCC.value='';return false;}else if(isNaN(thisform.PayAmtCC.value)){alert("Amount must be in numeric number.");thisform.PayAmtCC.value="";thisform.PayAmtCC.focus();return false;}else{if(thisform.PayAmtCC.value.length==null){paid+=thisform.PayAmtCC.value*1;}else{var table=document.getElementById(cartTable);var rowSize=table.rows.length;for(var i=1;i<rowSize;i++){var thisRow=table.rows[i];var amtcc=thisRow.getAttribute("amtcc");paid+=(amtcc*1);}
for(var i=0;i<thisform.PayAmt.length;i++){paid+=(thisform.PayAmt[i].value*1);}}
if(document.getElementById("amountPaid")!=null)
amountPaid.innerHTML=paid==0?"0.00":decFormat(paid);if(document.getElementById("balance")!=null)
balance.innerHTML=decFormat(Grandtotal.innerHTML- paid);}}