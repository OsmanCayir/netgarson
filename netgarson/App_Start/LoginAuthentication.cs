using netgarson.Entities;
using netgarson.Helpers;
using netgarson.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace netgarson.Controllers
{
    public class LoginAuthentication : ActionFilterAttribute
    {
        public string ViewName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext httpContext)
        {
            if ((User)HttpContext.Current.Session["user"] != null)
            {
                string mail = ((User)HttpContext.Current.Session["user"]).Mail;
                string password = ((User)HttpContext.Current.Session["user"]).Password;
                int errorCode = InputControl.LoginUserControl(mail, password);
                if (errorCode != 100)
                {
                    if (ViewName == "" || ViewName == null)
                    {
                        httpContext.Result = new RedirectResult(string.Format("/Admin/Login", httpContext.HttpContext.Request.Url.AbsolutePath));
                    }
                    else
                    {
                        httpContext.Result = new RedirectResult(string.Format("/Admin/LoginAuthenticationRouter?view=" + ViewName, httpContext.HttpContext.Request.Url.AbsolutePath));
                    }

                }
            }
            else
            {
                if (ViewName == "" || ViewName == null)
                {
                    httpContext.Result = new RedirectResult(string.Format("/Admin/Login", httpContext.HttpContext.Request.Url.AbsolutePath));
                }
                else
                {
                    httpContext.Result = new RedirectResult(string.Format("/Admin/LoginAuthenticationRouter?view=" + ViewName, httpContext.HttpContext.Request.Url.AbsolutePath));
                }
            }
        }

    }



}