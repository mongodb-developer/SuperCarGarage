namespace SuperCarGarage.Models
{
    public class MongoDBSettings
    {
        public string AtlasURI { get; set; }
        public string DatabaseName { get; set; }
    }

    public class AppSettings
    {
        public string? JWTKey { get; set; }
    }

}
