(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope','apiService', 'notificationService'];

    function indexCtrl($scope, apiService, notificationService) {
        $scope.pageClass = 'page-home';
        $scope.loadingProductions = true;
        $scope.loadingSummary = true;
        //$scope.loadingOptions = true;
        $scope.isReadOnly = true;

        $scope.latestProductions = [];
        $scope.loadData = loadData;

        function loadData() {
            apiService.get('/api/productions/latest', null,
                        productionsLoadCompleted,
                        productionsLoadFailed);

            apiService.get("/api/productions/latest", null,
                summaryLoadCompleted,
                summaryLoadFailed);
        }

        function productionsLoadCompleted(result) {
            $scope.latestProductions = result.data;
            $scope.loadingProductions = false;
        }

        function summaryLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function productionsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function summaryLoadCompleted(result) {
            var summary = result.data;
            Morris.Bar({
                element: "summary-bar",
                data: summary,
                xkey: "DayToRecord",
                ykeys: ["DailyActual"],
                labels: ["Daily Actual"],
                barRatio: 0.4,
                xLabelAngle: 55,
                //ymin: 1000,
                //numLines: 10,
                hideHover: "auto",
                resize: 'true'
            });
            //.on('click', function (i, row) {
            //    $location.path('/genres/' + row.ID);
            //    $scope.$apply();
            //});

            $scope.loadingSummary = false;
        }

        loadData();
    }

})(angular.module('gwcltdApp'));