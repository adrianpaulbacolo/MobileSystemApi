window.w88Mobile.Gateways.Banner = Banner();
var _w88_paymentbanner = window.w88Mobile.Gateways.Banner;

function Banner() {

    var banners = {
        "Alipay": [
            {
                Image: "/_Static/Images/payments/Deposit-RMB-AliPay-Limited-CN.png",
                Videos: [
                    {
                        Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-AliPay-Limited-CN.mp4",
                        Type: "video/mp4",
                    },
                ],
                Flash: {
                    Title: "AliPay Limited",
                    Video: "http%3A%2F%2Fanecdn.w88media.com%2FCN%2Fvid%2FDeposit-RMB-AliPay-Limited-CN.mp4",
                }
            },
            {
                Image: "/_Static/Images/payments/Deposit-RMB-All-Request-Submission-Failed-CN.png",
                Videos: [
                    {
                        Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-All-Request-Submission-Failed-CN.mp4",
                        Type: "video/mp4",
                    }
                ],
                Flash: {
                    Title: "Request Submission Failed",
                    Video: "http%3A%2F%2Fanecdn.w88media.com%2FCN%2Fvid%2FDeposit-RMB-All-Request-Submission-Failed-CN.mp4",
                },
            }
        ],
        "AlipayTransfer": [
           {
               Image: "/_Static/Images/payments/Deposit-RMB-AliPay-Cannot-Redirect.png",
               Videos: [
                   {
                       Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-AliPay-Transfer-Cannot-Redirect-CN.mp4",
                       Type: "video/mp4",
                   },
               ],
               Flash: {
                   Title: "AliPay Cannot Redirect",
                   Video: "http%3A%2F%2Fanecdn.w88media.com%2FCN%2Fvid%2FDeposit-RMB-AliPay-Transfer-Cannot-Redirect-CN.mp4",
               }
           }
        ],
        "QuickOnline": [
           {
               Image: "/_Static/Images/payments/Deposit-RMB-Quick-Online-Cannot-Redirect.png",
               Videos: [
                   {
                       Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-Quick-Online-Cannot-Redirect-CN.mp4",
                       Type: "video/mp4",
                   }
               ],
               Flash: {
                   Title: "Quick Online Cannot Redirect",
                   Video: "http%3A%2F%2Fanecdn.w88media.com%2FCN%2Fvid%2FDeposit-RMB-Quick-Online-Cannot-Redirect-CN.mp4",
               },
           },
           {
               Image: "/_Static/Images/payments/Deposit-RMB-Quick-Online-Failed-CN.png",
               Videos: [
                   {
                       Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-Quick-Online-Failed-CN.mp4",
                       Type: "video/mp4",
                   }
               ],
               Flash: {
                   Title: "Quick Online Failed",
                   Video: "http%3A%2F%2Fanecdn.w88media.com%2FCN%2Fvid%2FDeposit-RMB-Quick-Online-Failed-CN.mp4",
               },
           },
           {
               Image: "/_Static/Images/payments/Deposit-RMB-All-Request-Submission-Failed-CN.png",
               Videos: [
                   {
                       Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-All-Request-Submission-Failed-CN.mp4",
                       Type: "video/mp4",
                   }
               ],
               Flash: {
                   Title: "Request Submission Failed",
                   Video: "http%3A%2F%2Fanecdn.w88media.com%2FCN%2Fvid%2FDeposit-RMB-All-Request-Submission-Failed-CN.mp4",
               },
           },
        ],
        "Wechat": [
           {
               Image: "/_Static/Images/payments/Deposit-RMB-All-Request-Submission-Failed-CN.png",
               Videos: [
                   {
                       Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-All-Request-Submission-Failed-CN.mp4",
                       Type: "video/mp4",
                   }
               ],
               Flash: {
                   Title: "Request Submission Failed",
                   Video: "http%3A%2F%2Fanecdn.w88media.com%2FCN%2Fvid%2FDeposit-RMB-All-Request-Submission-Failed-CN.mp4",
               },
           },
           {
               Image: "/_Static/Images/payments/Deposit-RMB-WeChat-Add-Bank-Card-CN.png",
               Videos: [
                   {
                       Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-WeChat-Add-Bank-Card-CN.mp4",
                       Type: "video/mp4",
                   }
               ],
               Flash: {
                   Title: "WeChat Add Bank Card",
                   Video: "http%3A%2F%2Fanecdn.w88media.com%2FCN%2Fvid%2FDeposit-RMB-WeChat-Add-Bank-Card-CN.mp4",
               },
           },
           {
               Image: "/_Static/Images/payments/Deposit-RMB-WeChat-Limited-CN.png",
               Videos: [
                   {
                       Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-WeChat-Limited-CN.mp4",
                       Type: "video/mp4",
                   }
               ],
               Flash: {
                   Title: "WeChat Limited",
                   Video: "http%3A%2F%2Fanecdn.w88media.com%2FCN%2Fvid%2FDeposit-RMB-WeChat-Limited-CN.mp4",
               },
           }
        ],
    };

    var paymentbanner = {
        init: init,
        openVideo: openVideo,
        closeVideo: closeVideo,
        forceStop: forceStop,
        forceStopWhenSubmit: forceStopWhenSubmit,
    }

    function init(id) {
        var bannerTpl = _.template(
            $("script#paymentBanner").html()
        );

        var banner = _.find(banners, function (item, objectName) {
            return _.isEqual(objectName, id);
        });

        $("div.payment-banner").append(
            bannerTpl({ Banner: banner })
        );

        $('div.payment-banner').slick({
            dots: true,
            arrows: true,
            infinite: false,
            focusOnSelect: true,
            zIndex: 1,
            appendArrows: $(".arrow-container"),
            prevArrow: '<button type="button" class="slick-prev"><span class="icon icon-arrow-left"></span></button>',
            nextArrow: '<button type="button" class="slick-next"><span class="icon icon-arrow-right"></span></button>'
        });

        $('div.payment-banner').on('swipe', function (event, slick, direction) {
            if ($(".embed-responsive").is(":visible")) {
                $(".embed-responsive video")[0].pause();
                $(".img-responsive").show(); // video
                $(".embed-responsive").hide(); // video
            }
        });

        $('div.arrow-container .slick-arrow').on('click', function (event) {
            if ($(".embed-responsive").is(":visible")) {
                $(".embed-responsive video")[0].pause();
                $(".img-responsive").show(); // video
                $(".embed-responsive").hide(); // video
            }
        });

        $('div.payment-banner .slick-dots button').on('click', function (event) {
            if ($(".embed-responsive").is(":visible")) {
                $(".embed-responsive video")[0].pause();
                $(".img-responsive").show(); // video
                $(".embed-responsive").hide(); // video
            }
        });

        $('.embed-responsive').hide();
    }

    function openVideo(me) {
        $(me.nextElementSibling).show(); // video
        $(me.nextElementSibling).find('video')[0].play();
        $(me).hide(); // image
    }


    function closeVideo(me) {
        $(me).parent().parent().find('img').show(); //image
        $(me).parent().hide(); // video
    }


    function forceStop(me) {
        $(me).parent().find('video')[0].pause(); // video
        $(me).parent().parent().find('img').show(); //image
        $(me).parent().hide(); // video
    }

    function forceStopWhenSubmit() {
        $(".payment-banner .embed-responsive video")[0].pause();
        $(".img-responsive").show(); // video
        $(".embed-responsive").hide(); // video
    }

    return paymentbanner;
}