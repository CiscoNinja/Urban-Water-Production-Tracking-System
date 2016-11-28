(function (app) {
    'use strict';

    app.controller('productionDetailsCtrl', productionDetailsCtrl);

    productionDetailsCtrl.$inject = ['$scope', '$location', '$routeParams', '$modal', 'apiService', 'notificationService'];

    function productionDetailsCtrl($scope, $location, $routeParams, $modal, apiService, notificationService) {
        $scope.pageClass = 'page-productions';
        $scope.production = {};
        $scope.loadingProduction = true;
        $scope.isReadOnly = true;
        $scope.clearSearch = clearSearch;

        function loadProduction() {

            $scope.loadingProduction = true;

            apiService.get('/api/productions/details/' + $routeParams.id, null,
            productionLoadCompleted,
            productionLoadFailed);
        }

        function loadProductionDetails() {
            loadProduction();
        }

        function clearSearch()
        {
            $scope.filterRentals = '';
        }

        function productionLoadCompleted(result) {
            $scope.production = result.data;
            $scope.loadingProduction = false;
        }

        function productionLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadProductionDetails();
    }

})(angular.module('gwcltdApp'));