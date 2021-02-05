using System.Collections.Generic;

namespace AwsDotnetCsharp
{
     public class VolunteerComment
    {
        public string comment { get; set; }
        public List<string> imageUrls { get; set; }

        public VolunteerComment(string Comment, List<string> ImgURLs)
        {
            comment = Comment;
            imageUrls = ImgURLs;
        }
        public VolunteerComment() { }
    }

}