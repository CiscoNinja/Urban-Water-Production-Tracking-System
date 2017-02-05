using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace GwcltdApp.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/Vendors/modernizr.js"));

           bundles.Add(new ScriptBundle("~/bundles/vendors").Include(
                "~/Scripts/Vendors/jquery.js",
                "~/Scripts/Vendors/bootstrap.js",
                "~/Scripts/Vendors/toastr.js",
                "~/Scripts/Vendors/jquery.raty.js",
                "~/Scripts/Vendors/respond.src.js",
                "~/Scripts/Vendors/angular.js",
                "~/Scripts/Vendors/angular-route.js",
                "~/Scripts/Vendors/angular-cookies.js",
                "~/Scripts/Vendors/angular-validator.js",
                "~/Scripts/Vendors/angular-base64.js",
                "~/Scripts/Vendors/angular-file-upload.js",
                "~/Scripts/Vendors/angucomplete-alt.min.js",
                "~/Scripts/Vendors/ui-bootstrap-tpls-0.13.1.js",
                "~/Scripts/Vendors/underscore.js",
                "~/Scripts/Vendors/raphael.js",
                "~/Scripts/Vendors/morris.js",
                //"~/Scripts/Vendors/d3.v4.min.js",
                //"~/Scripts/Vendors/jquery.stickyheader.js",
                //"~/Scripts/Vendors/debouncejs.js",
                "~/Scripts/Vendors/jquery.fancybox.js",
                "~/Scripts/Vendors/jquery.fancybox-media.js",
                "~/Scripts/Vendors/loading-bar.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/spa").Include(
                "~/Scripts/spa/modules/common.core.js",
                "~/Scripts/spa/modules/common.ui.js",
                "~/Scripts/spa/app.js",
                "~/Scripts/spa/services/apiService.js",
                "~/Scripts/spa/services/notificationService.js",
                "~/Scripts/spa/services/membershipService.js",
                "~/Scripts/spa/services/fileUploadService.js",
                "~/Scripts/spa/layout/topBar.directive.js",
                "~/Scripts/spa/layout/sideBar.directive.js",
                "~/Scripts/spa/layout/customPager.directive.js",
                "~/Scripts/spa/directives/rating.directive.js",
                "~/Scripts/spa/directives/availableMovie.directive.js",
                "~/Scripts/spa/account/loginCtrl.js",
                "~/Scripts/spa/account/registerCtrl.js",
                "~/Scripts/spa/home/rootCtrl.js",
                "~/Scripts/spa/home/indexCtrl.js",
                "~/Scripts/spa/gwclareas/gwclareasCtrl.js",
                "~/Scripts/spa/gwclareas/gwclareasAddCtrl.js",
                "~/Scripts/spa/gwclareas/gwclareaEditCtrl.js", 
                "~/Scripts/spa/gwclregions/gwclregionsCtrl.js",
                "~/Scripts/spa/gwclregions/gwclregionsAddCtrl.js",
                "~/Scripts/spa/gwclregions/gwclregionEditCtrl.js",
                "~/Scripts/spa/gwclstations/gwclstationsCtrl.js",
                "~/Scripts/spa/gwclstations/gwclstationsAddCtrl.js",
                "~/Scripts/spa/gwclstations/gwclstationEditCtrl.js",
                "~/Scripts/spa/gwclsystems/gwclsystemsCtrl.js",
                "~/Scripts/spa/gwclsystems/gwclsystemsAddCtrl.js",
                "~/Scripts/spa/gwclsystems/gwclsystemEditCtrl.js",
                "~/Scripts/spa/productions/productionsCtrl.js",
                "~/Scripts/spa/productions/productionAddCtrl.js",
                "~/Scripts/spa/productions/productionDetailsCtrl.js",
                "~/Scripts/spa/productions/productionEditCtrl.js",
                "~/Scripts/spa/productions/summaryCtrl.js",
                "~/Scripts/spa/productions/chartsCtrl.js",
                "~/Scripts/spa/controllers/rentalCtrl.js",
                "~/Scripts/spa/rental/rentMovieCtrl.js",
                "~/Scripts/spa/rental/rentStatsCtrl.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/content/css/site.css",
                "~/content/css/bootstrap.css",
                "~/content/css/bootstrap-theme.css",
                 "~/content/css/font-awesome.css",
                "~/content/css/morris.css",
                "~/content/css/toastr.css",
                "~/content/css/jquery.fancybox.css",
                "~/content/css/loading-bar.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}