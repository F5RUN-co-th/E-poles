'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var EPoles = function () {
    function EPoles() {
        _classCallCheck(this, EPoles);
        this.Loading = $("#loadingDiv");
        this.imgsrc = "http://cdn.mapmarker.io/api/v1/pin?text=P&size=50&hoffset=1";
        this.popupInput = document.getElementById('popup');
        this.popupContent = document.getElementById('popup-content');
    }

    _createClass(EPoles, [{
        key: 'init',
        value: function init() {
            var me = this;
            this.createMap();
        }
    },
    {
        key: 'createMap',
        value: function createMap() {
            var me = this;
            fetch(this.getUrl)
                .then(response => response.json())
                .then(data => me.initiMap(data))
                .catch(error => console.error('Unable to create map.', error));
        }
    },
    {
        key: 'initiMap',
        value: function initiMap(_data) {
            var me = this;
            let markers = _data;
            var map = new ol.Map({
                target: 'map',
                layers: [
                    new ol.layer.Tile({
                        source: new ol.source.OSM()
                    })
                ],
                view: new ol.View({
                    center: ol.proj.fromLonLat([100.840838, 14.197160]),// center thaiLand
                    zoom: 6,
                    minZoom: 6,
                    maxZoom: 20,
                })
            });
            var features = [];
            for (var i = 0; i < markers.length; i++) {
                var item = markers[i];
                var longitude = item.longitude;
                var latitude = item.latitude;
                var name = item.name;

                var iconFeature = new ol.Feature({
                    geometry: new ol.geom.Point(ol.proj.transform([longitude, latitude], 'EPSG:4326', 'EPSG:3857')),
                    name: name,
                });

                var iconStyle = new ol.style.Style({
                    image: new ol.style.Icon(({
                        anchor: [0.5, 1],
                        //scale: 0.4 // set the size of the img on the map
                        src: me.imgsrc
                    }))
                });

                iconFeature.setStyle(iconStyle);
                features.push(iconFeature);
            }
            var vectorSource = new ol.source.Vector({
                features: features
            });
            var vectorLayer = new ol.layer.Vector({
                source: vectorSource
            });
            map.addLayer(vectorLayer);

            var container = me.popupInput;
            var content = me.popupContent;
            var overlay = new ol.Overlay({
                element: container,
                autoPan: true,
                autoPanAnimation: {
                    duration: 250
                }
                //positioning: 'bottom-center',
                //stopEvent: false,
            });
            map.addOverlay(overlay);

            //display the pop with on mouse over event
            map.on('pointermove', function (event) {
                const features = map.getFeaturesAtPixel(event.pixel);
                if (features.length > 0) {
                    var coordinate = event.coordinate;
                    var pointName = features
                        .filter(feature => feature.getGeometry().getType() == 'Point')
                        .map(feature => feature.get('name'))
                    var longAndlat = features.
                        filter(feature => feature.getGeometry().getType() == 'Point')
                        .map(feature => `${ol.proj.toLonLat(feature.getGeometry().getCoordinates()).join(', ')}`)
                    content.innerHTML = `<b>${pointName}</b><br>${longAndlat}`;
                    overlay.setPosition(coordinate);
                } else {
                    overlay.setPosition(undefined);
                }
            });

            /*
            // display popup on click
            map.on('click', function (evt) {
                const feature = map.forEachFeatureAtPixel(evt.pixel, function (feature) {
                    return feature;
                });
                if (feature) {
                    popup.setPosition(evt.coordinate);
                    $(element).popover('dispose');
                    $(element).popover({
                        placement: 'top',
                        html: true,
                        content: feature.get('name'),
                    });
                    $(element).popover('show');
                } else {
                    $(element).popover('dispose');
                }
            });
            // change mouse cursor when over marker
            map.on('pointermove', function (e) {
                const pixel = map.getEventPixel(e.originalEvent);
                const hit = map.hasFeatureAtPixel(pixel);
                map.getTargetElement().style.cursor = hit ? 'pointer' : '';
            });
            // Close the popup when the map is moved
            map.on('movestart', function () {
                $(element).popover('dispose');
            });*/
        }
    }
    ]);


    return EPoles;
}();