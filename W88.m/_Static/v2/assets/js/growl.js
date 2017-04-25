﻿function Notification() {
    var Notification = {
        options: {
            hasButtons: false,
            lang: 'en',
            close: 'x',
            ok: "OK"
        },
        init: function() {

            var template = '<div class="modal fade" id="w88modal" tabindex="-1" role="dialog" aria-labelledby="sample-modal1">' +
                '<div class="modal-dialog" role="document">' +
                '<div class="modal-content">' +
                '<div class="modal-header mheader-notitle">' +
                '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span class="icon icon-close"></span></button>' +
                '</div>' +
                '<div id="ModalMessage"></div>' +
                '</div></div></div>';

            $('body').append(template);
        },
        shout: function(msg, callback) {

            var message = ' <div class="modal-body"><p>' + msg + '</p></div>';

            $("#ModalMessage").html(message);
            $('#w88modal').modal('show');

            if (!_.isUndefined(callback) && _.isFunction(callback))
                $('#w88modal').on("hidden.bs.modal", callback);
        },
        bulletedList: function(messages) {
            var message = "";
            if (_.isArray(messages)) {
                message = "<ul class='list-unstyled'>";

                for (var i = 0; i < messages.length; i++) {
                    message = message + "<li>" + messages[i] + "</li>";
                }

                return message + "</ul>";
            }
            return message;
        },
        modal: null
    };

    return Notification;
}

window.w88Mobile.Growl = Notification();