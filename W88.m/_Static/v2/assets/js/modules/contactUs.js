window.w88Mobile.ContactUs = ContactUs();
var _w88_ContactUs = window.w88Mobile.ContactUs;

function ContactUs() {

    var contact = {};

    contact.init = function () {
        
        var lang = Cookies().getCookie('language');
        var key = "contact-" + lang;

        var contactData = amplify.store(key);
        var template = _.template($("script#ContactUsTemplate").html());

        if (_.isEmpty(contactData)) {
            _w88_ContactUs.send("", "/Contact", "GET", function (response) {
                if (_.isEqual(response.ResponseCode, 1)) {

                    amplify.store(key, response.ResponseData, window.User.storageExpiration);

                    $("#ContactContainer").html(template({
                        data: response.ResponseData
                    }));
                }
            });
        } else {

            $("#ContactContainer").html(template({
                data: contactData
            }));
        }
    };

    contact.send = function (data, resource, method, success, complete) {

        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: function () {
                pubsub.publish('startLoadItem', { selector: "" });
            },
            headers: headers,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            },
            complete: function () {
                if (!_.isUndefined(complete)) complete();
                pubsub.publish('stopLoadItem', { selector: "" });
            }
        });
    };

    return contact;
}