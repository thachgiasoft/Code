$(document).ready(function(){
   $( ".box-nhap ul li" ).click(function() {
  
 
 // $( ".content-hide" ).css('display','none'); 
 // $( ".content-hide" ).slideToggle(); 
  $(".box-nhap ul li span").addClass("open");
  var toggle_switch = $(".img-check");
        $(".content-show").slideToggle(function () {
            if ($(this).css('display') == 'none') {
                toggle_switch.html('<img src="images/left.svg" /> ');
				$( ".content-hide" ).css('display','block');
            } else {
                toggle_switch.html('<img src="images/down.svg"/>');
				$( ".content-hide" ).css('display','none');
            }
        });
    
});
});
$(document).ready(function(){
  
 $( ".tbl-grid ul .btn-hide button" ).click(function() {
	  $( ".content-show" ).slideUp();

  $( ".content-hide" ).css('display','block'); 
});
});