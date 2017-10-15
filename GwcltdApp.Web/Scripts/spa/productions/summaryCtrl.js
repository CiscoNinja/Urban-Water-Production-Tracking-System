(function (app) {
    'use strict';

    app.controller('summaryCtrl', summaryCtrl);

    summaryCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

    function summaryCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
        $scope.loadingStatistics = false;
        $scope.waterType = "";
        $scope.buttonVal = "";
        $scope.excelName = "";
        $scope.listOption = { ID: 1000 };
        var yearList = [];

        function loadListOptions() {
            yearList.push({ name: "All", optio: 1000 })
            for (var i = 2016 ; i < 2050; i++) {
                yearList.push({ name: i, optio: i })
            }
            $scope.yearOptions = yearList;
        }

        $(document).on('click', '.mybtn', function (event) {
            event.preventDefault();
            $('.mybtn').prop('disabled', true);
            var $this = $(this);
            $scope.buttonVal = $(this).text()
            
            switch ($scope.buttonVal) {
                case "Treatment Plant Losses": {
                    $scope.waterType = "Treatment Plant Losses(%)";
                    $scope.excelName = "Plant Losses(%)";
                    break;
                }
                case "Treatment Plant Losses(m³m)": {
                    $scope.waterType = $scope.buttonVal;
                    $scope.excelName = "Plant Losses(m³m)";
                    break;
                }
                case "Treatment Plant Capacity": {
                    $scope.waterType = "Treatment Plant Capacity(%)";
                    $scope.excelName = "Plant Capacity(%)";
                    break;
                }
                default: {
                    $scope.waterType = $scope.buttonVal;
                    $scope.excelName = $scope.buttonVal;
                    break;
                }
            };

            $scope.loadingStatistics = true;
            apiService.get('./api/productions/summary/' + $rootScope.repository.loggedUser.stationid + '/' + $scope.listOption.ID + '/' + $scope.buttonVal, null,
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
                collectionName: $scope.waterType + ' ' + $rootScope.repository.loggedUser.stationname + ' ' + $scope.listOption.ID,
                excelWorkbookName: $scope.excelName + ' ' + $rootScope.repository.loggedUser.stationname + ' ' + $scope.listOption.ID
            });
            table.render();
            $('.mybtn').prop('disabled', false);
        }

        function summaryLoadFailed(response) {
            notificationService.displayError(response.data);
            $('.mybtn').prop('disabled', false);
        }
        loadListOptions();
    }
})(angular.module('gwcltdApp'));