(function (app) {
    'use strict';

    app.controller('gwclregionEditCtrl', gwclregionEditCtrl);

    gwclregionEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function gwclregionEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateRegion = updateRegion;

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

        function updateRegion()
        {
            console.log($scope.EditedRegion);
            apiService.post('./api/gwclregions/update/', $scope.EditedRegion,
            updateRegionCompleted,
            updateRegionLoadFailed);
        }

        function updateRegionCompleted(response)
        {
            notificationService.displaySuccess($scope.EditedRegion.Name + ' with code ' + $scope.EditedRegion.Code + ' has been updated');
            $scope.EditedRegion = {};
            $modalInstance.dismiss();
        }

        function updateRegionLoadFailed(response)
        {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }
        loadAreas();
    }

})(angular.module('gwcltdApp'));