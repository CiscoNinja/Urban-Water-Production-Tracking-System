(function (app) {
    'use strict';

    app.controller('gwclstationsAddCtrl', gwclstationsAddCtrl);

    gwclstationsAddCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService'];

    function gwclstationsAddCtrl($scope, $location, $rootScope, apiService) {

        $scope.newStation = {};
        $scope.regions = [];

        $scope.AddStation = AddStation;

        $scope.submission = {
            successMessages: [''],
            errorMessages: ['']
        };

        function AddStation() {
            apiService.post('./api/gwclstations/add', $scope.newStation,
           addStationSucceded,
           addStationFailed);
        }

        function loadRegions() {
            apiService.get('./api/gwclregions/loadregions', null,
            regionsLoadCompleted,
            regionsLoadFailed);
        }

        function regionsLoadCompleted(response) {
            $scope.regions = response.data;
        }

        function regionsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function addStationSucceded(response) {
            //$scope.submission.errorMessages = ['Submition errors will appear here.'];
            console.log(response);
            var gwclstationAdded = response.data;
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newStation.Name + ' has been successfully added');
            $scope.newStation = {};
        }

        function addStationFailed(response) {
            console.log(response);
            if (response.status == '400')
                $scope.submission.errorMessages = 'Error: ' + response.data;
            else
                $scope.submission.errorMessages = 'Error: enter appropriate values/' + response.statusText;
        }

        loadRegions();
    }

})(angular.module('gwcltdApp'));