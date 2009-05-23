using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
	public class WidgetPair : Control
	{
		public Widget Widget;
		public bool Expands = true;
		public bool Fills = true;
		public Label Marker;
		public bool HasLabel = true;
		
		public WidgetPair(Control Original)
		{
			this.Flags = Original.Flags;
			this.Info = Original.Info;
			this.Label = Original.Label;
			this.SubType = Original.SubType;
			this.Type = Original.Type;
		}
		
		public WidgetPair()
		{
		}
	}
}
