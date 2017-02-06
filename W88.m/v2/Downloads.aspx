<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Downloads.aspx.cs" Inherits="v2_Downloads" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="download-centre">
    </div>
</asp:Content>

<asp:Content ID="InnerScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">

    <script type="text/template" id='downloadItem'>

        <div class="container container-small container-thin">
            <div class="row thin-gutter">
                {% _.forEach( tplData.items, function( item ){ %}
                <div class="col-xs-6">
                    {% if(tplData.deviceId == 1 && !_.isUndefined(item.ios)){ %}
                    <a href="{%-item.ios.link%}" class="download-box">
                        <img src="{%-item.ios.src%}" alt=""><span>{%-item.title%}</span>
                    </a>
                    {% }else if(tplData.deviceId != 1  && !_.isUndefined(item.others)){ %}
                    <a href="{%-item.others.link%}" class="download-box">
                        <img src="{%-item.others.src%}" alt=""><span>{%-item.title%}</span>
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
                    title: "Massimo - Slots",
                    others: {
                        src: "/_Static/Images/bnr-clubmassimo-slots2.jpg",
                        link: "http://resigner.qfcontent.com/w88.apk",
                    }
                },
                {
                    title: "Massimo Live Casino",
                    others: {
                        src: "/_Static/Images/bnr-clubmassimo-casino-android.jpg",
                        link: "https://livegames.cdn.gameassists.co.uk/AIR/Poria/Installer/V20021/w88/Download.html",
                    }
                },
                {
                    title: "Palazzo Slots",
                    others: {
                        src: "/_Static/Images/bnr-clubpalazzo-slots2.jpg",
                        link: "/_static/palazzo/slots.aspx",
                    }
                },
                {
                    title: "Palazzo Casino",
                    others: {
                        src: "/_Static/Images/bnr-clubpalazzo-casino-android.jpg",
                        link: "/_static/palazzo/slots.aspx",
                    }
                },
                {
                    title: "<%=commonCulture.ElementValues.getResourceString("LuckyFishing", commonVariables.LeftMenuXML)%>",
                    ios: {
                        src: "/_Static/v2/assets/Images/downloads/W88-Mobile-Poker-iOS.jpg",
                        link: "itms-services://?action=download-manifest&url=https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/manifest.plist"
                    },
                    others: {
                        src: "/_Static/v2/assets/Images/downloads/W88-Mobile-Poker-Android.jpg",
                        link: "https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/FishingMaster.apk"
                    }
                },
                {
                    title: "<%=commonCulture.ElementValues.getResourceString("poker", commonVariables.LeftMenuXML)%>",
                    ios: {
                        src: "/_Static/Images/W88-Mobile-Poker-iOS.jpg",
                        link: "itms-services://?action=download-manifest&url=https://dlportal.good-game-network.com/mobile/installer/ios/W88"
                    },
                    others: {
                        src: "/_Static/Images/W88-Mobile-Poker-Android.jpg",
                        link: "http://dlportal.good-game-network.com/mobile/installer/android/W88",
                    }
                },
                {
                    title: "<%=commonCulture.ElementValues.getResourceString("texasmahjong", commonVariables.LeftMenuXML)%>",
                    ios: {
                        src: "/_Static/Images/Download/TexasMahjong-IOS.jpg",
                        link: "/_static/TexasMahjong/download.aspx",
                    },
                    others: {
                        src: "/_Static/Images/Download/TexasMahjong-Android.jpg",
                        link: "https://tm.gp2play.com/mobile/android/install.html",
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
