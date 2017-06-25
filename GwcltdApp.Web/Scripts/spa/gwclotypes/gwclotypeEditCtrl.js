(function (app) {
    'use strict';

    app.controller('gwclotypeEditCtrl', gwclotypeEditCtrl);

    gwclotypeEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function gwclotypeEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateType = updateType;

        function updateType()
        {
            console.log($scope.EditedType);
            apiService.post('./api/gwclotypes/update/', $scope.EditedType,
            updateTypeCompleted,
            updateTypeLoadFailed);
        }

        function updateTypeCompleted(response)
        {
            notificationService.displaySuccess($scope.EditedType.Name + ' has been updated');
            $scope.EditedType = {};
            $modalInstance.dismiss();
        }

        function updateTypeLoadFailed(response)
        {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }

    }

})(angular.module('gwcltdApp'));