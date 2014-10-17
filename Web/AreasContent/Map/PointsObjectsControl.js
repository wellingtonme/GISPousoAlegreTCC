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
    
    displayObjsInSelectedArea: function (coords) {
        
        var schoolsInArea = null;
        if (coords != null && coords != undefined) {
            /*convert coords from wkid 102100 to wkid 4326*/
            //coords = CoordinatesConfiguration.convertRings(coords, 5);
            var params = coords;
            $.ajax({
                url: "/Map/GisMap/GetSchoolInSelectedArea?geometry="+params,
                async: false,
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result != null)
                        points = result;
                }
            });
        }
    }

});

var PointsObjectsConfiguration = new PointsObjectsClass();