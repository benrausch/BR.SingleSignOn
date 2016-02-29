namespace BR.SingleSignOn
{
    using System;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Security.Claims;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.HttpRequest;
    using Sitecore.Security.Accounts;

    public class UserResolver : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (Sitecore.Context.User != null && !Sitecore.Context.User.IsAuthenticated)
            {
                SessionSecurityToken token;
                if (System.IdentityModel.Services.FederatedAuthentication.SessionAuthenticationModule.TryReadSessionTokenFromCookie(out token))
                {
                    var virtualUser = BuildVirtualUser(token.ClaimsPrincipal);
                    AuthenticateVirtualUser(virtualUser);
                }
            }
        }

        private User BuildVirtualUser(ClaimsPrincipal claimsPrincipal)
        {
            Assert.ArgumentNotNull(claimsPrincipal, "claimsPrincipal");

            var claims = claimsPrincipal.Claims.ToList();
            var isStaff = claims.Any(x => x.Type.Equals("StaffType", StringComparison.OrdinalIgnoreCase));
            var user = Sitecore.Security.Authentication.AuthenticationManager.BuildVirtualUser(@"extranet\authenticated", true);

            if (isStaff)
            {
                user.Roles.Add(Role.FromName(@"extranet\staff"));
            }

            return user;
        }

        private bool AuthenticateVirtualUser(User user)
        {
            Assert.ArgumentNotNull(user, "user");

            var loginResult = Sitecore.Security.Authentication.AuthenticationManager.LoginVirtualUser(user);

            return loginResult;
        }
    }
}