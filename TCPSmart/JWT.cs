using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace TCPSmart
{
    class JWT
    {
        public static Dictionary<string, object> ValidarJwtToken(string token)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var jwtToken = new JwtSecurityToken(token);
                foreach (var claim in jwtToken.Claims)
                {
                    if (claim.Type == "exp")
                    {
                        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        result.Add(claim.Type + "_datetime", dtDateTime.AddSeconds(double.Parse(claim.Value)));
                        result.Add(claim.Type, claim.Value);
                    }
                    else
                        result.Add(claim.Type, claim.Value);
                }
            }
            catch { }

            return result;
        }
    }
}