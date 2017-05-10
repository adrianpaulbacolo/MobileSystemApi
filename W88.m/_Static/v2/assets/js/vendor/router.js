/**
 * A simple router class to be used in conjunction with History.js.
 * Triggers the appropriate callback function when `router.route(url)`
 * is called.
 * 
 * Usage:
 *
 *     <script src="history.js"></script>
 *     <script src="router.js"></script>
 *     <script>
 *     (function (window, undefined) {
 *         var History = window.History;
 *         if (! History.enabled) {
 *             return false;
 *         }
 *         
 *         function handler_one () {
 *             console.log ('handler_one() called!');
 *         }
 *         
 *         function handler_two (name) {
 *             console.log ('handler_two() called!');
 *             console.log (name);
 *         }
 *         
 *         router.set_routes ({
 *             '/one': handler_one,
 *             '/hello/:name': handler_two
 *         });
 *         
 *         History.Adapter.bind (window, 'statechange', function () {
 *             var State = History.getState ();
 *             router.route (State.url);
 *         });
 *     
 *         History.pushState (null, null, '/one');
 *         History.pushState (null, null, '/hello/world');
 *     })(window);
 *     </script>
 */
var router = (function () {
	var router = {};

	// list of routes
	router.routes = {};

	// create once, used by get_path()
	router.a = document.createElement ('a');
	
	// turn a url into a regex and params
	router.regexify = function (url) {
		var res = {
			url: url,
			regex: null,
			params: []
		};
	
		// parse for params
		var matches = url.match (/\:([a-zA-Z0-9_]+)/g);
		if (matches !== null) {
			for (var i in matches) {
				matches[i] = matches[i].substring (1);
			}
			res.params = matches;
			url = url.replace (/\:([a-zA-Z0-9_]+)/g, '(.*?)');
		}

		res.regex = url.replace ('/', '\\/');
	
		return res;
	};
	
	// set a list of routes and their callbacks
	router.set_routes = function (routes) {
		for (var url in routes) {
			res = router.regexify (url);
			var r = {
				url: url,
				regex: new RegExp ('^' + res.regex + '/?$', 'g'),
				params: res.params,
				callback: routes[url]
			};
			router.routes[url] = r;
		}
	};
	
	// get the relative path from a full url
	router.get_path = function (url) {
		router.a.href = url;
		return router.a.pathname + router.a.search + router.a.hash;
	};
	
	// handle the routing for a url
	router.route = function (url) {
	    url = url.toLowerCase();
		var path = router.get_path (url);
		for (var i in router.routes) {
			var matches = router.routes[i].regex.exec (path);
			router.routes[i].regex.lastIndex = 0;
			if (matches !== null) {
				if (matches.length > 1) {
					matches.shift ();
					router.routes[i].callback.apply (null, matches);
				} else {
					router.routes[i].callback ();
				}
				break;
			}
		}
	};
	
	return router;
})();