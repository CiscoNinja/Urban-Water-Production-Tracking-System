(function () {
    'use strict';

    angular.module('gwcltdApp', ['common.core', 'common.ui'])
        .config(config)
        .run(run);

    config.$inject = ['$routeProvider'];
    function config($routeProvider) {
        $routeProvider
            .when("/", {
                templateUrl: "scripts/spa/home/index.html",
                controller: "indexCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/login", {
                templateUrl: "scripts/spa/account/login.html",
                controller: "loginCtrl"
            })
            .when("/register", {
                templateUrl: "scripts/spa/account/register.html",
                controller: "registerCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclareas", {
                templateUrl: "scripts/spa/gwclareas/gwclareas.html",
                controller: "gwclareasCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclareas/add", {
                templateUrl: "scripts/spa/gwclareas/add.html",
                controller: "gwclareasAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclregions", {
                templateUrl: "scripts/spa/gwclregions/gwclregions.html",
                controller: "gwclregionsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclregions/add", {
                templateUrl: "scripts/spa/gwclregions/add.html",
                controller: "gwclregionsAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclstations", {
                templateUrl: "scripts/spa/gwclstations/gwclstations.html",
                controller: "gwclstationsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclstations/add", {
                templateUrl: "scripts/spa/gwclstations/add.html",
                controller: "gwclstationsAddCtrl",
                resolve: { isAuthenticated: isAuthenticated}
            })
            .when("/gwclsystems", {
                templateUrl: "scripts/spa/gwclsystems/gwclsystems.html",
                controller: "gwclsystemsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclsystems/add", {
                templateUrl: "scripts/spa/gwclsystems/add.html",
                controller: "gwclsystemsAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwcloptions", {
                templateUrl: "scripts/spa/gwcloptions/gwcloptions.html",
                controller: "gwcloptionsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwcloptions/add", {
                templateUrl: "scripts/spa/gwcloptions/add.html",
                controller: "gwcloptionsAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclotypes", {
                templateUrl: "scripts/spa/gwclotypes/gwclotypes.html",
                controller: "gwclotypesCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/gwclotypes/add", {
                templateUrl: "scripts/spa/gwclotypes/add.html",
                controller: "gwclotypesAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/productions", {
                templateUrl: "scripts/spa/productions/productions.html",
                controller: "productionsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/productions/add", {
                templateUrl: "scripts/spa/productions/add.html",
                controller: "productionAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/productions/summary/:id", {
                templateUrl: "scripts/spa/productions/summary.html",
                controller: "summaryCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
                .when("/productions/charts/:id", {
                    templateUrl: "scripts/spa/productions/charts.html",
                    controller: "chartsCtrl",
                    resolve: { isAuthenticated: isAuthenticated }
                })
            .when("/productions/:id", {
                templateUrl: "scripts/spa/productions/details.html",
                controller: "productionDetailsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/productions/edit/:id", {
                templateUrl: "scripts/spa/productions/edit.html",
                controller: "productionEditCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/downtimes", {
                templateUrl: "scripts/spa/downtimes/downtimes.html",
                controller: "downtimesCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/downtimes/add", {
                templateUrl: "scripts/spa/downtimes/add.html",
                controller: "downtimeAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/downtimes/:id", {
                templateUrl: "scripts/spa/downtimes/details.html",
                controller: "downtimeDetailsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/downtimes/edit/:id", {
                templateUrl: "scripts/spa/downtimes/edit.html",
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