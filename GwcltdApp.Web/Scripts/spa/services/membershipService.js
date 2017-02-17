(function (app) {
    'use strict';

    app.factory('membershipService', membershipService);

    membershipService.$inject = ['apiService', 'notificationService', '$http', '$base64', '$cookieStore', '$rootScope'];

    function membershipService(apiService, notificationService, $http, $base64, $cookieStore, $rootScope) {

        var service = {
            login: login,
            register: register,
            saveCredentials: saveCredentials,
            //loadUserStation: loadUserStation,
            loadWsystems: loadWsystems,
            removeCredentials: removeCredentials,
            isUserLoggedIn: isUserLoggedIn
        }

        function loadWsystems(completed) {
            apiService.get('/api/gwclsystems/loadsystems/' + $rootScope.repository.loggedUser.stationid, null,
            completed,
            wsystemsLoadFailed);
        }

        function wsystemsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        //function loadUserStation(user, completed) {
        //    apiService.get('/api/account/userstation/' + user.username, user,
        //    completed,
        //    stationLoadFailed);
        //}

        //function stationLoadFailed(response) {
        //    notificationService.displayError(response.data);
        //}

        function login(user, completed) {
            apiService.post('/api/account/authenticate', user,
            completed,
            loginFailed);
        }

        function register(user, completed) {
            apiService.post('/api/account/register', user,
            completed,
            registrationFailed);
        }

        function saveCredentials(user, userstation) {
            var membershipData = $base64.encode(user.username + ':' + user.password);

            $rootScope.repository = {
                loggedUser: {
                    username: user.username,
                    stationid: userstation.GwclStationId,
                    stationname: userstation.GwclStation,
                    regionname: userstation.GwclRegion,
                    authdata: membershipData
                    //log other user parameters here
                }
            };
            
            $http.defaults.headers.common['Authorization'] = 'Basic ' + membershipData;
            $cookieStore.put('repository', $rootScope.repository);
        }

        function removeCredentials() {
            $rootScope.repository = {};
            $cookieStore.remove('repository');
            $http.defaults.headers.common.Authorization = '';
        }

        function loginFailed(response) {
            notificationService.displayError(response.data);
        }

        function registrationFailed(response) {

            notificationService.displayError('Registration failed. Try again.');
        }

        function isUserLoggedIn() {
            return $rootScope.repository.loggedUser != null;
        }

        return service;
    }



})(angular.module('common.core'));