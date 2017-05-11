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
                    title: "<%=commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoSlots/Label", commonVariables.ProductsXML).Replace("<br />", "")%><small><%=commonCulture.ElementValues.getResourceString("slots", commonVariables.LeftMenuXML)%></small>",
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/MGS-Slots-Android.jpg",
                        link: "http://resigner.qfcontent.com/w88.apk",
                    }
                },
                {
                    title: "<%=commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoSlots/Label", commonVariables.ProductsXML).Replace("<br />", "")%><small><%=commonCulture.ElementValues.getResourceString("livecasino", commonVariables.LeftMenuXML)%></small>",
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/MGS-LiveCasino-Android.jpg",
                        link: "https://livegames.cdn.gameassists.co.uk/AIR/Poria/Installer/V20021/w88/Download.html",
                        target: "_blank"
                    }
                },
                {
                    title: "<%=commonCulture.ElementValues.getResourceXPathString("Products/ClubPalazzoSlots/Label", commonVariables.ProductsXML).Replace("<br />", "")%><small><%=commonCulture.ElementValues.getResourceString("slots", commonVariables.LeftMenuXML)%></small>",
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/PT-Slots-Android.jpg",
                        link: "/v2/Downloads/palazzo-slots",
                    }
                },
                {
                    title: "<%=commonCulture.ElementValues.getResourceXPathString("Products/ClubPalazzoSlots/Label", commonVariables.ProductsXML).Replace("<br />", "")%><small><%=commonCulture.ElementValues.getResourceString("livecasino", commonVariables.LeftMenuXML)%></small>",
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/PT-LiveCasino-Android.jpg",
                        link: "/v2/Downloads/palazzo-casino",
                    }
                },
                {
                    title: "<%=commonCulture.ElementValues.getResourceString("LuckyFishing", commonVariables.LeftMenuXML)%>",
                    ios: {
                        src: "/_Static/v2/Assets/Images/downloads/FishingMaster-iOS.jpg",
                        link: "itms-services://?action=download-manifest&url=https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/manifest.plist"
                    },
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/FishingMaster-Android.jpg",
                        link: "https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/FishingMaster.apk"
                    }
                },
                {
                    title: "<%=commonCulture.ElementValues.getResourceString("poker", commonVariables.LeftMenuXML)%>",
                    ios: {
                        src: "/_Static/v2/Assets/Images/downloads/Poker-iOS.jpg",
                        link: "itms-services://?action=download-manifest&url=https://dlportal.good-game-network.com/mobile/installer/ios/W88"
                    },
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/Poker-Android.jpg",
                        link: "http://dlportal.good-game-network.com/mobile/installer/android/W88",
                    }
                },
                {
                    title: "<%=commonCulture.ElementValues.getResourceString("texasmahjong", commonVariables.LeftMenuXML)%>",
                    ios: {
                        src: "/_Static/v2/Assets/Images/downloads/TexasMahjong-iOS.jpg",
                        link: "/v2/Downloads/texas-mahjong-ios",
                    },
                    others: {
                        src: "/_Static/v2/Assets/Images/downloads/TexasMahjong-Android.jpg",
                        link: "https://tm.gp2play.com/mobile/android/install.html",
                    }
                },
                {
                    title: _w88_contents.translate("LABEL_MENU_SUPERBULL"),
                    ios: {
                        src: "/_Static/Images/Download/bnr-superbull-ios.jpg",
                        link: "/v2/Downloads/super-bull-ios",
                    },
                    others: {
                        src: "/_Static/Images/Download/bnr-superbull-android.jpg",
                        link: "<%= commonFunctions.GetSuperBullDownloadLink()%>",
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
