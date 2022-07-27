using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RejTech
{
    class Line : System.IDisposable
    {
        private Texture2D pixel;
        private Vector2 p1, p2; //this will be the position in the center of the line
        private int length, thickness; //length and thickness of the line, or width and height of rectangle
        private Rectangle rect; //where the line will be drawn
        private float rotation; // rotation of the line, with axis at the center of the line
        private Color color;

        /// <summary>Line drawing object</summary>
        /// <param name="graphics">Graphics device for line</param>
        /// <param name="p1">Begin Point</param>
        /// <param name="p2">End point</param>
        /// <param name="thickness">Line thickness</param>
        /// <param name="color">Line color</param>
        public Line(GraphicsDevice graphics, Vector2 p1, Vector2 p2, int thickness, Color color)
        {
            pixel = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
            this.p1 = p1;
            this.p2 = p2;
            this.thickness = thickness;
            this.color = color;
        }

        /// <summary>Destructor during garbage collect</summary>
        ~Line()
        {
            pixel.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            length = (int)Vector2.Distance(p1, p2); //gets distance between the points
            rotation = GetRotation(p1.X, p1.Y, p2.X, p2.Y); //gets angle between points(method on bottom)
            rect = new Rectangle((int)p1.X, (int)p1.Y, length, thickness);
        //To change the line just change the positions of p1 and p2
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(pixel, rect, null, color, rotation, new Vector2(0, 0), SpriteEffects.None, 0.0f);
        }

        //this returns the angle between two points in radians 
        private float GetRotation(float x, float y, float x2, float y2)
        {
            float adj = x - x2;
            float opp = y - y2;
            float tan = opp / adj;
            float res = MathHelper.ToDegrees((float)Math.Atan2(opp, adj));
            res = (res - 180) % 360;
            if (res < 0) { res += 360; }
            res = MathHelper.ToRadians(res);
            return res;
        }

        public void Dispose()
        {
            if (pixel != null)
            {
                pixel.Dispose();
                pixel = null;
            }
        }
    }
}
