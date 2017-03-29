(function (app) {
    'use strict';

    app.controller('downtimeEditCtrl', downtimeEditCtrl);

    downtimeEditCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', '$rootScope'];

    function downtimeEditCtrl($scope, $location, $routeParams, apiService, notificationService,$rootScope) {
        $scope.pageClass = 'page-downtimes';
        $scope.downtime = {};
        $scope.wsystems = [];
        $scope.loadingDowntime = true;
        $scope.isReadOnly = false;
        $scope.UpdateDowntime = UpdateDowntime;
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
            $("#date_picker").change(function () {
                calculateMins();
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
            $("#date_picker2").change(function () {
                calculateMins();
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

            $("#hoursdown").val(minDiff);
            var event1 = document.createEvent("HTMLEvents");
            event1.initEvent('change', true, true);
            document.getElementById('hoursdown').dispatchEvent(event1);
            document.getElementById('lostLabel').innerHTML = "Time Lost: " + diffDays + "days " + diffHrs + "hrs " + diffMins + "mins";
        }

        function loadDowntime() {

            $scope.loadingDowntime = true;

            apiService.get('/api/downtimes/details/' + $routeParams.id, null,
            downtimeLoadCompleted,
            downtimeLoadFailed);
        }

        function downtimeLoadCompleted(result) {
            $scope.downtime = result.data;
            $scope.loadingDowntime = false;

            loadWsystems();
        }

        function downtimeLoadFailed(response) {
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

        function UpdateDowntime() {
            
            UpdateDowntimeModel();
        }

        function UpdateDowntimeModel() {
            apiService.post('/api/downtimes/update', $scope.downtime,
            updateDowntimeSucceded,
            updateDowntimeFailed);
        }

        function updateDowntimeSucceded(response) {
            console.log(response);
            notificationService.displaySuccess($scope.downtime.CurrentDate + ' has been updated');
            $scope.downtime = response.data;
            redirectToDetails();
        }

        function updateDowntimeFailed(response) {
            notificationService.displayError(response);
        }

        function openDatePicker($event, dpicker) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker[dpicker] = true;
        };

        function redirectToDetails() {
            $location.url('downtimes/' + $scope.downtime.ID);
        }

        loadDowntime();
    }

})(angular.module('gwcltdApp'));