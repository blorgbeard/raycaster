using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
    static class ResourceLoader
    {
        public static Image GetTexture(string filename)
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var s = asm.GetManifestResourceStream("Raycaster.Textures." + filename))
            {
                return Bitmap.FromStream(s);
            }
        }
    }
}
