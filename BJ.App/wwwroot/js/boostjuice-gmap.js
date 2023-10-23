var boostjuiceSettings = {
        startMarker: "dark-orange@2x.png",
        storeMarker: "green@2x.png",
        markerClusters: "0",
        autoLocate: "0",
        autoLoad: "0",
        mapType: "roadmap",
        zoomLevel: "3",
        zoomLatlng: "-25.274398,133.775136",
        streetView: "1",
        panControls: "1",
        controlPosition: "right",
        controlStyle: "small",
        markerBounce: "1",
        newWindow: "0",
        resetMap: "1",
        directionRedirect: "1",
        moreInfo: "1",
        storeUrl: "0",
        phoneUrl: "1",
        moreInfoLocation: "store listings",
        mouseFocus: "0",
        templateId: "0",
        markerStreetView: "1",
        markerZoomTo: "1",
        maxResults: "10",
        searchRadius: "50",
        distanceUnit: "km",
        ajaxurl: "/wp/wp-admin/admin-ajax.php",
        path: "/wp-admin/wp-content/plugins/wp-store-locator/"
    },
    wpslLabels = {
        preloader: "Searching...",
        noResults: "No results found",
        moreInfo: "More info",
        generalError: "Something went wrong, please try again!",
        queryLimit: "API usage limit reached",
        directions: "Directions",
        noDirectionsFound: "No route could be found between the origin and destination",
        phone: "Phone",
        fax: "Fax",
        hours: "Hours",
        startPoint: "You are here",
        back: "Back",
        streetView: "Street view",
        zoomHere: "Zoom here"
    };
jQuery(document).ready(function(e) {
    function o() {
        var o, n, a, r, l, c, d = 1 == boostjuiceSettings.streetView;
        switch ("" !== boostjuiceSettings.zoomLatlng ? (r = boostjuiceSettings.zoomLatlng.split(","), A = new google.maps.LatLng(r[0], r[1]), l = parseInt(boostjuiceSettings.zoomLevel)) : (A = new google.maps.LatLng(0, 0), l = 1), O = new google.maps.Geocoder, E = new google.maps.InfoWindow, R = new google.maps.DirectionsRenderer, P = new google.maps.DirectionsService, n = "right" == boostjuiceSettings.controlPosition ? google.maps.ControlPosition.RIGHT_TOP : google.maps.ControlPosition.LEFT_TOP, a = "small" == boostjuiceSettings.controlStyle ? google.maps.ZoomControlStyle.SMALL : google.maps.ZoomControlStyle.LARGE, boostjuiceSettings.mapType) {
            case "roadmap":
                c = google.maps.MapTypeId.ROADMAP;
                break;
            case "satellite":
                c = google.maps.MapTypeId.SATELLITE;
                break;
            case "hybrid":
                c = google.maps.MapTypeId.HYBRID;
                break;
            case "terrain":
                c = google.maps.MapTypeId.TERRAIN;
                break;
            default:
                c = google.maps.MapTypeId.ROADMAP
        }
        var u = [{
            featureType: "administrative",
            elementType: "labels.text.fill",
            stylers: [{
                color: "#444444"
            }]
        }, {
            featureType: "landscape",
            elementType: "all",
            stylers: [{
                color: "#f2f2f2"
            }]
        }, {
            featureType: "poi",
            elementType: "all",
            stylers: [{
                visibility: "off"
            }]
        }, {
            featureType: "poi.business",
            elementType: "geometry.fill",
            stylers: [{
                visibility: "on"
            }]
        }, {
            featureType: "road",
            elementType: "all",
            stylers: [{
                saturation: -100
            }, {
                lightness: 45
            }]
        }, {
            featureType: "road.highway",
            elementType: "all",
            stylers: [{
                visibility: "simplified"
            }]
        }, {
            featureType: "road.arterial",
            elementType: "labels.icon",
            stylers: [{
                visibility: "off"
            }]
        }, {
            featureType: "transit",
            elementType: "all",
            stylers: [{
                visibility: "off"
            }]
        }, {
            featureType: "water",
            elementType: "all",
            stylers: [{
                color: "#b4d4e1"
            }, {
                visibility: "on"
            }]
        }];
        o = {
            zoom: l,
            center: A,
            mapTypeId: c,
            mapTypeControl: !1,
            panControl: !1,
            scrollwheel: !1,
            styles: u,
            streetViewControl: d,
            zoomControlOptions: {
                style: a,
                position: n
            }
        }, M = new google.maps.Map(document.getElementById("boostjuice-gmap"), o), !s() && e(".boostjuice-dropdown").length ? I() : (e("#boostjuice-search-wrap select").show(), e("#boostjuice-wrap").addClass("boostjuice-mobile")), 1 == boostjuiceSettings.autoLocate ? i() : 1 == boostjuiceSettings.autoLoad && t(), 1 == boostjuiceSettings.mouseFocus && e("#boostjuice-search-input").focus()
    }

    function t() {
        var e = {
            store: wpslLabels.startPoint
        };
        h(A, 0, e, !0)
    }

    function s() {
        return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)
    }

    function i() {
        if (navigator.geolocation) {
            var e = !1,
                o = setTimeout(t, 0);
            navigator.geolocation.getCurrentPosition(function(t) {
                clearTimeout(o), k(e), n(t, z)
            }, function(e) {
                clearTimeout(o), t()
            })
        } else t()
    }

    function n(e, o) {
        if ("undefined" == typeof e) t();
        else {
            var s = new google.maps.LatLng(e.coords.latitude, e.coords.longitude);
            _ = e, u(s), M.setCenter(s), h(s, 0, "", !0), g(s, o, V)
        }
    }

    function a() {
        "undefined" != typeof x && "" !== x && (x.setMap(null), x = "")
    }

    function r(o) {
        var t, s, i, n, a;
        for (a = o.parent("li").length > 0 ? o.parent("li").data("store-id") : o.parents(".boostjuice-info-window").data("store-id"), "undefined" != typeof x && "" !== x && (s = x.getPosition()), N = {
            centerLatlng: M.getCenter(),
            zoomLevel: M.getZoom()
        }, t = 0, n = D.length; n > t; t++) 0 != D[t].storeId || "undefined" != typeof s && "" !== s ? D[t].storeId == a && (i = D[t].getPosition()) : s = D[t].getPosition();
        s && i ? (e("#boostjuice-direction-details ul").empty(), e(".boostjuice-direction-before, .boostjuice-direction-after").remove(), c(s, i)) : (swal("Here's a message!", wpslLabels.generalError), e("#boost-store-locator").addClass("hide-stores"), e("#boost-stores-loading").fadeOut())
    }

    function l(e, o) {
        var t, s, i = "";
        for (i = "start" == o ? google.maps.Animation.BOUNCE : null, t = 0, s = D.length; s > t; t++) D[t].storeId == e && (marker = D[t], marker.setAnimation(i))
    }

    function c(o, t) {
        var s, i, n, a, r, l, c, d, u, p = "",
            g = {};
        d = "km" == boostjuiceSettings.distanceUnit ? google.maps.UnitSystem.METRIC : google.maps.UnitSystem.IMPERIAL, g = {
            origin: o,
            destination: t,
            travelMode: google.maps.DirectionsTravelMode.DRIVING,
            unitSystem: d
        }, P.route(g, function(o, t) {
            if (t == google.maps.DirectionsStatus.OK) {
                if (R.setMap(M), R.setDirections(o), o.routes.length > 0) {
                    for (r = o.routes[0], l = 0; l < r.legs.length; l++)
                        for (s = r.legs[l], c = 0, i = s.steps.length; i > c; c++) n = s.steps[c], a = c + 1, p = p + "<li><div class='boostjuice-direction-index'>" + a + "</div><div class='boostjuice-direction-txt'>" + n.instructions + "</div><div class='boostjuice-direction-distance'>" + n.distance.text + "</div></li>";
                    for (e("#boostjuice-direction-details ul").append(p).before("<p class='boostjuice-direction-before'><a class='boostjuice-back' id='boostjuice-direction-start' href='#'>" + wpslLabels.back + "</a>" + r.legs[0].distance.text + " - " + r.legs[0].duration.text + "</p>").after("<p class='boostjuice-direction-after'>" + o.routes[0].copyrights + "</p>"), e("#boostjuice-direction-details").show(), l = 0, i = D.length; i > l; l++) D[l].setMap(null);
                    $ && $.clearMarkers(), "undefined" != typeof x && "" !== x && x.setMap(null), e("#boostjuice-stores").hide(), 1 == boostjuiceSettings.templateId && (u = e("#boostjuice-gmap").offset(), e(window).scrollTop(u.top))
                }
            } else T(t)
        })
    }

    function d() {
        var o, t = !1,
            s = !1;
        "Chadstone, Victoria" == e("#boostjuice-search-input").val() ? address = "Chadstone, VIC" : address = e("#boostjuice-search-input").val() + ", Australia", O.geocode({
            address: address + ", Australia"
        }, function(e, i) {
            console.log(address), i == google.maps.GeocoderStatus.OK ? (o = e[0].geometry.location, k(s), h(o, 0, "", !0), g(o, z, t)) : S(i)
        })
    }

    function u(o) {
        var t;
        O.geocode({
            latLng: o
        }, function(o, s) {
            s == google.maps.GeocoderStatus.OK ? (t = p(o), "" !== t && (e("#boostjuice-search-input").val(t), 1 == window.dragged && (e("#boostjuice-search-btn").click(), window.dragged = 0))) : S(s)
        })
    }

    function p(e) {
        var o, t, s, i = e[0].address_components.length;
        for (s = 0; i > s; s++) t = e[0].address_components[s].types, (/^postal_code$/.test(t) || /^postal_code_prefix,postal_code$/.test(t)) && (o = e[0].address_components[s].long_name);
        return o
    }

    function g(e, o, t) {
        1 == boostjuiceSettings.directionRedirect ? b(e, function() {
            m(e, o, t)
        }) : m(e, o, t)
    }

    function b(e, o) {
        O.geocode({
            latLng: e
        }, function(e, t) {
            t == google.maps.GeocoderStatus.OK ? (U = e[0].formatted_address, o()) : S(t)
        })
    }

    function m(o, t, s) {
        var i, n = {},
            a = "",
            c = !1,
            d = e("#boostjuice-stores"),
            u = {
                action: "store_search",
                lat: o.lat(),
                lng: o.lng()
            },
            p = {
                src: "#boostjuice-direction-start",
                target: ""
            };
        1 == s && (u.autoload = 1), e.get(boostjuiceSettings.ajaxurl, u, function(o) {
            console.log("creating results"), e(".boostjuice-preloader").remove(), o.success !== !1 ? (e("#boost-stores-loading").fadeOut(), e("#boost-store-locator").removeClass("hide-stores"), e("body").animate({
                scrollTop: e("#boostjuice-result-list").offset().top
            }, 600), o.length > 0 && (e.each(o, function(e) {
                n = {
                    store: o[e].store,
                    address: o[e].address,
                    address2: o[e].address2,
                    city: o[e].city,
                    country: o[e].country,
                    state: o[e].state,
                    zip: o[e].zip,
                    lat: o[e].lat,
                    lng: o[e].lng,
                    clickid: String(e)
                }, i = new google.maps.LatLng(o[e].lat, o[e].lng), h(i, o[e].id, n, c), a += y(o[e], p, e)
            }), e("#boostjuice-result-list").off("click", ".boostjuice-directions"), e("#boostjuice-stores").html(""), d.append(a), e("#boostjuice-result-list").on("click", ".boostjuice-directions", function() {
                return 1 != boostjuiceSettings.directionRedirect ? (r(e(this)), !1) : void 0
            }), f(), L(), e("#boostjuice-stores").on("mouseenter", "div", function() {
                l(e(this).data("store-id"), "start")
            }), e("#boostjuice-stores").on("mouseleave", "div", function() {
                l(e(this).data("store-id"), "stop")
            })), e(".show-openhours").on("click", function() {
                e(this).prev().toggleClass("show"), e(this).text(function(o, t) {
                    "Opening Hours" == t ? e(this).html("Hide") : e(this).html(trans_opening_hours)
                })
            })) : (swal("Here's a message!", wpslLabels.generalError), e("#boost-store-locator").addClass("hide-stores"), e("#boost-stores-loading").fadeOut());
            var t = new Date,
                s = t.getDay();
            0 == s && (s = 7), e("#boostjuice-result-list ul li:nth-child(" + s + ")").wrapInner("<span></span>")
        }), 1 == boostjuiceSettings.mouseFocus && e("#boostjuice-search-input").focus()
    }

    function f() {
        if (1 == boostjuiceSettings.markerClusters) {
            var e = Number(boostjuiceSettings.clusterZoom),
                o = Number(boostjuiceSettings.clusterSize);
            isNaN(e) && (e = ""), isNaN(o) && (o = ""), $ = new MarkerClusterer(M, D, {
                gridSize: o,
                maxZoom: e
            })
        }
    }

    function h(o, t, s, i) {
        var n, a, r, l = !0;
        n = 0 === t ? "/app/themes/boostjuice/dist/img/gmap/map_start_pointer.png" : "/app/themes/boostjuice/dist/img/gmap/icon_cup.png", a = {
            url: n,
            scaledSize: new google.maps.Size(34, 60),
            origin: new google.maps.Point(0, 0),
            anchor: new google.maps.Point(25, 45)
        }, r = new google.maps.Marker({
            position: o,
            map: M,
            optimized: !1,
            title: s.clickid,
            draggable: i,
            storeId: t,
            icon: a
        }), D.push(r);
        new google.maps.InfoWindow({
            maxWidth: 220
        });
        e.fn.scrollTo = function(o, t) {
            return e(this).animate({
                scrollTop: e(this).scrollTop() - e(this).offset().top + e(o).offset().top
            }, void 0 == t ? 1e3 : t), this
        }, google.maps.event.addListener(r, "click", function() {
            if (E.setContent(v(s, t, H)), 0 != t) {
                E.open(M, r);
                var o = e(this).attr("title");
                console.log(o), e("#boostjuice-stores").scrollTo("#" + o, 600)
            } else E.open(M, r)
        }), i && google.maps.event.addListener(r, "dragend", function(e) {
            k(l), M.setCenter(e.latLng), u(e.latLng), window.dragged = 1, console.log(window.dragged)
        })
    }

    function v(e, o, t) {
        if (e.address2) var s = ", ";
        else var s = "";
        if (e.store) var i = '<div class="map-info"><h3>' + e.store + "</h3><div>" + e.address + ", " + e.address2 + s + " " + e.city + "</div></div>";
        else var i = "<div><div>"+trans_you_are_here+"</div><div><div>"+trans_current_direction+"</div></div></div>";
        return i
    }

    function w(e, o, t, s, i, n) {
        var a, r;
        return "undefined" == typeof U && (U = ""), a = i + ", " + n, r = {
            src: "https://maps.google.com/maps?saddr=" + j(U) + "&daddr=" + j(a),
            target: "target='_blank'"
        }
    }

    function j(e) {
        return encodeURIComponent(e).replace(/[!'()*]/g, escape)
    }

    function y(e, o, t) {
        console.log(e);
        var s = "",
            i = e.id,
            n = e.store,
            a = e.phone,
            r = ((e.state + "/" + e.store.replace(/\s+/g, "-")).toLowerCase(), e.url, e.address),
            l = e.address2,
            c = e.city,
            d = e.lat,
            u = e.lng,
            p = "Vietnam",
            g = "",
            b = "",
           // b = e.state,
            m = e.hours,
            f = parseFloat(e.distance).toFixed(1) + " " + boostjuiceSettings.distanceUnit;
        if (a = a ? "<span>Phone: " + a + "</span>" : "", o = w(r, c, g, p, d, u), l) var h = ",<br>";
        else var h = "";
        return s = 0 == t ? "<div class='item store-list' id='" + t + "' data-store-id='" + i + "'><h2>"+ trans_closest_bar + " " + f + "</h2><h3><em>" + f + "</em>" + n + "</h3><span class='boostjuice-street'>" + r + h + l + ",<br> " + c + " " + b + "</span>" + a + m + "<a class='button show-openhours'>" + trans_opening_hours + "</a> <a class='button button-primary' " + o.target + " href='" + o.src + "'>" + trans_get_directions + "</a></div>" : "<div class='item store-list' id='" + t + "' data-store-id='" + i + "'><h3><em>" + f + "</em>" + n + "</h3><span class='boostjuice-street'>" + r + h + l + ",<br> " + c + " " + b + "</span>" + a + m + "<a class='button show-openhours'><span>"+trans_opening_hours+"</a> <a class='button button-primary' " + o.target + " href='" + o.src + "'>"+trans_get_directions+"</a></div>"
    }

    function L() {
        var e, o, t = 12,
            s = new google.maps.LatLngBounds;
        for (google.maps.event.addListenerOnce(M, "bounds_changed", function(e) {
            this.getZoom() > t && this.setZoom(t)
        }), e = 0, o = D.length; o > e; e++) s.extend(D[e].position);
        M.fitBounds(s)
    }

    function k(e) {
        var o, t;
        if (R.setMap(), D) {
            for (t = 0, o = D.length; o > t; t++)
                D[t].setMap();
            D.length = 0
        }
        $ && $.clearMarkers()
    }

    function S(o) {
        console.log(o);
        var t;
        switch (o) {
            case "ZERO_RESULTS":
                t = wpslLabels.noResults;
                break;
            case "OVER_QUERY_LIMIT":
                t = wpslLabels.queryLimit;
                break;
            default:
                t = wpslLabels.generalError
        }
        swal("Here's a message!", t), e("#boost-store-locator").addClass("hide-stores"), e("#boost-stores-loading").fadeOut()
    }

    function T(o) {
        var t;
        switch (o) {
            case "NOT_FOUND":
            case "ZERO_RESULTS":
                t = wpslLabels.noDirectionsFound;
                break;
            case "OVER_QUERY_LIMIT":
                t = wpslLabels.queryLimit;
                break;
            default:
                t = wpslLabels.generalError
        }
        swal("Here's a message!", t), e("#boost-store-locator").addClass("hide-stores"), e("#boost-stores-loading").fadeOut()
    }

    function I() {
        e(".boostjuice-dropdown").each(function(o) {
            var t, s, i = e(this);
            i.$dropdownWrap = i.wrap("<div class='boostjuice-dropdown'></div>").parent(), i.$selectedVal = i.val(), i.$dropdownElem = e("<div><ul/></div>").appendTo(i.$dropdownWrap), i.$dropdown = i.$dropdownElem.find("ul"), i.$options = i.$dropdownWrap.find("option"), i.hide().removeClass("boostjuice-dropdown"), e.each(i.$options, function() {
                t = e(this).val() == i.$selectedVal ? 'class="boostjuice-selected-dropdown"' : "", i.$dropdown.append("<li " + t + ">" + e(this).text() + "</li>")
            }), i.$dropdownElem.before("<span class='boostjuice-selected-item'>" + i.find(":selected").text() + "</span>"), i.$dropdownItem = i.$dropdownElem.find("li"), i.$dropdownWrap.on("click", function(o) {
                C(), e(this).toggleClass("boostjuice-active"), s = 0, e(this).hasClass("boostjuice-active") ? (i.$dropdownItem.each(function(o) {
                    s += e(this).outerHeight()
                }), i.$dropdownElem.css("height", s + 2 + "px")) : i.$dropdownElem.css("height", 0), o.stopPropagation()
            }), i.$dropdownItem.on("click", function(o) {
                i.$dropdownWrap.find(e(".boostjuice-selected-item")).html(e(this).text()), i.$dropdownItem.removeClass("boostjuice-selected-dropdown"), e(this).addClass("boostjuice-selected-dropdown"), C(), o.stopPropagation()
            })
        }), e(document).click(function() {
            C()
        })
    }

    function C() {
        e(".boostjuice-dropdown").removeClass("boostjuice-active"), e(".boostjuice-dropdown div").css("height", 0)
    }
    var O, M, E, R, P, _, $, x, U, A, D = [],
        N = {},
        z = !1,
        H = !1,
        V = boostjuiceSettings.autoLoad;
    e("#autoLocateUser, #get-my-location").click(function() {
        function o(e) {}

        function t(o) {
            switch (o.code) {
                case o.PERMISSION_DENIED:
                    swal("Uh oh!", "Looks like youâ€™ve rejected our geolocation request."), e("#boost-store-locator").addClass("hide-stores"), e("#boost-stores-loading").fadeOut();
                    break;
                case o.POSITION_UNAVAILABLE:
                    swal("Uh oh!", "Location information is unavailable."), e("#boost-store-locator").addClass("hide-stores"), e("#boost-stores-loading").fadeOut();
                    break;
                case o.TIMEOUT:
                    swal("Uh oh!", "Looks like your location timed out."), e("#boost-store-locator").addClass("hide-stores"), e("#boost-stores-loading").fadeOut();
                    break;
                case o.UNKNOWN_ERROR:
                    swal("Uh oh!", "An unknown error occurred."), e("#boost-store-locator").addClass("hide-stores"), e("#boost-stores-loading").fadeOut()
            }
        }
        e("#wpsl-result-list").empty(), e("#wpsl-result-list").html("<div id='wpsl-stores'></div>"), e("#boost-stores-loading").fadeIn(), navigator.geolocation && navigator.geolocation.getCurrentPosition(o, t), i()
    }), e("#boostjuice-search-btn").on("click", function() {
        var o = !1;
        e("#boostjuice-search-input").val() ? (e("#boostjuice-search-input").removeClass(), e("#boost-stores-loading").fadeIn(), console.log("removing results"), e("#boostjuice-result-list").empty(), e("#boostjuice-result-list").html("<div id='boostjuice-stores'></div>"), e("#boostjuice-stores").show(), e(".boostjuice-direction-before, .boostjuice-direction-after").remove(), e("#boostjuice-direction-details").hide(), z = !1, k(o), a(), d()) : e("#boostjuice-search-input").addClass("boostjuice-error").focus()
    }), e("#boostjuice-result-list").on("click", ".boostjuice-back", function() {
        var o, t;
        for (R.setMap(null), o = 0, t = D.length; t > o; o++) D[o].setMap(M);
        return "undefined" != typeof x && "" !== x && x.setMap(M), $ && f(), M.setCenter(N.centerLatlng), M.setZoom(N.zoomLevel), e(".boostjuice-direction-before, .boostjuice-direction-after").remove(), e("#boostjuice-stores").show(), e("#boostjuice-direction-details").hide(), !1
    }), e("#boostjuice-search-input").keydown(function(o) {
        var t = o.keyCode || o.which;
        13 == t && e("#boostjuice-search-btn").trigger("click")
    }), e("#boostjuice-stores").on("click", ".boostjuice-store-details", function() {
        var o, t, s = e(this).parents("li"),
            i = s.data("store-id");
        if ("info window" == boostjuiceSettings.moreInfoLocation)
            for (o = 0, t = D.length; t > o; o++) D[o].storeId == i && google.maps.event.trigger(D[o], "click");
        else s.find(".boostjuice-more-info-listings").is(":visible") ? e(this).removeClass("boostjuice-active-details") : e(this).addClass("boostjuice-active-details"), s.siblings().find(".boostjuice-store-details").removeClass("boostjuice-active-details"), s.siblings().find(".boostjuice-more-info-listings").hide(), s.find(".boostjuice-more-info-listings").toggle();
        return 1 != boostjuiceSettings.templateId || "store listings" == boostjuiceSettings.moreInfoLocation ? !1 : void 0
    }), e("#boostjuice-gmap").length && google.maps.event.addDomListener(window, "load", o)
}), $("#boostjuice-search-input").geocomplete({
    country: "vn"
}).bind("geocode:result", function(e, o) {
    $("#boostjuice-search-btn").click()
});