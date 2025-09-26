using System.Net.Http;
using System;
namespace ServerSideRequestForgery
{
    public class Ssrf
    {
        #region Pattern 1
        //{fact rule=server-side-request-forgery@v1.0 defects=1}
        // ruleid: ssrf
        public void HttpClientAsync(string host)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(host).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=1}
        // ruleid: ssrf
        public void HttpClientAsync2(string host)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(host + "constant").Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=0}
        // ok: ssrf
        public void HttpClientAsync1(string host)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync("constant").Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        #endregion

        #region Pattern 2
        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=1}
        // ruleid: ssrf
        public void HttpClientAsyncWithStringConcatenation(string host)
        {
            string uri = host + "constant";

            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=0}
        // ok: ssrf
        public void HttpClientAsyncWithStringConcatenation1(string host)
        {
            string uri = "constant";

            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=1}
        // ruleid: ssrf
        public void HttpClientAsyncWithUri(string host)
        {
            Uri uri = new Uri(host);

            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=0}
        // ok: ssrf
        public void HttpClientAsyncWithUri1(string host)
        {
            Uri uri = new Uri("constant");

            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        #endregion

        #region Pattern 3
        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=1}
        // ruleid: ssrf
        public void HttpClientStringAsync(string host)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(host).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=0}
        // ok: ssrf
        public void HttpClientStringAsync1(string host)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync("constant").Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        #endregion

        #region Pattern 4
        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=1}
        // ruleid: ssrf
        public void HttpClientStringAsyncWithStringConcatenation(string host)
        {
            string uri = host + "constant";

            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=0}
        // ok: ssrf
        public void HttpClientStringAsyncWithStringConcatenation1(string host)
        {
            string uri = "constant";

            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=1}
        // ruleid: ssrf
        public void HttpClientStringAsyncWithUri(string host)
        {
            Uri uri = new Uri(host);

            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=0}
        // ok: ssrf
        public void HttpClientStringAsyncWithUri1(string host)
        {
            Uri uri = new Uri("constant");

            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        #endregion
    }
}
        //{/fact}
