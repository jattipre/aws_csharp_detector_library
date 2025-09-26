using System;
namespace ServerSideRequestForgery
{

    //error related object
    public class Ssrf2
    {
        private object result;
        #region Pattern 1
        //{fact rule=server-side-request-forgery@v1.0 defects=1}
        // ruleid: ssrf
        public void RestClientGet(string host)
        {
            try
            {
                RestClient client = new RestClient(host);

                var request = new RestRequest("/");
                var response = client.Get(request);

               // result = response.Content;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=0}
        // ok: ssrf
        public void RestClientGet1(string host)
        {
            try
            {
                RestClient client = new RestClient("constant");

                var request = new RestRequest("/");
                var response = client.Get(request);

               // result = response.Content;

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
        public void RestClientGetWithStringConcatenation(string host)
        {
            string uri = host + "constant";

            try
            {
                RestClient client = new RestClient(uri);

                var request = new RestRequest("/");
                var response = client.Get(request);

               // result = response.Content;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=0}
        // ok: ssrf
        public void RestClientGetWithStringConcatenation1(string host)
        {
            string uri = "constant";

            try
            {
                RestClient client = new RestClient(uri);

                var request = new RestRequest("/");
                var response = client.Get(request);

             //   result = response.Content;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=1}
        // ruleid: ssrf
        public void RestClientGetWithUri(string host)
        {
            Uri uri = new Uri(host + "constant");

            try
            {
                RestClient client = new RestClient(uri);

                var request = new RestRequest("/");
                var response = client.Get(request);

            //    result = response.Content;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        //{/fact}

        //{fact rule=server-side-request-forgery@v1.0 defects=0}
        // ok: ssrf
        public void RestClientGetWithUri1(string host)
        {
            Uri uri = new Uri("constant");

            try
            {
                RestClient client = new RestClient(uri);

                var request = new RestRequest("/");
                var response = client.Get(request);

               // result = response.Content;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        #endregion
    }

    internal class RestRequest
    {
        private string v;

        public RestRequest(string v)
        {
            this.v = v;
        }
    }

    internal class RestClient
    {
        private string host;
        private Uri uri;

        public RestClient(string host)
        {
            this.host = host;
        }

        public RestClient(Uri uri)
        {
            this.uri = uri;
        }

        internal object Get(RestRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
        //{/fact}
