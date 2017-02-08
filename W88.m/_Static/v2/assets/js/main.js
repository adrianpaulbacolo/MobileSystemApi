$(document).ready(function() {

    var controller = new slidebars();
    controller.init();

	$('#nav-btn').on( 'click', function ( event ) {
		event.stopPropagation();
		event.preventDefault();
		controller.toggle( 'side-nav' );

		$('.canvas').removeClass('expanded');
		$('.side-nav').removeClass('overflow-shown');
		$('.nav-category-items').removeClass('nav-category-items-shown');
	});


	$('.side-nav-items li a').on( 'click', function (event) {
		if($(this).parent().hasClass('nav-category')){
			event.stopPropagation();
			event.preventDefault();

			$('.canvas').addClass('expanded');
			$('.side-nav').addClass('overflow-shown');
			$('.nav-category-items').removeClass('nav-category-items-shown');
			$(this).parent().find('.nav-category-items').addClass('nav-category-items-shown');

		}
		else{
			controller.close( 'side-nav' );

			$('.canvas').removeClass('expanded');
			$('.side-nav').removeClass('overflow-shown');

		}
	});

	$('.nav-category-items a').on( 'click', function () {
		controller.close( 'side-nav' );

		$('.canvas').removeClass('expanded');
		$('.side-nav').removeClass('overflow-shown');
	});

	$('.canvas').on( 'click', function () {
		controller.close( 'side-nav' );

		$('.canvas').removeClass('expanded');
		$('.side-nav').removeClass('overflow-shown');
	});


	$('.home-banner').slick({
		arrows: false,
		dots:true,
		autoplay: true
	});
		
});