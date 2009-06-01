using NUnit.Framework;
using Selene.Backend;

using System.Xml.Serialization;
using System.IO;
using System;
using Qyoto;
using Gtk;

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

#if GTK
using Selene.Gtk.Frontend;
#endif

namespace Selene.Testing
{
    [TestFixture]
    public class Serializing : ITest
    {
        public class Person
        {
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
        public void Run()
        {
            var Disp = new NotebookDialog<Person>("Selene demo application");
            var Test = Person.Load(Filename);
            Assert.IsTrue(Disp.Run(Test));
            Test.Save(Filename);
        }
    }
}
