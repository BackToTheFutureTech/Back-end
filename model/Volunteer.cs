namespace AwsDotnetCsharp
{
    public class Volunteer
    {
        public string volunteerId { get; set; }
        public string volunteerName { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public int groupNum { get; set; }

        public Volunteer(string Id, string Name, string Email, string Mobile, int GroupNum)
        {
            volunteerId = Id;
            volunteerName = Name;
            email = Email;
            mobile = Mobile;
            groupNum = GroupNum;
        }
        public Volunteer() { }
    }
}