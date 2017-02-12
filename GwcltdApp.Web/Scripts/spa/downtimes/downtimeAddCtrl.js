(function (app) {
    'use strict';

    app.controller('downtimeAddCtrl', downtimeAddCtrl);

    downtimeAddCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService'];

    function downtimeAddCtrl($scope, $location, $routeParams, apiService, notificationService) {

        $scope.pageClass = 'page-downtimes';
        $scope.downtime = { WSystemId: 1 };

        $scope.wsystems = [];
        $scope.isReadOnly = false;
        $scope.AddDowntime = AddDowntime;
        $scope.openDatePicker = openDatePicker;
        $scope.calculateMins = calculateMins;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker.currentdate = false;
        
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

        function calculateMins() {
            var timeStart = new Date(document.getElementById('date_picker').value);
            var timeEnd = new Date(document.getElementById('date_picker2').value);
            
            var hourDiff = timeEnd - timeStart; //in ms 

            var diffDays = Math.floor(hourDiff / 86400000); // days
            var diffHrs = Math.floor((hourDiff % 86400000) / 3600000); // hours
            var diffMins = Math.round(((hourDiff % 86400000) % 3600000) / 60000); // minutes

            var secDiff = hourDiff / 1000; //in s
            var minDiff = hourDiff / 60 / 1000; //in minutes
            var hDiff = hourDiff / 3600 / 1000; //in hours

            document.getElementById('hoursdown').value = minDiff;
            document.getElementById('lostLabel').innerHTML = "Time Lost: " + diffDays + "days " + diffHrs + "hrs " + diffMins + "mins";
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

        function AddDowntime() {
            AddDowntimeModel();
        }

        function AddDowntimeModel() {
            apiService.post('/api/downtimes/add', $scope.downtime,
            addDowntimeSucceded,
            addDowntimeFailed);
        }

        function addDowntimeSucceded(response) {
            notificationService.displaySuccess($scope.downtime.CurrentDate + ' has been submitted');
            $scope.downtime = response.data;
                redirectToEdit();
        }

        function addDowntimeFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function openDatePicker($event, dpicker) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker[dpicker] = true;
        }

        function redirectToEdit() {
            $location.url('downtimes/edit/' + $scope.downtime.ID);
        }

        loadWsystems();
    }

})(angular.module('gwcltdApp'));