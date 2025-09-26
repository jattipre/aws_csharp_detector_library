using System.Text.RegularExpressions;
using System;
namespace RegularExpressionsDosInfiniteTimeout
{
    
    public class RegularExpressionsDosInfiniteTimeout
    {
        private static string pattern;

        // ok
        Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
        
        //{fact rule=regular-expression-dos-infinite-timeout@v1.0 defects=1}
        // ruleid: regular-expression-dos-infinite-timeout
        Regex rgx0 = new Regex(pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(10));
        
        //{/fact}

        //{fact rule=regular-expression-dos-infinite-timeout@v1.0 defects=1}
        // ruleid: regular-expression-dos-infinite-timeout
        Regex rgx1 = new Regex(pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(10));

        //{/fact}

        //{fact rule=regular-expression-dos-infinite-timeout@v1.0 defects=1}
        // ruleid: regular-expression-dos-infinite-timeout
        Regex rgx2 = new Regex(pattern, RegexOptions.IgnoreCase, TimeSpan.FromMinutes(1));
        
        //{/fact}

        //{fact rule=regular-expression-dos-infinite-timeout@v1.0 defects=1}
        // ruleid: regular-expression-dos-infinite-timeout
        Regex rgx3 = new Regex(pattern, RegexOptions.IgnoreCase, TimeSpan.FromHours(1));
    }
}
        //{/fact}
