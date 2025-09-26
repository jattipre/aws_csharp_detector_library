using System;
using System.Web;

namespace System.Web
{
    public class HttpCookie
    {
        public HttpCookie(string name) { }
        public string Value { set { } }
        public DateTime Expires { set { } }

        public string Domain { get; internal set; }
        public string Path { get; internal set; }
    }
}

namespace PersistentCookie
{
    class Main
    {
        static public void AddCookie()
        {
            HttpCookie aCookie = new HttpCookie("lastVisit");
            aCookie.Value = DateTime.Now.ToString();
            //{fact rule=sensitive-information-leak@v1.0 defects=1}
            aCookie.Expires = DateTime.Now.AddDays(1);  // BAD
            //{/fact}

            //{fact rule=sensitive-information-leak@v1.0 defects=1}
            aCookie.Expires = DateTime.Now.Add(new TimeSpan(1000));  // BAD
            //{/fact}

            //{fact rule=sensitive-information-leak@v1.0 defects=1}
            aCookie.Expires = DateTime.Now.AddMinutes(10);  // BAD
            //{/fact}

            //{fact rule=sensitive-information-leak@v1.0 defects=0}
            aCookie.Expires = DateTime.Now.AddMinutes(-10.9);  // GOOD
            //{/fact}

            //{fact rule=sensitive-information-leak@v1.0 defects=0}
            aCookie.Expires = DateTime.Now.AddSeconds(109);  // GOOD
            //{/fact}

            //{fact rule=sensitive-information-leak@v1.0 defects=0}
            aCookie.Expires = DateTime.Now;  // GOOD
            //{/fact}
        }
    }
}