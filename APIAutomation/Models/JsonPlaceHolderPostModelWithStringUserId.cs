
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

    }
}
