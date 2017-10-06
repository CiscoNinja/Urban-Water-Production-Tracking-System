(function () {
    'use strict';

    angular.module('gwcltdApp', ['common.core', 'common.ui'])
        .config(config)
        .run(run);

    config.$inject = ['$routeProvider'];
    function config($routeProvider) {
        $routeProvider
            .when("/", {
                templateUrl: "./Scripts/spa/home/index.html",
                controller: "indexCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/login", {
                templateUrl: "./Scripts/spa/account/login.html",
                controller: "loginCtrl"
            })
            .when("/register", {
                templateUrl: "./Scripts/spa/account/register.html",
                controller: "registerCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclareas", {
                templateUrl: "./Scripts/spa/gwclareas/gwclareas.html",
                controller: "gwclareasCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclareas/add", {
                templateUrl: "./Scripts/spa/gwclareas/add.html",
                controller: "gwclareasAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclregions", {
                templateUrl: "./Scripts/spa/gwclregions/gwclregions.html",
                controller: "gwclregionsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclregions/add", {
                templateUrl: "./Scripts/spa/gwclregions/add.html",
                controller: "gwclregionsAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclstations", {
                templateUrl: "./Scripts/spa/gwclstations/gwclstations.html",
                controller: "gwclstationsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclstations/add", {
                templateUrl: "./Scripts/spa/gwclstations/add.html",
                controller: "gwclstationsAddCtrl",
                resolve: { isAuthenticated: isAuthenticated}
            })
            .when("/gwclsystems", {
                templateUrl: "./Scripts/spa/gwclsystems/gwclsystems.html",
                controller: "gwclsystemsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclsystems/add", {
                templateUrl: "./Scripts/spa/gwclsystems/add.html",
                controller: "gwclsystemsAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwcloptions", {
                templateUrl: "./Scripts/spa/gwcloptions/gwcloptions.html",
                controller: "gwcloptionsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwcloptions/add", {
                templateUrl: "./Scripts/spa/gwcloptions/add.html",
                controller: "gwcloptionsAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclotypes", {
                templateUrl: "./Scripts/spa/gwclotypes/gwclotypes.html",
                controller: "gwclotypesCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclotypes/add", {
                templateUrl: "./Scripts/spa/gwclotypes/add.html",
                controller: "gwclotypesAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/productions", {
                templateUrl: "./Scripts/spa/productions/productions.html",
                controller: "productionsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/productions/add", {
                templateUrl: "./Scripts/spa/productions/add.html",
                controller: "productionAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
                .when("/productions/exceldata", {
                    templateUrl: "./Scripts/spa/productions/exceldata.html",
                    controller: "excelDataCtrl",
                    resolve: { isAuthenticated: isAuthenticated }
                })
            .when("/productions/summary/:id", {
                templateUrl: "./Scripts/spa/productions/summary.html",
                controller: "summaryCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/productions/masterSummary/:id", {
                templateUrl: "./Scripts/spa/productions/masterSummary.html",
                controller: "masterSummaryCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
                .when("/productions/charts/:id", {
                    templateUrl: "./Scripts/spa/productions/charts.html",
                    controller: "chartsCtrl",
                    resolve: { isAuthenticated: isAuthenticated }
                })
            .when("/productions/:id", {
                templateUrl: "./Scripts/spa/productions/details.html",
                controller: "productionDetailsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/productions/edit/:id", {
                templateUrl: "./Scripts/spa/productions/edit.html",
                controller: "productionEditCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/downtimes", {
                templateUrl: "./Scripts/spa/downtimes/downtimes.html",
                controller: "downtimesCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/downtimes/add", {
                templateUrl: "./Scripts/spa/downtimes/add.html",
                controller: "downtimeAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/downtimes/:id", {
                templateUrl: "./Scripts/spa/downtimes/details.html",
                controller: "downtimeDetailsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/downtimes/edit/:id", {
                templateUrl: "./Scripts/spa/downtimes/edit.html",
                controller: "downtimeEditCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            });
    }

    run.$inject = ['$rootScope', '$location', '$cookieStore', '$http'];

    function run($rootScope, $location, $cookieStore, $http) {
        // handle page refreshes
        $rootScope.repository = $cookieStore.get('repository') || {};
        if ($rootScope.repository.loggedUser) {
            $http.defaults.headers.common['Authorization'] = $rootScope.repository.loggedUser.authdata;
        }

        $(document).ready(function () {
            $(".fancybox").fancybox({
                openEffect: 'none',
                closeEffect: 'none'
            });

            $('.fancybox-media').fancybox({
                openEffect: 'none',
                closeEffect: 'none',
                helpers: {
                    media: {}
                }
            });

            $('[data-toggle=offcanvas]').click(function () {
                $('.row-offcanvas').toggleClass('active');
            });
        });
    }

    isAuthenticated.$inject = ['membershipService', '$rootScope', '$location'];

    function isAuthenticated(membershipService, $rootScope, $location) {
        if (!membershipService.isUserLoggedIn()) {
            $rootScope.previousState = $location.path();
            $location.path('/login');
        }
    }

})();