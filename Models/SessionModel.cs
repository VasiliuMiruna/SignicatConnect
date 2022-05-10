using System;

namespace SignicatIdentification.Models
{
    public class SessionModel
    {
        public string id { get; set; }
        public string url { get; set; }
        public string status { get; set; }
        public DateTime created { get; set; }
        public DateTime expires { get; set; }
        public string[] allowedProviders { get; set; }
        public string language { get; set; }
        public string flow { get; set; }
        public string[] include { get; set; }
        public Redirectsettings redirectSettings { get; set; }
        public Audittrail auditTrail { get; set; }
    }

    public class Redirectsettings
    {
        public string successUrl { get; set; }
        public string abortUrl { get; set; }
        public string errorUrl { get; set; }
    }

    public class Audittrail
    {
        public Event[] events { get; set; }
    }

    public class Event
    {
        public int id { get; set; }
        public string name { get; set; }
        public string eventType { get; set; }
        public DateTime timestamp { get; set; }
        public Eventdata eventData { get; set; }
    }

    public class Eventdata
    {
        public string ipAddress { get; set; }
        public string requestUrl { get; set; }
    }

}
