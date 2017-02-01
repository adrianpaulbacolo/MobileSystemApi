var _paq = _paq || [];
_paq.push(['setCustomUrl', location.href.toLowerCase()]);
_paq.push(['setCustomDimension', 1, window.location.hostname]);
_paq.push(['trackPageView']);
//_paq.push(['enableLinkTracking']); Disabled, we will manually track outlinks and downloads

(function () {
    var u = "//trk.liveperson88.com/";
    _paq.push(['setTrackerUrl', u + 'piwik.php']);
    // Set 21 for Test Environment; Set 15 for Prod; 
    _paq.push(['setSiteId', 21]); 

    var d = document, g = d.createElement('script'), s = d.getElementsByTagName('script')[0];
    g.type = 'text/javascript';
    g.async = true;
    g.defer = true;
    g.src = u + 'piwik.js';
    s.parentNode.insertBefore(g, s);
})();