window.w88Mobile.Gateways.Banner = Banner();
var _w88_paymentbanner = window.w88Mobile.Gateways.Banner;

function Banner() {

    var banners = {
        "Alipay": [
            {
                Image: "/_Static/Images/payments/Deposit-RMB-AliPay-Limited-CN.png",
                Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-AliPay-Limited-CN.mp4"
            },
            {
                Image: "/_Static/Images/payments/Deposit-RMB-All-Request-Submission-Failed-CN.png",
                Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-All-Request-Submission-Failed-CN.mp4"
            }
        ],
        "AlipayTransfer": [
           {
               Image: "/_Static/Images/payments/Deposit-RMB-AliPay-Cannot-Redirect.png",
               Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-AliPay-Transfer-Cannot-Redirect-CN.mp4"
           }
        ],
        "QuickOnline": [
           {
               Image: "/_Static/Images/payments/Deposit-RMB-Quick-Online-Cannot-Redirect.png",
               Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-Quick-Online-Cannot-Redirect-CN.mp4"
           },
           {
               Image: "/_Static/Images/payments/Deposit-RMB-Quick-Online-Failed-CN.png",
               Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-Quick-Online-Failed-CN.mp4"
           },
           {
               Image: "/_Static/Images/payments/Deposit-RMB-All-Request-Submission-Failed-CN.png",
               Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-All-Request-Submission-Failed-CN.mp4"
           }
        ],
        "Wechat": [
           {
               Image: "/_Static/Images/payments/Deposit-RMB-All-Request-Submission-Failed-CN.png",
               Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-All-Request-Submission-Failed-CN.mp4"
           },
           {
               Image: "/_Static/Images/payments/Deposit-RMB-WeChat-Add-Bank-Card-CN.png",
               Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-WeChat-Add-Bank-Card-CN.mp4"
           },
           {
               Image: "/_Static/Images/payments/Deposit-RMB-WeChat-Limited-CN.png",
               Video: "http://anecdn.w88media.com/CN/vid/Deposit-RMB-WeChat-Limited-CN.mp4"
           }
        ],
    };

    var paymentbanner = {
        init: init,
        openVideo: openVideo,
        closeVideo: closeVideo
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
            arrows: false,
            infinite: false,
            focusOnSelect: true,
        });

        $('.video-responsive').hide();
    }

    function openVideo(me) {
        $(me.nextElementSibling).show(); // video
        $(me.nextElementSibling)[0].play();
        $(me).hide(); // image
    }


    function closeVideo(me) {
        $(me.previousElementSibling).show(); //image
        $(me).hide(); // video
    }

    return paymentbanner;
}