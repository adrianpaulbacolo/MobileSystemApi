<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <meta name="viewport" id="viewport" content="minimal-ui, width=device-width, initial-scale=1.0, maximum-scale=1.0,user-scalable=no,target-densityDpi=device-dpi" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <script type="text/javascript" src="//js.w2script.com/_Static/JS/Mobile/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="//js.w2script.com/_Static/JS/Mobile/jquery.mobile-1.4.2.min.js"></script>
    <script src="_Static/Scripts/iscroll.js"></script>
    <script type="text/javascript">

        var myScroll;
        function loaded() {
            myScroll = new iScroll('wrapper');
        }
        document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
        document.addEventListener('DOMContentLoaded', loaded, false);

    </script>

    <style type="text/css" media="all">
       
        #wrapper {
            position: absolute;
            z-index: 1;
            top: 45px;
            bottom: 0;
            left: 0;
            width: 100%;
            background: #aaa;
            overflow: auto;
        }

        #scroller {
            width: 2040px;
            height: 200px;
            float: left;
            padding: 0;
        }

        #scroller ul {
                list-style: none;
                display: block;
                float: left;
                width: 100%;
                height: 100%;
                padding: 0;
                margin: 0;
                text-align: left;
            }

            #scroller li {
                display: block;
                vertical-align: middle;
                float: left;
                padding: 0 10px;
                width: 80px;
                height: 100%;
                border-left: 1px solid #ccc;
                border-right: 1px solid #fff;
                background-color: #fafafa;
                font-size: 14px;
            }
    </style>
   
</head>
<body>

    <div id="wrapper" style="overflow: hidden;">
        <div id="scroller">
            <ul id="thelist">
                <li>Pretty col 1</li>
                <li>Pretty col 2</li>
                <li>Pretty col 3</li>
                <li>Pretty col 4</li>
                <li>Pretty col 5</li>
                <li>Pretty col 6</li>
                <li>Pretty col 7</li>
                <li>Pretty col 8</li>
                <li>Pretty col 9</li>
                <li>Pretty col 10</li>
                <li>Pretty col 11</li>
                <li>Pretty col 12</li>
                <li>Pretty col 13</li>
                <li>Pretty col 14</li>
                <li>Pretty col 15</li>
                <li>Pretty col 16</li>
                <li>Pretty col 17</li>
                <li>Pretty col 18</li>
                <li>Pretty col 19</li>
                <li>Pretty col 20</li>
            </ul>
        </div>
    </div>

</body>
</html>
