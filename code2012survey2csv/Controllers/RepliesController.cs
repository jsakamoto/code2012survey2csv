using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using code2012survey2csv.Models;
using Newtonsoft.Json;

namespace code2012survey2csv.Controllers
{
    public class RepliesController : ApiController
    {
        private static Reply[] GetRepliesFromAPISite()
        {
            var urlOfAPI = ConfigurationManager.AppSettings["UrlOfAPI"];
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            var json = webClient.DownloadString(urlOfAPI);
            var replies = JsonConvert.DeserializeObject<Reply[]>(json);
            return replies;
        }

        // GET api/replies
        [HttpGet]
        public IQueryable<Reply> Replies()
        {
            var replies = GetRepliesFromAPISite();
            return replies.AsQueryable();
        }

 
        // GET api/ascsv
        [HttpGet]
        public HttpResponseMessage AsCsv(string encoding = "utf-8", bool safeformat = false, bool header = true)
        {
            Func<string, string> esacape1 = s => s
                .Replace("\\", "\\ux005c")
                .Replace("\"", "\\ux0022")
                .Replace(",", "\\ux002c")
                .Replace("\r", "\\ux000d")
                .Replace("\n", "\\ux000a");
            Func<string, string> esacape2 = s => s.Replace("\"", "\"\"");
            Func<string, string> esacape = safeformat ? esacape1 : esacape2;

            Func<object, string> quote = (a) => "\"" + esacape((a ?? "").ToString()) + "\"";
            
            var csvRows = GetRepliesFromAPISite()
                .Select(r => string.Join(",",
                    new object[]{ 
                        r.id,
                        r.app_name,
                        r.GetChoiceOf(0),
                        r.GetChoiceOf(1),
                        r.GetChoiceOf(2),
                        r.GetChoiceOf(3),
                        r.GetChoiceOf(4),
                        r.GetChoiceOf(5),
                        r.created_at.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"),
                        r.locale,
                        r.how_year,
                        r.why,
                        r.free_comment,
                        r.language
                    }
                    .Select(quote)));

            var csvHeaderRow = string.Join(",",
                    new []{ 
                        "id",
                        "app_name",
                        "c1of_why",
                        "c2of_why",
                        "c3of_why",
                        "c4of_why",
                        "c5of_why",
                        "c6of_why",
                        "created_at",
                        "locale",
                        "how_year",
                        "why",
                        "free_comment",
                        "language"
                    }
                    .Select(quote));

            csvRows = header ? new[]{csvHeaderRow}.Concat(csvRows) : csvRows;

            var response = new HttpResponseMessage();
            response.Content = new StringContent(
                content: string.Join("\r\n", csvRows),
                encoding: Encoding.GetEncoding(encoding),
                mediaType: "text/csv"
                );

            return response;
        }
    }
}