using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI_WEB_IDs
{
    class Env
    {
        public static string get_Env_Value(string Key, string FilePath = @"C:\ENV\prod.env")
        {
            // Search for the .env file and use the path provided first, if provided
            string envfilepath = string.Empty;
            List<string> filepaths = new List<string>()
            {
                FilePath,
                Directory.GetCurrentDirectory() + @"\.env",
                System.IO.Path.GetFullPath(@"..\..\..\..\") + @"\.env",
                System.IO.Path.GetFullPath(@"..\..\..\") + @"\.env",
                System.IO.Path.GetFullPath(@"..\..\") + @"\.env",
                System.IO.Path.GetFullPath(@"..\") + @"\.env"

            };

            try
            {
                // Search for .env file in build location or use program location
                foreach (string filepath in filepaths)
                {
                    if (File.Exists(filepath))
                    {
                        envfilepath = filepath;
                        break;
                    }

                }

                foreach (var line in File.ReadAllLines(envfilepath))
                {   // Skip empty lines and comments
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    // Skip lines that are not key value pairs with an = sign
                    var key_value_pairs = line.Split('=', 2);
                    if (key_value_pairs.Length != 2)
                        continue;

                    // If case sensative key found return the value
                    if (key_value_pairs[0].Trim() == Key)
                    {
                        return key_value_pairs[1].Trim();
                    }

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Not found";

        }


    }
}
