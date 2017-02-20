(function (app) {
    'use strict';

    app.controller('gwclsystemsCtrl', gwclsystemsCtrl);

    gwclsystemsCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService', '$rootScope'];

    function gwclsystemsCtrl($scope, $modal, apiService, notificationService, $rootScope) {

        $scope.pageClass = 'page-gwclsystems';
        $scope.loadingGwclSystems = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.GwclSystems = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingGwclSystems = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterGwclSystems
                }
            };

            apiService.get('/api/gwclsystems/', config,
            gwclsystemsLoadCompleted,
            gwclsystemsLoadFailed);
        }

        function openEditDialog(gwclsystem) {
            $scope.EditedSystem = gwclsystem;
            $modal.open({
                templateUrl: 'scripts/spa/gwclsystems/editGwclSystemsModal.html',
                controller: 'gwclsystemEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function gwclsystemsLoadCompleted(result) {
            $scope.GwclSystems = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingGwclSystems = false;

            if ($scope.filterGwclSystems && $scope.filterGwclSystems.length) {
                notificationService.displayInfo(result.data.Items.length + ' no system found');
            }

        }

        function gwclsystemsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterGwclSystems = '';
            search();
        }

        $scope.search();
    }

})(angular.module('gwcltdApp'));