using System.Security.Claims;

namespace api.extensions;

public static class ClaimsExtensions
{
 public static string GetUsername(this ClaimsPrincipal user)
 {
  return user.Claims.SingleOrDefault(claim => claim.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
 }
}