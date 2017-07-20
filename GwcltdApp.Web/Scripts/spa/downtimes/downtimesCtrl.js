(function (app) {
    'use strict';

    app.controller('downtimesCtrl', downtimesCtrl);

    downtimesCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function downtimesCtrl($scope, apiService, notificationService) {
        $scope.pageClass = 'page-downtimes';
        $scope.loadingDowntimes = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.openDatePicker = openDatePicker;
        
        $scope.Downtimes = [];

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

            $scope.loadingDowntimes = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 6,
                    //filter: $scope.filterDowntimes,
                    filter1: $scope.filterLeft,
                    filter2: $scope.filterRight
                }
            };

            apiService.get('./api/downtimes/' + 1, config,
            downtimesLoadCompleted,
            downtimesLoadFailed);
        }

        function downtimesLoadCompleted(result) {
            $scope.Downtimes = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingDowntimes = false;

            if ($scope.filterLeft || $scope.filterRight && $scope.filterLeft.length && $scope.filterRight.length)
            {
                notificationService.displayInfo(result.data.TotalCount + ' item(s) found');
            }
            
        }

        function openDatePicker($event, dpicker) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker[dpicker] = true;
        };

        function downtimesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            //$scope.filterProductions = '';
            search();
        }

        $scope.search();
    }

})(angular.module('gwcltdApp'));