(function (app) {
    'use strict';

    app.controller('masterChartsCtrl', masterChartsCtrl);

    masterChartsCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

    function masterChartsCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
        $scope.loadWsystems = loadWsystems;
        $scope.msChart = { ID: $rootScope.repository.loggedUser.stationid }
        $scope.emptyStatistics = false;
        $scope.wSystems = [];
        $scope.Charts = [];
        $scope.waterOType = "";
        $scope.listOption = { ID: 1000 };
        var yearList = [];
        var myChart;

        function loadListOptions() {
            yearList.push({ name: "All", optio: 1000 })
            for (var i = 2012 ; i < 2050; i++) {
                yearList.push({ name: i, optio: i })
            }
            $scope.yearOptions = yearList;
        }


        $(document).on('click', '.mycbtn', function (event) {
            loadWsystems();
            $("#summary-chat").empty()
            event.preventDefault();
            var $this = $(this);
            $scope.waterOType = $(this).text();

            $scope.loadingCharts = true;
            apiService.get('./api/productions/charts/' + $scope.msChart.ID + '/' + $scope.listOption.ID + '/' + $scope.waterOType, null,
            chartsLoadCompleted,
            chartsLoadFailed);
        });

        function loadWsystems() {

            apiService.get('./api/gwclsystems/syscodes/' + $scope.msChart.ID, null,
            wsystemsLoadCompleted,
            wsystemsLoadFailed);
        }

        function wsystemsLoadCompleted(result) {
            $scope.wSystems = result.data;
        }

        function wsystemsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function loadStations() {

            apiService.get('./api/gwclstations/loadstations/', null,
            stationsLoadCompleted,
            stationsLoadFailed);
        }

        function stationsLoadCompleted(result) {
            $scope.wStations = result.data;
        }

        function stationsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function chartsLoadCompleted(result) {
            var chat = result.data;
            $scope.loadingCharts = false;
            if (!chat) {
                $scope.loadingStatistics = false;
                $scope.emptyStatistics = true;
            }
            else {
                myChart = Morris.Line({
                   element: "summary-chat",
                   data: chat,
                    xkey: 'month',
                    ykeys: $scope.wSystems,
                    labels: $scope.wSystems,
                    smooth: true,
                    hideHover: "auto",
                    resize: 'true',
                    parseTime: false
                });
            }
            //myChart.destroy();
        }

        function chartsLoadFailed(response) {
            notificationService.displayError(response.data);
        }
        
        loadStations();
        loadListOptions();
    }
})(angular.module('gwcltdApp'));