using System.Web;
using System.Web.Optimization;

namespace EIS.FEW
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquerybase").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.extensions.js",
                        "~/Scripts/serviceInvoker.js"));

            bundles.Add(new ScriptBundle("~/bundles/metronic/base").Include(
                        "~/Content/themes/metronic/assets/global/plugins/jquery-migrate.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/jquery.blockui.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/jquery.cokie.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/uniform/jquery.uniform.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/metro-ui/js/metro-dialog.js",
                        "~/Content/themes/metronic/assets/global/plugins/metro-ui/js/metro-notify.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/metronic/layout").Include(
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-touchspin/bootstrap.touchspin.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/icheck/icheck.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/select2/select2.js",
                        "~/Content/themes/metronic/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-datepicker/js/locales/bootstrap-datepicker.vi.js",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-daterangepicker/moment.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-daterangepicker/daterangepicker.js",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js"
                        //"~/Content/themes/metronic/assets/global/scripts/metronic.js",
                        //"~/Content/themes/metronic/assets/admin/layout5/scripts/layout.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/metronic/login").Include(
                        "~/Content/themes/metronic/assets/global/plugins/jquery-validation/js/jquery.validate.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/backstretch/jquery.backstretch.min.js",
                        "~/Content/themes/metronic/assets/global/plugins/select2/select2.min.js",
                        "~/Content/themes/metronic/assets/global/scripts/metronic.js",
                        "~/Content/themes/metronic/assets/admin/layout/scripts/layout.js",
                        "~/Content/themes/metronic/assets/admin/pages/scripts/login-soft.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/app/modules").Include(
                        "~/Scripts/jquery.timer.js",
                        "~/Scripts/jquery.lazyload.js",
                        "~/Scripts/report.resizelayout.js",
                        "~/Scripts/jquery.colresize.js",
                        "~/Scripts/jquery.autonumber.js",
                        "~/Scripts/jquery.columns.table.js",
                        "~/Scripts/jquery.tooltipster.js",
                        "~/Scripts/common.js",
                        "~/Scripts/common.*",
                        "~/Scripts/app/module.*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/tableexport").Include(
                        "~/Scripts/exporthtml/jspdf/libs/sprintf.js",
                        "~/Scripts/exporthtml/jspdf/libs/base64.js",
                        "~/Scripts/exporthtml/jspdf/jspdf.js",
                        "~/Scripts/exporthtml/html2canvas.js",
                        "~/Scripts/exporthtml/tableExport.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/chartreport").Include(
                        "~/Scripts/highcharts/highcharts.js",
                        "~/Scripts/highcharts/modules/exporting.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/colorpicker").Include(
                        "~/Scripts/colorpicker/spectrum.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/fancytree").Include(
                        "~/Scripts/FancyTree/jquery.fancytree.js",
                        "~/Scripts/FancyTree/jquery.fancytree.menu.js",
                        "~/Scripts/FancyTree/jquery.fancytree.table.js",
                        "~/Scripts/FancyTree/jquery.fancytree.filter.js",
                        "~/Scripts/FancyTree/jquery.fancytree.dnd.js",
                        "~/Scripts/FancyTree/jquery.fancytree.edit.js",
                        "~/Scripts/FancyTree/extensions/contextmenu/js/jquery.contextMenu-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/customtree").Include(
                        "~/Scripts/tree.jquery.js",
                        "~/Scripts/droptree.jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/openlayer3").Include(
                        "~/Scripts/openlayer/build/ol.js",
                        "~/Scripts/openlayer/ol.ext/ol3-layerswitcher.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                        "~/Scripts/ckeditor/ckeditor.js",
                        "~/Scripts/ckeditor/lang/vi.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/codemirror").Include(
                        "~/Scripts/codemirror/lib/codemirror.js",

                        "~/Scripts/codemirror/addon/edit/matchbrackets.js",
                        "~/Scripts/codemirror/addon/edit/closebrackets.js",

                        "~/Scripts/codemirror/addon/lint/lint.js",
                        "~/Scripts/codemirror/addon/lint/csslint.js",
                        "~/Scripts/codemirror/addon/lint/javascript-lint.js",
                        "~/Scripts/codemirror/addon/lint/coffeescript-lint.js",
                        "~/Scripts/codemirror/addon/lint/json-lint.js",
                        "~/Scripts/codemirror/addon/lint/css-lint.js",

                        "~/Scripts/codemirror/addon/hint/show-hint.js",
                        "~/Scripts/codemirror/addon/hint/sql-hint.js",
                        "~/Scripts/codemirror/addon/hint/css-hint.js",
                        "~/Scripts/codemirror/addon/hint/jshint.js",
                        "~/Scripts/codemirror/addon/hint/javascript-hint.js",

                        "~/Scripts/codemirror/addon/selection/active-line.js",

                        "~/Scripts/codemirror/addon/display/placeholder.js",
                        "~/Scripts/codemirror/addon/display/fullscreen.js",

                        "~/Scripts/codemirror/mode/sql/sql.js",
                        "~/Scripts/codemirror/mode/xml/xml.js",
                        "~/Scripts/codemirror/mode/javascript/javascript.js",
                        "~/Scripts/codemirror/mode/css/css.js",
                        "~/Scripts/codemirror/mode/htmlmixed/htmlmixed.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/themes/metronic/base").Include(
                        "~/Content/themes/metronic/assets/global/plugins/font-awesome/font-awesome.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap/css/bootstrap.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/uniform/css/uniform.default.css",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-datepicker/css/datepicker3.css",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-daterangepicker/daterangepicker-bs3.css",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/fullcalendar/fullcalendar.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/morris/morris.css",
                        "~/Content/themes/metronic/assets/global/plugins/metro-ui/metro-bootstrap-2.0.css",
                        "~/Content/themes/metronic/assets/global/plugins/metro-ui/metro-ui-custom.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/metronic/theme").Include(
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-select/bootstrap-select.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/icheck/skins/all.css",
                        "~/Content/themes/metronic/assets/global/plugins/bootstrap-touchspin/bootstrap-touchspin.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/select2/select2.css",
                        "~/Content/themes/metronic/assets/global/plugins/select2/select2.min.css",
                        "~/Content/themes/metronic/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.css",
                        "~/Content/themes/metronic/assets/global/plugins/jquery-multi-select/css/multi-select.css",
                        "~/Content/themes/metronic/assets/global/css/components.css",
                        "~/Content/themes/metronic/assets/global/css/components-rounded.css",
                        "~/Content/themes/metronic/assets/global/css/plugins.css",
                        "~/Content/themes/metronic/assets/admin/layout5/css/layout.css",
                        "~/Content/themes/metronic/assets/admin/layout5/css/themes/light.css",
                        "~/Content/themes/metronic/assets/admin/layout5/css/custom.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/metronic/login").Include(
                        "~/Content/themes/metronic/assets/admin/pages/css/login-soft.css",
                        "~/Content/themes/metronic/assets/global/css/components.css",
                        "~/Content/themes/metronic/assets/global/css/plugins.css",
                        "~/Content/themes/metronic/assets/admin/layout5/css/layout.css",
                        "~/Content/themes/metronic/assets/admin/layout5/css/themes/light.css",
                        "~/Content/themes/metronic/assets/admin/layout5/css/custom.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/codemirror/css").Include(
                        "~/Content/themes/codemirror/codemirror.css",
                        "~/Scripts/codemirror/addon/display/fullscreen.css",
                        "~/Scripts/codemirror/addon/lint/lint.css",
                        "~/Scripts/codemirror/addon/hint/sqlcss.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/openlayer3").Include(
                        "~/Scripts/openlayer/css/ol.css",
                        "~/Scripts/openlayer/css/ol3-layerswitcher.css"));

            bundles.Add(new ScriptBundle("~/Content/colorpicker").Include(
                        "~/Scripts/colorpicker/spectrum.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/FancyTree").Include(
                        "~/Scripts/FancyTree/skin-lion/ui.fancytree.css",
                        "~/Scripts/FancyTree/extensions/contextmenu/css/jquery.contextMenu.css"));

            bundles.Add(new StyleBundle("~/Content/TinyMce").Include(
                        "~/Scripts/tinymce/skins/lightgray/*.css", new CssRewriteUrlTransform()
                        ));

            bundles.Add(new StyleBundle("~/Content/Uploader").Include(
                        "~/Scripts/uploader/css/*.css", new CssRewriteUrlTransform()
                        ));

            bundles.Add(new StyleBundle("~/Content/Uploaderie8").Include(
                        "~/Scripts/uploader/css/demo-ie8.css", new CssRewriteUrlTransform()
                        ));

            bundles.Add(new StyleBundle("~/Content/UploaderNoscript").Include(
                        "~/Scripts/uploader/css/*.css", new CssRewriteUrlTransform()
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/*.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/Devexpress.css",
                "~/Content/site.css"));

         
        }
    }
}