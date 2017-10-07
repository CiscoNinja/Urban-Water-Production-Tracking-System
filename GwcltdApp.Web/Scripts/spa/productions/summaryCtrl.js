(function (app) {
    'use strict';

    app.controller('summaryCtrl', summaryCtrl);

    summaryCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

    function summaryCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
        $scope.loadingStatistics = true;
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
                    systemName: "System Name",
                    Jan: "Jan",
                    Feb: "Feb",
                    March: "March",
                    April: "April",
                    May: "May",
                    June: "June",
                    July: "July",
                    Aug: "Aug",
                    Sept: "Sept",
                    Oct: "Oct",
                    Nov: "Nov",
                    Dec: "Dec",
                },
                page: 1,
                size: 200,
                sortBy: "systemName asc",
                fixed: true,
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