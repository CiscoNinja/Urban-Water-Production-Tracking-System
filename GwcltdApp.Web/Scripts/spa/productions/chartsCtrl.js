(function (app) {
    'use strict';

    app.controller('chartsCtrl', chartsCtrl);

    chartsCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout', '$rootScope'];

    function chartsCtrl($scope, $routeParams, apiService, notificationService, $timeout, $rootScope) {
        $scope.loadWsystems = loadWsystems;
        $scope.emptyStatistics = false;
        $scope.wSystems = [];
        $scope.Charts = [];
        $scope.waterOType = "";
        $scope.listOption = { ID: 1000 };
        var yearList = [];

        function loadListOptions() {
            yearList.push({ name: "All", optio: 1000 })
            for (var i = 2016 ; i < 2050; i++) {
                yearList.push({ name: i, optio: i })
            }
            $scope.yearOptions = yearList;
        }


        $(document).on('click', '.mycbtn', function (event) {
            $("#summary-chat").empty()
            event.preventDefault();
            var $this = $(this);
            $scope.waterOType = $(this).text();

            $scope.loadingCharts = true;
            apiService.get('./api/productions/charts/' + $rootScope.repository.loggedUser.stationid + '/' + $scope.listOption.ID + '/' + $scope.waterOType, null,
            chartsLoadCompleted,
            chartsLoadFailed);
        });

        function loadWsystems() {

            apiService.get('./api/gwclsystems/syscodes/' + $rootScope.repository.loggedUser.stationid, null,
            wsystemsLoadCompleted,
            wsystemsLoadFailed);
        }

        function wsystemsLoadCompleted(result) {
            $scope.wSystems = result.data;
        }

        function wsystemsLoadFailed(response) {
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
                Morris.Line({
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

        }

        function chartsLoadFailed(response) {
            notificationService.displayError(response.data);
        }
        loadWsystems();
        loadListOptions();
    }
})(angular.module('gwcltdApp'));