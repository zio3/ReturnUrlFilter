using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ReturnUrlFilter
{
    public class SetReturnUrlAttribute : ActionFilterAttribute
    {
        public const string KeyName = "__ReturnUrl";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod.ToLower() == "get")
            {
                if (filterContext.HttpContext.Request.UrlReferrer != null)
                {
                    filterContext.Controller.ViewBag.__ReturnUrl = filterContext.HttpContext.Request.UrlReferrer.ToString();
                }
            }
            else
            {
                filterContext.Controller.ViewBag.__ReturnUrl = filterContext.HttpContext.Request.Form[KeyName];
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class RedirectToReturnUrlAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result.GetType() == typeof(RedirectResult))
            {
                var returnUrl = filterContext.RequestContext.HttpContext.Request.Form[SetReturnUrlAttribute.KeyName];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    filterContext.Result = new RedirectResult(returnUrl);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
