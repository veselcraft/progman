using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;

namespace MinimizeToIcon
{
    public class MinimizingFormParent : Form
    {
        private const int BORDER_WIDTH = 4;

        private MdiClient mMdiClient;

        private ToolStripMenuItem mMenuItem;

        [PropertyTab("MenuItem")]
        [Browsable(true)]
        [Description("Menu item to insert windows list into")]
        [Category("Minimizing Parent")]
        public ToolStripMenuItem MenuItem
        {
            get 
            { 
                return mMenuItem;
            }

            set
            {
                mMenuItem = value;
            }
        }

        protected internal MinimizingFormParent()
            : base()
        {
            this.Load += MinimizingFormParent_Load;
        }

        private void MinimizingFormParent_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c is MdiClient)
                {
                    mMdiClient = (MdiClient)c;

                    mMdiClient.SizeChanged += MinimizingFormParent_Resize;

                    Region mc = new Region();
                    mc.MakeInfinite();

                    mMdiClient.Region = mc;
                }
            }
        }

        private void MinimizingFormParent_Resize(object sender, EventArgs e)
        {
            RearrangeAllIcons();
        }

        /// <summary>
        /// Re-arrange client windows
        /// </summary>
        /// <param name="layout"></param>
        protected internal new void LayoutMdi(MdiLayout layout)
        {
            switch (layout)
            {
                case MdiLayout.ArrangeIcons:

                    ArrangeIcons();
                    break;

                case MdiLayout.Cascade:

                    CascadeWindows();
                    break;

                case MdiLayout.TileHorizontal:

                    TileHorizontally();
                    break;

                case MdiLayout.TileVertical:

                    TileVerically();
                    break;
            }
        }

        protected internal void ChildClosed(MinimizableForm childForm)
        {
            if (MenuItem != null)
            {
                foreach (ToolStripMenuItem tsmi in MenuItem.DropDownItems)
                {
                    if (tsmi.Tag == childForm)
                    {
                        MenuItem.DropDownItems.Remove(tsmi);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Re-Position a minimized window
        /// </summary>
        /// Minimized window will be positioned so as not to obscure any minimized icon
        /// that has not moved since minimization.
        /// 
        /// Upon positionning, the minimized window is marked "Has Not Moved".
        /// <param name="childForm"></param>
        protected internal void PositionMinimizedWindow(MinimizableForm childForm)
        {
            Region totalRegion = new Region();
            totalRegion.MakeEmpty();

            // form a region of all icons that have *not* moved.

            // exclude current (minimized or otherwize) window. This will have
            // the effect of allowing the current window to stay put, as its
            // place will be regarded as avaliable.

            this.MdiChildren
                .Cast<MinimizableForm>()
                .Where((b) => b != childForm && b.IsMinimized && !b.HasMovedSinceMinimization)
                .All( (t) =>
                    {
                        totalRegion.Union(new Region(new Rectangle(t.Location, t.Size)));
                        return true;
                    });

            // try to place the (minimized) window 

            Size parentClientSize = GetEffectiveClientArea();
            Size iconSize = childForm.IconSize;

            for (int y = iconSize.Height; y < parentClientSize.Height; y += iconSize.Height)           
            {
                for (int x = 0; x < parentClientSize.Width - iconSize.Width; x += iconSize.Width)    
                {
                    Size gridLocation = new Size(x, -y);
                    Point iconLocation = new Point(0, parentClientSize.Height) + gridLocation;

                    Region newIconRegion = new Region(new Rectangle(iconLocation, iconSize));
                    newIconRegion.Intersect(totalRegion);

                    using (Graphics g = mMdiClient.CreateGraphics())
                    {
                        if (newIconRegion.IsEmpty(g))
                        {
                            childForm.IconGridLocation = gridLocation;
                            childForm.Location = iconLocation;
                            childForm.HasMovedSinceMinimization = false;

                            return; // "goto"
                        }
                    }                  
                }
            }

            // Found no place to put the icon...

            childForm.Location = new Point(0, 0);
            childForm.HasMovedSinceMinimization = false;
        }

        /// <summary>
        /// Return the real MDI client area. This will also take into effect the scrollbars...
        /// </summary>
        /// <returns></returns>
        private Size GetEffectiveClientArea()
        {
            Size parentClientSize = mMdiClient.DisplayRectangle.Size;

            return parentClientSize;
        }

        protected internal void AddToWindowsMenu(MinimizableForm form)
        {
            if (MenuItem != null)
            {
                // "tool strip menu item"...
                ToolStripMenuItem tsmi = new ToolStripMenuItem()
                {
                    Text = form.Text,
                    Image = form.bmpFrmBack,
                    Tag = form,
                };

                tsmi.Click += tsmi_Click;

                // place before rest
                MenuItem.DropDownItems.Insert(0, tsmi);
            }
        }

        private void tsmi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            MinimizableForm form = (MinimizableForm)tsmi.Tag;

            if (form.IsMinimized)
            {
                form.MinimizableForm_ActivateAction();
            }

            form.Activate();
        }

        /// <summary>
        /// Maintain icon position during resize. May re-arrrange all icons, regardless if they moved.
        /// </summary>
        private void RearrangeAllIcons()
        {
            Size parentClientSize = GetEffectiveClientArea();

            // move it to track the new client window size

            this.MdiChildren
                .Cast<MinimizableForm>()
                .Where((s) => s.IsMinimized && !s.HasMovedSinceMinimization)
                .All((t) => 
                {
                    t.Location = new Point(0, parentClientSize.Height) + t.IconGridLocation;
                    return true;
                });

            Region pr = new Region(new Rectangle(new Point(0, 0), parentClientSize));
   
            foreach (Form c in this.MdiChildren)
            {
                MinimizableForm mdic = (MinimizableForm)c;

                // move it if it is partally outside the client area

                if (mdic.IsMinimized && !mdic.IsMoving)
                {
                    using (Graphics g = mMdiClient.CreateGraphics())
                    {
                        Region cr = new Region(new Rectangle(mdic.Location, mdic.Size));
                        cr.Exclude(pr);

                        if (!cr.IsEmpty(g))
                        {
                            // the child is partially outside... bring it in!

                            this.PositionMinimizedWindow(mdic);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Re-arrange icons
        /// </summary>
        private void ArrangeIcons()
        {
            this.MdiChildren
                .Cast<MinimizableForm>()
                .Where((s) => s.IsMinimized)
                .All((t) =>
                {
                    t.HasMovedSinceMinimization = false;
                    this.PositionMinimizedWindow(t);

                    return true;
                });
        }

        /// <summary>
        /// Cascade windows
        /// </summary>
        private void CascadeWindows()
        {
            // start cascading from here.
            Point windowLocation = new Point(0, 0); 

            // cascading offset. Set to match that of WinForm's cascase operation
            Size offset = new Size(26,26);

            this.MdiChildren
                .Cast<MinimizableForm>()
                .Where((s) => !s.IsMinimized)
                .All((t) =>
                {
                    t.Location = windowLocation;
                    t.BringToFront();

                    windowLocation += offset;

                    return true;
                });
        }

        private void TileVerically()
        {
            Size parentClientSize = GetEffectiveClientArea();
            Size outsideClientSize = this.ClientSize;
            outsideClientSize.Width -= BORDER_WIDTH;

            int numNonMinimized = this.MdiChildren
                .Cast<MinimizableForm>()
                .Where((s) => !s.IsMinimized)
                .Count();

            if (numNonMinimized == 0)
            {
                return;
            }

            // reduce height by menustrip height (if any)

            outsideClientSize.Height -= this.Controls
                .Cast<Control>()
                .Where((s) => s is MenuStrip)
                .Sum((t) => t.Height);

            //

            int newWidth = outsideClientSize.Width / numNonMinimized;
            int leftOver = outsideClientSize.Width % numNonMinimized;

            int currentX = 0;

            foreach (Form c in this.MdiChildren)
            {
                MinimizableForm mdic = (MinimizableForm)c;

                if (!mdic.IsMinimized)
                {
                    mdic.Width = newWidth + (currentX == 0 ? leftOver : 0);
                    mdic.Height = parentClientSize.Height;
                    mdic.Location = new Point(currentX, 0);

                    currentX += mdic.Width;
                }
            }        
        }

        private void TileHorizontally()
        {
            Size parentClientSize = GetEffectiveClientArea();
            Size outsideClientSize = this.ClientSize;
            outsideClientSize.Width -= BORDER_WIDTH;
            outsideClientSize.Height -= BORDER_WIDTH;

            int numNonMinimized = this.MdiChildren
                .Cast<MinimizableForm>()
                .Where((s) => !s.IsMinimized)
                .Count();

            if (numNonMinimized == 0)
            {
                return;
            }

            // reduce height by menustrip height (if any)

            outsideClientSize.Height -= this.Controls
                .Cast<Control>()
                .Where((s) => s is MenuStrip)
                .Sum((t) => t.Height);

            //

            int newHeight = outsideClientSize.Height / numNonMinimized;
            int leftOver = outsideClientSize.Height % numNonMinimized;

            int currentY = 0;

            foreach (Form c in this.MdiChildren)
            {
                MinimizableForm mdic = (MinimizableForm)c;

                if (!mdic.IsMinimized)
                {
                    mdic.Height = newHeight + (currentY == 0 ? leftOver : 0);
                    mdic.Width = outsideClientSize.Width;
                    mdic.Location = new Point(0, currentY);

                    currentY += mdic.Height;
                }
            }
        }

    } // class "MinimizingFormParent"
}
