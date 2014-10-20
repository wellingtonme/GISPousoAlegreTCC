var PointsObjectsClass = $.Class.create({
    
    initialize: function () {
        
    },

    getSchoolsPoints: function () {
        var points;
        $.ajax({
            url: "/Map/GisMap/GetGraphicsLayersSchools",
            async: false,
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result != null)
                    points = result;
            }
        });

        return points;
    },

    getCopsPoints: function () {
        var points;
        $.ajax({
            url: "/Map/GisMap/GetGraphycsLayerCops",
            async: false,
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result != null)
                    points = result;
            }
        });

        return points;
    },
    
    getCriminalIndexPolygons: function(){
        var criminals = null;
        $.ajax({
            url: "/Map/GisMap/GetGraphicsCriminalIndex",
            async: false,
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result != null)
                    criminals = result;
            }
        });

        return criminals;
    },

    getObjsInSelectedArea: function (coords) {
        var objsToDisplay = null;
        if (coords != null && coords != undefined) {
            var params = coords;
            $.ajax({
                url: "/Map/GisMap/GetObjectsInSelectedArea?geometry=" + params,
                async: false,
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result != null)
                        objsToDisplay = result;
                },
                error: function (data) {
                    objsToDisplay = {reqStatus: 500}
                }
            });
        }

        return objsToDisplay;
    }

});

var PointsObjectsConfiguration = new PointsObjectsClass();