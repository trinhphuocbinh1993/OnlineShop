using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Web.Routing;

namespace OnlineShop
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var sessions = (Common.UserLogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION];
            if (sessions == null)
            {
                return false;
            }
            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(sessions.UserName);

            if (privilegeLevels.Contains(this.RoleID) || sessions.GroupID == CommonConstants.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var sessions = (Common.UserLogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION];
            if (sessions.GroupID == "MEMBER")
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/401.cshtml"
                };
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Areas/Admin/Views/Shared/401.cshtml"
                };
            }

        }
        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            var credentials = (List<string>)HttpContext.Current.Session[Common.CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}