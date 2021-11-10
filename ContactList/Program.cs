using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
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

            string cont;
            do
            {
                Console.Clear();
                Console.WriteLine("Hello, What are you looking to do?");
                Console.WriteLine("1: Add a contact");
                Console.WriteLine("2: Look up a contact");
                Console.WriteLine("3: Quit");
                string userInput = Console.ReadLine();
                if (userInput == "1" || userInput == "Add a contact")
                {
                    bool nameValidation = false;
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
                    bool lNameValidation = false;
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
                    var formats = new[] { "MM/dd/yyyy" };
                    bool validDate = false;
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
                    bool validEmail = false;
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
                    bool validPhone = false;
                    while (validPhone == false)
                    {
                        if (Regex.IsMatch(phone, @"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]
                                                    |[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]
                                                    \s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$"))
                            validPhone = true;
                        else
                        {
                            Console.WriteLine("Sorry, invalid phone number entered. Please try again.");
                            Environment.Exit(0);
                        }

                    }
                    string contact = new Contact(name, lName, bday, email, phone).ToString();
                    Console.WriteLine($"{contact} has been added to the file!");
                    string contactCsv = new Contact(name, lName, bday, email, phone).ToCsv();
                    using var writer = new StreamWriter(@"C:\Users\bzega\source\repos\ContactList\ContactList\ContactInfo.csv", true);
                    using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
                    csvWriter.WriteField(contactCsv);
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
                    StreamReader reader = new(@"C:\Users\bzega\source\repos\ContactList\ContactList\ContactInfo.csv", true);
                    CsvReader csvReader = new(reader, config);
                    IEnumerable<Contact> record = csvReader.GetRecords<Contact>();
                    foreach (Contact items in record)
                    {
                        Contact c = new(items.FirstName, items.LastName, items.Birthday, items.Email, items.Phone)
                        {
                            FirstName = items.FirstName,
                            LastName = items.LastName,
                            Birthday = items.Birthday,
                            Email = items.Email,
                            Phone = items.Phone
                        };
                    }




                    Console.WriteLine(record);


                    bool success = record.Any(contacts => contactInfo.Contains(contacts.FirstName) || contactInfo.Contains(contacts.LastName)
                    || contactInfo.Contains(contacts.Birthday) || contactInfo.Contains(contacts.Email) || contactInfo.Contains(contacts.Phone));
                    if (success)
                        Console.WriteLine(record);
                    else
                    {
                        Console.WriteLine("Sorry it appears this person isn't in your contacts. Please try again.");
                        break;
                    }
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
                cont = Console.ReadLine();
            } while (cont == "Yes");

            Console.WriteLine("Thank you! You can close the application with any key.");
            Console.ReadKey();


            //var p = new Contact($"Taylor", "Hudnutt", "07/09/1989", "taylor_hudnutt@yahoo.com", "502-445-9424");
            //Console.WriteLine(p);
            //Console.ReadLine();
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
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
