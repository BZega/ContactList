namespace ContactList
{
    public abstract class AddContact
    {

        public virtual string ToCsv()
        {
            string output = "";

            var properties = GetType().GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                output += properties[i].GetValue(this).ToString();
                if (i != properties.Length - 1)
                {
                    output += ",";
                }
            }
            return output.Replace("\"", "");
        }

    }

}
