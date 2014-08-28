using System.Web.Optimization;

namespace ParTech.ImageLibrary.Website
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery.tmpl.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/shoppingcart-script*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryupload").Include(
                        "~/Scripts/jquery.knob*",
                        "~/Scripts/jquery.ui.widget*",
                        "~/Scripts/jquery.iframe-transport*", 
                        "~/Scripts/jquery.fileupload*",
                        "~/Scripts/upload-script*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryprocesses").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/processes-script*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Css/css").Include("~/Css/site.css"));

            bundles.Add(new StyleBundle("~/Css/themes/base/css").Include(
                        "~/Css/themes/base/jquery.ui.core.css",
                        "~/Css/themes/base/jquery.ui.resizable.css",
                        "~/Css/themes/base/jquery.ui.selectable.css",
                        "~/Css/themes/base/jquery.ui.accordion.css",
                        "~/Css/themes/base/jquery.ui.autocomplete.css",
                        "~/Css/themes/base/jquery.ui.button.css",
                        "~/Css/themes/base/jquery.ui.dialog.css",
                        "~/Css/themes/base/jquery.ui.slider.css",
                        "~/Css/themes/base/jquery.ui.tabs.css",
                        "~/Css/themes/base/jquery.ui.datepicker.css",
                        "~/Css/themes/base/jquery.ui.progressbar.css",
                        "~/Css/themes/base/jquery.ui.theme.css"));
        }
    }
}