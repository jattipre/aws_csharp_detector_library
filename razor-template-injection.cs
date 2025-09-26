namespace RazorVulnerableApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string inert, string razorTpl)
        {
            // WARNING This code is vulnerable on purpose: do not use in production and do not take it as an example!
            //{fact rule=code-injection@v1.0 defects=1}
            // ruleid: razor-template-injection
            ViewBag.RenderedTemplate = Razor.Parse(razorTpl);
            ViewBag.Template = razorTpl;
            return View();
        }

        private ActionResult View()
        {
            throw new NotImplementedException();
        }

        //{/fact}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index2(string inter, string razorTpl)
        {
            var junk = someFunction(razorTpl);
            // WARNING This code is vulnerable on purpose: do not use in production and do not take it as an example!

            //{fact rule=code-injection@v1.0 defects=0}
            // ok: razor-template-injection
            ViewBag.RenderedTemplate = Razor.Parse(junk);
            ViewBag.Template = razorTpl;
            return View();
        }

        private string someFunction(string razorTpl)
        {
            throw new NotImplementedException();
        }
    }

    internal class Razor
    {
        internal static object Parse(string razorTpl)
        {
            throw new NotImplementedException();
        }
    }

    internal class ViewBag
    {
        public static object? RenderedTemplate { get; internal set; }
        public static string? Template { get; internal set; }
    }

    public class ActionResult
    {
    }

    internal class ValidateInputAttribute : Attribute
    {
        private bool v;

        public ValidateInputAttribute(bool v)
        {
            this.v = v;
        }
    }

    internal class HttpPostAttribute : Attribute
    {
    }

    public class Controller
    {
    }
}            //{/fact}
