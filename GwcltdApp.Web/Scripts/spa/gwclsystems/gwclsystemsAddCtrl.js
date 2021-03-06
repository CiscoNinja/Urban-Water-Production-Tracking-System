(function (app) {
    'use strict';

    app.controller('gwclsystemsAddCtrl', gwclsystemsAddCtrl);

    gwclsystemsAddCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService'];

    function gwclsystemsAddCtrl($scope, $location, $rootScope, apiService) {

        $scope.newSystem = {};
        $scope.stations = [];

        $scope.AddSystem = AddSystem;

        $scope.submission = {
            successMessages: [''],
            errorMessages: ['']
        };

        function AddSystem() {
            apiService.post('./api/gwclsystems/add', $scope.newSystem,
           addSystemSucceded,
           addSystemFailed);
        }

        function loadStations() {
            apiService.get('./api/gwclstations/loadstations', null,
            stationsLoadCompleted,
            stationsLoadFailed);
        }

        function stationsLoadCompleted(response) {
            $scope.stations = response.data;
        }

        function stationsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function addSystemSucceded(response) {
            //$scope.submission.errorMessages = ['Submition errors will appear here.'];
            console.log(response);
            var gwclstationAdded = response.data;
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newSystem.Name + ' has been successfully added');
            $scope.newSystem = {};
        }

        function addSystemFailed(response) {
            console.log(response);
            if (response.status == '400')
                $scope.submission.errorMessages = 'Error: '+ response.data;
            else
                $scope.submission.errorMessages = 'Error: enter appropriate values/' + response.statusText;
        }

        loadStations();
    }

})(angular.module('gwcltdApp'));