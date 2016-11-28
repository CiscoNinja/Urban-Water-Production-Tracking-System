(function (app) {
    'use strict';

    app.controller('summaryCtrl', summaryCtrl);

    summaryCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout'];

    function summaryCtrl($scope, $routeParams, apiService, notificationService, $timeout) {
        $scope.loadTable = loadTable;
        $scope.loadingStatistics = true;
        $scope.Summary = [];
        $scope.waterType = "";

        function loadTable() {

            $scope.loadingStatistics = true;
            $scope.waterType = $routeParams.id;
            apiService.get('/api/productions/summary/' + $routeParams.id, null,
            summaryLoadCompleted,
            summaryLoadFailed);
        }

        function loadTableDetails() {
            loadTable();
        }

        function summaryLoadCompleted(result) {
            $scope.Summary = result.data;

            $scope.loadingStatistics = false;
        }

        function summaryLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadTableDetails();
    }

})(angular.module('gwcltdApp'));