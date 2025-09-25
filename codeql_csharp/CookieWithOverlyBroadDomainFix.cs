using System.Web;

class CookieWithOverlyBroadDomainFix
{
    //{fact rule=incorrect-authentication-exploitation@v1.0 defects=0}
    static public void AddCookie()
    {
        HttpCookie cookie = new HttpCookie("sessionID")
        {
            Domain = "ebanking.online-bank.com"
        };
    }
    //{/fact}
}
