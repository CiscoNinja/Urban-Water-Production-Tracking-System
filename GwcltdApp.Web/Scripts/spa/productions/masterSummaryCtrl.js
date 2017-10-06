(function (app) {
    'use strict';

    app.controller('masterSummaryCtrl', masterSummaryCtrl);

    masterSummaryCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

    function masterSummaryCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
        $scope.loadTable = loadTable;
        $scope.emptyStatistics = false;
        $scope.waterType = "";

        function loadTable() {
            $(document).on('click', '.mybtn', function (event) {
                event.preventDefault();
                var $this = $(this);
                $scope.waterType = $(this).text();
                KingTable.defaults.excelWorkbookName = "data";
                var table = new KingTable({
                    element: document.getElementById("my-table"),
                    url: "/api/productions/masterSummary/"+$scope.waterType+"",
                    fixed: true
                });
                table.render();
                table.setColumnsOrder;
            });
        }

        function loadTableDetails() {
            loadTable();
        }

        loadTableDetails();
    }

})(angular.module('gwcltdApp'));