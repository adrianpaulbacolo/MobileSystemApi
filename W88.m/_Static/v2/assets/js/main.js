$(document).ready(function() {

    var controller = new slidebars();
    controller.init();

	$('#nav-btn').on( 'click', function ( event ) {
		event.stopPropagation();
		event.preventDefault();
		controller.toggle( 'side-nav' );

		$('.canvas').removeClass('expanded');
		$('.side-nav').removeClass('overflow-shown');
		$('.nav-category-items').removeClass('nav-category-items-shown');
	});


	$('.side-nav-items li a').on( 'click', function (event) {
		if($(this).parent().hasClass('nav-category')){
			event.stopPropagation();
			event.preventDefault();

			$('.canvas').addClass('expanded');
			$('.side-nav').addClass('overflow-shown');
			$('.nav-category-items').removeClass('nav-category-items-shown');
			$(this).parent().find('.nav-category-items').addClass('nav-category-items-shown');

		}
		else{
			controller.close( 'side-nav' );

			$('.canvas').removeClass('expanded');
			$('.side-nav').removeClass('overflow-shown');

		}
	});

	$('.nav-category-items a').on( 'click', function () {
		controller.close( 'side-nav' );

		$('.canvas').removeClass('expanded');
		$('.side-nav').removeClass('overflow-shown');
	});

	$('.canvas').on( 'click', function () {
		controller.close( 'side-nav' );

		$('.canvas').removeClass('expanded');
		$('.side-nav').removeClass('overflow-shown');
	});
		
});

function NotAllowDecimal(e) {
    var key = e.keyCode;
    if ($.browser.mozilla) {
        key = e.which;
    }
    if (key != 0 && key != 8) {
        var regex = new RegExp("^[0-9]+$");
        var code = String.fromCharCode(key);
        if (!regex.test(code))
            return false;
    }
}

function CheckWholeNumber(element) {
    if (element.val().length > 0) {
        if (element.val().indexOf('.') >= 0) {
            element.parent("div.ui-input-text").attr("style", "border-bottom: 2px solid red !important");
            return false;
        } else {
            element.parent("div.ui-input-text").removeAttr("style");
            return true;
        }
    } else {
        return false;
    }
}

function ValidatePositiveDecimal(ctrl, e, cur) {
    var allowDecimal;
    if (cur === undefined) cur = "";
    switch (cur) {
        case "JPY":
            allowDecimal = false;
            break;
        default:
            allowDecimal = true;
            break;
    }

    var key = e.keyCode;
    if ($.browser.mozilla) {
        key = e.which;
    }
    if (key != 0 && key != 8) {
        if (key == 46) {
            if (!allowDecimal) return false;

            var code = String.fromCharCode(key);
            if (ctrl.value.indexOf(code) >= 0)
                return false;
        }
        else if (key < 48 || key > 57)
            return false;
        else {
            var num = parseFloat($(ctrl).val() + String.fromCharCode(key));
            var cleanNum = num.toFixed(2);
            if (num / cleanNum != 1)
                return false;
        }
    }
}

function PositiveOneDecimalValidation(value, element) {
    if (isNaN(value) || value <= 0)
        return false;
    var numArr = value.split(".");
    if (numArr.length > 1 && numArr[1].length > 1)
        return false;
    return true;
}

function TwoDecimalAndroid(ctrl, event) {
    var $this = ctrl;
    if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
       ((event.which < 48 || event.which > 57) &&
       (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }

    var text = $this.val();
    if ((event.which == 46) && (text.indexOf('.') == -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }

    if ((text.indexOf('.') != -1) && (text.substring(text.indexOf('.')).length > 2) && (event.which != 0 && event.which != 8) && ($this[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
}

function getQueryStringValue(key) {
    return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}

function addMonths(date, months) {
    date.setMonth(date.getMonth() + months);
    return date;
}

function addHours(date, hours) {
    date.setMonth(date.getHours() + hours);
    return date;
}

function _w88_send(resource, method, data, success, complete) {

    var selector = "";
    if (!_.isEmpty(data) && !_.isEmpty(data.selector)) {
        selector = _.clone(data.selector);
        delete data["selector"];
    }

    var url = w88Mobile.APIUrl + resource;

    $.ajax({
        type: method,
        url: url,
        data: data,
        beforeSend: function () {
            pubsub.publish('startLoadItem', { selector: selector });
        },
        success: success,
        error: function () {
            console.log("Error connecting to api");
        },
        complete: function () {
            if (_.isFunction(complete)) complete();
            pubsub.publish('stopLoadItem', { selector: selector });
        }
    });
}