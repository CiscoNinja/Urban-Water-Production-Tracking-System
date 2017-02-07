(function (app) {
    'use strict';

    app.controller('gwclotypesCtrl', gwclotypesCtrl);

    gwclotypesCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function gwclotypesCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-gwclotypes';
        $scope.loadingGwclTypes  = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.GwclTypes = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingGwclTypes = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterGwclTypes
                }
            };

            apiService.get('/api/gwclotypes/', config,
            gwclotypesLoadCompleted,
            gwclotypesLoadFailed);
        }

        function openEditDialog(gwclotype) {
            $scope.EditedType = gwclotype;
            $modal.open({
                templateUrl: 'scripts/spa/gwclotypes/editGwclTypesModal.html',
                controller: 'gwclotypeEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function gwclotypesLoadCompleted(result) {
            $scope.GwclTypes = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingGwclTypes = false;

            if ($scope.filterGwclTypes && $scope.filterGwclTypes.length) {
                notificationService.displayInfo(result.data.Items.length + ' type(s) found');
            }

        }

        function gwclotypesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterGwclTypes = '';
            search();
        }

        $scope.search();
    }

})(angular.module('gwcltdApp'));