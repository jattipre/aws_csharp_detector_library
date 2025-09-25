using System.Web;

class CookieWithOverlyBroadPathFix
{
    //{fact rule=incorrect-authentication-exploitation@v1.0 defects=0}
    static public void AddCookie()
    {
        HttpCookie cookie = new HttpCookie("sessionID");
        cookie.Path = "/ebanking";
    }
    //{/fact}
}