# ContactList
This project was created through the concept of reading from and writing to an external file. My class is a Contact class which has the properties: Name (first and last), birthday, email, and phone number. The main program asks the user for information pertaining to the class and converts the data into a csv format and saves the information to a csv file. The program also has the ability to read from the csv file and use the information to display information the user is asking for. 

My 3+ features include but are not limited to:
1. Implement a master loop. The user can select from three options 
    a. Add a contact
    b. Look up a contact
    c. Quit

    These commands can be repeatedly used until the user decides to quit or puts in invalid information.
2. Read data from an external file. When "Look up a contact" is selected the user is given a prompt asking who they are looking for. Whatever type of information they enter whether it be their name, email, birthday etc. the application will read from a csv file and display all data that pertains to what they have requested. This includes partial known information i.e. Bran will look for all people with "Bran" in any field.
3. Implement a regular expression (Regex). The application ensures that all fields have valid entries. This includes both phone number and email. 
4. Conversion tool. Despite being a small implementation the user's "Birthday" input will convert the birthday given to the age of the respective contact they enter. This conversion is displayed both when looking up the contact as well as after the user has successfully added a contact.

