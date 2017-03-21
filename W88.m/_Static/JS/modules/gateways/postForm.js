function PostForm() {
    var form = null;
    var paymentform = {
        create: create
        , createv2: createv2
        , submit: submit
        , form: form
        , destroy: destroy
    };

    return paymentform;

    function create(data, url, appendTo) {
        var self = this;
        self.form = $("<form>", { action: url, target: "_blank", method: "post", id: "payment-form-v2" });
        _.forEach(data, function (value, key) {
            var inputField = $("<input>", { value: value, name: key });
            self.form.append(inputField);
        });
        self.form.appendTo(appendTo);
    }

    function createv2(data, url, appendTo) {
        var self = this;
        self.form = $("<form>", { action: url, method: "post", id: "payment-form-v2" });
        _.forEach(data, function (value, key) {
            var inputField = $("<input>", { value: value, name: key });
            self.form.append(inputField);
        });
        self.form.appendTo(appendTo);
    }

    function submit() {
        var self = this;
        self.form.submit();
    }

    function destroy() {
        var self = this;
        if (!_.isEmpty(self.form)) {
            self.form.remove();
        }
    }
}

window.w88Mobile.PostPaymentForm = PostForm();