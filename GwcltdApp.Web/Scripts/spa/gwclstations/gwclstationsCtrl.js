(function (app) {
    'use strict';

    app.controller('gwclstationsCtrl', gwclstationsCtrl);

    gwclstationsCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function gwclstationsCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-gwclsations';
        $scope.loadingGwclStations = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.GwclStations = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingGwclStations = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterGwclStations
                }
            };

            apiService.get('/api/gwclstations/', config,
            gwclstationsLoadCompleted,
            gwclstationsLoadFailed);
        }

        function openEditDialog(gwclstation) {
            $scope.EditedStation = gwclstation;
            $modal.open({
                templateUrl: 'scripts/spa/gwclstations/editGwclStationsModal.html',
                controller: 'gwclstationEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function gwclstationsLoadCompleted(result) {
            $scope.GwclStations = result.data;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingGwclStations = false;

            if ($scope.filterGwclStations && $scope.filterGwclStations.length) {
                notificationService.displayInfo(result.data.Items.length + ' no station found');
            }

        }

        function gwclstationsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterGwclStations = '';
            search();
        }

        $scope.search();
    }

})(angular.module('gwcltdApp'));