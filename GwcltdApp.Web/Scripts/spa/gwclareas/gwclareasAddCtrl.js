(function (app) {
    'use strict';

    app.controller('gwclareasAddCtrl', gwclareasAddCtrl);

    gwclareasAddCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService'];

    function gwclareasAddCtrl($scope, $location, $rootScope, apiService) {

        $scope.newWArea = {};

        $scope.AddWArea = AddWArea;

        $scope.submission = {
            successMessages: [''],
            errorMessages: ['']
        };

        function AddWArea() {
            apiService.post('./api/gwclareas/add', $scope.newWArea,
           addWAreaSucceded,
           addWAreaFailed);
        }

        function addWAreaSucceded(response) {
            //$scope.submission.errorMessages = ['Submition errors will appear here.'];
            console.log(response);
            var gwclareaAdded = response.data;
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newWArea.Name + ' has been successfully added');
            $scope.newWArea = {};
        }

        function addWAreaFailed(response) {
            console.log(response);
            if (response.status == '400')
                $scope.submission.errorMessages = 'Error: ' + response.data;
            else
                $scope.submission.errorMessages = 'Error: enter appropriate values/' + response.statusText;
        }
    }

})(angular.module('gwcltdApp'));