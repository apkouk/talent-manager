using System.Web;
using System.Web.Mvc;
using TalentManager.App_Start;

namespace TalentManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}