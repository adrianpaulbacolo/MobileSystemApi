
$(document).ready(function () {
    var url = window.location.pathname.toLowerCase();

    switch (url) {
        case "/slots.aspx":
        clickHeatGroup = 'W88MobileSlotsHome';
            break;

        case "/":
        case "/index.aspx":
        clickHeatGroup = 'W88MobileIndex';
            break;
    }

    if (clickHeatGroup.length > 0) {
        clickHeatSite = 'W88Mobile';
        clickHeatServer = 'https://clickheat.liveperson88.com/clickheat/clickempty.html';
        initClickHeat();
    }
});