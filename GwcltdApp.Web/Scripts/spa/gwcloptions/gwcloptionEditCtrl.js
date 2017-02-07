(function (app) {
    'use strict';

    app.controller('gwcloptionEditCtrl', gwcloptionEditCtrl);

    gwcloptionEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function gwcloptionEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateOption = updateOption;
        $scope.dataoptions = [{ "Name": "Production" }, { "Name": "Downtime" }];

        function updateOption() {
            console.log($scope.EditedOption);
            apiService.post('/api/gwcloptions/update/', $scope.EditedOption,
            updateOptionCompleted,
            updateOptionLoadFailed);
        }

        function updateOptionCompleted(response) {
            notificationService.displaySuccess($scope.EditedOption.Name + ' has been updated');
            $scope.EditedOption = {};
            $modalInstance.dismiss();
        }

        function updateOptionLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }

    }

})(angular.module('gwcltdApp'));