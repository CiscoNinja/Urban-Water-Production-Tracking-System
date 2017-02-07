(function (app) {
    'use strict';

    app.controller('gwclotypesAddCtrl', gwclotypesAddCtrl);

    gwclotypesAddCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService'];

    function gwclotypesAddCtrl($scope, $location, $rootScope, apiService) {

        $scope.newType = {};

        $scope.AddType = AddType;

        $scope.submission = {
            successMessages: ['Successfull submission will appear here.'],
            errorMessages: ['Submition errors will appear here.']
        };

        function AddType() {
            apiService.post('/api/gwclotypes/add', $scope.newType,
           addTypeSucceded,
           addTypeFailed);
        }

        function addTypeSucceded(response) {
            $scope.submission.errorMessages = ['Submition errors will appear here.'];
            console.log(response);
            var gwclotypeAdded = response.data;
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newType.Name + ' has been successfully added');
            $scope.newType = {};
        }

        function addTypeFailed(response) {
            console.log(response);
            if (response.status == '400')
                $scope.submission.errorMessages = response.data;
            else
                $scope.submission.errorMessages = response.statusText;
        }
    }

})(angular.module('gwcltdApp'));