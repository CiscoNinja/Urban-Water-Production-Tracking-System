﻿//(function (app) {
//    'use strict';

//    app.controller('masterSummaryCtrl', masterSummaryCtrl);

//    masterSummaryCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

//    function masterSummaryCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
//        $scope.loadTable = loadTable;
//        $scope.emptyStatistics = false;
//        $scope.waterType = "";

//        $(document).on('click', '.mybtn', function (event) {
//            event.preventDefault();
//            var $this = $(this);
//            $scope.waterType = $(this).text();
//            loadTableDetails();
//        });

//        function loadTable() {
//            var table = new KingTable({
//                element: document.getElementById("my-table"),
//                url: "/api/productions/masterSummary/" + $scope.waterType + "",
//                sortBy: "systemName asc",
//                fixed: true,
//            });
//            table.render();
//        }

//        function loadTableDetails() {
//            loadTable();
//        }

       
//    }

//})(angular.module('gwcltdApp'));

(function (app) {
    'use strict';

    app.controller('masterSummaryCtrl', masterSummaryCtrl);

    masterSummaryCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

    function masterSummaryCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
        $scope.loadingStatistics = false;
        $scope.waterType = "";

        $(document).on('click', '.mymbtn', function (event) {
            event.preventDefault();
            $('.mymbtn').prop('disabled', true);
            var $this = $(this);
            $scope.waterType = $(this).text();
            $scope.loadingStatistics = true;
            apiService.get('./api/productions/masterSummary/' + $scope.waterType, null,
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
            $('.mymbtn').prop('disabled', false);
        }

        function summaryLoadFailed(response) {
            notificationService.displayError(response.data);
            $('.mymbtn').prop('disabled', false);
        }
    }

})(angular.module('gwcltdApp'));