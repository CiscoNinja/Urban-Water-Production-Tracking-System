(function (app) {
    'use strict';

    app.controller('productionsCtrl', productionsCtrl);

    productionsCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function productionsCtrl($scope, apiService, notificationService) {
        $scope.pageClass = 'page-productions';
        $scope.loadingProductions = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        
        $scope.Productions = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingProductions = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 6,
                    filter: $scope.filterProductions
                }
            };

            apiService.get('/api/productions/', config,
            productionsLoadCompleted,
            productionsLoadFailed);
        }

        function productionsLoadCompleted(result) {
            $scope.Productions = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingProductions = false;

            if ($scope.filterProductions && $scope.filterProductions.length)
            {
                notificationService.displayInfo(result.data.Items.length + ' productions found');
            }
            
        }

        function productionsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterProductions = '';
            search();
        }

        $scope.search();
    }

})(angular.module('gwcltdApp'));