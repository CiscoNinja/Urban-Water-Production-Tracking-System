(function (app) {
    'use strict';

    app.controller('gwclareasCtrl', gwclareasCtrl);

    gwclareasCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function gwclareasCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-gwclareas';
        $scope.loadingGwclAreas = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.GwclAreas = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingGwclAreas = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterGwclAreas
                }
            };

            apiService.get('/api/gwclareas/', config,
            gwclareasLoadCompleted,
            gwclareasLoadFailed);
        }

        function openEditDialog(gwclarea) {
            $scope.EditedWArea = gwclarea;
            $modal.open({
                templateUrl: 'scripts/spa/gwclareas/editGwclAreasModal.html',
                controller: 'gwclareaEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function gwclareasLoadCompleted(result) {
            $scope.GwclAreas = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingGwclAreas = false;

            if ($scope.filterGwclAreas && $scope.filterGwclAreas.length) {
                notificationService.displayInfo(result.data.Items.length + ' no area found');
            }

        }

        function gwclareasLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterGwclAreas = '';
            search();
        }

        $scope.search();
    }

})(angular.module('gwcltdApp'));