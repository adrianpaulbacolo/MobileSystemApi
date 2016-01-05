var _paq = _paq || [];
_paq.push(['trackPageView']);
_paq.push(['enableLinkTracking']);

(function () {
    var u = "//bnd.liveperson88.com/";
    _paq.push(['setTrackerUrl', u + 'piwik.php']);
    // Set 1 or 2 for Test Environment; Set 3 for Prod; 
    _paq.push(['setSiteId', 3]); 

    var d = document, g = d.createElement('script'), s = d.getElementsByTagName('script')[0];
    g.type = 'text/javascript';
    g.async = true;
    g.defer = true;
    g.src = u + 'piwik.js';
    s.parentNode.insertBefore(g, s);
})();