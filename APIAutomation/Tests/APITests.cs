using System;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Collections.Generic;
using System.Net;
using Assert = NUnit.Framework.Assert;
using APIAutomation;
using System.Configuration;


namespace JsonPlaceHolder
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class APITests
    {

        RestRequest request;
        IRestResponse response;

        //allows tests to run in parallel without interfering with each other
        private sealed class TestScope : IDisposable
        {
            public RestClient client { get; set; }

            public TestScope()
            {
                client = new RestClient(ConfigurationManager.AppSettings["base_url"]);

            }

            public void Dispose()
            {

               
            }
        }


        [Test]
        public void Retrieve_All_Json_Posts()
        {
            using (var scope = new TestScope())
            {
                response = scope.client.Execute(new RestRequest("/posts", Method.GET));
                var jsonModel = new JsonDeserializer().Deserialize<List<JsonPlaceHolderPostModel>>(response);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonModel.Count, Is.GreaterThan(0));
            }

        }

        //parameterizing tests
        [TestCase(5, "Test", "Body",  TestName = "Happy Path Test")]
        [TestCase(6, "", "Body",  TestName = "Check if title is required")]
        [TestCase(12, "title", "",  TestName = "Check if body is required")]
        [TestCase(13,"@#$^&*?<>|:+-=!()", "Body", TestName = "Check if characters are allowed" )]
        [TestCase(-10, "title", "Body", TestName = "Check if negative integers are allowed")]
        public void Create_A_Json_Post(int userid, string title, string body)
        {
            using (var scope = new TestScope())
            {
                request = new Helper().CreateComplexRequest(new RestRequest("/posts", Method.POST), new JsonPlaceHolderPostModel(userid,title,body));
                response = scope.client.Execute(request);
                var jsonModel = new JsonDeserializer().Deserialize<List<JsonPlaceHolderPostModel>>(response);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonModel[0].UserId, Is.EqualTo(userid));
                Assert.That(jsonModel[0].id, Is.TypeOf(typeof(int)));
                Assert.That(jsonModel[0].Title, Is.EqualTo(title));
                Assert.That(jsonModel[0].Body, Is.EqualTo(body));
                
            }
        }

        [Test]
        public void Delete_A_Json_Post()
        {
            using (var scope = new TestScope())
            {
                response = scope.client.Execute(new RestRequest("/posts/59", Method.DELETE));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }

        }

        
        [Test]
        public void Update_A_Json_Post_With_Put()
        {
            using (var scope = new TestScope())
            {
                request = new Helper().CreateComplexRequest(new RestRequest("/posts/3", Method.PUT), new JsonPlaceHolderPostModel(6,"this test", "foo test"));
                response = scope.client.Execute(request);
                var jsonModel = new JsonDeserializer().Deserialize<List<JsonPlaceHolderPostModel>>(response);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonModel[0].UserId, Is.EqualTo(6));
                Assert.That(jsonModel[0].id, Is.TypeOf(typeof(int)));
                Assert.That(jsonModel[0].Title, Is.EqualTo("this test"));
                Assert.That(jsonModel[0].Body, Is.EqualTo("foo test"));
                

            }
        }

        [TestCase("Name", "c_jung@yahoo.com", "body", TestName = "Happy Path Test Comments")]
        [TestCase("", "c_jung@yahoo.com", "body", TestName = "Check if name is required")]
        [TestCase("name", "c_jung@yahoo.com", "", TestName = "Check if comments body is required")]
        [TestCase("name", "", "body", TestName = "Check if email is required")]
        [TestCase("name", "c_jung@yahoo", "body", TestName = "Check if invalid email format is accepted")]
        [TestCase("name", "c_jung@yahoo.com", "@#$^&*?<>|:+-=!()", TestName = "Check if characters are allowed")]
        public void Create_A_Json_Post_With_Comments(string name, string email, string body)
        {
            using (var scope = new TestScope())
            {
                request = new Helper().CreateComplexRequest(new RestRequest("/posts/7/comments", Method.POST), new JsonPlaceHolderPostCommentsModel(name, email, body));
                response = scope.client.Execute(request);
                var jsonModel = new JsonDeserializer().Deserialize<List<JsonPlaceHolderPostCommentsModel>>(response);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonModel[0].postId, Is.TypeOf(typeof(int)));
                Assert.That(jsonModel[0].id, Is.TypeOf(typeof(int)));
                Assert.That(jsonModel[0].Name, Is.EqualTo(name));
                Assert.That(jsonModel[0].Email, Is.EqualTo(email));
                Assert.That(jsonModel[0].Body, Is.EqualTo(body));
            }

        }

       [Test]
        public void Retrieve_A_Json_Post_With_Comments()
        {
            using (var scope = new TestScope())
            {
                request = new RestRequest("/posts/1/comments", Method.GET);
                response = scope.client.Execute(request);
                var jsonModel = new JsonDeserializer().Deserialize<List<JsonPlaceHolderPostCommentsModel>>(response);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonModel.Count, Is.GreaterThan(0));
            }

        }

        [TestCase(2, 18, "voluptate et itaque vero tempora molestiae", "eveniet quo quis\nlaborum totam consequatur non dolor\nut et est repudiandae\nest voluptatem vel debitis et magnam", TestName = "Retrieve Post Id 18")]
        [TestCase(3, 22, "dolor sint quo a velit explicabo quia nam", "eos qui et ipsum ipsam suscipit aut\nsed omnis non odio\nexpedita earum mollitia molestiae aut atque rem suscipit\nnam impedit esse", TestName = "Retrieve Post Id 22")]
        public void Retrieve_Json_Post(int userId, int id, string title, string body)
        {
            using (var scope = new TestScope())
            {
                response = scope.client.Execute(new RestRequest("/posts/" + id, Method.GET));
                var jsonModel = new JsonDeserializer().Deserialize<List<JsonPlaceHolderPostModel>>(response);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonModel[0].UserId, Is.EqualTo(userId));
                Assert.That(jsonModel[0].Title, Is.EqualTo(title));
                Assert.That(jsonModel[0].Body, Is.EqualTo(body));

            }
        }

        [Test]
        public void Create_A_Json_Post_With_Comments_And_No_Payload()
        {
            using (var scope = new TestScope())
            {
                request = new Helper().CreateComplexRequest(new RestRequest("/posts/1/comments", Method.POST), new JsonPlaceHolderPostCommentsModel());
                response = scope.client.Execute(request);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
               
            }

        }


        [Test]
        public void Retrieve_Json_Post_with_incorrect_method()
        {
            using (var scope = new TestScope())
            {
                response = scope.client.Execute(new RestRequest("/posts/4", Method.POST));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
      
            }
        }

        [Test]
        public void Create_A_Json_Post_With_No_Payload()
        {
            using (var scope = new TestScope())
            {
                
                request = new Helper().CreateComplexRequest(new RestRequest("/posts", Method.POST), new JsonPlaceHolderPostModel());
                response = scope.client.Execute(request);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            }
        }

        [Test]
        public void Create_A_Json_Post_With_String_UserId()
        {
            using (var scope = new TestScope())
            {

                request = new Helper().CreateComplexRequest(new RestRequest("/posts", Method.POST), new JsonPlaceHolderPostModelWithStringUserId("Title", "Test", "1"));

                response = scope.client.Execute(request);
                var jsonModel = new JsonDeserializer().Deserialize<List<JsonPlaceHolderPostModel>>(response);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonModel[0].UserId, Is.EqualTo(0));
            }
        }

        [Test]
        public void Create_A_Json_Post_With_Incorrect_Resource()
        {
            using (var scope = new TestScope())
            {

                request = new Helper().CreateComplexRequest(new RestRequest("/post", Method.POST), new JsonPlaceHolderPostModel(109, "title", "body"));
                response = scope.client.Execute(request);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
               
            }
        }

        [Test]
        public void Verify_Json_Header()
        {
            using (var scope = new TestScope())
            {
                request = new Helper().CreateComplexRequest(new RestRequest("/posts", Method.POST), new JsonPlaceHolderPostModel(5, "title", "body"));
                response = scope.client.Execute(request);
                Assert.That(response.ContentType, Is.EqualTo("application/json; charset=utf-8"));



            }
        }


    }

}
