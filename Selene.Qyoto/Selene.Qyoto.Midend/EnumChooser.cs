using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
	public class EnumChooser : EnumBase<WidgetPair>
	{	
		protected override int GetIndex (WidgetPair Original)
		{
			if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default) 
				return (Original.Widget as QComboBox).CurrentIndex;
			else if(Original.SubType == ControlType.Radio)
				return ((Original.Widget as QBoxLayout).Children()[0] as QButtonGroup).CheckedId();
			
			return 0;
		}

		protected override void SetIndex (WidgetPair Original, int Index)
		{
			if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
				(Original.Widget as QComboBox).CurrentIndex = Index;
			else
			{
				QButtonGroup Lay = (Original.Widget as QBoxLayout).Children()[0] as QButtonGroup;
				Lay.Button(Index).Checked = true;
			}
		}
		
		protected override Control ToWidget (WidgetPair Start, string[] Values)
		{
			if(Start.SubType == ControlType.Dropdown || Start.SubType == ControlType.Default)
			{
				QComboBox Box = new QComboBox();
				
				foreach(string Value in Values)
					Box.AddItem(Value);
				
				Start.Widget = Box;
			
				return Start;
			}
			else if(Start.SubType == ControlType.Radio)
			{
				bool Vertical = false;
				Start.GetFlag<bool>(ref Vertical);
				
				QBoxLayout Lay;
				if(Vertical) Lay = new QVBoxLayout();
				else Lay = new QHBoxLayout();
				QButtonGroup Group = new QButtonGroup(Lay);
				
				// Wierdest bug ever: without this call, we'll either get a 
				// segfault from Qt or a NullReferenceException later on
				Console.Write(string.Empty);
				
				for(int i = 0; i < Values.Length; i++)
				{
					string Value = Values[i];
					
					QRadioButton Add = new QRadioButton(Value);
					Group.AddButton(Add, i);
					Lay.AddWidget(Add);
				}
				Start.Widget = Lay;
				
				return Start;
			}
			else throw new OverrideException(typeof(Enum), Start.SubType, ControlType.Radio, ControlType.Dropdown);
		}
	}
}
