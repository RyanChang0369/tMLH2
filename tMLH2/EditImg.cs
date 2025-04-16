using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace tMLH2
{
    class EditImg
    {
        public Bitmap ReplaceColor(Bitmap bitmap, Color replacementColor)
        {
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    bitmap.SetPixel(x, y, replacementColor);
                }
            }
            return bitmap;
        }
    }
}
