﻿(function (app) {
    'use strict';

    app.controller('masterSummaryCtrl', masterSummaryCtrl);

    masterSummaryCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

    function masterSummaryCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
        $scope.loadingStatistics = false;
        $scope.waterType = "";
        $scope.buttonVal = "";
        $scope.listOption = { ID: 1000 };
        var yearList = [];

        function loadListOptions() {
            yearList.push({ name: "All", optio: 1000 })
            for (var i = 2016 ; i < 2050; i++) {
                yearList.push({ name: i, optio: i })
            }
            $scope.yearOptions = yearList;
        }

        $(document).on('click', '.mymbtn', function (event) {
            event.preventDefault();
            $('.mymbtn').prop('disabled', true);
            var $this = $(this);
            $scope.buttonVal = $(this).text();

            switch ($scope.buttonVal) {
                case "Treatment Plant Losses": {
                    $scope.waterType = "Treatment Plant Losses(%)";
                    break;
                }
                case "Treatment Plant Capacity": {
                    $scope.waterType = "Treatment Plant Capacity(%)";
                    break;
                }
                default: {
                    $scope.waterType = $scope.buttonVal;
                    break;
                }
            };

            $scope.loadingStatistics = true;
            apiService.get('./api/productions/masterSummary/' + $scope.listOption.ID +'/'+ $scope.buttonVal, null,
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
            $('.mymbtn').prop('disabled', false);
        }

        function summaryLoadFailed(response) {
            notificationService.displayError(response.data);
            $('.mymbtn').prop('disabled', false);
        }
        loadListOptions();
    }
})(angular.module('gwcltdApp'));