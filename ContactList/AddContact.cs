using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactList
{
    public abstract class AddContact 
    {
       
        public virtual string ToCsv()
        {
            string output = null;

            var properties = GetType().GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                output += properties[i].GetValue(this).ToString();
                if (i != properties.Length - 1)
                {
                    output += ",";
                }
            }
            return output;
        }

    }

}
