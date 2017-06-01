<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Downloads.aspx.cs" Inherits="v2_Downloads" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="download-centre">
    </div>
</asp:Content>

<asp:Content ID="InnerScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">

    <script type="text/template" id='downloadItem'>

        <div class="container container-small container-extra-thin">
            <div class="row extra-thin-gutter">
                {% _.forEach( tplData.items, function( item ){ %}
                <div class="col-xs-6">
                    {% if(tplData.deviceId == 1 && !_.isUndefined(item.ios)){ %}
                    <a href="{%-item.ios.link%}" class="download-box" target="{%- (!_.isUndefined(item.ios.target)) ? item.ios.target : '_self' %}">
                        <img src="{%-item.ios.src%}" alt="">
                        <div class="download-box-title">
                            <span>{{item.title}}</span>
                        </div>
                    </a>
                    {% }else if(tplData.deviceId != 1  && !_.isUndefined(item.others)){ %}
                    <a href="{%-item.others.link%}" class="download-box" target="{%- (!_.isUndefined(item.others.target)) ? item.others.target : '_self' %}">
                        <img src="{%-item.others.src%}" alt="">
                        <div class="download-box-title">
                            <span>{{item.title}}</span>
                        </div>
                    </a>
                    {% } %}
                </div>
                {% }); %}
            </div>
        </div>

    </script>
    <script type="text/javascript">
        var deviceId = "<%=commonFunctions.getMobileDevice(Request) %>";

        var downloadItems = [
                {
                    title: $.i18n("LABEL_PRODUCTS_CLUB_W"),
                    ios: {
                        src: "/_Static/Images/sports/bnr-clubW88-iOS.jpg",
                        link: _constants.DOWNLOAD_CLUBW_IOS_URL,
                    },
                    others: {
                        src: "/_Static/Images/sports/bnr-clubW88-Android.jpg",
                        link: _constants.CLUBW_APK_URL,
                        target: "_blank"
                    }
                },
                {
                    title: $.i18n("LABEL_PRODUCTS_MASSIMO") + "<small>" + $.i18n("LABEL_MENU_SLOTS") + "</small>",
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/MGS-Slots-Android.jpg",
                        link: _constants.MASSIMO_SLOTS_URL,
                    }
                },
                {
                    title: $.i18n("LABEL_PRODUCTS_MASSIMO") + "<small>" + $.i18n("LABEL_MENU_LIVE_CASINO") + "</small>",
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/MGS-LiveCasino-Android.jpg",
                        link: _constants.MASSIMO_URL,
                        target: "_blank"
                    }
                },
                {
                    title: $.i18n("LABEL_PRODUCTS_PALAZZO") + "<small>" + $.i18n("LABEL_MENU_SLOTS") + "</small>",
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/PT-Slots-Android.jpg",
                        link: _constants.DOWNLOAD_PALAZZO_SLOT_URL,
                    }
                },
                {
                    title: $.i18n("LABEL_PRODUCTS_PALAZZO") + "<small>" + $.i18n("LABEL_MENU_LIVE_CASINO") + "</small>",
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/PT-LiveCasino-Android.jpg",
                        link: _constants.DOWNLOAD_PALAZZO_CASINO_URL,
                    }
                },
                {
                    title: $.i18n("LABEL_PRODUCTS_FISHING_MASTER"),
                    ios: {
                        src: "/_Static/v2/Assets/Images/downloads/FishingMaster-iOS.jpg",
                        link: _constants.FISHING_MASTER_IOS_URL
                    },
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/FishingMaster-Android.jpg",
                        link: _constants.FISHING_MASTER_APK_URL
                    }
                },
                {
                    title: $.i18n("LABEL_MENU_POKER"),
                    ios: {
                        src: "/_Static/v2/Assets/Images/downloads/Poker-iOS.jpg",
                        link: _constants.POKER_IOS_URL
                    },
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/Poker-Android.jpg",
                        link: _constants.POKER_APK_URL
                    }
                },
                {
                    title: $.i18n("LABEL_MENU_TEXAS_MAHJONG"),
                    ios: {
                        src: "/_Static/v2/Assets/Images/downloads/TexasMahjong-iOS.jpg",
                        link: _constants.DOWNLOAD_TM_IOS_URL,
                    },
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/TexasMahjong-Android.jpg",
                        link: _constants.TEXAS_MAHJONG_APK_URL
                    }
                },
                {
                    title: $.i18n("LABEL_MENU_SUPERBULL"),
                    ios: {
                        src: "/_Static/Images/Download/bnr-superbull-ios.jpg",
                        link: _constants.DOWNLOAD_SB_IOS_URL,
                    },
                    others: {
                        src: "/_Static/Images/Download/bnr-superbull-android.jpg",
                        link: _constants.SUPER_BULL_APK_URL,
                        target: "_blank"
                    }
                }
        ];

        var walletListTpl = _.template(
            $("script#downloadItem").html()
        );

        $("div.download-centre").append(
            walletListTpl({ items: downloadItems, deviceId: deviceId })
        );
    </script>
</asp:Content>
