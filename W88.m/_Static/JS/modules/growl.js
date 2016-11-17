function Notification() {
    var Notification = {
        options: {
            hasButtons: false,
            lang: 'en',
            close: 'x',
            ok: "OK"
        },
        init: function (msg, ok) {
            var self = this;
            if (self.modal == null) {
                $('body').append(self.getTemplate(ok));
                self.modal = $('#PopUpModal');
            }
            $("#ModalMessage").html(msg);
            self.modal.popup({
                afterclose: function (event, ui) { }
            });
        },
        shout: function (msg, callback) {
            this.init(msg, '');
            this.modal.popup('open');

            if (!_.isUndefined(callback) && _.isFunction(callback))
                this.modal.on("popupafterclose", callback);
        },
        notif: function (msg) {
            var self = this;

            var okButton = '<div class="row row-no-padding"><div class="col">' +
           '<a href="#" data-rel="back" class="ui-btn btn-primary">' + self.options.ok + '</a>' +
           '</div>';

            this.init(msg, okButton);
            this.modal.popup('open');
        },
        getTemplate: function (ok) {
            var self = this;

            var template = '<div id="PopUpModal" data-role="popup" data-overlay-theme="b" data-theme="b" data-history="false">' +
            '<a href="#" data-rel="back" class="close close-enhanced">&times;</a>' + '<br>' +
            '<div class="padding">' +
            '<div id="ModalMessage" class="download-app padding"></div>' +
            '</div>' + ok + '</div></div>';

            return template;
        },
        modal: null
    }

    return Notification;
}

window.w88Mobile.Growl = Notification();