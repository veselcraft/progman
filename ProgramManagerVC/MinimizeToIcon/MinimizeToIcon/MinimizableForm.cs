using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Linq;

namespace MinimizeToIcon
{
    public class MinimizableForm : Form
    {
        private Point mClickPoint;
        private Point mWindowLocation;
        private Point mLocationWhenMimimized;
        private Size mSizeWhenMinimized;
        private FormBorderStyle mOriginalBorderStyle;

        /// <summary>
        /// Is the icon on the move now?
        /// </summary>
        internal bool IsMoving
        {
            get;
            private set;
        }

        /// <summary>
        /// This form's icon
        /// </summary>
        internal Bitmap bmpFrmBack
        {
            get;
            private set;
        }

        /// <summary>
        /// Location in the parent window's grid
        /// </summary>
        protected internal Size IconGridLocation;

        /// <summary>
        /// This form's icon size
        /// </summary>
        protected internal Size IconSize
        {
            get
            {
                return bmpFrmBack.Size;
            }
        }

        private bool _HasMovedSinceMinimization;

        protected internal bool HasMovedSinceMinimization
        {
            get
            {
                return _HasMovedSinceMinimization;
            }
            set
            {
                _HasMovedSinceMinimization = value;
            }
        }

        private bool _IsMinimized;

        protected internal bool IsMinimized
        {
            get
            {
                return _IsMinimized;
            }

            set
            {
                // must call before we "minimize", otherwize the calle might think we have already 
                // minimized, and thus our location is no avaliable, causing the icon to jump around.

                // "minimize" only if we aren't already so.

                if (value && this.MdiParent != null)
                {
                    ((MinimizingFormParent)this.MdiParent).PositionMinimizedWindow(this);
                }

                _IsMinimized = value;
            }
        }

        public new FormWindowState WindowState
        { 
            get
            {
                if (this.IsMinimized)
                {
                    return FormWindowState.Minimized;
                }

                return base.WindowState;
            }

            set
            {
                if (value == FormWindowState.Minimized)
                {
                    MinimizableForm_MinimizeAction();
                    return;
                }

                base.WindowState = value;
            }
        }

        private void CommonInitialization()
        {
            this.IsMinimized = false;
            this.HasMovedSinceMinimization = false;

            base.MouseMove += MinimizableForm_MouseMove;
            base.DoubleClick += MinimizableForm_DoubleClick;
            base.FormClosed += MinimizableForm_FormClosed;
        }

        /// <summary>
        /// Minimizable form with default icon constructor
        /// </summary>
        protected internal MinimizableForm(int iconSize)
            : base()
        {
            using (Bitmap tmpBitmap = this.Icon.ToBitmap())
            {
                bmpFrmBack = tmpBitmap.ResizeImage(new Size(iconSize, iconSize));
            }

            CommonInitialization();
        }

        /// <summary>
        /// Minimizable form with iconFile icon constuctor
        /// </summary>
        /// <param name="iconFile">
        /// Form's minimize icon.
        /// </param>
        /// <remarks>
        /// Icon may either be a PNG file with transparencies or a BMP file with a pseicifc color (pixel at (0,0))
        /// designated as transparent color key.
        /// </remarks>
        protected internal MinimizableForm(String iconFile)
            : base()
        {
            bmpFrmBack = new Bitmap(iconFile);

            IntPtr icH = bmpFrmBack.ResizeImage(new Size(16, 16)).GetHicon();
            Icon ico = Icon.FromHandle(icH);

            this.Icon = ico;

            CommonInitialization();
        }

        /// <summary>
        /// Minimizable form with Bitmap icon (from resource)
        /// </summary>
        /// <param name="iconBitmap"></param>
        protected internal MinimizableForm(Bitmap iconBitmap)
            : base()
        {
            bmpFrmBack = iconBitmap;

            System.IntPtr icH = bmpFrmBack.ResizeImage(new Size(16, 16)).GetHicon();
            Icon ico = Icon.FromHandle(icH);

            this.Icon = ico;

            CommonInitialization();
        }

        /// <summary>
        /// Make VS designer happy...
        /// </summary>
        protected internal MinimizableForm()
            : base()
        {

        }

        private void MinimizableForm_MinimizeAction()
        {
            mLocationWhenMimimized = this.Location;
            mSizeWhenMinimized = this.Size;

            base.WindowState = FormWindowState.Normal;

            this.IsMinimized = true; // + reposition icon.
            this.HasMovedSinceMinimization = false;

            this.mOriginalBorderStyle = this.FormBorderStyle;
            this.FormBorderStyle = FormBorderStyle.None;

            this.Controls
                .Cast<Control>()
                .All((s) => { s.Visible = false; return true; });

            // sets the form's region
            BitmapRegion.CreateControlRegion(this, bmpFrmBack);
        }

        // Make sure the "minimized" window does not apprear -- not even for a microsecond...

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xF020;

        protected override void WndProc(ref Message m)
        {
            if (true
                && m.Msg == WM_SYSCOMMAND
                && m.WParam.ToInt32() == SC_MINIMIZE
                && !this.IsMinimized
                )
            {
                MinimizableForm_MinimizeAction();

                m.Result = IntPtr.Zero; // tell windows we have processed this message.
                return;
            }

            // alow someone else to process the message.

            base.WndProc(ref m);
        }

        protected internal void MinimizableForm_ActivateAction()
        {
            if (!this.IsMinimized)
            {
                return;
            }

            IsMinimized = false;

            this.FormBorderStyle = this.mOriginalBorderStyle;
            this.BackgroundImage = null;
            this.Region = null;

            // note: it is assumed all controls where initiazliy visible. Otherwize, their original
            // visibility state will needt to be saved, and restored here.

            this.Controls
                .Cast<Control>()
                .All((s) => { s.Visible = true; return true; });

            this.Location = mLocationWhenMimimized;
            this.Size = mSizeWhenMinimized;
        }

        private void MinimizableForm_DoubleClick(object sender, EventArgs e)
        {
            MinimizableForm_ActivateAction();
        }

        private void MinimizableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((MinimizingFormParent)this.MdiParent).ChildClosed(this);
        }

        private void MinimizableForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (true
                && this.IsMinimized
                && e.Button == MouseButtons.Left
                )
            {
                this.HasMovedSinceMinimization = true;

                if (this.IsMoving)
                {
                    this.Location += new Size(e.Location) - new Size(mClickPoint);
                }
                else
                {
                    this.IsMoving = true;
                    mClickPoint = e.Location;
                    mWindowLocation = this.Location;
                }
            }
            else
            {
                mWindowLocation = this.Location;
                this.IsMoving = false;
            }
        }

    } // class "MinimizableForm"

    /// <summary>
    /// Bitmap extension methods
    /// </summary>
    internal static class BitmapExtensions
    {
        /// <summary>
        /// Resize Bitmap to the size provided
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        internal static Bitmap ResizeImage(this Bitmap imgToResize, Size size)
        {
            Bitmap b = new Bitmap(size.Width, size.Height);

            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }

            return b;
        }
    }
}