(function (app) {
    'use strict';

    app.controller('summaryCtrl', summaryCtrl);

    summaryCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

    function summaryCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
        $scope.loadTable = loadTable;
        $scope.loadingStatistics = true;
        $scope.emptyStatistics = false;
        $scope.Summary = [];
        $scope.waterType = "";

        function loadTable() {
            $(document).on('click', '.mybtn', function (event) {
                event.preventDefault();
                var $this = $(this);
                $scope.waterType = $(this).text();
            });
            $scope.loadingStatistics = true;
            apiService.get('./api/productions/summary/'+ $rootScope.repository.loggedUser.stationid + '/' + $routeParams.id, null,
            summaryLoadCompleted,
            summaryLoadFailed);
        }

        function loadTableDetails() {
            loadTable();
        }

        function summaryLoadCompleted(result) {
            $scope.Summary = result.data;
            $scope.loadingStatistics = false;
            if (!$scope.Summary)
            {
                $scope.loadingStatistics = false;
                $scope.emptyStatistics = true;
            }
        }

        function summaryLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadTableDetails();
    }

})(angular.module('gwcltdApp'));