(function (app) {
    'use strict';

    app.controller('productionAddCtrl', productionAddCtrl);

    productionAddCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', '$rootScope'];

    function productionAddCtrl($scope, $location, $routeParams, apiService, notificationService,$rootScope) {

        $scope.pageClass = 'page-productions';
        $scope.production = { WSystemId: 1, OptionId: 1, OptionTypeId: 1, GwclStationId: $rootScope.repository.loggedUser.stationid };

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
            apiService.get('./api/gwclsystems/loadsystems/' + $rootScope.repository.loggedUser.stationid, null,
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
            apiService.post('./api/hrlyproductions/add', $scope.production,
            addProductionSucceded,
            addProductionFailed);
        }

        function addProductionSucceded(response) {
            notificationService.displaySuccess($scope.production.DayToRecord + ' has been submitted');
            $scope.production = response.data;
            redirectToDetails();
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

        function redirectToDetails() {
            $location.url('productions/' + $scope.production.ID);
        }

        loadOptions();
        loadWsystems();
        loadOptionTypes();
    }

})(angular.module('gwcltdApp'));