function Notification() {
    var Notification = {
        options: {
            hasButtons: false,
            lang: 'en',
            close: 'x',
            ok: "OK"
        },
        init: function (msg) {
            var self = this;

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
        shout: function (msg, callback) {

            var message = ' <div class="modal-body"><p>' + msg + '</p></div>';

            //this.init(msg, '');
            var self = this;

            $("#ModalMessage").html(message);
            $('#w88modal').modal('show');

            //if (!_.isUndefined(callback) && _.isFunction(callback))
            //    this.modal.on("popupafterclose", callback);
        },
        //notif: function (msg) {
        //    var self = this;

        //    var okButton = '<div class="row row-no-padding"><div class="col">' +
        //   '<a href="#" data-rel="back" class="ui-btn btn-primary">' + self.options.ok + '</a>' +
        //   '</div>';

        //    this.init(msg, okButton);
        //    this.modal.popup('open');
        //},
        bulletedList: function (messages) {
            var message = "";
            if (_.isArray(messages)) {
                message = "<ul>";

                for (var i = 0; i < messages.length; i++) {
                    message = message + "<li>" + messages[i] + "</li>";
                }

                return message + "</ul>";
            }
            return message;
        },
        modal: null
    }

    return Notification;
}

window.w88Mobile.Growl = Notification();