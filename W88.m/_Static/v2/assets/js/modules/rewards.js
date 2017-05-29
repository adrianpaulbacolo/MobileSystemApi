var _w88_rewards = w88Mobile.Rewards = Rewards();

function Rewards() {
    return {
        init: init,
    };

    function init(options) {
        var selector = _.isUndefined(options) ? "rewards-value" : options.selector;
        getPoints({ selector: selector });
    }

    function getPoints(selector) {
        var resource = "/user/rewards";
        _w88_send(resource, "GET", selector, function (response) {
            if (_.isUndefined(response.ResponseData)) {
                console.log('Unable to fetch rewards.');
                return;
            }
            pubsub.publish("rewardsPointLoaded", response.ResponseData);
        });
    }
}