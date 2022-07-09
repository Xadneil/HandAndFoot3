namespace HAF.WebServer.Models
{
    public class SessionResponseModel
    {
        public int SessionId { get; private set; }
        public string Task { get; private set; }

        public SessionResponseModel(int sessionId, string task)
        {
            SessionId = sessionId;
            Task = task;
        }
    }
}
