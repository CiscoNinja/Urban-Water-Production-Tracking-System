(function (app) {
    'use strict';

    app.controller('downtimeDetailsCtrl', downtimeDetailsCtrl);

    downtimeDetailsCtrl.$inject = ['$scope', '$location', '$routeParams', '$modal', 'apiService', 'notificationService'];

    function downtimeDetailsCtrl($scope, $location, $routeParams, $modal, apiService, notificationService) {
        $scope.pageClass = 'page-downtimes';
        $scope.downtime = {};
        $scope.loadingDowntime = true;
        $scope.isReadOnly = true;
        $scope.clearSearch = clearSearch;

        function loadDowntime() {

            $scope.loadingDowntime = true;

            apiService.get('/api/downtimes/details/' + $routeParams.id, null,
            downtimeLoadCompleted,
            downtimeLoadFailed);
        }

        function loadDowntimeDetails() {
            loadDowntime();
        }

        function clearSearch()
        {
            $scope.filterDowntimes = '';
        }

        function downtimeLoadCompleted(result) {
            $scope.downtime = result.data;
            $scope.loadingDowntime = false;
        }

        function downtimeLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadDowntimeDetails();
    }

})(angular.module('gwcltdApp'));