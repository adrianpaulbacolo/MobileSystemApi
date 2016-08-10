function Notification() {
    var Notification = {
        options: {
            hasButtons: false,
            lang: 'en',
            close: 'x'
        },
        init: function (msg) {
            var self = this;
            if (self.modal == null) {
                $('body').append(self.getTemplate());
                self.modal = $('#PopUpModal');
            }
            $("#ModalMessage").html(msg);
            self.modal.popup();
        },
        shout: function (msg) {
            this.init(msg);
            this.modal.popup('open');
        },
        getTemplate: function () {
            var self = this;
            var template = '<div id="PopUpModal" data-role="popup" data-overlay-theme="b" data-theme="b" data-history="false">' +
            '<a href="#" data-rel="back" class="close close-enhanced">&times;</a>' + '<br>' +
            '<div class="padding">' +
            '<div id="ModalMessage" class="download-app padding">' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>';

            return template;
        },
        modal: null
    }

    return Notification;
}

window.w88Mobile.Growl = Notification();