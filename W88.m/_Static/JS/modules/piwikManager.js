window.w88Mobile.PiwikManager = piwikManager();

function piwikManager() {
    var goals = {
        playNow: 2,
        tryNow: 1
    }

    var memberId = "";

    function getInstance() {
        if (typeof Piwik !== "undefined") {
            return Piwik.getAsyncTracker();
        }
        return {};
    }

    function setGoals(configGoals) {
        if (!_.isEmpty(configGoals)) goals = configGoals;
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

    function trackEvent(event) {
        var piwik = getInstance();
        if (typeof piwik === "object" && typeof piwik.trackEvent === "function") {
            piwik.trackEvent(event.category, event.action, event.name);
        } else {
            _paq.push(['trackEvent', event.category, event.action, event.name]);
        }
    }

    function setUserId(id) {
        memberId = id;
        var piwik = getInstance();
        if (typeof piwik === "object" && typeof piwik.setUserId === "function") {
            piwik.setUserId(id);
        } else if (!_.isUndefined(_paq)) {
            // use native variable instead
            _paq.push(["setUserId", id]);
        }
    }

    function setDeviceId(obj) {
        if (_.isEmpty(memberId)) return;

        var piwik = getInstance();
        if (typeof piwik === "object" && typeof piwik.setCustomVariable === "function") {
            piwik.setCustomVariable(obj.index, obj.name, obj.value, obj.scope);
        } else if (!_.isUndefined(_paq)) {
            _paq.push(["setCustomVariable", obj.index, obj.name, obj.value, obj.scope]);
        } 
    }

    function setDomain() {
        var hosts = location.hostname;

        var piwik = getInstance();
        if (typeof piwik === "object" && typeof piwik.setDomains === "function") {
            piwik.setDomains(hosts);
        } else if (!_.isUndefined(_paq)) {
            _paq.push(["setDomains", hosts]);
        }
    }

    return {
        trackPlayNow: trackPlayNow
        , trackTryNow: trackTryNow
        , trackEvent: trackEvent
        , setUserId: setUserId
        , setDeviceId: setDeviceId
        , setGoals: setGoals
        , setDomain: setDomain
    }
}