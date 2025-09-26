using RazorVulnerableApp.Controllers;
using static AtLeast21Controller;
using Microsoft.AspNetCore.Mvc;

//{fact rule=missing-authorization@v1.0 defects=1}
// ruleid: missing-or-broken-authorization
[AllowAnonymous]
public class AtLeast21Controller : Controller
{

    public IActionResult Index() => View();

    private IActionResult View()
    {
        throw new NotImplementedException();
    }

    public interface IActionResult
    {
    }
}
//{/fact}
internal class AllowAnonymousAttribute : Attribute
{
}

//{fact rule=missing-authorization@v1.0 defects=0}
// ok: missing-or-broken-authorization
[Authorize(Roles = "LegalAdultGroup")]
public class AtLeast21Controller2 : Controller
{
//{/fact}
   public IActionResult Index() => View();

    private IActionResult View()
    {
        throw new NotImplementedException();
    }
}

internal class AuthorizeAttribute : Attribute
{
    public string Roles { get; set; }
    public string Policy { get; set; }
}


//{fact rule=missing-authorization@v1.0 defects=0}
// ok: missing-or-broken-authorization
[Authorize(Policy = "AtLeast21")]
public class AtLeast21Controller3 : Controller
{
//{/fact}
    public IActionResult Index() => View();

    private IActionResult View()
    {
        throw new NotImplementedException();
    }
}
