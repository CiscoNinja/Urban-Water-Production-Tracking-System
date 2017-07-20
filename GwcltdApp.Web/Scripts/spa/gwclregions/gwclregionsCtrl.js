(function (app) {
    'use strict';

    app.controller('gwclregionsCtrl', gwclregionsCtrl);

    gwclregionsCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function gwclregionsCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-gwclregions';
        $scope.loadingGwclRegions = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.GwclRegions = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingGwclRegions = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterGwclRegions
                }
            };

            apiService.get('./api/gwclregions/', config,
            gwclregionsLoadCompleted,
            gwclregionsLoadFailed);
        }

        function openEditDialog(gwclregion) {
            $scope.EditedRegion = gwclregion;
            $modal.open({
                templateUrl: './Scripts/spa/gwclregions/editGwclRegionsModal.html',
                controller: 'gwclregionEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function gwclregionsLoadCompleted(result) {
            $scope.GwclRegions = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingGwclRegions = false;

            if ($scope.filterGwclRegions && $scope.filterGwclRegions.length) {
                notificationService.displayInfo(result.data.Items.length + ' item(s) found');
            }

        }

        function gwclregionsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterGwclRegions = '';
            search();
        }

        $scope.search();
    }

})(angular.module('gwcltdApp'));