using System.Text.RegularExpressions;

namespace RegularExpressionsDos
{
    public class RegularExpressionsDos
    {
        //{fact rule=regular-expression-dos@v1.0 defects=1}
        // ruleid: regular-expression-dos
        public void ValidateRegex(string search)
        {
            Regex rgx = new Regex("^A(B|C+)+D");
            rgx.Match(search);

        }

        //{/fact}

        //{fact rule=regular-expression-dos@v1.0 defects=1}
        // ruleid: regular-expression-dos
        public void ValidateRegex2(string search)
        {
            Regex rgx = new Regex("^A(B|C+)+D", new RegexOptions { });
            rgx.Match(search);

        }

        //{/fact}

        //{fact rule=regular-expression-dos@v1.0 defects=0}
        // ok: regular-expression-dos
        public void ValidateRegex3(string search)
        {
            Regex rgx = new Regex("^A(B|C+)+D", new RegexOptions { }, TimeSpan.FromSeconds(4));
            rgx.Match(search);

        }

        //{/fact}

        //{fact rule=regular-expression-dos@v1.0 defects=1}
        // ruleid: regular-expression-dos
        public void Validate4(string search)
        {
            var pattern = @"^A(B|C+)+D";
            var result = Regex.Match(search, pattern);
        }

        //{/fact}

        //{fact rule=regular-expression-dos@v1.0 defects=1}
        // ruleid: regular-expression-dos
        public void Validate5(string search)
        {
            var pattern = @"^A(B|C+)+D";
            var result = Regex.Match(search, pattern, new RegexOptions { });
        }

        //{/fact}

        //{fact rule=regular-expression-dos@v1.0 defects=0}
        // ok: regular-expression-dos
        public void Validate6(string search)
        {
            var pattern = @"^A(B|C+)+D";
            var result = Regex.Match(search, pattern, new RegexOptions { }, TimeSpan.FromSeconds(4));
        }
    }
}
        //{/fact}
