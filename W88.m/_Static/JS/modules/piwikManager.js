window.w88Mobile.PiwikManager = piwikManager();

function piwikManager() {
    var goals = {
        playNow: 2,
        tryNow: 1
    }

    function getInstance() {
        if (typeof Piwik !== "undefined") {
            return Piwik.getAsyncTracker();
        }
        return {};
    }

    function setPiwik(customPiwik){
        piwik = customPiwik;
    }

    function trackGoal(goal) {
        var piwik = getInstance();
        if (typeof piwik === "object" && typeof piwik.trackGoal === "function") {
            piwik.trackGoal(goal);
        }
    }

    function trackPlayNow() {
        trackGoal(goals.playNow);
    }

    function trackTryNow() {
        trackGoal(goals.tryNow);
    }

    function setUserId(id) {
        var piwik = getInstance();
        if (typeof piwik === "object" && typeof piwik.setUserId === "function") {
            piwik.setUserId(id);
        } else if (!_.isUndefined(_paq)) {
            // use native variable instead
            _paq.push(["setUserId", id]);
        }
    }

    return {
        trackPlayNow: trackPlayNow,
        trackTryNow: trackTryNow,
        setUserId: setUserId
    }
}