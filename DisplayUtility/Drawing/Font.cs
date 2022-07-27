using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RejTech.Drawing
{
    class Font : System.IDisposable
    {
        internal SpriteFont font = null;

        public Font(Graphics graphics, string fontName)
        {
            font = graphics.game.Content.Load<SpriteFont>(fontName);
        }

        public void Dispose()
        {
            if (font != null)
            {
                font.Texture.Dispose();
                font = null;
            }
        }

        ~Font()
        {
            Dispose();
        }

    }
}
