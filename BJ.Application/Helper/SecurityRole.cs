using BJ.Contract;
using Microsoft.AspNetCore.Authorization;

namespace BJ.Application.Helper
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class SecurityRole : AuthorizeAttribute
    {

        public AuthorizeRole[] AuthorizedRoles { get; set; }

        public SecurityRole(params AuthorizeRole[] roles)
        {

            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("roles");

            this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));

        }
    }
}
