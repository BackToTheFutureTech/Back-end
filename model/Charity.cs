namespace AwsDotnetCsharp
{
     public class Charity
    {
        public string charityId { get; set; }
        public string charityName { get; set; }
        public string imageUrl { get; set; }
        public string charityDescription { get; set; }

        public Charity(string Id, string Name, string ImgURL, string Description)
        {
            charityId = Id;
            charityName = Name;
            imageUrl = ImgURL;
            charityDescription = Description;
        }
        public Charity() { }
    }

}