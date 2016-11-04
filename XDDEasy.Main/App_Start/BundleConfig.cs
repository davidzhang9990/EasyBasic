using System.Web;
using System.Web.Optimization;

namespace XDDEasy.Main
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);
            RegisterStyles(bundles);

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"
                      ));
        }

        public static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/formCommon").Include(
                 
               "~/js/libs/formValidator/jquery.validationEngine.js",
               "~/js/libs/formValidator/languages/jquery.validationEngine-en.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                   "~/Scripts/jquery-{version}.js",
                   "~/scripts/customize/common.js",
                    "~/lib/modernizr.custom.js",
                   "~/scripts/customize/setup.js",
                   "~/scripts/customize/eq.modal.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/aciTree").Include(
                   "~/Scripts/jquery-{version}.js",
                   "~/lib/aciTree/js/jquery.aciPlugin.min.js",
                    "~/lib/aciTree/js/jquery.aciTree.dom.js",
                   "~/lib/aciTree/js/jquery.aciTree.core.js",
                   "~/lib/aciTree/js/jquery.aciTree.checkbox.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/formCommon").Include(
               "~/lib/formValidator/jquery.validationEngine.js",
               "~/lib/formValidator/languages/jquery.validationEngine-en.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/IndexCommon").Include(
                       "~/lib/pagination/jquery.pagination.js",
                       "~/lib/spin.min.js",
                       "~/scripts/customize/customizePaging.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
               "~/lib/DataTables/jquery.dataTables.js",
               "~/lib/DataTables/jquery.dataTables.rowReordering.js",
               "~/scripts/customize/jquery.dataTables.odata.js",
               "~/scripts/customize/jquery.dataTables.custom.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/libs/lodash").Include(
                       "~/lib/lodash/lodash.underscore.min.js"));

        }

        public static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/lib/pagination/pagination").Include(
                     "~/lib/pagination/pagination.css"
                     ));

            bundles.Add(new StyleBundle("~/lib/DataTables/dataTables").Include(
                      "~/lib/DataTables/jquery.dataTables-simple.css"
                       ));

            bundles.Add(new StyleBundle("~/css/styles/common").Include(
                        "~/lib/formValidator/eq.validationEngine.css",
                         "~/css/modal.css",
                         "~/css/font-awesome.min.css"
                        ));

        }



    }
}
