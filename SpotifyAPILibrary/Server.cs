using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SpotifyAPILibrary
{
    internal class Server
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";

        public static async Task<string> HandleIncomingConnections()
        {
            bool runServer = true;

            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // If URL requested
                if (req.Url.AbsolutePath == "/auth")
                {
                    if (req.Url.Query.Contains("error"))
                    {
                        throw new Exception("failed request");
                    }

                    string? state = HttpUtility.ParseQueryString(req.Url.Query).Get("state");

                    if (state != null && state == "state")
                    {
                        string code = HttpUtility.ParseQueryString(req.Url.Query).Get("code");

                        // Write the response info
                        byte[] data = Encoding.UTF8.GetBytes("You can close this Page now.");
                        resp.ContentType = "text/html";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;

                        // Write out to the response stream (asynchronously), then close it
                        await resp.OutputStream.WriteAsync(data, 0, data.Length);
                        resp.Close();

                        return code;
                    }

                    runServer = false;
                    resp.Close();
                }
                else
                {
                    resp.Close();
                }
            }
            return null;
        }
    }
}
