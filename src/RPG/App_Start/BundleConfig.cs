using System.Web;
using System.Web.Optimization;

namespace RPG
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/documentready").Include(
                        "~/Scripts/documentready.js"));

            bundles.Add(new ScriptBundle("~/bundles/characterready").Include(
                                    "~/Scripts/characterready.js"));

            bundles.Add(new ScriptBundle("~/bundles/bagitemready").Include(
                        "~/Scripts/bagitemready.js"));

            bundles.Add(new ScriptBundle("~/bundles/casterlevelready").Include(
                        "~/Scripts/casterlevelready.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/ajaxhelper.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/charcreatorscripts").Include(
                        "~/Scripts/charactercreator.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/at").Include(
                "~/Scripts/jquery.atwho.min.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/leaflet").Include(
                "~/Scripts/leaflet.js",
                "~/Scripts/leaflet.draw.js",
                "~/Scripts/leaflet.measurecontrol.js",
                //"~/Scripts/leaflet-measure.min.js",
                "~/Scripts/rastercoords.js",
                "~/Scripts/Leaflet.Bookmarks.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                        "~/Scripts/chosen.jquery.min.js",
                        "~/Scripts/chosen.proto.min.js"
                        ));
            bundles.Add(new StyleBundle("~/Content/chosen").Include(
                                    "~/Content/chosen.min.css"
                                    ));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                                    "~/Scripts/select2.full.min.js"
                                    ));
            bundles.Add(new StyleBundle("~/Content/select2").Include(
                                    "~/Content/select2.css"
                                    ));
            bundles.Add(new StyleBundle("~/Content/at").Include(
                "~/Content/jquery.atwho.min.css"
            ));

            bundles.Add(new StyleBundle("~/Content/leaflet").Include(
                "~/Content/leaflet.css",
                "~/Content/leaflet.draw.css",
                "~/Content/leaflet.measurecontrol.css",
                //"~/Content/leaflet-measure.css",
                "~/Content/leaflet.bookmarks.min.css"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/bootstrap.min.css",
                "~/Content/bootstrap-responsive.min.css",
                "~/Content/bootstrap-custom.css",
                "~/Content/css/select2.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}