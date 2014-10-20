var GisMapClass = $.Class.create({
    initialize: function () {

        require(["esri/Color",
              "esri/InfoTemplate",
              "dojo/string",
              "dijit/registry",
              "esri/config",
              "esri/layers/GraphicsLayer",
              "esri/graphic",
              "esri/map",
              "esri/layers/ArcGISDynamicMapServiceLayer",
              "esri/layers/FeatureLayer",
              "esri/geometry/webMercatorUtils",
              "esri/tasks/Geoprocessor",
              "esri/tasks/FeatureSet",
              "esri/toolbars/draw",
              "esri/symbols/SimpleLineSymbol",
              "esri/symbols/SimpleFillSymbol",
              "esri/dijit/PopupTemplate",
              "esri/geometry/Point",
              "dojo/on",
              "dojo/_base/array",
              "esri/request",
              "esri/symbols/SimpleMarkerSymbol",
              "esri/SpatialReference",
              "dojo/dom",
              "dojo/domReady!"

        ],
            function (Color,InfoTemplate, string, registry, esriConfig, GraphicsLayer, Graphic, Map, ArcGISDynamicMapServiceLayer,
                FeatureLayer, webMercatorUtils, Geoprocessor, FeatureSet, Draw, SimpleLineSymbol, SimpleFillSymbol,
                PopupTemplate, Point, on, array, esriRequest, SimpleMarkerSymbol, SpatialReference, dom) {

                var map, gp, toolbar;

                app = {
                    "map": map,
                    "toolbar": toolbar
                };

                /*Initialize map, GP & image params*/
                app.map = map = new Map("mapDiv", {
                    basemap: "streets",
                    center: [-45.936, -22.231],
                    zoom: 14
                });

                map.on("load", initTools);

                var populationMap = new ArcGISDynamicMapServiceLayer("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Population_World/MapServer");
                populationMap.setOpacity(0.5);

                map.addLayer(populationMap);

                //identify proxy page to use if the toJson payload to the geoprocessing service is greater than 2000 characters.
                //If this null or not available the gp.execute operation will not work.  Otherwise it will do a http post to the proxy.
                esriConfig.defaults.io.proxyUrl = "/arcgisserver/apis/javascript/proxy/proxy.ashx";
                esriConfig.defaults.io.alwaysUseProxy = false;

                function initTools(evtObj) {
                    GisMapConfiguration.setEventListeners();
                    GisMapConfiguration.hideDivs();
                    gp = new Geoprocessor("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Population_World/GPServer/PopulationSummary");
                    gp.setOutputSpatialReference({ wkid: 102100 });
                    gp.on("execute-complete", computeValues);
                    gp.on("error", processGpError);

                    app.toolbar = toolbar = new Draw(evtObj.map);
                    toolbar.on("draw-end", computeZonalStats);

                    map.on("mouse-move", showCoordinates);

                    addSchoolsPoints();
                    addCopsPoints();
                    addCriminalPolygons();
                }

                function showCoordinates(evt) {
                    //the map is in web mercator but display coordinates in geographic (lat, long)
                    var mp = webMercatorUtils.webMercatorToGeographic(evt.mapPoint);
                    //display mouse coordinates
                    //document.getElementById("coordinates").innerHTML = mp.x.toFixed(5) + ", " + mp.y.toFixed(5);
                }


                function addSchoolsPoints() {
                    
                    var points = PointsObjectsConfiguration.getSchoolsPoints();

                    var graphicLayer = new GraphicsLayer();
                    if (points !== null && points !== undefined) {
                        points.forEach(function (school) {
                            var gra = new esri.Graphic(school);
                            graphicLayer.add(gra);
                        });
                    }
                    map.addLayer(graphicLayer);
                    map.reorderLayer(graphicLayer, 1);
                }

                function addCopsPoints() {
                    var points = PointsObjectsConfiguration.getCopsPoints();

                    var graphicLayer = new GraphicsLayer();
                    if (points !== null && points !== undefined) {
                        points.forEach(function (cop) {
                            var gra = new esri.Graphic(cop);
                            graphicLayer.add(gra);
                        });
                    }
                    map.addLayer(graphicLayer);
                    map.reorderLayer(graphicLayer, 2);
                }

                function addCriminalPolygons() {
                    var criminals = PointsObjectsConfiguration.getCriminalIndexPolygons();
                    var graphicLayer = new GraphicsLayer();
                    if (criminals !== null) {
                        if (criminals.length > 0) {
                            criminals.forEach(function (criminal) {
                                var infoTemp = new InfoTemplate(criminal.infoTemplate.title, criminal.infoTemplate.content);
                                criminal.infoTemplate = infoTemp;
                                criminal.symbol.color[3] = 64;
                                var gra = new esri.Graphic(criminal);
                                graphicLayer.add(gra);
                            });
                        }
                    }
                    map.addLayer(graphicLayer);
                    map.reorderLayer(graphicLayer, 3);
                    //var t = "Bairro: ${District}<br/> Criminalidade: ${CriminalIndex}";
                    //var info = new InfoTemplate("Criminalidade", t);
                    //var myPolygon = {
                    //    "geometry": {
                    //        "rings": [[[-115.3125, 37.96875], [-111.4453125, 37.96875], [-99.84375, 36.2109375], [-99.84375, 23.90625], [-116.015625, 24.609375], [-115.3125, 37.96875]]],
                    //        "spatialReference": { "wkid": 4326 }
                    //    },
                    //    "symbol": {
                    //        "color": [0, 255, 0, 64],
                    //        "outline": {
                    //            "color": [0, 0, 0, 255],
                    //            "width": 1,
                    //            "type": "esriSLS",
                    //            "style": "esriSLSSolid"
                    //        },
                    //        "type": "esriSFS",
                    //        "style": "esriSFSSolid"
                    //    },
                    //    "attributes": {
                    //        "District": "teste",
                    //        "CriminalIndex": 1
                    //    },
                    //    "infoTemplate": info
                    //};
                    //var gra = new Graphic(myPolygon);
                    //map.graphics.add(gra);
                }

                function computeZonalStats(evtObj) {
                    var geometry = evtObj.geometry;
                    /*After user draws shape on map using the draw toolbar compute the zonal*/
                    map.showZoomSlider();
                    map.graphics.clear();

                    var symbol = new SimpleFillSymbol("none", new SimpleLineSymbol("dashdot", new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25]));
                    var graphic = new Graphic(geometry, symbol);

                    map.graphics.add(graphic);
                    toolbar.deactivate();
                    
                    /*Verify objs*/
                    GisMapConfiguration.displaySchoolsInSelectedArea(geometry.rings[0]);
                    /*down here is the calc of population*/
                    var features = [];
                    features.push(graphic);

                    var featureSet = new FeatureSet();
                    featureSet.features = features;

                    var params = { "inputPoly": featureSet };

                    gp.execute(params);
                }

                function computeValues(eventObj) {
                    var results = eventObj.results;
                    //var content = string.substitute("<h4>The population in the user defined polygon is ${number:dojo.number.format}.</h4>", { number: results[0].value.features[0].attributes.SUM });

                    var populationNumber = string.substitute("${number:dojo.number.format}", { number: results[0].value.features[0].attributes.SUM });
                    $("#lbPopulationNumer").text(populationNumber);
                    //registry.byId("dialog1").setContent(content);
                    //registry.byId("dialog1").show();
                }
                
                function processGpError(eventObj) {
                    console.log(eventObj.error);
                    $("#lbErrorResponse").text("Problemas ao verificar população.");
                    $("#reponseErrorMessage").show();
                }

            })
    },

    setEventListeners: function () {
        $("#btnAlertClose").on("click", function () {
            $("#reponseErrorMessage").hide();
        });

        $("#btnLimpar").on("click", function () {
            GisMapConfiguration.clearResultFields();
        });
    },

    clearResponseMessage: function(){
        $("#reponseErrorMessage").hide();
    },

    clearResultFields: function () {
        $("#lbPopulationNumer").text("");
        $("#lbSchools").text("");
        $("#lbCops").text("");
        $("#copsContent").empty();
        $("#schoolsContent").empty();
        $("#lbCrimeIndex").empty();
        $("#lbCrimeIndex").text("");
        $("#lbCrimeDistrict").text("");
        GisMapConfiguration.hideDivs();
    },

    hideDivs: function(){
        $("#copsContent").hide();
        $("#schoolsContent").hide();
    },

    displaySchoolsInSelectedArea: function(coords){
        var objsToDisplay = PointsObjectsConfiguration.getObjsInSelectedArea(coords);
        if (objsToDisplay.reqStatus != 500) {
            if (objsToDisplay.schools.length > 0) {
                $("#lbSchools").text("");
                $("#schoolsContent").show();
                GisMapConfiguration.mountList(objsToDisplay.schools, $("#schoolsContent"));
            } else {
                $("#schoolsContent").empty();
                $("#schoolsContent").hide();
                $("#lbSchools").text("Nenhuma escola na área selecionada");
            }

            if (objsToDisplay.cops.length > 0) {
                $("#lbCops").text("");
                $("#copsContent").show();
                GisMapConfiguration.mountList(objsToDisplay.cops, $("#copsContent"));
            } else {
                $("#copsContent").empty();
                $("#copsContent").hide();
                $("#lbCops").text("Nenhum posto policial na área selecionada");
            }

            if (objsToDisplay.criminalIndex !== null) {
                $("#lbCrimeIndex").text(objsToDisplay.criminalIndex.CriminalIndex);
                $("#lbCrimeDistrict").text(objsToDisplay.criminalIndex.Districts);
            } else {
                $("#lbCrimeIndex").text("Praticamente Inofencivo");
                $("#lbCrimeDistrict").text("");
            }
        }
    },

    mountList: function (list, component) {
        component.empty();
        list.forEach(function (item) {
            var li = '<li><a href="#">' + item.Name + '</a></li><hr />';
            component.append(li);
        });
    }

});

var GisMapConfiguration = new GisMapClass();