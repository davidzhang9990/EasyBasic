$(document).ready(function(){$("#nav > li > a").on("click",function(e){if($(this).parent().has("ul")){e.preventDefault();}
if(!$(this).hasClass("open")){$("#nav li ul").slideUp(350);$("#nav li a").removeClass("open");$(this).next("ul").slideDown(350);$(this).addClass("open");}
else if($(this).hasClass("open")){$(this).removeClass("open");$(this).next("ul").slideUp(350);}});});$(document).ready(function(){$("#nav > li > ul > li > a").on("click",function(e){if($(this).parent().has("ul")){}
if(!$(this).hasClass("open")){$("#nav li ul li ul").slideUp(350);$("#nav li ul li a").removeClass("open");$(this).next("ul").slideDown(350);$(this).addClass("open");}
else if($(this).hasClass("open")){$(this).removeClass("open");$(this).next("ul").slideUp(350);}});});