namespace AwsDotnetCsharp
{
public class Opportunity
    {
        public int id { get; set; }
        public string charity { get; set; }
        public string name { get; set; }
        public string taskType { get; set; }
        public int numVolunteers { get; set; }
        public string date { get; set; }
        public string postcode { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string thumbnail { get; set; }
        public int numRegVolunteers { get; set; }

        public Opportunity(int Id, string Charity, string Name, string TaskType, int NumVolunteers, string Date, string Postcode, string Address1, string Address2, string City, string Description, string Thumbnail, int NumRegVolunteers)
        {
            id = Id;
            charity = Charity;
            name = Name;
            taskType = TaskType;
            numVolunteers = NumVolunteers;
            date = Date;
            postcode = Postcode;
            address1 = Address1;
            address2 = Address2;
            location = City;
            description = Description;
            thumbnail = Thumbnail;
            numRegVolunteers = NumRegVolunteers;
        }

        public Opportunity() { }
    }
}
