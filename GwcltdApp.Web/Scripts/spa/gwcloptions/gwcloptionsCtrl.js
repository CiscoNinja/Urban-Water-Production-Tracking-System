(function (app) {
    'use strict';

    app.controller('gwcloptionsCtrl', gwcloptionsCtrl);

    gwcloptionsCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function gwcloptionsCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-gwcloptions';
        $scope.loadingGwclOptions = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.GwclOptions = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingGwclOptions = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterGwclOptions
                }
            };

            apiService.get('/api/gwcloptions/', config,
            gwcloptionsLoadCompleted,
            gwcloptionsLoadFailed);
        }

        function openEditDialog(gwcloptions) {
            $scope.EditedOption = gwcloptions;
            $modal.open({
                templateUrl: 'scripts/spa/gwcloptions/editGwclOptionsModal.html',
                controller: 'gwcloptionEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function gwcloptionsLoadCompleted(result) {
            $scope.GwclOptions = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingGwclOptions = false;

            if ($scope.filterGwclOptions && $scope.filterGwclOptions.length) {
                notificationService.displayInfo(result.data.Items.length + ' no system found');
            }

        }

        function gwcloptionsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterGwclOptions = '';
            search();
        }

        $scope.search();
    }

})(angular.module('gwcltdApp'));