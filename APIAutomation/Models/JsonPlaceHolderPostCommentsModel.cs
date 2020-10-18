
namespace APIAutomation
{
    public class JsonPlaceHolderPostCommentsModel : JsonPlaceHolderModel
    {

        public int postId { get; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Body { get; set; }

        public JsonPlaceHolderPostCommentsModel(string name, string email, string body)
        {
            this.Name = name;
            this.Email = email;
            this.Body = body;
          
        }

        public JsonPlaceHolderPostCommentsModel()
        {


        }

    }




}
