using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RejTech.Drawing
{
    public class Image : System.IDisposable
    {
        private Graphics graphics;
        internal Texture2D texture = null;

        public Image(Graphics useGraphics, string resource)
        {
            graphics = useGraphics;
            texture = graphics.game.Content.Load<Texture2D>(resource);
        }

        public void DrawImage(Rectangle rect)
        {
            if (texture != null) graphics.spriteBatch.Draw(texture, rect, Color.White);
        }

        public int Width => (texture != null) ? texture.Width : 0;
        public int Height => (texture != null) ? texture.Height : 0;

        public void Dispose()
        {
            if (texture != null)
            {
                texture.Dispose();
                texture = null;
            }
        }

        ~Image()
        {
            Dispose();
        }
    }
}
