using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RejTech.Drawing
{
    class TextFormatter
    {
        private Graphics graphics;
        private Vector2 pos = Vector2.Zero;
        private Vector2 origPos = Vector2.Zero;

        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public int LineSpacing { get; set; }

        public TextFormatter(Graphics useGraphics, Drawing.Font setFont, Vector2 setPos, Color setColor)
        {
            graphics = useGraphics;
            Font = setFont.font;
            LineSpacing = setFont.font.LineSpacing;
            Color = setColor;

            pos = setPos;
            origPos = setPos;
        }

        public void BeginDraw()
        {
            pos = origPos;
        }

        public void LineBreak(int pixels)
        {
            pos.Y += pixels;
        }

        public void SetPosition(Vector2 setPos)
        {
            pos = setPos;
        }

        public void WriteLine(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                graphics.spriteBatch.DrawString(this.Font, text, pos, this.Color);
            }
            pos.Y += this.LineSpacing;
        }

        public void WriteLine(string text, Color useColor)
        {
            if (!string.IsNullOrEmpty(text))
            {
                graphics.spriteBatch.DrawString(this.Font, text, pos, useColor);
            }
            pos.Y += this.LineSpacing;
        }
    }
}
