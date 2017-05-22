namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Reflection;
    using System.Security.Principal;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;
    using Newtonsoft.Json;

    public abstract class BaseControllerTests<T> where T : ApiController
    {
        protected T Controller;

        protected void SetupController()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage();
            var urlHelper = new UrlHelper(request);
            var route = config.Routes.MapHttpRoute("ARouteName", "ARouteTemplate");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "AControllerName" } });

            Controller.RequestContext = new HttpRequestContext
            {
                Url = urlHelper,
                Principal = new GenericPrincipal(new GenericIdentity("foo"), new[] { "A role" })
            };

            Controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            Controller.Url = urlHelper;
            Controller.Request = request;

            //Don't think we need these
            //Controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            //Controller.Request.Properties["MS_HttpContext"] = null;
        }

        protected MethodInfo GetMethod(Expression<Func<T, object>> methodSelector)
        {
            // Note: this is a bit simplistic implementation. It will
            // not work for all expressions.
            return ((MethodCallExpression)methodSelector.Body).Method;
        }

        protected MethodInfo GetMethod(Expression<Action<T>> methodSelector)
        {
            return ((MethodCallExpression)methodSelector.Body).Method;
        }

        protected TAttribute[] GetAttributes<TAttribute>(MemberInfo member) where TAttribute : Attribute
        {
            var attributes =
                member.GetCustomAttributes(typeof(TAttribute), true);

            return (TAttribute[])attributes;
        }

        protected TModel GetResponseObject<TModel>(HttpResponseMessage httpResponseMessage)
        {
            var message = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TModel>(message);
        }
    }
}
