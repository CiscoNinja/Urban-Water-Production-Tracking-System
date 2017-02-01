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
                controller: "indexCtrl"
            })
            .when("/login", {
                templateUrl: "scripts/spa/account/login.html",
                controller: "loginCtrl",
            })
            .when("/register", {
                templateUrl: "scripts/spa/account/register.html",
                controller: "registerCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            //.when("/customers", {
            //    templateUrl: "scripts/spa/customers/customers.html",
            //    controller: "customersCtrl"
            //})
            //.when("/customers/register", {
            //    templateUrl: "scripts/spa/customers/register.html",
            //    controller: "customersRegCtrl",
            //    resolve: { isAuthenticated: isAuthenticated }
            //})
            .when("/options", {
                templateUrl: "scripts/spa/options/options.html",
                controller: "opttionsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/options/add", {
                templateUrl: "scripts/spa/options/add.html",
                controller: "optionAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/options/:id", {
                templateUrl: "scripts/spa/options/details.html",
                controller: "optionDetailsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/options/edit/:id", {
                templateUrl: "scripts/spa/options/edit.html",
                controller: "optionEditCtrl",
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
                controller: "productionEditCtrl"
            })
            .when("/optiontypes", {
                templateUrl: "scripts/spa/optiontypes/optiontypes.html",
                controller: "optiontypesCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/optiontypes/add", {
                templateUrl: "scripts/spa/optiontypes/add.html",
                controller: "optiontypeAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/optiontypes/:id", {
                templateUrl: "scripts/spa/optiontypes/details.html",
                controller: "optiontypeDetailsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/optiontypes/edit/:id", {
                templateUrl: "scripts/spa/optiontypes/edit.html",
                controller: "optiontypeEditCtrl"
            })
            .when("/wsystems", {
                templateUrl: "scripts/spa/wsystems/wsystems.html",
                controller: "wsystemsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/wsystems/add", {
                templateUrl: "scripts/spa/wsystems/add.html",
                controller: "wsystemAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/wsystems/:id", {
                templateUrl: "scripts/spa/wsystems/details.html",
                controller: "wsystemDetailsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/wsystems/edit/:id", {
                templateUrl: "scripts/spa/wsystems/edit.html",
                controller: "wsystemEditCtrl"
            }).otherwise({ redirectTo: "/" });
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