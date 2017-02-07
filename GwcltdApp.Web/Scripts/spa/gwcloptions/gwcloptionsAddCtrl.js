(function (app) {
    'use strict';

    app.controller('gwcloptionsAddCtrl', gwcloptionsAddCtrl);

    gwcloptionsAddCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService'];

    function gwcloptionsAddCtrl($scope, $location, $rootScope, apiService) {

        $scope.newOption = {};
        $scope.dataoptions = [{ "Name": "Production" }, { "Name": "Downtime" }];

        $scope.AddOption = AddOption;

        $scope.submission = {
            successMessages: ['Successfull submission will appear here.'],
            errorMessages: ['Submition errors will appear here.']
        };

        function AddOption() {
            apiService.post('/api/gwcloptions/add', $scope.newOption,
           addOptionSucceded,
           addOptionFailed);
        }

        function addOptionSucceded(response) {
            $scope.submission.errorMessages = ['Submition errors will appear here.'];
            console.log(response);
            var gwcloptionAdded = response.data;
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newOption.Name + ' has been successfully added');
            $scope.newOption = {};
        }

        function addOptionFailed(response) {
            console.log(response);
            if (response.status == '400')
                $scope.submission.errorMessages = response.data;
            else
                $scope.submission.errorMessages = response.statusText;
        }
    }

})(angular.module('gwcltdApp'));