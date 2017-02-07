window.w88Mobile.PiwikManager = piwikManager();

function piwikManager() {
    var goals = {
        playNow: 2,
        tryNow: 1,
        registerSuccess: 1
    };

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
        } else {
            _paq.push(['trackGoal', goal]);
        }
    }

    function trackPlayNow(obj) {
        trackGoal(goals.playNow);
        if (_.isEmpty(obj.data)) return;
        
        if (typeof obj.data.club === "string" && typeof obj.data.url === "string"){
            obj.data.playType = "Real";
            _reportGpiSlotClick(obj.data);
        }
    }

    function trackTryNow(obj) {
        trackGoal(goals.tryNow);
        if (_.isEmpty(obj.data)) return;

        if (typeof obj.data.club === "string" && typeof obj.data.url === "string"){
            obj.data.playType = "Fun";
            _reportGpiSlotClick(obj.data);
        }
    }

    function trackSignup() {
        trackGoal(goals.registerSuccess);
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

    /**
     * @function setCustomDimension
     * @description Tracking Custom Dimensions on various tracking requests, within scope ACTION or VISIT
     *
     * @param {string} useCase - Required, determines which logic to execute. Refer to switch statement in function
     * @param {Object} dimensions - Flexible object, structure is determined by the useCase
     */
    function setCustomDimension(useCase,dimensions) {
        var piwik = getInstance();
        var async = (typeof piwik === "object" && typeof piwik.setCustomDimension === "function") ? true 
                  : (!_.isUndefined(_paq)) ? false 
                  : undefined;

        //Define ALL dimension cases below. If the case is not defined, we will skip the tracking request.
        switch(useCase){
            case "gpiSlotClick":
                if (async) {
                    piwik.trackLink(dimensions.url,'link', {dimension4: dimensions.gameType, dimension5: dimensions.gameName, dimension6: dimensions.club, dimension7: dimensions.playType});
                } else if (!async && typeof aync !== "undefined") {
                    _paq.push(['trackLink', dimensions.url, "link", {dimension4: dimensions.gameType, dimension5: dimensions.gameName, dimension6: dimensions.club, dimension7: dimensions.playType}]);
                }
                break;
            default:
                console.log("...skipping Dimension request");
        }
    }

    //THE FUNCTIONS BELOW ARE USED FOR PRE-PROCESSING DIMENSION TRACKING//

    /**
     * @function _reportGpiSlotClick
     * @description Process a click on a GPI game and report the click with associated Dimensions to piwik
     *
     * @param {Object} obj - Required parameter, must contain below properties
     * @param {string} obj.club - The club the GPI click originated from, ex: "ClubBravado", "ClubMassimo", "ClubPalazzo"
     * @param {string} obj.url -  The URL of the game clicked, gathered from the href attribute of the clicked node
     * @param {string} obj.playType -  "Real" or "Fun" ONLY
     */
    function _reportGpiSlotClick(obj){
        var a = document.createElement("a");
        a.href = obj.url;
        obj.gameName = a.pathname.replace(/\//g, '');
        obj.gameType = "Slot";
        setCustomDimension("gpiSlotClick",obj);
    }

    return {
        trackPlayNow: trackPlayNow,
        trackTryNow: trackTryNow,
        trackEvent: trackEvent,
        trackSignup: trackSignup,
        setUserId: setUserId,
        setDeviceId: setDeviceId,
        setGoals: setGoals,
        setDomain: setDomain,
        setCustomDimension: setCustomDimension
    };
}