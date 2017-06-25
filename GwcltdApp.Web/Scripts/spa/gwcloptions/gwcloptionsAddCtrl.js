(function (app) {
    'use strict';

    app.controller('gwcloptionsAddCtrl', gwcloptionsAddCtrl);

    gwcloptionsAddCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService'];

    function gwcloptionsAddCtrl($scope, $location, $rootScope, apiService) {

        $scope.newOption = {};
        $scope.dataoptions = [{ "Name": "Production" }, { "Name": "Downtime" }];

        $scope.AddOption = AddOption;

        $scope.submission = {
            successMessages: [''],
            errorMessages: ['']
        };

        function AddOption() {
            apiService.post('./api/gwcloptions/add', $scope.newOption,
           addOptionSucceded,
           addOptionFailed);
        }

        function addOptionSucceded(response) {
            //$scope.submission.errorMessages = ['Submition errors will appear here.'];
            console.log(response);
            var gwcloptionAdded = response.data;
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newOption.Name + ' has been successfully added');
            $scope.newOption = {};
        }

        function addOptionFailed(response) {
            console.log(response);
            if (response.status == '400')
                $scope.submission.errorMessages = 'Error: ' + response.data;
            else
                $scope.submission.errorMessages = 'Error: enter appropriate values/' + response.statusText;
        }
    }

})(angular.module('gwcltdApp'));