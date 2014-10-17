var CoordinatesClass = $.Class.create({

    queueCount: null,
    convertedPoints: {},
    /*Pass -1 in fixed param to not format coords*/
    convertToLonLag: function (geometry, fixed, callback) {
        var pt = null;
        var countTime = 0;
        require([
            "esri/graphic",
            "esri/symbols/SimpleMarkerSymbol",
            "esri/geometry/Point",
            "esri/tasks/GeometryService",
            "esri/tasks/ProjectParameters",
            "esri/SpatialReference",
            "dojo/dom",
            "dojo/domReady!"
        ], function (Graphic, SimpleMarkerSymbol, Point, GeometryService, ProjectParameters, SpatialReference, dom) {
            
            var gsvc = new GeometryService("http://tasks.arcgisonline.com/ArcGIS/rest/services/Geometry/GeometryServer");
            //gsvc.on("execute-complete", incrementQueue);
            var outSR = new SpatialReference(4326);
            var params = new ProjectParameters();
            var point = new Point(geometry, new SpatialReference(102100));
            params.geometries = [point.normalize()];
            params.outSR = outSR;

            gsvc.project(params, function (projectedPoints) {
                convertedPoints.push(projectedPoints[0]);
                //if (fixed > -1) {
                //    pt.x = pt.x.toFixed(fixed);
                //    pt.y = pt.y.toFixed(fixed);
                //}
            });
            
        });
    },

    convertRings: function (rings, fixed, callback) {

        CoordinatesConfiguration.queueCount = 0;
        var ringsLength = rings.length;
        while (CoordinatesConfiguration.queueCount < ringsLength) {
            var currentIndexInQueue = CoordinatesConfiguration.queueCount;
            for (var index = 0; index < ringsLength; index++) {
                CoordinatesConfiguration.convertToLonLag(rings[index], fixed, callback);
                //while (currentIndexInQueue == CoordinatesConfiguration.queueCount) { };
                currentIndexInQueue++;
                rings[index][0] = pt.x;
                rings[index][1] = pt.y;
            }
        };

        callback(convertedPoints.convertedPoints);
        //var breakpoint;
        //require([
        //    "esri/graphic",
        //    "esri/symbols/SimpleMarkerSymbol",
        //    "esri/geometry/Point",
        //    "esri/tasks/GeometryService",
        //    "esri/tasks/ProjectParameters",
        //    "esri/SpatialReference",
        //    "dojo/dom",
        //    "dojo/domReady!"
        //], function (Graphic, SimpleMarkerSymbol, Point, GeometryService, ProjectParameters, SpatialReference, dom) {

        //    var gsvc = new GeometryService("http://tasks.arcgisonline.com/ArcGIS/rest/services/Geometry/GeometryServer");
        //    var outSR = new SpatialReference(4326);
        //    var params = new ProjectParameters();

            
        //    for (var x = 0; x < rings.length; x++) {
        //        var point = new Point(rings[x], new SpatialReference(102100));
        //        params.geometries = [point.normalize()];
        //        params.outSR = outSR;
        //        gsvc.project(params, function (projectedPoints) {
        //            rings[x][0] = projectedPoints[0].x.toFixed(fixed);
        //            rings[x][1] = projectedPoints[0].y.toFixed(fixed);
        //        });                
        //    }
            
        //});
        return rings;
    }

});

var CoordinatesConfiguration = new CoordinatesClass();