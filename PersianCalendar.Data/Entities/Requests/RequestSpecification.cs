namespace PersianCalendar.Data.Entities.Requests
{
    public class RequestSpecification
    {
        public RequestSpecification(string endpoint)
        {
            Endpoint = endpoint;
            QueryParameters = new Dictionary<string, string>();
        }
        public string Endpoint { get; set; }

        public string Body { get; set; }

        public Dictionary<string, string> QueryParameters { get; set; }
    }
}
