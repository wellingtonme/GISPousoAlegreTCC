
var GisMapClass = $.Class.create({

    initMap: function () {

        require(["esri/Color",
              "dojo/string",
              "dijit/registry",

              "esri/config",
              "esri/map",
              "esri/layers/ArcGISDynamicMapServiceLayer",
              "esri/graphic",
              "esri/tasks/Geoprocessor",
              "esri/tasks/FeatureSet",
              "esri/toolbars/draw",
              "esri/symbols/SimpleLineSymbol",
              "esri/symbols/SimpleFillSymbol"
        ],
        function (Color, string, registry, esriConfig, Map, ArcGISDynamicMapServiceLayer, Graphic, Geoprocessor, FeatureSet, Draw, SimpleLineSymbol, SimpleFillSymbol) {

            var map, gp, toolbar;

            app = {
                "map": map,
                "toolbar": toolbar
            };

            /*Initialize map, GP & image params*/
            app.map = map = new Map("mapDiv", {
                basemap: "streets",
                center: [-87.572, 33.329],
                zoom: 6
            });

            map.on("load", initTools);

            var populationMap = new ArcGISDynamicMapServiceLayer("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Population_World/MapServer");
            populationMap.setOpacity(0.5);
            map.addLayer(populationMap);

            //identify proxy page to use if the toJson payload to the geoprocessing service is greater than 2000 characters.
            //If this null or not available the gp.execute operation will not work.  Otherwise it will do a http post to the proxy.
            esriConfig.defaults.io.proxyUrl = "/proxy";
            esriConfig.defaults.io.alwaysUseProxy = false;

            function initTools(evtObj) {
                gp = new Geoprocessor("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Population_World/GPServer/PopulationSummary");
                gp.setOutputSpatialReference({ wkid: 102100 });
                gp.on("execute-complete", displayResults);

                app.toolbar = toolbar = new Draw(evtObj.map);
                toolbar.on("draw-end", computeZonalStats);
            }

            function computeZonalStats(evtObj) {
                var geometry = evtObj.geometry;

                //function to verify schools.
                $.ajax({
                    url: "/GisMap/GetSchoolInSelectedArea?geometry=" + geometry.rings,
                    type: 'POST',
                    async: false,
                    success: function (data) {
                    },
                    error: function (data) {
                    },
                });

                /*After user draws shape on map using the draw toolbar compute the zonal*/
                map.showZoomSlider();
                map.graphics.clear();

                var symbol = new SimpleFillSymbol("none", new SimpleLineSymbol("dashdot", new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25]));
                var graphic = new Graphic(geometry, symbol);

                map.graphics.add(graphic);
                toolbar.deactivate();

                var features = [];
                features.push(graphic);

                var featureSet = new FeatureSet();
                featureSet.features = features;

                var params = { "inputPoly": featureSet };
                gp.execute(params);
            }

            function displayResults(evtObj) {
                var results = evtObj.results;
                var content = string.substitute("<h4>The population in the user defined polygon is ${number:dojo.number.format}.</h4>", { number: results[0].value.features[0].attributes.SUM });

                registry.byId("dialog1").setContent(content);
                registry.byId("dialog1").show();
            }

        });
    }
});

var GisMapConfiguration = new GisMapClass();

$(function(){
    GisMapConfiguration.initMap();
});