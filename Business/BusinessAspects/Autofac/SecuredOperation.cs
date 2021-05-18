using Castle.DynamicProxy;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;
        private HttpRequest _httpRequest;


        public SecuredOperation(string roles)
        {

            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _httpRequest = _httpContextAccessor.HttpContext.Request;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();

            var value = _httpRequest.HttpContext.Session.GetString("Staff");
            StaffSession staffSession = value == null ? default(StaffSession) : JsonConvert.DeserializeObject<StaffSession>(value);

            foreach (var role in _roles)
            {
                if (staffSession != null)
                    foreach (var claim in staffSession.OperationClaims)
                    {
                        if (role == claim.Name)
                        {
                            return;
                        }
                    }
            }

            throw new Exception("AuthorizationDenied!");
        }
    }
}
