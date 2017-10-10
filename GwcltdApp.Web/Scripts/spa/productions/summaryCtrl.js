(function (app) {
    'use strict';

    app.controller('summaryCtrl', summaryCtrl);

    summaryCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

    function summaryCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
        $scope.loadingStatistics = false;
        $scope.waterType = "";

        $(document).on('click', '.mybtn', function (event) {
            event.preventDefault();
            $('.mybtn').prop('disabled', true);
            var $this = $(this);
            $scope.waterType = $(this).text();
            $scope.loadingStatistics = true;
            apiService.get('./api/productions/summary/'+ $rootScope.repository.loggedUser.stationid + '/' + $scope.waterType, null,
            summaryLoadCompleted,
            summaryLoadFailed);
        });

        function summaryLoadCompleted(result) {
            $scope.Summary = result.data;
            $scope.loadingStatistics = false;
            var table = new KingTable({
                element: document.getElementById("my-table"),
                data: $scope.Summary,
                columns: {
                    systemName: "SYSTEM",
                    Jan: "JAN",
                    Feb: "FEB",
                    March: "MAR",
                    April: "APR",
                    May: "MAY",
                    June: "JUN",
                    July: "JUL",
                    Aug: "AUG",
                    Sept: "SEP",
                    Oct: "OCT",
                    Nov: "NOV",
                    Dec: "DEC",
                },
                page: 1,
                size: 200,
                sortBy: "systemName asc",
                collectionName: $scope.waterType,
                excelWorkbookName: $scope.waterType
            });
            table.render();
            $('.mybtn').prop('disabled', false);
        }

        function summaryLoadFailed(response) {
            notificationService.displayError(response.data);
            $('.mybtn').prop('disabled', false);
        }
    }

})(angular.module('gwcltdApp'));