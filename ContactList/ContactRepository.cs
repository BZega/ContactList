using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;

namespace ContactList
{
    public class ContactRepository
    {
        public readonly Contact[] _allContacts;
        public ContactRepository()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var reader = new StreamReader(@"C:\Users\bzega\source\repos\ContactList\ContactList\ContactInfo.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                while (csv.Read())
                {
                    var record = csv.GetRecords<Contact>();
                    Console.WriteLine(record);
                }

            }
        }
    }
}
