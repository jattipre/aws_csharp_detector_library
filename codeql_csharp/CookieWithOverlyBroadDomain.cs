using System.Web;

class CookieWithOverlyBroadDomain
{
    static public void AddCookie()
    {
        //{fact rule=incorrect-authentication-exploitation@v1.0 defects=1}
        System.Web.HttpCookie cookie1 = new HttpCookie("sessionID");
        cookie1.Domain = "online-bank.com";

        HttpCookie cookie2 = new HttpCookie("sessionID");
        cookie2.Domain = ".ebanking.online-bank.com";
        //{/fact}
    }
}
