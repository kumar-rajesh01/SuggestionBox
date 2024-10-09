namespace SuggestionBox.Helper
{
    public static class CommonHelper
    {
        public static string GetRequestIP(this IHttpContextAccessor httpContextAccessor)
        {
            string ip;
            var headers = httpContextAccessor.HttpContext.Request.Headers.ToList();
            if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
            {
                var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                ip = header.Remove(header.IndexOf(':')).ToString();
            }
            else
            {
                ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            return ip.ToString();
        }
    }
}
