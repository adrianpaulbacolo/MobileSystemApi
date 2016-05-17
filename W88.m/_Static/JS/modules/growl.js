function Notification() {
    var Notification = {
        options: {
            hasButtons: false
        },
        init: function (msg) {

            var self = this;
            $('[data-remodal-id=modal]').remove();
            $('body').append(self.template);
            $('#growl-msg').html(msg);

            if (self.hasButtons) {
                $('[data-remodal-id=modal]').append(self.toolBars);
            }
            self.modal = $('[data-remodal-id=modal]').remodal({
                hashTracking: false,
                closeOnOutsideClick: false
            });
        },
        start: function (msg) {
            this.init(msg);
            this.open();
        },
        open: function () {
            this.modal.open();
        },
        close: function () {
            this.modal.close();
        },
        destroy: function () {
            this.modal.destroy();
        },
        template: '<div data-remodal-id="modal">' +
            '<button data-remodal-action="close" class="remodal-close"></button>' +
            '<p id="growl-msg"></p>' +
            '</div>',
        toolBars: '<br /><button data-remodal-action="cancel" class="remodal-cancel">Cancel</button>' +
            ' <button data-remodal-action="confirm" class="remodal-confirm">OK</button>',
        modal: {}
    }

    return Notification;
}

window.w88Mobile.Growl = Notification();