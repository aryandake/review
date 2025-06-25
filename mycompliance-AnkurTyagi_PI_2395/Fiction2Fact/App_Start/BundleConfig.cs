using System.Web;
using System.Web.Optimization;

namespace Fiction2Fact
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Bundles/js").Include(
                      Global.site_url("/Scripts/jquery-3.5.0.min.js", true),
                      Global.site_url("/Scripts/bootstrap.js", true),
                      Global.site_url("/Scripts/js/legacy/menu.js", true),
                      Global.site_url("/Scripts/respond.js", true)));

            bundles.Add(new ScriptBundle("~/Bundles/legacy").Include(
                      "~/Content/js/legacy/menu.js"));

            bundles.Add(new StyleBundle("~/Content/css.css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/main.css",
                      "~/Content/controlStyle.css",
                      "~/Content/menu_topbar.css",
                      "~/Content/css/fa-all.css",
                      "~/Content/css/site.css"));
        }
    }
}
