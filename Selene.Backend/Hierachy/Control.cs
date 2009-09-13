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

using System;
using System.Reflection;
using System.Xml.Serialization;

namespace Selene.Backend
{
    public class Control
    {
        static Exception MemberInfoWrong;

        static Control()
        {
            MemberInfoWrong = new Exception("MemberInfo not field or property. This should not happen");
        }

        public string Label;

        [XmlIgnore]
        public MemberInfo Info;

        [XmlIgnore]
        public Type Type {
            get
            {
                if(Info.MemberType == MemberTypes.Field)
                    return (Info as FieldInfo).FieldType;
                else if(Info.MemberType == MemberTypes.Property)
                    return (Info as PropertyInfo).PropertyType;
                else throw MemberInfoWrong;
            }
        }

        // Loaded by XmlMiner
        public string WantedName;

        [XmlAttribute]
        public string Name {
            get { return Info.Name; }
            set { WantedName = value; }
        }

        [XmlAttribute]
        public ControlType SubType;

        [XmlElement("Flag")]
        public object[] Flags;

        public Control()
        {
        }

        public Control(MemberInfo Info, ControlFlagsAttribute FlagsAttr, ControlAttribute Attribute)
        {
            this.Info = Info;
            Flags = FlagsAttr == null ? null : FlagsAttr.Flags;

            // Use field name if no label is specified
            Label = Attribute == null || Attribute.Name == null ? Info.Name : Attribute.Name;
            SubType = Attribute == null ? ControlType.Default : Attribute.Override;
        }

        public void Save(object To, object Value)
        {
            if(Info.MemberType == MemberTypes.Field)
                (Info as FieldInfo).SetValue(To, Value);
            else if(Info.MemberType == MemberTypes.Property)
                (Info as PropertyInfo).SetValue(To, Value, null);
            else throw MemberInfoWrong;
        }

        public object Obtain(object From)
        {
            if(Info.MemberType == MemberTypes.Field)
                return (Info as FieldInfo).GetValue(From);
            else if(Info.MemberType == MemberTypes.Property)
                return (Info as PropertyInfo).GetValue(From, null);
            else throw MemberInfoWrong;
        }

        public T GetFlag<T>(int Num) where T : class
        {
            if(Flags == null) return null;

            int Current = 0;
            foreach(object Flag in Flags)
            {
                T Ret = Flag as T;
                if(Ret != null)
                {
                    if(Current == Num) return Ret;
                    Current++;
                }
            }

            return null;
        }

        public T GetFlag<T>() where T : class
        {
            return GetFlag<T>(0);
        }

        public bool GetFlag<T>(int Num, ref T Ret) where T : struct
        {
            if(Flags == null) return false;

            int Current = 0;
            foreach(object Flag in Flags)
            {
                T? Try = Flag as T?;
                if(Try != null)
                {
                    if(Current == Num)
                    {
                        Ret = Try.HasValue ? Try.Value : default(T);
                        return Try.HasValue;
                    }
                    Current++;
                }
            }

            return false;
        }

        public bool GetFlag<T>(ref T Ret) where T : struct
        {
            return GetFlag<T>(0, ref Ret);
        }
    }
}
