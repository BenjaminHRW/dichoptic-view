using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace RejTech.Drawing
{
    public class Box : System.IDisposable
    {
        private Texture2D box = null;
        private Graphics graphics = null;

        public Box(Graphics useGraphics)
        {
            graphics = useGraphics;
            box = new Texture2D(graphics.manager.GraphicsDevice, 1, 1);
            box.SetData(new[] { Color.White });
        }

        public void DrawBox(Rectangle rect, Color color)
        {
            color.A = 255;
            if (box != null) graphics.spriteBatch.Draw(box, rect, color);
        }

        public void Dispose()
        {
            if (box != null)
            {
                box.Dispose();
                box = null;
            }
        }

        ~Box()
        {
            Dispose();
        }
    }
}
