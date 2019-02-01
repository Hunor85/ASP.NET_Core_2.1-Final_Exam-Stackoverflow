define(['jquery', 'knockout', 'dataService', 'postman','jqcloud'], function ($, ko, ds,postman) {

    return function (params) {
        var words = ko.observableArray([]);

        ds.getWords(function (data) {
            words(data);
            $('#cloud').jQCloud(words());


        });
        var generateCloud = function () {

            
        };
       
        return {
            words,
            generateCloud
           
        };

    };
});