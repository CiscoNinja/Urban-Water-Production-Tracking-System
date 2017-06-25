(function (app) {
    'use strict';

    app.controller('gwclregionsAddCtrl', gwclregionsAddCtrl);

    gwclregionsAddCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService'];

    function gwclregionsAddCtrl($scope, $location, $rootScope, apiService) {

        $scope.newRegion = {};
        $scope.areas = [];

        $scope.AddRegion = AddRegion;

        $scope.submission = {
            successMessages: [''],
            errorMessages: ['']
        };

        function AddRegion() {
            apiService.post('./api/gwclregions/add', $scope.newRegion,
           addRegionSucceded,
           addRegionFailed);
        }

        function loadAreas() {
            apiService.get('./api/gwclareas/loadareas', null,
            areasLoadCompleted,
            areasLoadFailed);
        }

        function areasLoadCompleted(response) {
            $scope.areas = response.data;
        }

        function areasLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function addRegionSucceded(response) {
            //$scope.submission.errorMessages = ['Submition errors will appear here.'];
            console.log(response);
            var gwclareaAdded = response.data;
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newRegion.Name + ' has been successfully added');
            $scope.newRegion = {};
        }

        function addRegionFailed(response) {
            console.log(response);
            if (response.status == '400')
                $scope.submission.errorMessages = 'Error: ' + response.data;
            else
                $scope.submission.errorMessages = 'Error: enter appropriate values/' + response.statusText;
        }

        loadAreas();
    }

})(angular.module('gwcltdApp'));