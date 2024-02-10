using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// <a href="http://www.codeproject.com/Articles/6048/Creating-Bitmap-Regions-for-Forms-and-Buttons">Creating Bitmap Regions for Forms and Buttons</a>

namespace MinimizeToIcon
{
    public static class BitmapRegion
    {
        /// <summary>
        /// Create and apply the given bitmap region on the supplied control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="bitmap"></param>
        public static void CreateControlRegion(Control control, Bitmap bitmap)
        {
            // Return if control and bitmap are null

            if (control == null || bitmap == null)
            {
                return;
            }

            // Set our control's size to be the same as the bitmap
            control.Width = bitmap.Width;
            control.Height = bitmap.Height;

            // Check if we are dealing with Form here
            if (control is Form)
            {
                // Cast to a Form object
                Form form = (Form)control;

                // No border
                form.FormBorderStyle = FormBorderStyle.None;
            }

            // Check if we are dealing with Button here
            else if (control is Button)
            {
                // Cast to a button object
                Button button = (Button)control;

                // Do not show button text
                button.Text = String.Empty;

                // Change cursor to hand when over button
                button.Cursor = Cursors.Hand;
            }

            // Set background image
            control.BackgroundImage = bitmap;

            // Calculate the graphics path based on the bitmap supplied
            GraphicsPath graphicsPath = CalculateControlGraphicsPath(bitmap);

            // Apply new region
            control.Region = new Region(graphicsPath);
        }

        /// <summary>
        /// CalculateControlGraphicsPath() with Transparent color taken from the top left pixel.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap)
        {
            // Use the top left pixel as our transparent color
            Color colorTransparent = bitmap.GetPixel(0, 0);

            return CalculateControlGraphicsPath(bitmap, colorTransparent);
        }

        /// <summary>
        /// Calculate the graphics path representing the figure in the bitmap 
        /// excluding the transparent color
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="colorTransparent"></param>
        /// <returns></returns>
        private static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap, Color colorTransparent)
        {
            // Create GraphicsPath for our bitmap calculation

            GraphicsPath graphicsPath = new GraphicsPath();

            // This is to store the column value where an opaque pixel is first found.
            // This value will determine where we start scanning for trailing 
            // opaque pixels.

            int colOpaquePixel = 0;

            // Go through all rows (Y axis)

            for (int row = 0; row < bitmap.Height; row++)
            {
                // Reset value

                colOpaquePixel = 0;

                // Go through all columns (X axis)

                for (int col = 0; col < bitmap.Width; col++)
                {
                    // If this is an opaque pixel, mark it and search for anymore trailing behind

                    if (false
                        || Image.IsAlphaPixelFormat(bitmap.PixelFormat) && bitmap.GetPixel(col, row).A == 0
                        || bitmap.GetPixel(col, row) == colorTransparent
                        )
                    {
                        continue;
                    }

                    // Opaque pixel found, mark current position

                    colOpaquePixel = col;

                    // Create another variable to set the current pixel position

                    int colNext = col;

                    // Starting from current found opaque pixel, search for 
                    // anymore opaque pixels trailing behind, until a transparent
                    // pixel is found or maximum width is reached

                    for (colNext = colOpaquePixel; colNext < bitmap.Width; colNext++)
                    {
                        if (false
                            || Image.IsAlphaPixelFormat(bitmap.PixelFormat) && bitmap.GetPixel(colNext, row).A == 0 
                            || bitmap.GetPixel(colNext, row) == colorTransparent
                            )
                        {
                            break;
                        }
                    }

                    // Form a rectangle for line of opaque pixels found and add it to our graphics path

                    graphicsPath.AddRectangle(new Rectangle(colOpaquePixel, row, colNext - colOpaquePixel, 1));

                    // No need to scan the line of opaque pixels just found
                    col = colNext;

                } // for...
            }

            // Return calculated graphics path

            return graphicsPath;
        }
    }
}
