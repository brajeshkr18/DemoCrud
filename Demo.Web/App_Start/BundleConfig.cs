using System.Web.Optimization;

namespace TMS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.base64.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/KendoDatepicker")
                     .Include("~/html/vendor/kendo/styles/kendo.common-material.min.css", new CssRewriteUrlTransform())
                     .Include("~/html/vendor/kendo/styles/kendo.material.min.css", new CssRewriteUrlTransform())
                     .Include("~/html/vendor/kendo/styles/kendo.material.mobile.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/vendor/bootstrap/css")
                .Include("~/html/vendor/bootstrap/dist/css/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include(
                     "~/html/vendor/nprogress/nprogress.css",
                      "~/html/vendor/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css",
                     "~/html/vendor/iCheck/skins/flat/green.css",
                     "~/html/vendor/animate.css/animate.min.css",
                     "~/html/build/css/custom.min.css",
                     "~/Content/site.css",
                     "~/html/vendor/jquery-toast-plugin/css/jquery.toast.css",
                     "~/Content/custom.css")
                 .Include("~/html/vendor/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/vendors/locationpicker/js").Include(
          "~/Scripts/LocationPicker/locationpicker.jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/validations/js").Include(
           "~/Scripts/jquery.validate.min.js",
              "~/Scripts/jquery.validate.unobtrusive.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/toast/js").Include(
             "~/html/vendor/jquery-toast-plugin/js/jquery.toast.js",
              "~/html/build/js/toastalerts.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
               "~/Scripts/highcharts/highcharts.js",
               "~/Scripts/highcharts/modules/data.js",
               "~/Scripts/highcharts/modules/drilldown.js",
               "~/html/build/ApplicationJS/Dashboard.js"));




            bundles.Add(new ScriptBundle("~/bundles/user/js").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/html/vendor/bootstrap/dist/js/bootstrap.min.js",
                            //"~/Scripts/jquery.dataTables.min.js",
                            //"~/Scripts/dataTables.bootstrap.min.js",
                            //"~/Scripts/dataTables.buttons.min.js",
                            //"~/html/vendor/fastclick/lib/fastclick.js",
                            "~/html/vendor/nprogress/nprogress.js",
                                "~/html/vendor/bootstrap-progressbar/bootstrap-progressbar.min.js",
                            //    "~/html/vendor/lodash/dist/lodash.min.js",
                            "~/html/build/js/custom.min.js",
                            "~/html/vendor/jquery-toast-plugin/js/jquery.toast.js",
                            "~/html/build/js/toastalerts.js",
                            "~/Scripts/jquery.signalR-2.2.2.js"
                            ));


            bundles.Add(new ScriptBundle("~/bundles/export").Include(
                    "~/Scripts/TableExport/js/Blob.min.js",
                    "~/Scripts/TableExport/js/FileSaver.min.js",
                    "~/Scripts/TableExport/js/tableexport.min.js"));

                bundles.Add(new ScriptBundle("~/bundles/table2excel").Include(
            "~/html/js/jquery.table2excel.min.js",
            "~/html/vendor/datatables.net-fixedheader/js/dataTables.fixedHeader.min.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/html/vendor/bootstrap/dist/css/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include(                   
                   "~/html/vendor/font-awesome/css/font-awesome.min.css",
                   "~/html/vendor/nprogress/nprogress.css",
                   "~/html/vendor/iCheck/skins/flat/green.css",
                   "~/html/vendor/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css",
                   "~/html/vendor/jqvmap/dist/jqvmap.min.css",
                   "~/html/vendor/bootstrap-daterangepicker/daterangepicker.css",
                   "~/html/build/css/custom.min.css",
                   "~/Content/site.css",
                   "~/Content/custom.css")
                 .Include("~/html/vendor/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));


            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                    "~/html/vendor/jquery/dist/jquery.min.js",
                    "~/html/vendor/bootstrap/dist/js/bootstrap.min.js",
                    "~/html/vendor/fastclick/lib/fastclick.js",
                    "~/html/vendor/nprogress/nprogress.js",
                    "~/html/vendor/Chart.js/dist/Chart.min.js",
                    "~/html/vendor/gauge.js/dist/gauge.min.js",
                    "~/html/vendor/bootstrap-progressbar/bootstrap-progressbar.min.js",
                    "~/html/vendor/iCheck/icheck.min.js",
                    "~/html/vendor/skycons/skycons.js",
                    "~/html/vendor/Flot/jquery.flot.js",
                    "~/html/vendor/Flot/jquery.flot.pie.js",
                    "~/html/vendor/Flot/jquery.flot.time.js",
                    "~/html/vendor/Flot/jquery.flot.stack.js",
                    "~/html/vendor/Flot/jquery.flot.resize.js",
                    "~/html/vendor/flot.orderbars/js/jquery.flot.orderBars.js",
                    "~/html/vendor/flot-spline/js/jquery.flot.spline.min.js",
                    "~/html/vendor/flot.curvedlines/curvedLines.js",
                    "~/html/vendor/DateJS/build/date.js",
                    "~/html/vendor/jqvmap/dist/jquery.vmap.js",
                    "~/html/vendor/jqvmap/dist/maps/jquery.vmap.world.js",
                    "~/html/vendor/jqvmap/examples/js/jquery.vmap.sampledata.js",
                    "~/html/vendor/moment/min/moment.min.js",
                    "~/html/vendor/bootstrap-daterangepicker/daterangepicker.js",
                    "~/html/build/js/custom.min.js"
                    ));
            bundles.Add(new StyleBundle("~/bundles/css/datepicker")
               .Include("~/Content/bootstrap.min.css","~/html/datepicker/bootstrap-datetimepicker.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/js/datepicker")
               .Include("~/html/vendor/bootstrap/js/bootstrap.min.js", "~/html/datepicker/bootstrap-datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/timepickerJs").Include(
                   "~/html/js/bootstrap-timepicker.js",
                   "~/html/js/bootstrap-timepicker.min.js"));

            bundles.Add(new StyleBundle("~/Content/timepickerCss").Include(
                      "~/html/css/bootstrap-glyphicons.css",
                      "~/html/css/bootstrap-timepicker.min.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}
