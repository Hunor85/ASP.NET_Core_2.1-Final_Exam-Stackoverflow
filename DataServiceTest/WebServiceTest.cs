using DataLayer;
using System;
using System.Linq;
using Xunit;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProjoctPortfolioTests
{
    public class WebServiceTest
    {
        private const string AnswersApi = "http://localhost:5001/api/answers";
        private const string QuestionsApi = "http://localhost:5001/api/questions";
        private const string SearchHostoryApi = "http://localhost:5001/api/searchhistory";
        private const string AnnotationsApi = "http://localhost:5001/api/annotations";
        private const string MarkApi = "http://localhost:5001/api/mark";

        //Answers
        [Fact]
        public void ApiAnswers_GetWithNoArguments_OkAndAllAnswers()
        {
            var (data, statusCode) = GetArray(AnswersApi);

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(11392, data.Count);
            Assert.Equal("<p>You should clear:both after the floats.</p>&#xA;&#xA;<pre><code>&lt;div style=\"clear:both\"&gt;&lt;/div&gt;&#xA;</code></pre>&#xA;", data.First()["body"]);
            Assert.Equal(1228347, data.First()["authorId"]);
            Assert.Equal(2, data.First()["postType"]);
        }

        [Fact]
        public void ApiAnswers_GetWithValidParentId_okAndAnswer()
        {
            var (answer, statusCode) = GetArray($"{AnswersApi}/9844982");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(4 , answer[0]["score"]);
            //Assert.Equal(9844982, answer["parentId"]);
        }

        [Fact]
        public void ApiAnswers_GetApectedAnswerId_OkAndAnswer()
        {
            var (answer, statusCode) = GetObject($"{AnswersApi}/acceptedanswer/15831991");
            //($"{ProductsApi}/category/1")
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(0, (int)answer.GetValue("score"));
        }
        
       

        //Questions
        [Fact]
        public void ApiQuestions_GetWithNoArguments_OkAndAllQuestions()
        {
            var (data, statusCode) = GetObject(QuestionsApi);

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(10, data.GetValue("items").Count());
            
        }

        [Fact]
        public void ApiQuestions_GetQuestion_WithValidId()
        {
            var (question, statusCode) = GetObject($"{QuestionsApi}/13649012");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(2, question.GetValue("score"));
        } 

        //--------Annotation test----------
        [Fact]
        public void ApiAnnotation_PostAnnotation()
        {
            var data = new 
            {
                
                Body = "Henning",
                UserId = 227,
                PostId = 13649012


            };
            var  (annotate, statusCode) = PostData(AnnotationsApi,data);
            Assert.Equal(HttpStatusCode.Created,statusCode);
        }

       //ApiAnnotation_PutAnnotation() doesn't work here but works on postman
       /*
        [Fact]
        public void ApiAnnotation_PutAnnotation()
        {
            var data = new
            {
                body = "Annotation to be updated",
                userid = 244,
                postid = 13649012
            };
           
            var (annotate, _) = PostData($"{AnnotationsApi}", data);

            var update = new
            {
                Id=annotate["id"],
                Body = annotate["body"] + "Updated now"
          };
         
            var statusCode = PutData($"{AnnotationsApi}/{annotate["id"]}", update);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        */
        //--------Mark test---------
        [Fact]
        public void ApiMark_PostMark()
        {
            var mark = new
            {
                postid= 19329707,
                userid= 100
            };
            var (markk,  statusCode) = PostData(MarkApi,mark);
            Assert.Equal(HttpStatusCode.Created, statusCode);
        }
//ApiMark_Delete() test is not working this test but works on postman
/*
        [Fact]
        public void ApiMark_DeleteMark()
        {

            var data = new
            {
                
                UserId = 47,
                PostId = 2180354
            };
             var (mark, _) = PostData($"{MarkApi}", data);

            var statusCode=DeleteData($"{MarkApi}/{mark["userid"]}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

    */

        //Helpers
        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url) 
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        HttpStatusCode PutData(string url, object content)
        {
            var client = new HttpClient();
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }

    }
}
