(function (app) {
    'use strict';

    app.controller('productionsCtrl', productionsCtrl);

    productionsCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function productionsCtrl($scope, apiService, notificationService) {
        $scope.pageClass = 'page-productions';
        $scope.loadingProductions = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.openDatePicker = openDatePicker;
        
        $scope.Productions = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker.leftDate = false;
        $scope.datepicker.rightDate = false;

        function search(page) {
            page = page || 0;

            $scope.loadingProductions = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 6,
                    //filter: $scope.filterProductions,
                    filter1: $scope.filterLeft,
                    filter2: $scope.filterRight
                }
            };

            apiService.get('/api/productions/'+1, config,
            productionsLoadCompleted,
            productionsLoadFailed);
        }

        function productionsLoadCompleted(result) {
            $scope.Productions = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingProductions = false;

            if ($scope.filterLeft || $scope.filterRight && $scope.filterLeft.length && $scope.filterRight.length)
            {
                notificationService.displayInfo(result.data.TotalCount + ' productions found');
            }
            
        }

        function openDatePicker($event, dpicker) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker[dpicker] = true;
        };

        function productionsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            //$scope.filterProductions = '';
            search();
        }

        $scope.search();
    }

})(angular.module('gwcltdApp'));