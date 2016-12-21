﻿(function (app) {
    'use strict';

    app.controller('chartsCtrl', chartsCtrl);

    chartsCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService', '$timeout'];

    function chartsCtrl($scope, $routeParams, apiService, notificationService, $timeout) {
        $scope.loadCharts = loadCharts;
        $scope.loadWsystems = loadWsystems;
        $scope.emptyStatistics = false;
        $scope.wSystems = [];
        $scope.Charts = [];
        $scope.waterOType = "";

        function loadCharts() {
             $(document).on('click', '.btn', function (event) {
                event.preventDefault();
                var $this = $(this);
                $scope.waterOType = $(this).text();
            });

            $scope.loadingCharts = true;
            apiService.get('/api/productions/charts/' +$routeParams.id, null,
            chartsLoadCompleted,
            chartsLoadFailed);
        }

        function loadWsystems() {

            apiService.get('/api/wsystems/syscodes', null,
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
        loadCharts();
        
    }

})(angular.module('gwcltdApp'));