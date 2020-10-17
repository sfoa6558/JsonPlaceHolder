﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
namespace APIAutomation
{
    public class JsonPlaceHolderPostModel : JsonPlaceHolderModel
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public int UserId { get; set; }

       

        public JsonPlaceHolderPostModel(int userId, string title, string body)
        {
            this.UserId = userId;
            this.Title = title;
            this.Body = body;



        }

        public JsonPlaceHolderPostModel()
        {



        }

    }




}