// Copyright (c) 2009 Tobias Kappé
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// Except as contained in this notice, the name(s) of the above
// copyright holders shall not be used in advertising or otherwise
// to promote the sale, use or other dealings in this Software
// without prior written authorization.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using NUnit.Framework;

#if GTK
using Selene.Gtk.Frontend;
#endif

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

using System.Xml.Serialization;
using System.IO;

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