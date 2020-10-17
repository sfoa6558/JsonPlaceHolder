using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
namespace APIAutomation
{
    public class JsonPlaceHolderPostModelWithStringUserId : JsonPlaceHolderPostModel
    {
      
        public new string UserId { get; set; }



        public JsonPlaceHolderPostModelWithStringUserId(string title, string body, string userId)
        {

            this.Title = title;
            this.Body = body;
            this.UserId = userId;


        }

        public JsonPlaceHolderPostModelWithStringUserId()
        {



        }

    }




}
