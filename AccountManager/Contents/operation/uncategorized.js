/// <reference path="../ko/knockout-3.4.2.debug.js" />
var Index = (function () {
    var ViewModel = function () {
        var self = this;
        self.operations = ko.observableArray([]);
        self.selectedCategoryId = ko.observable("");
        self.selectedOperationId = ko.observable("");
        self.selectedLibelle = ko.observable("");

        self.init = function() {
            $.get("/operation/getUncategorized",
                function(response) {
                    self.operations(response);
                });
        };

        self.openModal = function (current) {
            self.selectedLibelle(current.libelle);
            self.selectedOperationId(current.id);
            $('#myModal').modal('show');
            
        }

        self.saveCategory = function (current) {
            var model = {
                operationId: self.selectedOperationId(),
                categoryId: self.selectedCategoryId()[0]
            };

            $.post("/operation/addCategory", model,
                function (response) {
                    if (response) {
                        self.operations.remove(function (item) { return item.id === model.operationId });
                        $('#myModal').modal('hide');
                    }
                });
        }
    }

    return function () {
        var vm = new ViewModel();
        ko.applyBindings(vm);
        vm.init();
    }
})();

$(document).ready(function () {
    Index();
})