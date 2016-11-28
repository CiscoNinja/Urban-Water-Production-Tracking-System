(function (app) {
    'use strict';

    app.controller('productionEditCtrl', productionEditCtrl);

    productionEditCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService'];

    function productionEditCtrl($scope, $location, $routeParams, apiService, notificationService) {
        $scope.pageClass = 'page-productions';
        $scope.production = {};
        $scope.wsystems = [];
        $scope.options = [];
        $scope.optiontypes = [];
        $scope.loadingProduction = true;
        $scope.isReadOnly = false;
        $scope.UpdateProduction = UpdateProduction;
        $scope.openDatePicker = openDatePicker;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker.todayDate = false;
        $scope.datepicker.dtRecord = false;

        function loadProduction() {

            $scope.loadingProduction = true;

            apiService.get('/api/productions/details/' + $routeParams.id, null,
            productionLoadCompleted,
            productionLoadFailed);
        }

        function productionLoadCompleted(result) {
            $scope.production = result.data;
            $scope.loadingProduction = false;

            loadOptions();
            loadWsystems();
            loadOptionTypes();
        }

        function productionLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function loadOptions() {
            apiService.get('/api/options/', null,
            optionsLoadCompleted,
            optionsLoadFailed);
        }

        function optionsLoadCompleted(response) {
            $scope.options = response.data;
        }

        function optionsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function loadOptionTypes() {
            apiService.get('/api/optiontypes/', null,
            optiontypesLoadCompleted,
            optiontypesLoadFailed);
        }

        function optiontypesLoadCompleted(response) {
            $scope.optiontypes = response.data;
        }

        function optiontypesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function loadWsystems() {
            apiService.get('/api/wsystems/', null,
            wsystemsLoadCompleted,
            wsystemsLoadFailed);
        }

        function wsystemsLoadCompleted(response) {
            $scope.wsystems = response.data;
        }

        function wsystemsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function UpdateProduction() {
            
            UpdateProductionModel();
        }

        function UpdateProductionModel() {
            apiService.post('/api/productions/update', $scope.production,
            updateProductionSucceded,
            updateProductionFailed);
        }

        function updateProductionSucceded(response) {
            console.log(response);
            notificationService.displaySuccess($scope.production.DayToRecord + ' has been updated');
            $scope.production = response.data;
        }

        function updateProductionFailed(response) {
            notificationService.displayError(response);
        }

        function openDatePicker($event, dpicker) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker[dpicker] = true;
        };

        loadProduction();
    }

})(angular.module('gwcltdApp'));