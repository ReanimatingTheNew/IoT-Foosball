using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace EmbeddedApp
{
    internal class Goal
    {
        public enum Side
        {
            Red = 0,
            Yellow = 1
        }

        public static RootObject Results = new RootObject
        {
            VanityString = "There is no current match on this table."
        };

        private readonly MainPage _mainPage;

        public Goal(MainPage mainPage)
        {
            _mainPage = mainPage;
        }

        public void Post(Side side)
        {
            var request =
                (HttpWebRequest)
                    WebRequest.Create(
                        "https://foosball3.azurewebsites.net/api/Goals/307936a0-9375-4c32-8f31-38a746915b63/ " +
                        (int) side);
            request.Method = "PUT";
            request.BeginGetResponse(PostCallBack, request);
        }

        private void PostCallBack(IAsyncResult result)
        {
            var request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    var response = request.EndGetResponse(result);
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        Results = JsonConvert.DeserializeObject<RootObject>(sr.ReadToEnd());
                    }
                }
                catch (Exception)
                {
                    Results = new RootObject
                    {
                        VanityString = "There is no current match on this table."
                    };
                }
                _mainPage.UpdateGui();
            }
        }
    }
}