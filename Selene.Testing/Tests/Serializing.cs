#if GTK
using NUnit.Framework;
using Selene.Backend;

using System.Xml.Serialization;
using System.IO;
using System;
using Gtk;

using Selene.Gtk.Frontend;

namespace Selene.Testing
{
    public partial class Harness
    {
        public class Person
        {
            #pragma warning disable 0649
            
            static XmlSerializer Serializer;
            
            public string Name;
            public bool Nut;
            
            public Movie[] Seen;
            
            static Person()
            {
                Serializer = new XmlSerializer(typeof(Person));
            }
            
            public static Person Load(string Filename)
            {
                if(!File.Exists(Filename)) return new Person();
                Person Ret;
                
                using (FileStream Stream = new FileStream(Filename, FileMode.Open))
                {
                    Ret = Serializer.Deserialize(Stream) as Person;
                }
                return Ret;
            }
            
            public void Save(string Filename)
            {
                using (FileStream Stream = new FileStream(Filename, FileMode.Create))
                {
                    Serializer.Serialize(Stream, this);
                    Stream.Flush();
                    Stream.Close();
                }
            }
        }
        
        public class Movie
        {
            public string Name;
            public string Year;
            public bool Liked;
        }
        
        const string Filename = "person.xml";

        [Test]
        public void Serializing()
        {
            var Disp = new NotebookDialog<Person>("Selene demo application");
            var Test = Person.Load(Filename);
            Assert.IsTrue(Disp.Run(Test));
            Test.Save(Filename);
        }
    }
}
#endif
