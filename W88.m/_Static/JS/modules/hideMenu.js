
function HideMenu() {

    var HideMenu = {
        CheckIfApp: function () {
            if (Cookies().getCookie('IsApp') == 1) {
                $("#aMenu").css('display', 'none');

                if (document.URL.toLowerCase().indexOf("/history") > 0) {
                    $("#NonAppMenu").css('display', 'none');
                }
            } else {
                $("#appMenu").css('display', 'none');
            }
        }
    };

    return HideMenu;
}

window.w88Mobile.HideMenu = HideMenu();