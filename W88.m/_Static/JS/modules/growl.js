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
            '<a href="#" data-rel="back" class="close close-enhanced">&times;</a>' + '<br>' + '<h1 class="title">' +
            '<img src="/_Static/Images/logo-' + self.options.lang + '.png" width="220" class="logo img-responsive" alt="logo">' +
            '</h1>' + '<div class="padding">' +
            '<div id="ModalMessage" class="download-app padding">' +
            '</div>' +
            '</div>' +
            '<div class="row row-no-padding">' +
            '<div class="col">' +
            '<a href="#" data-rel="back" class="ui-btn btn-primary">' + self.options.close + '</a>' +
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