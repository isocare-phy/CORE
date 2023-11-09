<?php
//session_start();
//@header('Content-Type: text/html; charset=tis-620');
?>
<script type="text/javascript" src="../js/jquery.min.js"></script>
<script>
$(document).ready(function() {
	var boxWidth = 400;
	
	function centerBox() {
		
		/* Preliminary information */
		var winWidth = $(window).width();
		var winHeight = $(document).height();
		var scrollPos = $(window).scrollTop();
		/* auto scroll bug */
		
		/* Calculate positions */
		
		var disWidth = (winWidth - boxWidth) / 2
		var disHeight = scrollPos + 95;
		
		/* Move stuff about */
		$('.popup-box').css({'width' : boxWidth+'px', 'left' : disWidth+'px', 'top' : disHeight+'px'});
		$('#blackout').css({'width' : winWidth+'px', 'height' : winHeight+'px'});
	
		return false;
	}
	
	
	$(window).resize(centerBox);
	$(window).scroll(centerBox);
	centerBox();	

	$('[class*=popup-link]').click(function(e) {
	
		/* Prevent default actions */
		e.preventDefault();
		e.stopPropagation();
		
		/* Get the id (the number appended to the end of the classes) */
		var name = $(this).attr('class');
		var id = name[name.length - 1];
		var scrollPos = $(window).scrollTop();
		
		/* Show the correct popup box, show the blackout and disable scrolling */
		$('#popup-box-'+id).show();
		$('#blackout').show();
		$('html,body').css('overflow', 'hidden');
		
		/* Fixes a bug in Firefox */
		$('html').scrollTop(scrollPos);
	});
	$('[class*=popup-box]').click(function(e) { 
		/* Stop the link working normally on click if it's linked to a popup */
		e.stopPropagation(); 
	});
	$('html').click(function() { 
		var scrollPos = $(window).scrollTop();
		/* Hide the popup and blackout when clicking outside the popup */
		$('[id^=popup-box-]').hide(); 
		$('#blackout').hide(); 
		$("html,body").css("overflow","auto");
		$('html').scrollTop(scrollPos);
	});
	$('.close').click(function() { 
		var scrollPos = $(window).scrollTop();
		/* Similarly, hide the popup and blackout when the user clicks close */
		$('[id^=popup-box-]').hide(); 
		$('#blackout').hide(); 
		$("html,body").css("overflow","auto");
		$('html').scrollTop(scrollPos);
	});
	
	
/* abox */
	$('[class*=popup-alink]').click(function(e) {
	
		/* Prevent default actions */
		e.preventDefault();
		e.stopPropagation();
		
		/* Get the id (the number appended to the end of the classes) */
		var name = $(this).attr('class');
		var id = name[name.length - 1];
		var scrollPos = $(window).scrollTop();
		
		/* Show the correct popup box, show the blackout and disable scrolling */
		$('#popup-abox-'+id).show();
		$('#blackout').show();
		$('html,body').css('overflow', 'hidden');
		
		/* Fixes a bug in Firefox */
		$('html').scrollTop(scrollPos);
	});
	$('[class*=popup-abox]').click(function(e) { 
		/* Stop the link working normally on click if it's linked to a popup */
		e.stopPropagation(); 
	});
	$('html').click(function() { 
		var scrollPos = $(window).scrollTop();
		/* Hide the popup and blackout when clicking outside the popup */
		$('[id^=popup-abox-]').hide(); 
		$('#blackout').hide(); 
		$("html,body").css("overflow","auto");
		$('html').scrollTop(scrollPos);
	});
	$('.close').click(function() { 
		var scrollPos = $(window).scrollTop();
		/* Similarly, hide the popup and blackout when the user clicks close */
		$('[id^=popup-abox-]').hide(); 
		$('#blackout').hide(); 
		$("html,body").css("overflow","auto");
		$('html').scrollTop(scrollPos);
	});
});

</script>

<style>
.popup-box {
	position: absolute;
	border-radius: 5px;
	background: #fff;
	display: none;
	box-shadow: 1px 1px 5px rgba(0,0,0,0.2);
	font-family: Tahoma;
	z-index: 9999999;
	font-size: 14px;
}

.popup-box .close {
	position: absolute;
	top: 0px;
	right: 0px;
	font-family: Tahoma;	
	font-weight: bold;
	cursor: pointer;
	color: #434343;
	padding: 15px;
	font-size: 20px;
}

.popup-box .close:hover {
	color: #000;
}

.popup-box h2 {
	padding: 0;
	margin: 0;
	font-size: 12px;
}
.popup-box .top {
	padding: 10px;
}

.popup-box .bottom {
	background: #eee;
	border-top: 1px solid #e5e5e5;
	padding: 20px;
	border-bottom-left-radius: 5px;
	border-bottom-right-radius: 5px;
}

#blackout {
	background: rgba(0,0,0,0.3);
	position: absolute;
	top: 0;
	overflow: hidden;
	z-index: 9999;
	left: 0;
	display: none;
}


</style>