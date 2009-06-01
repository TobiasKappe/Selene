using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class EnumChooser : EnumBase
    {   
        protected override int GetIndex (Control Start)
        {
            WidgetPair Original = Start as WidgetPair;
            if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
                return (Original.Widget as QComboBox).CurrentIndex;
            else if(Original.SubType == ControlType.Radio)
                return ((Original.Widget as QBoxLayout).Children()[0] as QButtonGroup).CheckedId();

            return 0;
        }

        protected override void SetIndex (Control Start, int Index)
        {
            WidgetPair Original = Start as WidgetPair;
            if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
                (Original.Widget as QComboBox).CurrentIndex = Index;
            else
            {
                QButtonGroup Lay = (Original.Widget as QBoxLayout).Children()[0] as QButtonGroup;
                Lay.Button(Index).Checked = true;
            }
        }

        protected override Control ToWidget (Control Start, string[] Values)
        {
            WidgetPair Original = new WidgetPair(Start);
            if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
            {
                QComboBox Box = new QComboBox();

                foreach(string Value in Values)
                    Box.AddItem(Value);

                Original.Widget = Box;

                return Original;
            }
            else if(Original.SubType == ControlType.Radio)
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
                Original.Widget = Lay;
                
                return Original;
            }
            else throw new OverrideException(typeof(Enum), Original.SubType, ControlType.Radio, ControlType.Dropdown);
        }
    }
}
