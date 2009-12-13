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
using Selene.Backend;
using SB = Selene.Backend;
using System.Windows.Forms;
using Forms = System.Windows.Forms;

namespace Selene.Winforms.Frontend
{
    public class TreeStoreDialog<T> : LeftNavFormBase<T>
    {
        TreeView Tree;

        public TreeStoreDialog (string Title) : base(Title)
        {
        }

        protected override Forms.Control Navigation {
            get { return Tree; }
        }

        protected override void Build (ControlManifest Manifest, int Column)
        {
            Tree = new TreeView();
            Tree.AutoSize = true;
            Tree.AfterSelect += TreeAfterSelect;

            Tree.BeginUpdate();

            CatPanel Helper = new CatPanel(ProcureState);

            foreach(ControlCategory Cat in Manifest.Categories)
            {
                // Make slight abuse of the "ImageIndex" property
                TreeNode CatNode = new TreeNode(Cat.Name, 1, Column-2);
                Tree.Nodes.Add(CatNode);

                foreach(ControlSubcategory Subcat in Cat.Subcategories)
                {
                    TreeNode SubcatNode = new TreeNode(Subcat.Name, 1, Column-2);
                    CatNode.Nodes.Add(SubcatNode);

                    TableLayoutPanel SubPanel = Helper.LayoutSubcat(State, Subcat);
                    SubPanel.SizeChanged += RightPanelResized;

                    Panel.Controls.Add(SubPanel, Column++, 1);

                    if(Column != 3) SubPanel.Visible = false;
                    else ActivePanel = SubPanel;
                }
            }

            Tree.EndUpdate();
        }

        void TreeAfterSelect (object sender, TreeViewEventArgs e)
        {
            ActivePanel.Visible = false;
            ActivePanel = Panel.Controls[Tree.SelectedNode.SelectedImageIndex];
            ActivePanel.Visible = true;
        }
    }
}
