'use strict';
app.factory('getset', function () {
    //$http, ngAuthSettings
    var tempdata = [];
    var data = {};
    return data = {
        Getdata: function () {
            return tempdata;
        },
        Setdata: function (data1) {
            tempdata = data1;
        },
        
        reset: function () {
            tempdata = {};
        }
    };

})