using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (XmlReader reader = XmlReader.Create("book3.xml"))
            {

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.IsEmptyElement)
                            Console.WriteLine("<{0}/>", reader.Name);
                        else
                        {
                            Console.Write("<{0}> ", reader.Name);
                            reader.Read(); // Read the start tag.
                            if (reader.IsStartElement())  // Handle nested elements.
                                Console.Write("\r\n<{0}>", reader.Name);
                            Console.WriteLine(reader.ReadString());  //Read the text content of the element.
                        }
                    }
                }



                Console.ReadLine();
            }
        }
    }
}
