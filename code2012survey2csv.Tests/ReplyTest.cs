using System;
using code2012survey2csv.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace code2012survey2csv.Tests
{
    [TestClass]
    public class ReplyTest
    {
        [TestMethod]
        public void DeserializeTest()
        {
            var json = @"{
                ""id"":123,
                ""app_name"":""some app name"",
                ""created_at"":""2012-07-17T03:44:45Z"",
                ""why"":""[true,false,true,true,false,true]"",
                ""locale"":""\u5317\u6d77\u9053"",
                ""how_year"":21,
                ""free_comment"":""\u3053\u308c\u306f\r\n\u30c6\u30b9\u30c8"",
                ""language"":""Ruby""
                }";
            var reply = JsonConvert.DeserializeObject<Reply>(json);

            reply.id.Is(123);
            reply.app_name.Is("some app name");
            reply.locale.Is("北海道");
            reply.how_year.Is(21);
            reply.why.Is("[true,false,true,true,false,true]");
            reply.created_at.ToLocalTime().Is(DateTime.Parse("2012/7/17 12:44:45"));
            reply.free_comment.Is("これは\r\nテスト");
        }

        [TestMethod]
        public void DeserializeTest_with_NullFields()
        {
            var json = @"{
                ""id"":123,
                ""app_name"":null,
                ""created_at"":""2012-07-17T03:44:45Z"",
                ""why"":null,
                ""locale"":null,
                ""how_year"":null,
                ""free_comment"":null,
                ""language"":null
                }";
            var reply = JsonConvert.DeserializeObject<Reply>(json);

            reply.id.Is(123);
            reply.app_name.IsNull();
            reply.locale.IsNull();
            reply.how_year.IsNull();
            reply.why.IsNull();
            reply.created_at.ToLocalTime().Is(DateTime.Parse("2012/7/17 12:44:45"));
            reply.free_comment.IsNull();
        }

        [TestMethod]
        public void GetChoicesTest()
        {
            var reply = new Reply { why = "[true,false]" };
            reply.GetChoices().Is(true, false);
        }

        [TestMethod]
        public void GetChoicesTest_with_null()
        {
            var reply = new Reply { why = "null" };
            reply.GetChoices().Is();
        }

        [TestMethod]
        public void GetChoicesTest_with_BadFormat()
        {
            var reply = new Reply { why = @"{""foo"":'bar'}" };
            reply.GetChoices().Is();
        }

        [TestMethod]
        public void GetChoiceOfTest()
        {
            var reply = new Reply { why = "[true,false]" };
            reply.GetChoiceOf(0).Is(true);
            reply.GetChoiceOf(1).Is(false);
            reply.GetChoiceOf(2).Is(false);
        }
    }
}
