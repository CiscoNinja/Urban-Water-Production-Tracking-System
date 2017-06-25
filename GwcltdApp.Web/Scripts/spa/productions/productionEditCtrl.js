(function (app) {
    'use strict';

    app.controller('productionEditCtrl', productionEditCtrl);

    productionEditCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', '$rootScope'];

    function productionEditCtrl($scope, $location, $routeParams, apiService, notificationService, $rootScope) {
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

        $(function () {
            $("#btndatetime1").click(function () {
                $('#date_picker').datetimepicker({
                    controlType: 'select',
                    timeFormat: 'hh:mm tt'
                }
                    ).datetimepicker("show")
            });
        });

        $(function () {
            $("#btndatetime2").click(function () {
                $('#date_picker2').datetimepicker({
                    controlType: 'select',
                    timeFormat: 'hh:mm tt'
                }
                    ).datetimepicker("show")
            });
        });

        function loadProduction() {

            $scope.loadingProduction = true;

            apiService.get('./api/hrlyproductions/details/' + $routeParams.id, null,
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
            apiService.get('./api/gwcloptions/loadoptions', null,
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
            apiService.get('./api/gwclotypes/loadoptypes', null,
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
            apiService.get('./api/gwclsystems/loadsystems/'+$rootScope.repository.loggedUser.stationid, null,
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
            apiService.post('./api/hrlyproductions/update', $scope.production,
            updateProductionSucceded,
            updateProductionFailed);
        }

        function updateProductionSucceded(response) {
            console.log(response);
            notificationService.displaySuccess($scope.production.DayToRecord + ' has been updated');
            $scope.production = response.data;
            redirectToDetails();
        }

        function updateProductionFailed(response) {
            notificationService.displayError(response);
        }

        function openDatePicker($event, dpicker) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker[dpicker] = true;
        };

        function redirectToDetails() {
            $location.url('productions/' + $scope.production.ID);
        }

        loadProduction();
    }

})(angular.module('gwcltdApp'));