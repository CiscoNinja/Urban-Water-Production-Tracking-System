(function (app) {
    'use strict';

    app.controller('excelDataCtrl', excelDataCtrl);

    excelDataCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', '$rootScope'];

    function excelDataCtrl($scope, $location, $routeParams, apiService, notificationService, $rootScope) {

        $scope.pageClass = 'page-productions';
        $scope.production = {
            WSystemId: 5, OptionId: 13, OptionTypeId: 17,
            GwclStationId: $rootScope.repository.loggedUser.stationid, Comment: "fetched from ecxel",
            TFPD: 0, FRPH: 0, FRPS: 0, NTFPD: 0, LOG: 0, DateCreated: "07/31/2017 01:00 am",
            DayToRecord: "07/31/2017 01:00 am", DailyActual: 0
        };

        $scope.wsystems = [];
        $scope.stations = [];
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

        function loadPicker1 () {
            $("#btndatetime1").click(function () {
                $('#date_picker').datetimepicker({
                    controlType: 'select',
                    timeFormat: 'hh:mm tt'
                }
                    ).datetimepicker("show")
            });
        }

        function loadPicker2 () {
            $("#btndatetime2").click(function () {
                $('#date_picker2').datetimepicker({
                    controlType: 'select',
                    timeFormat: 'hh:mm tt'
                }
                    ).datetimepicker("show")
            });
        }

        function getPath() {
            $('#fileName').on('change',function ()
            {
                var filePath = $(this).val();
                //$scope.fileName = filePath;
                $scope.production.filePath = filePath;
                //alert('The file "' + $scope.production.filePath + '" has been selected.');
            });
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
            apiService.get('./api/gwclsystems/loadsystems', null,
            wsystemsLoadCompleted,
            wsystemsLoadFailed);
        }

        function wsystemsLoadCompleted(response) {
            $scope.wsystems = response.data;
        }

        function wsystemsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function loadStations() {
            apiService.get('./api/gwclstations/loadstations', null,
            stationsLoadCompleted,
            stationsLoadFailed);
        }

        function stationsLoadCompleted(response) {
            $scope.stations = response.data;
        }

        function stationsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function AddProduction() {
            AddProductionModel();
        }

        function AddProductionModel() {
           
            apiService.post('./api/hrlyproductions/importExcel', $scope.production,
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
        loadStations();
        loadPicker1();
        loadPicker2();
        getPath();
    }

})(angular.module('gwcltdApp'));