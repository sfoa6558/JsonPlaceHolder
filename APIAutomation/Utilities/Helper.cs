using RestSharp;

namespace APIAutomation
{
    class Helper
    {

        //Creates request with Json Payload
        public RestRequest CreateComplexRequest(RestRequest request, JsonPlaceHolderModel model)
        {
            request.AddJsonBody(model);
            return request;
        }

      

    }
}
