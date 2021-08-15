namespace BankSimulator.Model
{
    public class Person : Client
    {
        public Gender? Sex { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
