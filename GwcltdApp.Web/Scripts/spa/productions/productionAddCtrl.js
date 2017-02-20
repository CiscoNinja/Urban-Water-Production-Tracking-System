(function (app) {
    'use strict';

    app.controller('productionAddCtrl', productionAddCtrl);

    productionAddCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', '$rootScope'];

    function productionAddCtrl($scope, $location, $routeParams, apiService, notificationService,$rootScope) {

        $scope.pageClass = 'page-productions';
        $scope.production = { WSystemId: 1, OptionId: 1, OptionTypeId: 1 };

        $scope.wsystems = [];
        $scope.options = [];
        $scope.optiontypes = [];
        $scope.isReadOnly = false;
        $scope.AddProduction = AddProduction;
        $scope.openDatePicker = openDatePicker;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker.todayDate = false;
        $scope.datepicker.dtRecord = false;


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
            apiService.get('/api/gwclsystems/loadsystems/' + $rootScope.repository.loggedUser.stationid, null,
            wsystemsLoadCompleted,
            wsystemsLoadFailed);
        }

        function wsystemsLoadCompleted(response) {
            $scope.wsystems = response.data;
        }

        function wsystemsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function AddProduction() {
            AddProductionModel();
        }

        function AddProductionModel() {
            apiService.post('/api/productions/add', $scope.production,
            addProductionSucceded,
            addProductionFailed);
        }

        function addProductionSucceded(response) {
            notificationService.displaySuccess($scope.production.DayToRecord + ' has been submitted');
            $scope.production = response.data;
                redirectToEdit();
        }

        function addProductionFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function openDatePicker($event, dpicker) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker[dpicker] = true;
        }

        function redirectToEdit() {
            $location.url('productions/edit/' + $scope.production.ID);
        }

        loadOptions();
        loadWsystems();
        loadOptionTypes();
    }

})(angular.module('gwcltdApp'));