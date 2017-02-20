using System;
using System.Linq;
using System.Web.Http.Controllers;
using PH.Well.Common.Contracts;
using System.Threading;
using PH.Well.Api.DependencyResolution;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;

namespace PH.Well.Api.Validators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false)]
    public class PHAuthorizeAttribute : AuthorizationFilterAttribute, IAuthorizationFilter
    {
        private readonly object _typeId = new object();
        private string[] permissionsSplit = new string[0];
        private string permissions;
        //this doesn't make sense been static
        [ThreadStatic]
        private static string[] currentUserPermissions;

        private static string[] CurrentUserPermissions
        {
            get
            {
                if (currentUserPermissions == null)
                {
                    currentUserPermissions = LoadPermissions();
                }

                return currentUserPermissions;
            }
        }

        private static string[] LoadPermissions()
        {
            var userRoleProvider = IoC.Container.GetInstance<IUserRoleProvider>();
            var name = Thread.CurrentPrincipal.Identity.Name;
            return userRoleProvider.GetRolesForUser(name);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            if (!(this.SkipAuthorization(actionContext) || this.IsAuthorized(actionContext)))
            {
                this.HandleUnauthorizedRequest(actionContext);
            }
        }

        protected virtual void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.Forbidden, 
                    "You are not authorization to view this resource.");
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized, 
                    "Authorization has been denied for this request.");
            }
        }

        private bool SkipAuthorization(HttpActionContext actionContext)
        {
            if (!actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            }

            return true;
        }

        protected bool IsAuthorized(HttpActionContext actionContext)
        {
            var hasPermission = CurrentUserPermissions
                .Join(
                    this.permissionsSplit,
                    left => left.ToLower().Trim(),
                    right => right.ToLower().Trim(),
                    (l, r) => l)
                .Any();

            return this.permissionsSplit.Length <= 0 || hasPermission;
        }

        public string Permissions
        {
            get
            {
                return this.permissions ?? string.Empty;
            }
            set
            {
                this.permissions = value;
                this.permissionsSplit = this.SplitString(value);
            }
        }

        private string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            return original.Split(',')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToArray();
        }

        public override object TypeId
        {
            get
            {
                return this._typeId;
            }
        }
    }
}