using System.Web;

class CookieWithOverlyBroadPath
{
    static public void AddCookie()
    {
        //{fact rule=incorrect-authentication-exploitation@v1.0 defects=1}
        HttpCookie cookie = new HttpCookie("sessionID")
        {
            Path = "/"
        };

        //{/fact}
    }
}
