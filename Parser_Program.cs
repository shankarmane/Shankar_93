using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BlogParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"H:\Projects\Coding Problems\blog.txt";
            StringBuilder line = new StringBuilder();
            Dictionary<string, string> data = new Dictionary<string, string>();
            int counter = 0;

            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        line.Append(sr.ReadLine());

                        if (line.ToString().StartsWith("---"))
                        {
                            counter++;
                            line.Replace("---", "");
                        }
                        if (String.IsNullOrEmpty(line.ToString()))
                        {
                            continue;
                        }

                        if (counter == 1)
                        {
                            var json_val = line.ToString().Split(":",2);
                            data.Add(json_val[0], json_val[1]);
                        }
                        else if(counter == 2)
                        {
                            data.Add("short-content", line.ToString());
                            counter++;
                        }
                        else if(counter == 3 && line.ToString() != "READMORE")
                        {
                            if (data.ContainsKey("content"))
                            {
                                data["content"] = data["content"] + line.ToString();
                            }
                            else
                            {
                                data.Add("content", line.ToString());
                            }
                            
                        }
                        line.Clear();
                    }
                }
            }

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            Console.WriteLine(json);
            Console.ReadLine();
        }
    }
}
