(function (app) {
    'use strict';

    app.controller('gwclareaEditCtrl', gwclareaEditCtrl);

    gwclareaEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function gwclareaEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateWArea = updateWArea;

        function updateWArea()
        {
            console.log($scope.EditedWArea);
            apiService.post('./api/gwclareas/update/', $scope.EditedWArea,
            updateWAreaCompleted,
            updateWAreaLoadFailed);
        }

        function updateWAreaCompleted(response)
        {
            notificationService.displaySuccess($scope.EditedWArea.Name + ' with code ' + $scope.EditedWArea.Code + ' has been updated');
            $scope.EditedWArea = {};
            $modalInstance.dismiss();
        }

        function updateWAreaLoadFailed(response)
        {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }

    }

})(angular.module('gwcltdApp'));