/// <reference path="../ko/knockout-3.4.2.debug.js" />
var Index = (function() {
    var ViewModel = function () {
        var self = this;
        self.sold = ko.observable(0);
        self.spents = ko.observable(0);
        self.receipts = ko.observable(0);
        self.left = ko.observable(0);
        self.annualSpents = ko.observableArray([]);
        self.annualReceipts = ko.observableArray([]);
        self.lastOperations = ko.observableArray([]);
        self.spentsByCategories = ko.observableArray([]);
        self.allByDates = ko.observableArray([]);
        self.allByCategories = ko.observableArray([]);
        self.amounts = ko.observableArray([]);
        self.donut;
        self.donutSpentsReceipts;

        self.getDatas = function() {
            $.get("/Stat",
                function(response) {
                    self.sold(response.sold);
                    self.spents(response.spents);
                    self.receipts(response.receipts);
                    self.left(response.left);
                    self.chartAnualSpentsReceipts(response.anualSpentsReceipts);
                    self.lastOperations(response.lastOperations);
                    self.spentsByCategories(response.spentsByCategories);
                    self.donutSpentsReceipts([{ label: "Dépense", value: response.spents }, { label: "Recette", value: response.receipts }]);

                    var array = $.map(response.spentsByCategories,
                        function(item) {
                            return {
                                label: item.category,
                                value: item.amount
                            }
                        });

                    self.donutCategories(array);
                });
        };

        self.updateCategoriesByDate = function(month) {
            $.get("/updateCategoriesByDate/" + month,
                function (response) {
                    self.spentsByCategories(response);
                    var array = $.map(response,
                        function (item) {
                            return {
                                label: item.category,
                                value: item.amount
                            }
                        });

                    self.donut.setData(array);
                });
        };

        self.updatSpentsReceiptsByDate = function (month) {
            $.get("/updateSpentsReceiptsByDate/" + month,
                function (response) {
                    self.donutSpentsReceipts.setData([{ label: "Dépense", value: response.spents }, { label: "Recette", value: response.receipts }]);
                });
        };

        self.chartAnualSpentsReceipts = function(array) {
            new Morris.Bar({
                // ID of the element in which to draw the chart.
                element: 'morris-area-chart',
                // Chart data records -- each entry in this array corresponds to a point on
                // the chart.
                data: array,
                // The name of the data record attribute that contains x-values.
                xkey: 'date',
                // A list of names of data record attributes that contain y-values.
                ykeys: ['spents', 'receipts'],
                // Labels for the ykeys -- will be displayed when you hover over the
                // chart.
                labels: ['Dépense', 'Recette'],
                barColors: ['#D9534F', '#008000'],
                fillOpacity: 0.5
            });
        };

        self.donutCategories = function (array) {
            self.donut = new Morris.Donut({
                // ID of the element in which to draw the chart.
                element: 'morris-donut-chart',
                // Chart data records -- each entry in this array corresponds to a point on
                // the chart.
                data: array,
                // The name of the data record attribute that contains x-values.
                colors: ["#E0E4CD", "#6DD3E6", "#A9DBDB", "#F1843A", "#F8661D", "#E74970", "#532633", "#31DA9C", "#31ACE1"]
            });
        };

        self.donutSpentsReceipts = function (array) {
            self.donutSpentsReceipts = new Morris.Donut({
                // ID of the element in which to draw the chart.
                element: 'morris-donut-chart-spents-receipts',
                // Chart data records -- each entry in this array corresponds to a point on
                // the chart.
                data: array,
                // The name of the data record attribute that contains x-values.
                colors: ["#D75051", "#5FB860"]
            });
        };
    }

    return function () {
        var vm = new ViewModel();
        ko.applyBindings(vm);
        vm.getDatas();
    }
})();

$(document).ready(function() {
    Index();
})