'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var EPoles = function () {
    function EPoles() {
        _classCallCheck(this, EPoles);
        this.popupInput = document.getElementById('popup');
        this.popupContent = document.getElementById('popup-content');
        this.popupInputHover = document.getElementById('popup-hover');
        this.popupContentHover = document.getElementById('popup-content-hover');

        this.vectorLayer = null;
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
        key: 'makeMarkers',
        value: function makeMarkers(markers) {
            var me = this;
            var features = [];
            for (var i = 0; i < markers.length; i++) {
                var item = markers[i];
                var longitude = item.longitude;
                var latitude = item.latitude;
                var name = item.name;
                var area = item.area;
                var street = item.street;
                var desc = item.description === null ? "" : item.description;
                var note = item.note === null ? "" : item.note;
                var status = item.status;
                var lblstatus = status === true ? '<span class="ml-auto badge badge-success">ใช้งาน</span>' : '<span class="ml-auto badge badge-danger">เสีย</span>';
                var lblheader = '<div class="card-header" style="padding: 0;height: 2.5rem;">' + name + '<div class="btn-actions-pane-right">' + lblstatus + '</div></div>';
                var iconFeature = new ol.Feature({
                    geometry: new ol.geom.Point(ol.proj.transform([longitude, latitude], 'EPSG:4326', 'EPSG:3857')),
                    type: 'Point',
                    makerid: item.id,
                    lat: latitude,
                    long: longitude,
                    name: name,
                    area: area,
                    street: street,
                    description: desc,
                    note: note,
                    status: status,
                    desc: '<pre style="margin-bottom: 0 !important;">' + lblheader + 'Latitude : ' + latitude + '<br>Longitude: ' + longitude
                        + '<br>Street: ' + street + '<br>Area: ' + area
                        + '<br>Description: ' + desc + '<br>Note: ' + note
                        + '</pre>'
                });
                if (status) {
                    var iconStyle = new ol.style.Style({
                        image: new ol.style.Circle({
                            radius: 5,
                            stroke: new ol.style.Stroke({
                                //color: 'lime'
                            }),
                            fill: new ol.style.Fill({
                                color: [0, 225, 0, 1]
                            }),
                        })
                    });
                }
                else {
                    var iconStyle = new ol.style.Style({
                        image: new ol.style.Circle({
                            radius: 5,
                            stroke: new ol.style.Stroke({
                                //color: 'red'
                            }),
                            fill: new ol.style.Fill({
                                color: [255, 0, 0]
                            }),
                        })
                    });
                }

                //var iconStyle = new ol.style.Style({
                //    image: new ol.style.Icon(({
                //        anchor: [0.5, 1],
                //        //scale: 0.4 // set the size of the img on the map
                //        //src: me.imgsrc
                //    }))
                //});
                iconFeature.setStyle(iconStyle);
                features.push(iconFeature);
            }
            this.vectorSource = new ol.source.Vector({ features: features });
            me.vectorLayer = new ol.layer.Vector({ source: me.vectorSource, style: new ol.style.Style({ image: new ol.style.Icon({ src: "../data/camera.png", scale: 0.8 }) }) });
            me.map.addLayer(me.vectorLayer);
        }
    },
    {
        key: 'initiMap',
        value: function initiMap(_data) {
            var me = this;
            let markers = _data;

            if ($("#map.mapboxgl-map").length > 0) {
                $("#map").removeClass("mapboxgl-map").empty();
            }

            this.map = new ol.Map({
                target: 'map',
                layers: [
                    new ol.layer.Group({
                        title: 'ประเภทแผนที่',
                        layers: [
                            new ol.layer.Tile({
                                title: 'ค่าเริ่มต้น',
                                type: 'base',
                                visible: true,
                                source: new ol.source.OSM()
                            }),
                            new ol.layer.Tile({
                                title: 'ดาวเทียม',
                                type: 'base',
                                visible: false,
                                source: new ol.source.BingMaps({
                                    key: "AiXsKpaFNMtLgALR6-1EzlzLDeN0jTA2mymenjZlaVu6RK-C6gASJL-YK5cc16Nj",
                                    imagerySet: "AerialWithLabelsOnDemand",
                                })
                            })
                        ]
                    })
                ],
                controls: ol.control.defaults({ attribution: false }),
                view: new ol.View({
                    center: ol.proj.fromLonLat([102.57400, 14.36129]),// center thaiLand
                    zoom: 6,
                    minZoom: 6,
                    maxZoom: 25,
                })
            });

            me.makeMarkers(_data);

            var container = me.popupInput;
            var content = me.popupContent;
            this.overlay = new ol.Overlay({
                element: container,
                autoPan: true,
                autoPanAnimation: {
                    duration: 250
                }
                //positioning: 'bottom-center',
                //stopEvent: false,
            });
            me.map.addOverlay(me.overlay);

            var container2 = me.popupInputHover;
            var content2 = me.popupContentHover;
            this.overlay2 = new ol.Overlay({
                element: container2,
                autoPan: true,
                autoPanAnimation: {
                    duration: 250
                }
                //positioning: 'bottom-center',
                //stopEvent: false,
            });
            me.map.addOverlay(me.overlay2);

            var layerSwitcher = new ol.control.LayerSwitcher({
                tipLabel: 'Optional', // Optional label for button
                groupSelectStyle: 'none' // Can be 'children' [default], 'group' or 'none'
            });
            me.map.addControl(layerSwitcher);

            // Set the control grid reference
            var search = new ol.control.SearchFeature(
                {   //target: $(".options").get(0),
                    source: me.vectorSource,
                    property: $(".options select").val()
                });
            me.map.addControl(search);

            var select_interaction = new ol.interaction.Select();
            me.map.addInteraction(select_interaction);

            // Select feature when click on the reference index
            search.on('select', function (e) {
                select_interaction.getFeatures().clear();
                select_interaction.getFeatures().push(e.search);
                var p = e.search.getGeometry().getFirstCoordinate();
                me.map.getView().animate({ center: p, zoom: Math.max(me.map.getView().getZoom(), 19) });
                content.innerHTML = e.search.get("desc");
                me.overlay.setPosition(p);
            });

            select_interaction.on('select', function (evt) {
                var features = select_interaction.getFeatures();
                if (features) {

                }
            });

            me.map.on('pointermove', function (event) {
                var feature = me.map.forEachFeatureAtPixel(event.pixel, function (feat, layer) { return feat; });
                if (feature && feature.get('type') == 'Point') {
                    var coordinate = event.coordinate;    //default projection is EPSG:3857 you may want to use ol.proj.transform
                    content2.innerHTML = feature.get('desc');
                    me.overlay2.setPosition(coordinate);
                }
                else {
                    const features = me.map.getFeaturesAtPixel(event.pixel);
                    if (features.length > 0) {
                        var coordinate = event.coordinate;
                        var pointName = features
                            .filter(feature => feature.getGeometry().getType() == 'Point')
                            .map(feature => feature.get('name'))
                        var longAndlat = features.
                            filter(feature => feature.getGeometry().getType() == 'Point')
                            .map(feature => `${ol.proj.toLonLat(feature.getGeometry().getCoordinates()).join(', ')}`)
                        content2.innerHTML = `<b>${pointName}</b><br>${longAndlat}`;
                        me.overlay2.setPosition(coordinate);
                    } else {
                        me.overlay2.setPosition(undefined);
                    }
                }
            });

            me.map.on('singleclick', function (evt) {
                //if (me.doDrawEnd) {
                //    return;
                //}
                var pixel = me.map.getEventPixel(evt.originalEvent);
                var coord = evt.coordinate;
                var popupText = "";
                me.map.forEachFeatureAtPixel(pixel, function (feature, layer) {
                    if (feature instanceof ol.Feature && layer && (layer.get("interactive") || layer.get("interactive") == undefined)) {
                        if (typeof feature.get("desc") !== "undefined") {
                            popupText = feature.get("desc");
                        }
                    }
                });

                if (popupText) {
                    me.overlay.setPosition(coord);
                    content.innerHTML = popupText;
                }
                else {
                    me.overlay.setPosition(undefined);
                }
            });
        }
    }
    ]);


    return EPoles;
}();