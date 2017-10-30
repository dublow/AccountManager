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

        self.getDatas = function() {
            $.get("/Stat",
                function(response) {
                    self.sold(response.sold);
                    self.spents(response.spents);
                    self.receipts(response.receipts);
                    self.left(response.left);
                    self.createChart(response.anualSpentsReceipts);
                    self.lastOperations(response.lastOperations);
                    self.spentsByCategories(response.spentsByCategories);
                    self.allByDates(response.allByDateAndCategories.dates);

                    $.each(response.allByDateAndCategories.categories, function (index, item) {
                        var category = item.category;
                        $.each(item.amounts, function (i, it) {
                            self.amounts.push(it);
                        });
                        self.allByCategories.push({ category: category, amounts: self.amounts });
                    });

                    var array = $.map(response.spentsByCategories,
                        function(item) {
                            return {
                                label: item.category,
                                value: item.amount
                            }
                        });

                    self.createDonut(array);
                });
        };

        self.createChart = function(array) {
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

        self.createDonut = function (array) {
            new Morris.Donut({
                // ID of the element in which to draw the chart.
                element: 'morris-donut-chart',
                // Chart data records -- each entry in this array corresponds to a point on
                // the chart.
                data: array,
                // The name of the data record attribute that contains x-values.
                colors: ["#E0E4CD", "#6DD3E6", "#A9DBDB", "#F1843A", "#F8661D", "#E74970", "#532633", "#31DA9C", "#31ACE1"]
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