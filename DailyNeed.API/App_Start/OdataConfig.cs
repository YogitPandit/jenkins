using DailyNeed.Model;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace DailyNeed.API.App_Start
{
    public static class OdataConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes(); //This has to be called before the following OData mapping, so also before 
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
           // builder.EntitySet<WarehouseCategory>("CategoryOdata");
            builder.EntitySet<SubsubCategory>("SubsubCategoryOdata");
            config.MapODataServiceRoute(
                  routeName: "ODataRoute",
                  routePrefix: "oapi",
                  model: builder.GetEdmModel());
            
        }
    }
}