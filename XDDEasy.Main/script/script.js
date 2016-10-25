function opennew(link){window.open(link,'','menubr=no,resizable=no,toolbar=no,scrollbars=yes,status=no,width=550,height=400,screenX=0,screenY=0');}
function opennewcal(link){window.open(link,'OjdkJ','menubr=no,resizable=no,toolbar=no,scrollbars=no,status=no,width=250,height=250,screenX=0,screenY=0');}
function opennewIDExamples(link){window.open(link,'','menubr=no,resizable=no,toolbar=no,scrollbars=no,status=no,width=400,height=200,screenX=0,screenY=0');}
function focusAndSelect(obj){obj.focus();obj.select();}
function loadCombobox(obj,selectedValue){for(var i=0;obj!=null&&i<obj.length;i++){if(obj.type=="select-one"){if(obj.options[i].value==selectedValue){obj.options[i].selected=true;return;}}else if(obj[i].type=="radio"){if(obj[i].value==selectedValue){obj[i].checked=true;return;}}}}
function TrimObjValue(obj){obj.value=Trim(obj.value);}
function Trim(str){var res=/^\s+/ig;var ree=/\s+$/ig;var out=str.replace(res,"").replace(ree,"");return out;}
function TrimAll(str){var rem=/\s+/ig;var out=Trim(str);out=out.replace(rem,"");return out;}
function enterMbrId(obj){var result=obj.value;if(result.indexOf(" ")>=0){var parseStr="";for(var i=0;i<result.length;i++){if(result.charAt(i)!=" ")
parseStr=parseStr+ result.charAt(i);}
result=parseStr;obj.value=result;}}
function enterNRIC(obj){var result=obj.value;if(result.indexOf("-")>=0){var parseStr="";for(var i=0;i<result.length;i++){if(result.charAt(i)!="-")
parseStr=parseStr+ result.charAt(i);}
result=parseStr;obj.value=result;}}
function validateMbrId(obj){var validId=/^[a-zA-Z0-9]*$/;var vl=TrimAll(obj.value);if((!validId.test(vl))||(vl.length<7)){obj.value=vl;focusAndSelect(obj);alert("Invalid Distributor ID.");return false;}else{return true;}}
function validateNRIC(obj){var validNRIC=/^\d{6}\d{2}\d{4}$/;var vl=TrimAll(obj.value);if(!validNRIC.test(vl)){obj.value=vl;focusAndSelect(obj);alert("Please fill in NRIC No. in a valid format,\ne.g. 810231074123.");return false;}else{return true;}}
function validateFormNo(obj){var validFormNo=/^[a-zA-Z0-9]*$/;var vl=TrimAll(obj.value);if((!(validFormNo.test(vl)))||(vl.length<6)){obj.value=vl;focusAndSelect(obj);alert("Invalid Registration Form No.");return false;}else{return true;}}
var dtCh="-";function isInteger(s){var i;for(i=0;i<s.length;i++){var c=s.charAt(i);if(((c<"0")||(c>"9")))return false;}
return true;}
function stripCharsInBag(s,bag){var i;var returnString="";for(i=0;i<s.length;i++){var c=s.charAt(i);if(bag.indexOf(c)==-1)returnString+=c;}
return returnString;}
function daysInFebruary(year){return(((year%4==0)&&((!(year%100==0))||(year%400==0)))?29:28);}
function DaysArray(n){for(var i=1;i<=n;i++){this[i]=31
if(i==4||i==6||i==9||i==11){this[i]=30}
if(i==2){this[i]=29}}
return this}
function isDate(obj){var dtStr=obj.value
var msg="Invalid date!"
var daysInMonth=DaysArray(12)
var pos1=dtStr.indexOf(dtCh)
var pos2=dtStr.indexOf(dtCh,pos1+1)
var strDay=dtStr.substring(0,pos1)
var strMonth=dtStr.substring(pos1+1,pos2)
var strYear=dtStr.substring(pos2+1)
strYr=strYear
if(strDay.charAt(0)=="0"&&strDay.length>1)strDay=strDay.substring(1)
if(strMonth.charAt(0)=="0"&&strMonth.length>1)strMonth=strMonth.substring(1)
for(var i=1;i<=3;i++){if(strYr.charAt(0)=="0"&&strYr.length>1)strYr=strYr.substring(1)}
month=parseInt(strMonth)
day=parseInt(strDay)
year=parseInt(strYr)
if(pos1==-1||pos2==-1){focusAndSelect(obj);alert(msg);return false}
if(strMonth.length<1||month<1||month>12){focusAndSelect(obj);alert(msg);return false}
if(strDay.length<1||day<1||day>31||(month==2&&day>daysInFebruary(year))||day>daysInMonth[month]){focusAndSelect(obj);alert(msg);return false}
if(strYear.length!=4||year==0){focusAndSelect(obj);alert(msg);return false}
if(dtStr.indexOf(dtCh,pos2+1)!=-1||isInteger(stripCharsInBag(dtStr,dtCh))==false){focusAndSelect(obj);alert(msg);return false}
return true}