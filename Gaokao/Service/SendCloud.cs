using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using Microsoft.EntityFrameworkCore.Internal;

namespace Gaokao.Service.SendCloud
{
    public class SendCloudClient
    {
        public SendCloudClient(string apiUser, string apiKey)
        {
            Api_key = apiKey;
            Api_user = apiUser;
        }
        public string Api_user { get; set; }
        public string Api_key { get; set; }
        public async Task SendEmailAsync(SendCloudMessage message)
        {
            string url = "http://api.sendcloud.net/apiv2/mail/send";

            HttpClient client = null;
            HttpResponseMessage response = null;
            string api_user = Api_user;
            string api_key = Api_key;
            string tos = message.To;
            string from = message.From.Address;
            string fromName = message.From.Name;
            string subject = message.Subject;
            string html = message.HtmlContent;

            try
            {
                client = new HttpClient();

                List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();

                paramList.Add(new KeyValuePair<string, string>("apiUser", api_user));
                paramList.Add(new KeyValuePair<string, string>("apiKey", api_key));
                paramList.Add(new KeyValuePair<string, string>("from", from));
                paramList.Add(new KeyValuePair<string, string>("fromName", fromName));
                paramList.Add(new KeyValuePair<string, string>("to", tos));
                paramList.Add(new KeyValuePair<string, string>("subject", subject));
                paramList.Add(new KeyValuePair<string, string>("html", html));

                response = await client.PostAsync(url, new FormUrlEncodedContent(paramList));
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("result:{0}", result);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                //throw e;
            }
            finally
            {
                if (null != client)
                {
                    client.Dispose();
                }
            }
        }

    }
    public class SendCloudMessage
    {
        public EmailAddress From { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
        public string To { get; set; }

        public void AddTo( EmailAddress emailAddress)
        {
            To += emailAddress.Address;
        }
    }

    public class EmailAddress
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public EmailAddress(string address, string name)
        {
            Address = address;
            Name = name;
        }
        public EmailAddress(string address)
        {
            Address = address;
            Name = null;
        }
    }
}
