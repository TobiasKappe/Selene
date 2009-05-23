using System;
using System.Reflection;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Resources;
using System.IO;

namespace Selene.Backend
{
	public enum ControlType { Default, Entry, Radio, Dropdown, Check, Toggle, FileSelect, DirectorySelect, Color }
	
	public delegate void ControlWalker(ref Control Current);
	
	public static class Introspector
	{	
		#region Temporary classes
		private class TempControlCategory
		{
			public Dictionary<string, TempControlSubcategory> Subcategories;
			public int ControlCount = 0;
			
			public TempControlCategory()
			{
				Subcategories = new Dictionary<string, TempControlSubcategory>();
			}
		}
		
		private class TempControlSubcategory
		{
			public List<Control> Controls;
			
			public TempControlSubcategory()
			{
				Controls = new List<Control>();
			}
		}
		#endregion
		
		#region Inspection functions
		public static ControlManifest Inspect(Type Root, string Manifest)
		{
			XmlSerializer Serializer = new XmlSerializer(typeof(ControlManifest));
			
			ControlManifest Ret;
			
			using (FileStream Stream = new FileStream(Manifest, FileMode.Open))
			{
				Ret = Serializer.Deserialize(Stream) as ControlManifest;
			}
			
			foreach(ControlCategory Category in Ret.Categories)
			{
				foreach(ControlSubcategory Subcatgory in Category.Subcategories)
				{
					foreach(Control Cont in Subcatgory.Controls)
					{
						FieldInfo Info = Root.GetField(Cont.Name);
						
						if(Info == null) 
							throw new InspectionException(Root, "Type "+Root+" does not contain field "+Cont.Name);
						
						Cont.Info = Info;
						Cont.Type = Info.FieldType;
					}
				}
			}
			
			return Ret;
		}
		
		public static ControlManifest Inspect(Type Root)
		{
			ControlManifestAttribute ManAttr = GetAttribute<ControlManifestAttribute>(Root);
			if(ManAttr != null) 
			{
				if(File.Exists(ManAttr.ManifestFile))
					return Inspect(Root, ManAttr.ManifestFile);
				else WarningFactory.Warn("No manifest file \""+ManAttr.ManifestFile+"\" found. Doing plain inspection");
			}
			
			Dictionary<string, TempControlCategory> Categories = new Dictionary<string, TempControlCategory>();
			List<string> OrderedCategories = new List<string>();
			string AddingCat = "Default", AddingSubcat = "Default";
			
			foreach(FieldInfo Info in Root.GetFields())
			{				
				ControlIgnoreAttribute Ignore = GetAttribute<ControlIgnoreAttribute>(Info);
				ControlAttribute ControlInfo = GetAttribute<ControlAttribute>(Info);
				ControlFlagsAttribute ControlFlags = GetAttribute<ControlFlagsAttribute>(Info);

				if(Ignore == null && Info.FieldType.IsArray && Info.FieldType.GetElementType() == Root)
				{
					throw new InspectionException(Info.FieldType, "Types should not contain arrays of themselves, " +
					                              "to prevent infinite recursion and StackOverflowException. " +
					                              "Add a ControlIgnore attribute to work around this");
				}
				
				// The field might be ignored. However, there could be a category attribute still here,
				// since field attributes are officially given to the next field to be found
				if(ControlInfo == null)
				{
					if(AddingCat == null) AddingCat = "Default";
					if(AddingSubcat == null) AddingSubcat = "Default";
				}
				else 
				{
					if(ControlInfo.Category != null) 
					{
						AddingCat = ControlInfo.Category;
						AddingSubcat = "Default";
					}
					if(ControlInfo.Subcategory != null) AddingSubcat = ControlInfo.Subcategory;
				}
				
				if(Ignore != null) continue;
								
				TempControlCategory CurrentCat;
				if(!OrderedCategories.Contains(AddingCat))
				{
					OrderedCategories.Add(AddingCat);
					Categories.Add(AddingCat, CurrentCat = new TempControlCategory());
				}
				else CurrentCat = Categories[AddingCat];
				
				TempControlSubcategory CurrentSubcat;
				if(!CurrentCat.Subcategories.ContainsKey(AddingSubcat))
					CurrentCat.Subcategories.Add(AddingSubcat, CurrentSubcat = new TempControlSubcategory());
				else CurrentSubcat = CurrentCat.Subcategories[AddingSubcat];
				
				Control C = new Control();
				C.Type = Info.FieldType;
				C.Name = Info.Name;
				C.Info = Info;
				C.Flags = ControlFlags == null ? null : ControlFlags.Flags;
				
				if(ControlInfo != null) 
				{
					C.Label = ControlInfo.Name == null ? Info.Name : ControlInfo.Name;
					C.SubType = ControlInfo.Override;
				}
				else
				{
					C.Label = Info.Name;
					C.SubType = ControlType.Default;
				}
				
				CurrentSubcat.Controls.Add(C);
				CurrentCat.ControlCount++;
			}
			
			List<ControlCategory> Ret = new List<ControlCategory>();
			
			foreach(string CatName in OrderedCategories)
			{
				List<ControlSubcategory> SubRet = new List<ControlSubcategory>();
				foreach(KeyValuePair<string, TempControlSubcategory> SubcatPair in Categories[CatName].Subcategories)
				{
					SubRet.Add(new ControlSubcategory { Controls = SubcatPair.Value.Controls.ToArray(), Name = SubcatPair.Key });
				}
				Ret.Add(new ControlCategory { Subcategories = SubRet.ToArray(), Name = CatName, 
					ControlCount = Categories[CatName].ControlCount });
			}
			
			return new ControlManifest { Categories = Ret.ToArray() };
		}
		
		public static IConverter[] GetConverters(Assembly Calling, out ConstructorInfo ArrayConverter)
		{
			var Ret = new List<IConverter>();
			ArrayConverter = null;
			
			foreach(Type Browse in Calling.GetTypes())
			{
				if(Browse.ContainsGenericParameters) continue;
				
				if(Browse.BaseType == typeof(ListViewerBase))
				{
					ArrayConverter = Browse.GetConstructor(Type.EmptyTypes);
					continue;
				}
				
				foreach(Type Interface in Browse.GetInterfaces())
				{
					if(Interface == typeof(IConverter))
					{
						IConverter Add = (IConverter) Browse.GetConstructor(Type.EmptyTypes).Invoke(null);
						Ret.Add(Add);
					}
				}
			}
			
			return Ret.ToArray();
		}
		#endregion
		
		private static A GetAttribute<A>(MemberInfo F) where A : Attribute
		{
			object[] Attributes = F.GetCustomAttributes(typeof(A), false);
			
			if(Attributes.Length == 0) return null;
			if(Attributes.Length > 1)				
			{
				throw new InspectionException(F.DeclaringType, "Field "+F.Name+" can't have more than one attribute of type "+typeof(A));
			}
			
			return Attributes[0] as A;
		}		
	}
}
