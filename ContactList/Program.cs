using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContactList
{
    class Program
    {
        static void Main(string[] args)
        {

            bool cont = true;
            while (cont == true)
            {

                Console.Clear();
                Console.WriteLine("Hello, What are you looking to do?");
                Console.WriteLine("1: Add a contact");
                Console.WriteLine("2: Look up a contact");
                Console.WriteLine("3: Quit");
                string userInput = Console.ReadLine();
                if (userInput == "1" || userInput == "Add a contact")
                {
                    Contact contact = AddContact();
                    using var writer = new StreamWriter("ContactInfo.csv", true);
                    using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
                    csvWriter.WriteRecord(contact);
                    csvWriter.NextRecord();
                    writer.Flush();

                }
                else if (userInput == "2" || userInput == "Look up a contact")
                {
                    Console.WriteLine("Who are you looking for?");
                    string contactInfo = Console.ReadLine();
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false,

                    };
                    using var reader = new StreamReader("ContactInfo.csv");
                    using var csvReader = new CsvReader(reader, config);
                    LookUpContact(contactInfo, csvReader);

                }
                else if (userInput == "3" || userInput == "quit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Selection");
                }

                Console.WriteLine("Is there anything else you'd like to do? [Yes/No]");
                if (Console.ReadLine() != "Yes")
                    cont = false;          
            }
            Console.WriteLine("Thank you! You can close the application with any key.");
            Console.ReadKey();
        }
        private static void LookUpContact(string contactInfo, CsvReader csvReader)
        {
            var record = csvReader.GetRecords<Contact>();
            foreach (Contact items in record)
            {
                Contact c = new(items.FirstName, items.LastName, items.Birthday, items.Email, items.Phone);
                c.FirstName = items.FirstName;
                c.LastName = items.LastName;
                c.Birthday = items.Birthday;
                c.Email = items.Email;
                c.Phone = items.Phone;
                var line = string.Format($"{c.FirstName} {c.LastName}, {c.Birthday}, Age: {Contact.CalculateAge(Convert.ToDateTime(c.Birthday))}, {c.Email}, {c.Phone}");
                bool success = line.Any(contacts => c.FirstName.Contains(contactInfo) || c.LastName.Contains(contactInfo)
                                || c.Birthday.Contains(contactInfo) || c.Email.Contains(contactInfo) || c.Phone.Contains(contactInfo));
                if (success)
                    Console.WriteLine(line);
            }
            Console.WriteLine("There was nothing else to find.");
        }
        private static Contact AddContact()
        {
            bool nameValidation = false;
            bool lNameValidation = false;
            bool validDate = false;
            bool validEmail = false;
            bool validPhone = false;
            var formats = new[] { "MM/dd/yyyy" };
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            Console.WriteLine("What is their first name");
            string name = Console.ReadLine();
            while (nameValidation == false)
            {
                if (Regex.IsMatch(name, @"^[a-zA-Z- ]+$"))
                {
                    nameValidation = true;
                }
                else
                {
                    Console.WriteLine("Sorry, invalid first name entered. Please try again.");
                    Environment.Exit(0);
                }
            }           
            Console.WriteLine("What is their last name?");
            string lName = Console.ReadLine();
            while (lNameValidation == false)
            {
                if (Regex.IsMatch(lName, @"^[a-zA-Z- ]+$"))
                {
                    lNameValidation = true;
                }
                else
                {
                    Console.WriteLine("Sorry, invalid last name entered. Please try again.");
                    Environment.Exit(0);
                }
            }
            Console.WriteLine("What is their birthday? (XX/XX/XXXX)");
            string bday = Console.ReadLine();                       
            while (validDate == false)
            {
                if (validDate = DateTime.TryParseExact(bday, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
                {
                    validDate = true;
                }
                else
                {
                    Console.WriteLine("Sorry, invalid date entered. Please try again.");
                    Environment.Exit(0);
                }
            }
            Console.WriteLine("What is their email?");
            string email = Console.ReadLine();            
            while (validEmail == false)
            {
                if (IsValidEmail(email) == true)
                {
                    validEmail = true;
                }
                else
                {
                    Console.WriteLine("Sorry, invalid E-Mail entered. Please try again.");
                    Environment.Exit(0);
                }
            }
            Console.WriteLine("What is their phone number? (XXX-XXX-XXXX)");
            string phone = Console.ReadLine();            
            while (validPhone == false)
            {
                if (Regex.IsMatch(phone, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"))
                    validPhone = true;
                else
                {
                    Console.WriteLine("Sorry, invalid phone number entered. Please try again.");
                    Environment.Exit(0);
                }

            }
            var contact = new Contact(myTI.ToTitleCase(name), myTI.ToTitleCase(lName), bday, email, phone);
            Console.WriteLine($"{contact} has been added to the file!");
            return contact;
        }
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
