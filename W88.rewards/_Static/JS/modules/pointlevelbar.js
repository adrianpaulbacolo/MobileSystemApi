var PointLevelBar = function(data, images) {
    this.data = data;
    this.url = '/api/rewards/pointlevelbar';
    this.images = images;
};

PointLevelBar.prototype.getPointLevelBar = function() {
    var self = this;
    $.ajax({
        type: 'GET',
        async: true,
        url: self.url,
        contentType: 'application/json',
        dataType: 'json',
        data: self.data,
        success: function(response) {
            try {
                if (!response || response.ResponseCode != 1 || !response.ResponseData) return;
                var pointLevelBarData = response.ResponseData,
                    currentText = $('#pointlevelNext').html();
                $("#pointlevelNext").html(currentText.replace('{0}', pointLevelBarData.RemainingPoints).replace('{1}', pointLevelBarData.NextPointLevel));

                var nextPointLevelImgSrc = self.images.to.replace('{0}', pointLevelBarData.NextPointLevel);
                $("#levelBar").find("img.ImgFrom").attr("src", self.images.from.replace('{0}', pointLevelBarData.CurrentLevel));
                $("#levelBar").find("img.ImgTo").attr("src", nextPointLevelImgSrc);

                self.colorBar(pointLevelBarData.CurrentLevel.toString(), pointLevelBarData.NextPointLevel.toString(), pointLevelBarData.PercentageColor, pointLevelBarData.Percentage);

                if (pointLevelBarData.CurrentLevel >= 5 || pointLevelBarData.NextPointLevel >= 6) {
                    $("#levelBar").find("div.levelDesc").hide();
                } else {
                    $("#levelBar").find("div.levelDesc").show();
                }

                if (pointLevelBarData.CurrentLevel == 8) {
                    $("#levelBar").find("div.PointsLevelBar").css("style", "visibility:visible;");
                    $("#levelBar").find("img.ImgFrom").attr("style", "visibility:hidden;");
                    $("#levelBar").find("img.ImgTo").attr("style", "visibility:visible;");
                    $("#levelBar").find("img.ImgTo").attr("src", nextPointLevelImgSrc);
                    $("#levelBar").find("span.level8").show();
                    $("#levelBar").find("span.levelNormal").remove();
                } else {
                    $("#levelBar").find("span.level8").remove();
                }

                $("#levelBar").show();
            } catch (e) {
            }
        }
    });
};

PointLevelBar.prototype.colorBar = function(currentLevel, nextLevel, percentageColor, percentage) {
    var self = this,
        color1 = "#fff100", //yellow
        color2 = "#a5dc00", //green
        color3 = "#ffa70e", //orange
        color4 = "#ff0000", //red
        color5 = "#57deff", //sky blue
        color6 = "#e636db", //pink
        color7 = "#9800f9", //purple 
        color8 = "#0717ed", //slateblue
        rgbColor1 = "rgb(255, 241, 0)",
        rgbColor2 = "rgb(165, 220, 0)",
        rgbColor3 = "rgb(255, 167, 14)",
        rgbColor4 = "rgb(255, 0, 0)",
        rgbColor5 = "rgb(87, 222, 255)",
        rgbColor6 = "rgb(230, 54, 219)",
        rgbColor7 = "rgb(152, 0, 249)",
        rgbColor8 = "rgb(7, 23, 237)",
        rgbStart;

    switch (currentLevel) {
        case "1":
            rgbStart = rgbColor1;
            break;
        case "2":
            rgbStart = rgbColor2;
            break;
        case "3":
            rgbStart = rgbColor3;
            break;
        case "4":
            rgbStart = rgbColor4;
            break;
        case "5":
            rgbStart = rgbColor5;
            break;
        case "6":
            rgbStart = rgbColor6;
            break;
        case "7":
            rgbStart = rgbColor7;
            break;
        case "8":
            rgbStart = rgbColor8;
            break;
    }

    var rgbEnd;
    switch (nextLevel) {
        case "1":
            rgbEnd = rgbColor1;
            break;
        case "2":
            rgbEnd = rgbColor2;
            break;
        case "3":
            rgbEnd = rgbColor3;
            break;
        case "4":
            rgbEnd = rgbColor4;
            break;
        case "5":
            rgbEnd = rgbColor5;
            break;
        case "6":
            rgbEnd = rgbColor6;
            break;
        case "7":
            rgbEnd = rgbColor7;
            break;
        case "8":
            rgbEnd = rgbColor8;
            break;
    }

    //current level >= 6 display 50%
    if (currentLevel >= "5") {
        percentageColor = "5";
        percentage = "50";
    }

    //get each rgb number
    //start
    var digitsStart = /(.*?)rgb\((\d+), (\d+), (\d+)\)/.exec(rgbStart),
        redStart = parseInt(digitsStart[2]),
        greenStart = parseInt(digitsStart[3]),
        blueStart = parseInt(digitsStart[4]);

    //end
    var digitsEnd = /(.*?)rgb\((\d+), (\d+), (\d+)\)/.exec(rgbEnd),
        redEnd = parseInt(digitsEnd[2]),
        greenEnd = parseInt(digitsEnd[3]),
        blueEnd = parseInt(digitsEnd[4]);

    //red
    var red = Math.floor((percentageColor / 10) * (redStart - redEnd)),
        redBar = redStart - red;

    var green = Math.floor((percentageColor / 10) * (greenStart - greenEnd)),
        greenBar = greenStart - green;

    //blue
    var blue = Math.floor((percentageColor / 10) * (blueStart - blueEnd)),
        blueBar = blueStart - blue;

    var resultStart = self.colorToHex(rgbStart),
        result = self.colorToHex("rgb(" + redBar + ", " + greenBar + ", " + blueBar + ")");

    //cross 1 level
    var hex;
    switch (parseInt(currentLevel) + 1) {
        case 2:
            hex = color2;
            break;
        case 3:
            hex = color3;
            break;
        case 4:
            hex = color4;
            break;
        case 5:
            hex = color5;
            break;
        case 6:
            hex = color6;
            break;
        case 7:
            hex = color7;
            break;
        case 8:
            hex = color8;
            break;
    }

    //cross 2 level
    var hex2;
    switch (parseInt(currentLevel) + 2) {
        case 3:
            hex2 = color3;
            break;
        case 4:
            hex2 = color4;
            break;
        case 5:
            hex2 = color5;
            break;
        case 6:
            hex2 = color6;
            break;
        case 7:
            hex2 = color7;
            break;
        case 8:
            hex2 = color8;
            break;
    }

    //cross 3 level
    var hex3;
    switch (parseInt(currentLevel) + 3) {
        case 4:
            hex3 = color4;
            break;
        case 5:
            hex3 = color5;
            break;
        case 6:
            hex3 = color6;
            break;
        case 7:
            hex3 = color7;
            break;
        case 8:
            hex3 = color8;
            break;
    }

    //cross 4 level
    var hex4;
    switch (parseInt(currentLevel) + 4) {
        case 5:
            hex4 = color5;
            break;
        case 6:
            hex4 = color6;
            break;
        case 7:
            hex4 = color7;
            break;
        case 8:
            hex4 = color8;
            break;
    }

    //cross 5 level
    var hex5;
    switch (parseInt(currentLevel) + 5) {
        case 6:
            hex5 = color6;
            break;
        case 7:
            hex5 = color7;
            break;
        case 8:
            hex5 = color8;
            break;
    }

    //cross 6 level
    var hex6;
    switch (parseInt(currentLevel) + 6) {
        case 7:
            hex6 = color7;
            break;
        case 8:
            hex6 = color8;
            break;
    }

    var display, display2;
    switch (nextLevel - currentLevel) {
        case 1:
            display = resultStart + " 0%, " + result + " 100%";
            display2 = "color-stop(0%, " + resultStart + "),color-stop(100%," + result + ")";
            break;
        case 2:
            display = resultStart + " 0%, " + hex + " 50%, " + result + " 100%";
            display2 = "color-stop(0%, " + resultStart + "),color-stop(50%," + hex + "),color-stop(100%," + result + ")";
            break;
        case 3:
            display = resultStart + " 0%, " + hex + " 35%, " + hex2 + " 65%, " + result + " 100%";
            display2 = "color-stop(0%, " + resultStart + "),color-stop(35%," + hex + "),color-stop(65%," + hex2 + "),color-stop(100%," + result + ")";
            break;
        case 4:
            display = resultStart + " 0%, " + hex + " 25%, " + hex2 + " 50%, " + hex3 + " 75%, " + result + " 100%";
            display2 = "color-stop(0%, " + resultStart + "),color-stop(25%," + hex + "),color-stop(50%," + hex2 + "),color-stop(75%," + hex3 + "),color-stop(100%," + result + ")";
            break;
        case 5:
            display = resultStart + " 0%, " + hex + " 20%, " + hex2 + " 40%, " + hex3 + " 60%, " + hex4 + " 80%, " + result + " 100%";
            display2 = "color-stop(0%, " + resultStart + "),color-stop(20%," + hex + "),color-stop(40%," + hex2 + "),color-stop(60%," + hex3 + "),color-stop(80%," + hex4 + "),color-stop(100%," + result + ")";
            break;
        case 6:
            display = resultStart + " 0%, " + hex + " 17%, " + hex2 + " 34%, " + hex3 + " 51%, " + hex4 + " 68%, " + hex5 + " 85%, " + result + " 100%";
            display2 = "color-stop(0%, " + resultStart + "),color-stop(17%," + hex + "),color-stop(34%," + hex2 + "),color-stop(51%," + hex3 + "),color-stop(68%," + hex4 + "),color-stop(85%," + hex5 + "),color-stop(100%," + result + ")";
            break;
        case 7:
            display = resultStart + " 0%, " + hex + " 14%, " + hex2 + " 28%, " + hex3 + " 42%, " + hex4 + " 56%, " + hex5 + " 70%, " + hex6 + " 84%, " + result + " 100%";
            display2 = "color-stop(0%, " + resultStart + "),color-stop(14%," + hex + "),color-stop(28%," + hex2 + "),color-stop(42%," + hex3 + "),color-stop(56%," + hex4 + "),color-stop(70%," + hex5 + "),color-stop(84%," + hex6 + "),color-stop(100%," + result + ")";
            break;
        case 0:
            display = "";
            display2 = "";
            break;
    }

    //change to pixel due to firefox not able to get % in width 
    //bar width = 200px
    var pixel = 200 * (percentage / 100);
    $("#levelBar").find("div.PointsLevelBar").attr("style", "height:11px;visibility:visible;width:" + pixel + "px; box-shadow: inset 8px 8px 8px rgba(255,255,255,0.60);background: " + resultStart + " url('" + self.images.barBackground + "') ; /* Old browsers */ background:url('" + self.images.barBackground + "'), -moz-linear-gradient(left, " + display + "); /* FF3.6+ */ background:url('" + self.images.barBackground + "'), -webkit-gradient(linear, left top, right top, " + display2 + "); /* Chrome,Safari4+ */ background:url('" + self.images.barBackground + "'), -webkit-linear-gradient(left, " + display + "); /* Chrome10+,Safari5.1+ */ background:url('" + self.images.barBackground + "'), -o-linear-gradient(left, " + display + "); /* Opera 11.10+ */ background:url('" + self.images.barBackground + "'), -ms-linear-gradient(-left,  " + display + "); /* IE10+ */ background:url('" + self.images.barBackground + "'), linear-gradient(to right, " + display + ") ; /* W3C */ filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='" + resultStart + "', endColorstr='" + result + "',GradientType=1 ); /* IE6-9 */ ");
};

PointLevelBar.prototype.colorToHex = function(color) {
    if (color.substr(0, 1) === '#') {
        return color;
    }
    var digits = /(.*?)rgb\((\d+), (\d+), (\d+)\)/.exec(color);

    var red = parseInt(digits[2]);
    var green = parseInt(digits[3]);
    var blue = parseInt(digits[4]);

    var rgb = blue | (green << 8) | (red << 16);
    return digits[1] + '#' + rgb.toString(16);
};