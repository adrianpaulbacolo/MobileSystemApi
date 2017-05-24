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
            _w88_send("", "/Contact", "GET", function (response) {
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

    return contact;
}