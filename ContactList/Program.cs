using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ContactList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, What are you looking to do?");
            string cont;
            do 
            {
                
                Console.WriteLine("1: Add a contact");
                Console.WriteLine("2: Look up a contact");
                Console.WriteLine("3: Quit");
                string userInput = Console.ReadLine();
                if (userInput == "1" || userInput == "Add a contact")
                {
                    Console.WriteLine("What is their first name");
                    string name = Console.ReadLine();
                    Console.WriteLine("What is their last name?");
                    string lName = Console.ReadLine();
                    Console.WriteLine("What is their birthday? (XX/XX/XXXX)");
                    string dateValue = Console.ReadLine();
                    string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "M/dd/yyyy", "MMddyyyy", "Mdyyyy", "Mddyyyyy"};
                    bool validDate = DateTime.TryParseExact(dateValue, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime bday);
                    if (validDate) 
                    {
                        Console.WriteLine("What is their email?");
                        string email = Console.ReadLine();
                        Console.WriteLine("What is their phone number? (XXX-XXX-XXXX)");
                        string phone = Console.ReadLine();
                        string contact = new Contact(name, lName, Convert.ToString(bday), email, phone).ToString();
                        Console.WriteLine($"{contact} has been added to the file!");
                        string contactCsv = new Contact(name, lName, Convert.ToString(bday), email, phone).ToCsv();
                        using var writer = new StreamWriter(@"C:\Users\bzega\source\repos\ContactList\ContactList\ContactInfo.csv", true);
                        using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
                        csvWriter.WriteField(contactCsv);
                        csvWriter.NextRecord();
                        writer.Flush();
                    }
                    else
                        Console.WriteLine("Sorry Invalid date entered, please try again");
                    
                }
                else if (userInput == "2" || userInput == "Look up a contact")
                {
                    Console.WriteLine("Who are you looking for?");
                    Console.ReadLine();
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
    }
}
