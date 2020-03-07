namespace Ateneo
{
    public class ZoneDetails
    {
        public string name { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string imageUrl { get; set; }

        public ZoneDetails(string name, string description, 
                    string iconUrl, string imageUrl) {
            this.name = name;
            this.description = description;
            this.iconUrl = iconUrl;
            this.imageUrl = imageUrl;
        }
    }
}
