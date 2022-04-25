'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var EPoles = function () {
    function EPoles() {
        _classCallCheck(this, EPoles);
        //this.imgsrc = "http://cdn.mapmarker.io/api/v1/pin?text=P&size=50&hoffset=1";
        this.popupInput = document.getElementById('popup');
        this.popupContent = document.getElementById('popup-content');

        this.popupInputAdd = document.getElementById('popup2');
        this.popupContentAdd = document.getElementById('popup-content2');
        this.popupCloserAdd = document.getElementById('popup-closer');

        this.popupInputDel = document.getElementById('popup3');
        this.popupContentDel = document.getElementById('popup-content3');
        this.popupClickDel = document.getElementById('popup-delete');
        this.popupClickEdit = document.getElementById('popup-edit');

        this.headform = $("#headform");
        this.form = $("#formPole");
        this.hiddenId = $("#Id");
        this.txtLatitude = $("#Latitude");
        this.txtLongitude = $("#Longitude");
        this.txtName = $("#Name");
        this.txtStreet = $("#Street");
        this.txtArea = $("#Area");
        this.txtDescription = $("#Description");
        this.txtNote = $("#Note");
        this.selectStatus = $("#Status");
        this.btnSubmit = $("#btnsubmit");

        this.vectorLayer = null;
        this.doDrawEnd = false;

    }

    _createClass(EPoles, [{
        key: 'init',
        value: function init() {
            var me = this;
            this.createMap();
            me.headform.text("เพิ่มข้อมูล");
            this.btnSubmit.click(function () {
                //var _model = $('form').serialize();
                var _model = me.form.serialize();
                $.validator.unobtrusive.parse(me.form);
                me.form.validate();

                if (me.form.valid()) {
                    let apiUrl = "";
                    if ($("#headform").text() === "เพิ่มข้อมูล") {
                        apiUrl = me.createPoleUrl;
                    }
                    else {
                        apiUrl = me.updatePoleUrl;
                    }
                    $.ajax({
                        url: apiUrl,
                        type: "POST",
                        data: _model,
                        success: function success(data) {
                            if (data) {

                                me.map.removeLayer(me.vectorLayer);
                                me.vectorLayer = null;

                                me.makeMarkers(data);
                                /*
                                let markers = data;
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
                                    features.push(iconFeature);
                                }
                                var vectorSource = new ol.source.Vector({ features: features });
                                me.vectorLayer = new ol.layer.Vector({ source: vectorSource });
                                me.map.addLayer(me.vectorLayer);
                                */
                                if (me.overlay3 != undefined) {
                                    me.overlay3.setPosition(undefined);
                                }
                                me.overlay2.setPosition(undefined);
                                me.popupCloserAdd.blur();
                                me.resetForm();

                                me.doDrawEnd = false;
                                //me.map.on('rendercomplete', function (event) {
                                //    me.map.getView().setZoom(me.current_zoom - 0.001);
                                //});
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert("error")
                        }
                    });
                }

            });
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
        key: 'resetForm',
        value: function resetForm() {
            var me = this;
            me.txtLatitude.val("");
            me.txtLongitude.val("");
            me.txtName.val("");
            me.txtStreet.val("");
            me.txtArea.val("");
            me.txtDescription.val("");
            me.txtNote.val("");
            me.selectStatus.prop('selectedIndex', 0);
            me.headform.text("เพิ่มข้อมูล");
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

            var button = document.createElement('button');
            button.innerHTML = '<i class="fa fa-map-pin"></i>';

            var element = document.createElement('div');
            element.className = 'rotate-north ol-unselectable ol-control';
            element.appendChild(button);

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
                controls: ol.control.defaults({ attribution: false }).extend([new ol.control.Control({ element: element })]),
                view: new ol.View({
                    center: ol.proj.fromLonLat([102.57400, 14.36129]),// center thaiLand
                    zoom: 6,
                    minZoom: 6,
                    maxZoom: 25,
                })
            });

            me.makeMarkers(_data);
            /*
            var features = [];
            for (var i = 0; i < markers.length; i++) {
                var item = markers[i];
                var longitude = item.longitude;
                var latitude = item.latitude;
                var name = item.name;

                var iconFeature = new ol.Feature({
                    geometry: new ol.geom.Point(ol.proj.transform([longitude, latitude], 'EPSG:4326', 'EPSG:3857')),
                    type: 'Point',
                    desc: '<pre> <b>' + name + ' </b> ' + '<br>' + 'Latitude : ' + latitude + '<br>Longitude: ' + longitude + '</pre>'
                });
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
            this.vectorLayer = new ol.layer.Vector({ source: me.vectorSource });
            me.map.addLayer(me.vectorLayer);
            */
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
            me.map.addOverlay(overlay);

            var container2 = me.popupInputAdd;
            var content2 = me.popupContentAdd;
            this.overlay2 = new ol.Overlay({
                element: container2,
                autoPan: {
                    animation: {
                        duration: 250,
                    },
                },
            });
            me.map.addOverlay(me.overlay2);

            var container3 = me.popupInputDel;
            var content3 = me.popupContentDel;
            this.overlay3 = new ol.Overlay({
                element: container3,
                positioning: "bottom-center",
                stopEvent: false
            });
            me.map.addOverlay(me.overlay3);

            var handleNewPole = function (e) {
                var Msource = new ol.source.Vector();
                var markLayer = new ol.layer.Vector({
                    source: Msource,
                    style: new ol.style.Style({
                        image: new ol.style.Icon({
                            opacity: 0.95,
                            src: "http://www.macfh.co.uk/Resources/Images/RGB00FF00.png"
                        })
                    })
                });
                var mark;
                mark = new ol.interaction.Draw({
                    source: Msource,
                    type: "Point"
                });
                me.map.addInteraction(mark);

                me.popupCloserAdd.onclick = function () {
                    me.overlay2.setPosition(undefined);
                    me.popupCloserAdd.blur();
                    me.resetForm();
                    me.doDrawEnd = false;
                    return false;
                };
                markLayer.on("change", function () {
                    /*
                     const coordinate = mark._v;
                    content2.innerHTML = `${coordinate.join(', ')}`;
                    overlay2.setPosition(coordinate);
                     */
                    me.map.removeInteraction(mark);
                });
                mark.on('drawend', function (evt) {
                    const coordinate = mark._v;
                    var feature = evt.feature;
                    content2.innerHTML = `${ol.proj.toLonLat(feature.getGeometry().getCoordinates()).join(', ')}`;
                    me.popupCloserAdd.blur();

                    me.txtLatitude.val(ol.proj.toLonLat(feature.getGeometry().getCoordinates())[1]);
                    me.txtLongitude.val(ol.proj.toLonLat(feature.getGeometry().getCoordinates())[0]);
                    $.get(me.lastareaUrl, function (data) {
                        me.txtStreet.val(data[0].street);
                        me.txtArea.val(data[0].area);
                    });
                    me.overlay2.setPosition(coordinate);
                    me.doDrawEnd = true;
                });
            };
            button.addEventListener('click', handleNewPole, false);

            /*
            var pointInteraction = new ol.interaction.Draw({
                type: 'Point',
                source: me.vectorLayer.getSource()
            });
            pointInteraction.setActive(false);
            var selectInteraction = new ol.interaction.Select({
                condition: ol.events.condition.click,
                wrapX: false,
            })
            var modifyInteraction = new ol.interaction.Modify({
                features: selectInteraction.getFeatures(),
                deleteCondition: function (event) {
                    return ol.events.condition.shiftKeyOnly(event) &&
                        ol.events.condition.singleClick(event);
                }
            });
            me.map.addInteraction(modifyInteraction);
            var setActiveEditing = function (active) {
                selectInteraction.getFeatures().clear();
                selectInteraction.setActive(active);
                modifyInteraction.setActive(active);
                //translateInteraction.setActive(active);
            };
            setActiveEditing(true);
            var snapInteraction = new ol.interaction.Snap({
                source: me.vectorLayer.getSource()
            });
            me.map.getInteractions().extend([
                pointInteraction,
                selectInteraction,
                modifyInteraction,
                snapInteraction]);
                */

            var layerSwitcher = new ol.control.LayerSwitcher({
                tipLabel: 'Optional', // Optional label for button
                groupSelectStyle: 'none' // Can be 'children' [default], 'group' or 'none'
            });
            me.map.addControl(layerSwitcher);

            me.map.on('pointermove', function (event) {
                var feature = me.map.forEachFeatureAtPixel(event.pixel, function (feat, layer) { return feat; });
                if (feature && feature.get('type') == 'Point') {
                    var coordinate = event.coordinate;    //default projection is EPSG:3857 you may want to use ol.proj.transform
                    content.innerHTML = feature.get('desc');
                    overlay.setPosition(coordinate);
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
                        content.innerHTML = `<b>${pointName}</b><br>${longAndlat}`;
                        overlay.setPosition(coordinate);
                    } else {
                        overlay.setPosition(undefined);
                    }
                }
                /*
                    const pixel = map.getEventPixel(e.originalEvent);
                    const hit = map.hasFeatureAtPixel(pixel);
                    map.getTargetElement().style.cursor = hit ? 'pointer' : '';
                 */
            });

            me.map.on('singleclick', function (evt) {
                if (me.doDrawEnd) {
                    return;
                }
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
                    me.overlay3.setPosition(coord);
                    content3.innerHTML = popupText;
                }
                else {
                    me.overlay3.setPosition(undefined);
                }
            });

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
                content3.innerHTML = e.search.get("desc");
                me.overlay3.setPosition(p);
            });

            select_interaction.on('select', function (evt) {
                var features = select_interaction.getFeatures();
                if (features) {

                }
            });

            me.popupClickEdit.onclick = function (evt) {
                var features = select_interaction.getFeatures();
                if (features) {
                    me.headform.text("แก้ไขข้อมูล");
                    me.hiddenId.val(select_interaction.getFeatures().item(0).get("makerid"));
                    me.txtLatitude.val(select_interaction.getFeatures().item(0).get("lat"));
                    me.txtLongitude.val(select_interaction.getFeatures().item(0).get("long"));
                    me.txtName.val(select_interaction.getFeatures().item(0).get("name"));
                    me.txtStreet.val(select_interaction.getFeatures().item(0).get("street"));
                    me.txtArea.val(select_interaction.getFeatures().item(0).get("area"));
                    me.txtDescription.val(select_interaction.getFeatures().item(0).get("description"));
                    me.txtNote.val(select_interaction.getFeatures().item(0).get("note"));
                    me.selectStatus.val(select_interaction.getFeatures().item(0).get("status").toString());
                    var coord = features.R[0].getGeometry().getCoordinates();
                    me.overlay3.setPosition(coord);
                    me.doDrawEnd = true;
                }
                return false;
            };

            me.popupClickDel.onclick = function (evt) {
                var features = select_interaction.getFeatures();
                if (features) {
                    var obj = {};
                    obj["Id"] = select_interaction.getFeatures().item(0).get("makerid");
                    obj["Name"] = select_interaction.getFeatures().item(0).get("name");
                    obj["Latitude"] = select_interaction.getFeatures().item(0).get("lat");
                    obj["Longitude"] = select_interaction.getFeatures().item(0).get("long");
                    obj["Area"] = select_interaction.getFeatures().item(0).get("area");
                    obj["Street"] = select_interaction.getFeatures().item(0).get("street");
                    obj["Note"] = select_interaction.getFeatures().item(0).get("note");
                    obj["Description"] = select_interaction.getFeatures().item(0).get("description");
                    obj["Status"] = select_interaction.getFeatures().item(0).get("status");
                    obj["UserId"] = me.userId.toString();
                    $.ajax({
                        type: "POST",
                        url: me.deletePoleUrl,
                        data: JSON.stringify(obj),
                        dataType: 'JSON',
                        contentType: "application/json",
                        success: function success(data) {
                            me.map.removeLayer(me.vectorLayer);
                            me.vectorLayer = null;

                            me.makeMarkers(data);
                            if (me.overlay3 != undefined) {
                                me.overlay3.setPosition(undefined);
                            }
                            me.popupClickDel.blur();
                            me.resetForm();
                            me.doDrawEnd = false;
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            console.log(error)
                        }
                    });
                }

                return false;
            };
            //me.map.on('singleclick', function (evt) {
            //    var coordinate = evt.coordinate;
            //    var hdms = ol.coordinate.toStringHDMS(ol.proj.transform(
            //        coordinate, 'EPSG:3857', 'EPSG:4326'));

            //    content3.innerHTML = '<p>You clicked here:</p><code>' + hdms +
            //        '</code>';
            //    overlay3.setPosition(coordinate);
            //});

            //me.map.on('click', function (evt) {
            //    const feature = me.map.forEachFeatureAtPixel(evt.pixel, function (feature) {
            //        return feature;
            //    });
            //    if (feature) {
            //        //var coordinates = feature.getGeometry().getCoordinates();
            //        overlay3.setPosition(evt.coordinate);
            //        $(element).popover({
            //            placement: "top",
            //            html: true,
            //            content: feature.get("desc")
            //        });
            //        $(element).popover("show");
            //        //overlay3.setPosition(evt.coordinate);  
            //        //$(element).popover('dispose');
            //        //$(element).popover({
            //        //    placement: 'top',
            //        //    html: true,
            //        //    content: feature.get('desc'),
            //        //});
            //        //$(element).popover('show');
            //    } else {
            //        $(element).popover('dispose');
            //    }
            //});

            // Close the popup when the map is moved
            //map.on('movestart', function () {
            //    $(element).popover('dispose');
            //});

            /*On click marker zoom at certain zoom level
             map.getView().setCenter(ol.extent.getCenter(feature.getGeometry().getExtent()));
             map.getView().setZoom(16);
             */
        }
    }
    ]);


    return EPoles;
}();