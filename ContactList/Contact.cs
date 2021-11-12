using CsvHelper.Configuration.Attributes;
using System;

namespace ContactList
{
    public class Contact 
    {
        public Contact (string firstName, string lastName, string birthday, string email, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            Email = email;
            Phone = phone;
        }
        [Index(0)]
        public string FirstName { get; set; }
        [Index(1)]
        public string LastName { get; set; }
        [Index(2)]
        public string Birthday { get; set; }
        [Index(3)]
        public string Email { get; set; }
        [Index(4)]
        public string Phone { get; set; }
        public override string ToString() => $"{FirstName} {LastName}, {Birthday}, Age: {CalculateAge(Convert.ToDateTime(Birthday))}, {Email}, {Phone}";
        public string ToCsv() => $"{FirstName},{LastName},{Birthday},{Email},{Phone}";
        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age--;
            return age;
        }

    }
}
