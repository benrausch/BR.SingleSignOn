namespace BR.SingleSignOn
{
    using System.IdentityModel.Tokens;

    public class SessionAuthenticationModule : System.IdentityModel.Services.SessionAuthenticationModule
    {
        protected override void SetPrincipalFromSessionToken(SessionSecurityToken sessionSecurityToken)
        {
            //Do nothing
        }
    }
}
