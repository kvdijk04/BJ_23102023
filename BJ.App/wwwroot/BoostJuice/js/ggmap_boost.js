let map, infoWindow;
let markers = [];
let infoWindows = [];
const iconBase = "/images/";
function distance(lat1, lon1, lat2, lon2, unit) {
    var radlat1 = Math.PI * lat1 / 180
    var radlat2 = Math.PI * lat2 / 180
    var theta = lon1 - lon2
    var radtheta = Math.PI * theta / 180
    var dist = Math.sin(radlat1) * Math.sin(radlat2) + Math.cos(radlat1) * Math.cos(radlat2) * Math.cos(radtheta);
    if (dist > 1) {
        dist = 1;
    }
    dist = Math.acos(dist)
    dist = dist * 180 / Math.PI
    dist = dist * 60 * 1.1515
    if (unit == "K") { dist = dist * 1.609344 }
    if (unit == "N") { dist = dist * 0.8684 }
    return dist
}
function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: new google.maps.LatLng(10.777502919418838, 106.69500171298014),
        zoom: 12,
    });
    infoWindow = new google.maps.InfoWindow();
    $("#get-my-location").on("click", function () {
        //jQuery("#boost-stores").fadeOut(350, function () {
        //    $('#boost-stores-loading').hide();
        //    $('.boost-home .footer').css("margin-top", "60px");
        //}).fadeIn(500);
        if (navigator.geolocation) {
            var a = {};
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude,
                    };

                    map.setCenter(pos);
                    if (!markers.length) {
                        var html = "";
                        let list = [];
                        var $loading = $('#boost-stores-loading');
                        $.ajax({
                            url: '/store',
                            datatype: "json",
                            type: "get",
                            async: true,
                            beforeSend: function () {
                                $loading.show();
                            },
                            success: function (results) {

                                const icons = {
                                    info: {
                                        icon: {
                                        }
                                    },
                                    current: {
                                        icon: {
                                            scaledSize: new google.maps.Size(34, 60), // scaled size
                                            origin: new google.maps.Point(0, 0), // origin
                                            anchor: new google.maps.Point(0, 0) // anchor
                                        },

                                    }
                                };
                                for (var i = 0; i < results.length; i++) {

                                    if (results[i].id != 1) { results[i].type = "info", icons.info.icon.url = iconBase + results[i].iconPath }
                                    else { icons.current.icon.url = iconBase + results[i].iconPath, results[i].type = "current"; results[i].latitude = position.coords.latitude; results[i].longitude = position.coords.longitude };
                                    var e = Math.round((distance(position.coords.latitude, position.coords.longitude, results[i].latitude, results[i].longitude, "K")) * 100) / 100;
                                    results[i].distance = e;
                                }
                                results.sort(function (a, b) { return a.distance - b.distance });
                                for (let i = 0; i < results.length; i++) {
                                    const marker = new google.maps.Marker({
                                        draggable: false,
                                        position: new google.maps.LatLng(results[i].latitude, results[i].longitude),
                                        icon: icons[results[i].type].icon,
                                        title: results[i].name,
                                        animation: google.maps.Animation.DROP,
                                        address: results[i].address,
                                        code: results[i].code,
                                        lat: results[i].latitude,
                                        lng: results[i].longitude,
                                        map: map,
                                        distance: results[i].distance
                                    });
                                    markers.push(marker);


                                    const contentString =
                                        '<div id="content">' +
                                        '<strong style="font-size:18px;font-weight:bold" id="firstHeading" class="firstHeading">' + marker.title + '</strong>' +
                                        '<p style="font-size:16px;font-weight:bold">' + marker.address + '<a href="' + marker.map.mapUrl + '"></a></p>' +
                                        "</div>";
                                    const infowindow = new google.maps.InfoWindow({
                                        content: contentString,
                                        ariaLabel: marker.title,
                                    });
                                    infoWindows.push(infowindow);

                                    map.addListener("click", (event) => {
                                        infowindow.close();
                                    });

                                    marker.addListener("click", () => {
                                        infowindow.open({
                                            anchor: marker,
                                            map,
                                        });
                                    });

                                    marker.addListener("click", toggleBounce);

                                    function toggleBounce() {
                                        if (marker.getAnimation() !== null) {
                                            marker.setAnimation(null);
                                        } else {
                                            marker.setAnimation(google.maps.Animation.BOUNCE);
                                        }
                                    };

                                }
                                $('#boostjuice-stores').html('<h2 style="color: #79C339;margin-bottom: 20px;font-weight: 600;text-align:center">Boost gần nhất ' + results[1].distance + ' km</h2>');
                                for (let i = 1; i < markers.length; i++) {
                                    html += '<div class="item store-list" id="' + i + '" data-store-id="' + results[i].id + '">' +
                                        '<h3><em>' + results[i].distance + 'km</em>' + results[i].name + '</h3><span class="boostjuice-street">' + results[i].address + '<br> ' + results[i].city + ' </span>' +
                                        '<table role="presentation" class="wpsl-opening-hours"><tbody><tr><td>Monday</td><td><time>8:00 AM - 11:00 PM</time></td></tr><tr><td>Tuesday</td><td><time>8:00 AM - 11:00 PM</time></td></tr><tr><td>Wednesday</td>' +
                                        '<td><time>8:00 AM - 11:00 PM</time></td></tr><tr><td>Thursday</td><td><time>8:00 AM - 11:00 PM</time></td></tr><tr><td>Friday</td><td><time>8:00 AM - 11:00 PM</time></td></tr>' +
                                        '<tr><td>Saturday</td><td><time>8:00 AM - 11:00 PM</time></td></tr><tr><td>Sunday</td><td><time>8:00 AM - 11:00 PM</time></td></tr></tbody></table>' +
                                        '<a class="show-openhours" id="showDetail" style="color: #79C339;background-color: transparent;padding-left: 0px;padding-right: 0px; margin-right: 20px; cursor: pointer; font-size: 18px; text-transform: uppercase; font-weight: 600" lat="' + results[i].latitude + '" lng="' + results[i].longitude + '" >Giờ mở cửa</a>' + /* onclick="showDetail(' + data[i].lat + ', ' + data[i].lng + ')" */
                                        '<a data-url="https://www.google.com/maps/dir/?api=1&destination=' + results[i].address + ", " + results[i].city + '&travelmode=driving" target="_blank" class="marker-link" data-markerid="' + i + '" style = "color: #79C339;background-color: transparent; padding-left: 0px;padding-right: 0px;margin-right: 20px; cursor: pointer; font-size: 18px; text-transform: uppercase; font-weight: 600">Chỉ đường</a></div>';
                                }
                                $('#boostjuice-stores').append(html);
                                console.log(markers);
                                console.log("infowindows: " + infoWindows);

                                $('.marker-link').on('click', function () {
                                    var e = $(".marker-link").index(this);
                                    for (var i = 0; i < infoWindows.length; i++) {
                                        if (markers[$(this).data('markerid')] != i) {
                                            infoWindows[i].close();
                                        }
                                    }
                                    google.maps.event.trigger(markers[$(this).data('markerid')], 'click');
                                    window.open($(this).attr('data-url'), '_blank');

                                });



                            },
                            error: function (xhr) {
                                console.log("error");   
                            },
                            complete: function () {
                                $loading.fadeOut(3000, function () {
                                    $('#boost-stores').fadeIn(1000);
                                    $('.boost-home .footer').css("margin-top", "60px");
                                });
                            },
                        });
                    }
                },
                () => {
                    handleLocationError(true, infoWindow, map.getCenter());
                },
            );

        } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, infoWindow, map.getCenter());
        }
    });
    function handleLocationError(browserHasGeolocation, infoWindow, pos) {
        infoWindow.setPosition(pos);
        infoWindow.setContent(
            browserHasGeolocation
                ? "Error: The Geolocation service failed."
                : "Error: Your browser doesn't support geolocation.",
        );
        infoWindow.open(map);
    }
    map.setOptions({
        styles: [{
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
        }]
    });

}

window.initMap = initMap;