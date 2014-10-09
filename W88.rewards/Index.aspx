<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>




<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link rel="stylesheet" href="/_Static/Css/add2home.css">
    <script type="application/javascript" src="/_Static/Js/add2home.js"></script>
    <!--[if IE]><link type="text/css" href="/_Static/Css/Index.css" rel="stylesheet"><![endif]-->
    <!--[if !IE]><!-->
    <link type="text/css" href="/_Static/Css/IndexScroll.css" rel="stylesheet">
    <!--<![endif]-->
    <style>
     

    </style>

    <script type="text/javascript">
      
      
    </script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div id="divLoginMessage" runat="server"><span id="lblLogin" runat="server">please login to start redemption</span></div>
    <div id="divLevel" runat="server" visible="False">
                <span id="lblPoint" runat="server"></span>
            </div>
            <div id="divContent">
                <div class="div-product-scroll">
                    <div>
            <asp:listview ID="Listview1" runat="server" GroupItemCount="3">
		        <LayoutTemplate>
			        <table id="tblCatalogue">
				        <div runat="server" id="groupPlaceholder">
				        </div>
			        </table>
		        </LayoutTemplate>
		         <GroupTemplate>
                        <tr>
					        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tr>
                    </GroupTemplate>
		        <ItemTemplate>
                    <td>
                        <a href="/Product?categoryId=<%#DataBinder.Eval(Container.DataItem,"categoryId")%>&sortBy=2">
							<img src="<%#DataBinder.Eval(Container.DataItem,"imagePathOff")%>" data-imageover="<%#DataBinder.Eval(Container.DataItem,"imagePathOn")%>" />
                        <br /><div class="dinpro catName"><%#DataBinder.Eval(Container.DataItem,"categoryName")%></div>
                        <br /><div class="line" style="height: 5px; background: url('../img/banner_catalogue/underline_grey.jpg') repeat-x; margin-top: 15px;" class="clear"></div> 
				        </a>
			        </td>

                    <td style="width: 5%; text-align: center;"></td>
		        </ItemTemplate>
            </asp:listview>
                        
                    </div>
                </div>


            </div>

        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
