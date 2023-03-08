
$(document).ready(function () {
    $(".owl-carousel").owlCarousel({
        loop: false,
        nav: true,
        dots: true,
        responsive: {
            0: {
                items: 2
            },
            300: {
                items: 3
            },
            450: {
                items: 4
            },
            650: {
                items: 5
            },
            1000: {
                items: 9
            }
        }
    });
    $(".owl-prev").html('<i class="owlLeft fa fa-chevron-left"></i>');
    $(".owl-next").html('<i class=" owlRight fa fa-chevron-right"></i>');
});


