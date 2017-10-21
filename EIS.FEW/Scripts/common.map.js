
(function ($, appName) {
    /// <summary>Populates global ko object.</summary>
    /// <param name="$">Reference to jquery object.</param>
    /// <param name="appName">namespace of application.</param>
    'use strict';

    appName.Map = function (params) {
        var self = this;

        var config = {
            baseUri: "",
            target: "map",
            projection: 'EPSG:3857',
            projectionSrc: 'EPSG:4326',
            vietnam: ol.proj.fromLonLat([105.7, 17]),
            minZoom: 5,
            maxZoom: 19,
            showDiaGioi: true,
        };

        var map = {};
        var isShowFull = false;
        var oldLevel = config.minZoom;
        var features = [];
        var layers = [];
        var levels = {
            Tinh: {
                LayerName: "Tỉnh/ Thành phố",
                Min: 0,
                Max: 9
            },
            Huyen: {
                LayerName: "Quận/ Huyện",
                Min: 10,
                Max: 12
            },
            Xa: {
                LayerName: "Xã/ Phường",
                Min: 13,
                Max: 19
            }
        }

        var geoTypes = {
            LinearRing: 'LinearRing',
            Polygon: 'Polygon',
            MultiPolygon: 'MultiPolygon',
            Point: 'Point'
        }

        var myDom = {
            Point: {
                text: "",
                align: 'center',
                baseline: 'middle',
                rotation: '0',
                font: 'Arial',
                weight: 'Normal',
                size: '12px',
                offsetX: '0',
                offsetY: '0',
                color: '#aa3300',
                outline: '#ffffff',
                outlineWidth: '3',
                maxreso: '1200'
            },
            Polygon: {
                text: "",
                align: 'center',
                baseline: 'middle',
                rotation: '0',
                font: 'Arial',
                weight: 'Normal',
                size: '12px',
                offsetX: '0',
                offsetY: '0',
                color: '#aa3300',
                outline: '#ffffff',
                outlineWidth: '3',
                maxreso: '1200'
            },
            MultiPolygon: {
                text: "",
                align: 'center',
                baseline: 'middle',
                rotation: '0',
                font: 'Arial',
                weight: 'Normal',
                size: '12px',
                offsetX: '0',
                offsetY: '0',
                color: '#aa3300',
                outline: '#ffffff',
                outlineWidth: '3',
                maxreso: '1200'
            }
        }

        var styleConfig = {
            title: '',//ten lop du lieu
            key: '',//ten truong quyet dinh mau vi du DienTich, DanSo, SoNguoi
            rules: []
        }

        function getUrl(method) {
            if (method[0] == "/") {
                method = method.substr(1);
            }
            return config.baseUri + common.moduleUrl(method);
        }

        function getType(geoM) {
            if (geoM.indexOf('POLYGON') === 0) {
                return geoTypes.Polygon;
            }
            if (geoM.indexOf('MULTIPOLYGON') === 0) {
                return geoTypes.MultiPolygon;
            }
            return geoTypes.Point;
        }

        function createStrokeColorByArea(geoArea) {
            return 'grey';
        }

        function createFillColor(fillColorData) {
            for (var i = 0; i < styleConfig.rules.length; i++) {
                var rule = styleConfig.rules[i];
                if (fillColorData >= rule.min && (fillColorData <= rule.max || rule.max == null)) {
                    return rule.color;
                }
            }
            return '#d8d4c6';
        }

        function createFillStyle(feature, resolution) {
            //var geoM = feature.getGeometry(),
            var fillColorData = feature.getProperties()[styleConfig.key];
            var color = createFillColor(fillColorData);

            return new ol.style.Fill({
                color: color
            });
        }

        function createStroke(feature, resolution) {
            var geoM = feature.getGeometry(),
                geoType = geoM.getType(),
                color = 'grey';

            switch (geoType) {
                case geoTypes.Polygon:
                case geoTypes.MultiPolygon:
                    color = createStrokeColorByArea(geoM.getArea())
                    break;
                case geoTypes.Point:
                    color = 'red';
                    break;
                default:
                    color = 'grey';
                    break;
            }

            return new ol.style.Stroke({
                color: color,
                width: 0.5,
                lineDash: [4],
            });
        }

        function stringDivider(str, width, spaceReplacer) {
            if (str.length > width) {
                var p = width;
                for (; p > 0 && (str[p] != ' ' && str[p] != '-') ; p--) {
                }
                if (p > 0) {
                    var left;
                    if (str.substring(p, p + 1) == '-') {
                        left = str.substring(0, p + 1);
                    } else {
                        left = str.substring(0, p);
                    }
                    var right = str.substring(p + 1);
                    return left + spaceReplacer + stringDivider(right, width, spaceReplacer);
                }
            }
            return str;
        }

        function getText(feature, resolution, dom) {
            var maxResolution = dom.maxreso;
            var text = feature.get('name');

            if (resolution > maxResolution) {
                text = '';
            } else {
                text = stringDivider(text, 16, '\n');
            }

            return text;
        };

        function createTextStyle(feature, resolution, dom) {
            var geoM = feature.getGeometry(),
                geoType = geoM.getType();

            if (geoType != geoTypes.Point)
                return;

            var dom = myDom[geoType];
            var align = dom.align;
            var baseline = dom.baseline;
            var size = dom.size;
            var offsetX = parseInt(dom.offsetX, 10);
            var offsetY = parseInt(dom.offsetY, 10);
            var weight = dom.weight;
            var rotation = parseFloat(dom.rotation);
            var font = weight + ' ' + size + ' ' + dom.font;
            var fillColor = dom.color;
            var outlineColor = dom.outline;
            var outlineWidth = parseInt(dom.outlineWidth, 10);

            return new ol.style.Text({
                textAlign: align,
                textBaseline: baseline,
                font: font,
                text: getText(feature, resolution, dom),
                fill: new ol.style.Fill({ color: fillColor }),
                stroke: new ol.style.Stroke({ color: outlineColor, width: outlineWidth }),
                offsetX: offsetX,
                offsetY: offsetY,
                rotation: rotation
            });
        };

        function styleFunction(feature, resolution) {
            var style = new ol.style.Style({
                stroke: createStroke(feature, resolution),
                fill: createFillStyle(feature, resolution),
                text: createTextStyle(feature, resolution)
            });
            return [style];
        }

        function getFeature(data) {
            var geoM = data.GeoM,
                geoType = getType(geoM),
                format = new ol.format.WKT(),
                feature = format.readFeature(geoM);

            feature.setProperties({ "name": data.Ten });
            feature.setProperties({ "key": data.Ma });
            feature.setProperties({ "id": data.Id });
            feature.key = data.Ma;

            feature.getGeometry().transform(config.projectionSrc, config.projection);

            return feature;
        }

        function createFeature(data) {
            var geoM = $.StringFormat("POINT ({0} {1})", data.XDaiDien, data.YDaiDien),
                geoType = getType(geoM),
                format = new ol.format.WKT(),
                feature = format.readFeature(geoM);

            feature.setProperties({ "name": data.Ten });
            feature.setProperties({ "key": 'p-' + data.Ma });
            feature.setProperties({ "id": data.Id });
            feature.key = 'p-' + data.Ma;

            feature.getGeometry().transform(config.projectionSrc, config.projection);

            return feature;
        }

        function addFeature(fc) {
            var existed = false;
            for (var i = 0; i < features.length; i++) {
                if (features[i].key == fc.key) {
                    existed = true;
                    break;
                }
            }
            if (!existed) {
                features.push(fc);
            }
        }

        function addLayerToMap(layer, isOverride) {
            var existed = false;
            for (var i = 0; i < layers.length; i++) {
                if (layers[i].key == layer.key) {
                    existed = true;
                    break;
                }
            }
            if (existed) {
                if (isOverride) {
                    map.removeLayer(layer);
                }
                else {//return not add
                    return;
                }
            } else {
                layers.push(layer)
            }

            map.addLayer(layer);
        }

        function removeVectorLayer(layerName) {
            var layerArr = map.getLayers().getArray();
            for (var i = 0; i < layerArr.length; i++) {
                var layer = layerArr[i];
                if (layer.getProperties().title == layerName) {
                    map.removeLayer(layer);
                }
            }
        }

        function addTinhs() {
            var layerName = levels.Tinh.LayerName;
            var option = {
                source: new ol.source.Vector({
                    features: features
                }),
                style: styleFunction,
                title: layerName,
                key: layerName,

            };
            if (config.showDiaGioi) {
                option.visible = true;
            } else {
                option.visible = false;
                option.type = 'base';
            }

            var vector = new ol.layer.Vector(option);

            vector.key = layerName;
            addLayerToMap(vector);
        }

        function getRegions(callback) {
            var url = getUrl('api/Map/GetRegions');
            serviceInvoker.get(url, {}, {
                success: function (response) {
                    if (response.Data) {
                        for (var i = 0; i < response.Data.length; i++) {
                            var data = response.Data[i];
                            var feature = getFeature(data);
                            addFeature(feature);
                            addFeature(createFeature(data));
                        }
                    }
                },
                complete: function () {
                    if (typeof callback == 'function') {
                        callback();
                    }
                }
            }, null, true, true);
        }

        function loadBounds(level, minX, minY, maxX, maxY, layerName) {
            var url = $.StringFormat('api/Map/GetRegionsByBoundBox/{0}/{1}/{2}/{3}/{4}',
                level, minX, minY, maxX, maxY);
            url = getUrl(url);
            serviceInvoker.get(url, {}, {
                success: function (response) {
                    if (response.Data) {
                        for (var i = 0; i < response.Data.length; i++) {
                            var feature = getFeature(response.Data[i]);
                            addFeature(feature);
                        }
                        var vector = new ol.layer.Vector({
                            source: new ol.source.Vector({
                                features: features
                            }),
                            style: styleFunction,
                            title: layerName,
                            key: layerName
                        });

                        vector.key = layerName;
                        addLayerToMap(vector);
                    }
                }
            }, null, true, true);
        }

        //thong tin chi tiet tinh
        function getInfo(point, callback) {
            var url = $.StringFormat('api/Map/GetRegionByPoint/{0}/{1}/{2}',
                1, point[0], point[1]);
            url = getUrl(url);
            var url = getUrl(url);
            serviceInvoker.get(url, {}, {
                success: function (response) {
                    callback(response)
                }
            }, null, true, true);
        }

        function changeResolution() {
            var view = map.getView();
            var level = view.getZoom();
            var extent = view.calculateExtent(map.getSize())
            var minXY = ol.proj.toLonLat([extent[0], extent[1]]);
            var maxXY = ol.proj.toLonLat([extent[2], extent[3]]);

            if (level >= levels.Tinh.Min && level <= levels.Tinh.Max) {
                if (oldLevel >= levels.Tinh.Min && oldLevel <= levels.Tinh.Max) {
                    return;
                }
                //removeVectorLayer(levels.Huyen.LayerName);
                loadBounds(1, minXY[0], minXY[1], maxXY[0], maxXY[1], levels.Tinh.LayerName);
            }
            if (level >= levels.Huyen.Min && level <= levels.Huyen.Max) {
                if (oldLevel >= levels.Huyen.Min && oldLevel <= levels.Huyen.Max) {
                    return;
                }
                //removeVectorLayer(levels.Xa.LayerName);
                //removeVectorLayer(levels.Tinh.LayerName);
                loadBounds(2, minXY[0], minXY[1], maxXY[0], maxXY[1], levels.Huyen.LayerName);
            }
            if (level >= levels.Xa.Min && level <= levels.Xa.Max) {
                if (oldLevel >= levels.Xa.Min && oldLevel <= levels.Xa.Max) {
                    return;
                }
                //removeVectorLayer(levels.Huyen.LayerName);
                loadBounds(3, minXY[0], minXY[1], maxXY[0], maxXY[1], levels.Xa.LayerName);
            }
        }

        function changeCenter() {
            var view = map.getView();
            var level = view.getZoom();
            var extent = view.calculateExtent(map.getSize())
            var minXY = ol.proj.toLonLat([extent[0], extent[1]]);
            var maxXY = ol.proj.toLonLat([extent[2], extent[3]]);

            if (level >= levels.Tinh.Min && level <= levels.Tinh.Max) {
                //removeVectorLayer(levels.Huyen.LayerName);
                loadBounds(1, minXY[0], minXY[1], maxXY[0], maxXY[1], levels.Tinh.LayerName);
            }
            if (level >= levels.Huyen.Min && level <= levels.Huyen.Max) {
                //removeVectorLayer(levels.Xa.LayerName);
                //removeVectorLayer(levels.Tinh.LayerName);
                loadBounds(2, minXY[0], minXY[1], maxXY[0], maxXY[1], levels.Huyen.LayerName);
            }
            if (level >= levels.Xa.Min && level <= levels.Xa.Max) {
                //removeVectorLayer(levels.Huyen.LayerName);
                loadBounds(3, minXY[0], minXY[1], maxXY[0], maxXY[1], levels.Xa.LayerName);
            }
        }

        function addEvents() {
            var layerSwitcher = new ol.control.LayerSwitcher({
                tipLabel: 'Bản đồ nền' // Optional label for button
            });
            map.addControl(layerSwitcher);

            // a normal select interaction to handle click
            var select = new ol.interaction.Select();
            map.addInteraction(select);

            //neu chi view dia gioi
            //them cac chuc nang xem huyen xa
            //khong thi chi phuc vu view bao cao len ban do
            if (config.showDiaGioi) {
                if (isShowFull) {
                    map.on('moveend', function (evt) {
                        changeCenter();
                    });

                    map.getView().on('propertychange', function (e) {
                        switch (e.key) {
                            case 'resolution':
                                changeResolution();
                                break;
                        }
                    });
                }
            }
        }

        function drawToMap(datas) {
            if (typeof datas != 'object') {
                if (datas.length == 0) {
                    return;
                }
            }

            var featureClasses = [];
            for (var i = 0; i < datas.length; i++) {
                var data = datas[i];
                if (typeof data[styleConfig.key] == 'undefined') {
                    throw styleConfig.key + ' not defined';
                }
                var donViHanhChinhId = data['DonViHanhChinhId'];
                if (typeof donViHanhChinhId == 'undefined') {
                    continue;
                }
                for (var j = 0; j < features.length; j++) {
                    if (features[j].key == donViHanhChinhId
                        || features[j].key == 'p-' + donViHanhChinhId) {
                        var feature = features[j].clone();
                        var prp = {};
                        prp[styleConfig.key] = data[styleConfig.key];
                        feature.setProperties(prp);

                        featureClasses.push(feature);
                    }
                }
            }

            var vector = new ol.layer.Vector({
                source: new ol.source.Vector({
                    features: featureClasses
                }),
                style: styleFunction,
                title: styleConfig.title,
                key: styleConfig.title
            });

            vector.key = styleConfig.title;
            addLayerToMap(vector, true);
        }

        function genMap() {
            var view = new ol.View({
                // the view's initial state
                center: config.vietnam,
                zoom: 5,
                minZoom: config.minZoom,
                maxZoom: config.maxZoom
            });

            var mousePositionControl = new ol.control.MousePosition({
                coordinateFormat: ol.coordinate.createStringXY(4),
                projection: config.projectionSrc,
                // comment the following two lines to have the mouse position
                // be placed within the map.
                className: 'custom-mouse-position',
                target: 'lonlat-info',
                undefinedHTML: '&nbsp;'
            });
            var raster = new ol.layer.Tile({
                title: 'Thế giới',
                source: new ol.source.OSM(),
                //type: 'base',
                visible: false
            });
            map = new ol.Map({
                layers: [],
                // Improve user experience by loading tiles while animating. Will make
                // animations stutter on mobile or slow devices.
                loadTilesWhileAnimating: true,
                target: config.target,
                controls: ol.control.defaults({
                    attributionOptions: /** @type {olx.control.AttributionOptions} */ ({
                        collapsible: false
                    })
                }).extend([mousePositionControl]),
                view: view
            });

            addEvents()

            return map;
        }

        function init(prs) {
            config = $.extend(config, prs);
            genMap();
        }

        self.getMap = function () {
            return map;
        }

        function getExtent() {
            var layers = map.getLayers(),
                array = layers.getArray(),
                layer = layers.getArray()[0],
                source = layer.getSource();
            return source.getExtent();
        }

        self.zoomToExtent = function() {
            var extent = getExtent();
            map.getView().fit(extent, map.getSize());
        }

        self.showDiaGoi = function (confg) {
            isShowFull = confg.isShowFull;
            getRegions(function () {
                //add lop nen tinh 
                addTinhs();
                self.zoomToExtent();
            });

            return self;
        }

        self.removeLayer = function (layerName) {
            removeVectorLayer(layerName);
            return self;
        }

        self.drawData = function (data, styleCfg) {
            styleConfig = $.extend(styleConfig, styleCfg);
            getRegions(function () {
                //add lop nen tinh 
                addTinhs();
                drawToMap(data);
            });

            return self;
        }

        self.showInfo = function (callback) {
            map.on('singleclick', function (evt) {
                var lonlat = evt.coordinate;
                var point = ol.proj.toLonLat(lonlat, config.projection);
                getInfo(point, callback);
            });

            return self;
        }

        self.zoomFeatureByName = function (name, callback) {
            var feature = {},
                hasFinded = false;
            var layers = map.getLayers().getArray();
            for (var j = 0; j < layers.length; j++) {
                var layer = layers[j];
                var src = layer.getSource();
                if (typeof src.getFeatures != 'function') {
                    continue;
                }
                var fcs = src.getFeatures()
                for (var i = 0; i < fcs.length; i++) {
                    if (fcs[i].getProperties().name == name) {
                        feature = fcs[i];
                        map.getView().fit(feature.getGeometry().getExtent(), map.getSize());
                        hasFinded = true;
                        break;
                    }
                }
                if (hasFinded) {
                    break;
                }
            }
            if (typeof callback == 'function') {
                callback(feature);
            }
            return self;
        }

        self.zoomFeatureById = function (id, callback) {
            var feature = {},
                hasFinded = false;
            var layers = map.getLayers().getArray();
            for (var j = 0; j < layers.length; j++) {
                var layer = layers[j];
                var src = layer.getSource();
                if (typeof src.getFeatures != 'function') {
                    continue;
                }
                var fcs = src.getFeatures()
                for (var i = 0; i < fcs.length; i++) {
                    if (fcs[i].getProperties().key == id) {
                        feature = fcs[i];
                        map.getView().fit(feature.getGeometry().getExtent(), map.getSize());
                        hasFinded = true;
                        break;
                    }
                }
                if (hasFinded) {
                    break;
                }
            }
            if (typeof callback == 'function') {
                callback(feature);
            }
            return self;
        }

        init(params);

        return self;
    }
}(jQuery, window.aiMapV3 = window.aiMapV3 || {}));