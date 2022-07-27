using Microsoft.Xna.Framework.Input;

namespace RejTech.Drawing
{
    /// <summary>Wrapper for keyboard and mouse input</summary>
    public class Input
    {
        private static KeyboardState oldKeyboardState;
        private static KeyboardState newKeyboardState;
        private static MouseState oldMouseState;
        private static MouseState newMouseState;

        /// <summary>Call to refresh keyboard and mouse state</summary>
        public static void Poll()
        {
            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
        }
            
        /// <summary>Is keyboard key down?</summary>
        /// <param name="key">Key code. Same as System.Windows.Form.Keys or Microsoft.Xna.Framework.Input.Keys</param>
        public static bool KeyDown(int key)
        {
            return newKeyboardState.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)key);
        }

        /// <summary>Is keyboard key up?</summary>
        /// <param name="key">Key code. Same as System.Windows.Form.Keys or Microsoft.Xna.Framework.Input.Keys</param>
        public static bool KeyUp(int key)
        {
            return newKeyboardState.IsKeyUp((Microsoft.Xna.Framework.Input.Keys)key);
        }

        /// <summary>Is keyboard key pressed?</summary>
        /// <param name="key">Key code. Same as System.Windows.Form.Keys or Microsoft.Xna.Framework.Input.Keys</param>
        public static bool KeyPressed(int key)
        {
            return !oldKeyboardState.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)key) && newKeyboardState.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)key);
        }

        public static int MouseX
        {
            get { return newMouseState.X; }
        }

        public static int MouseY
        {
            get { return newMouseState.Y; }
        }

        public static bool MouseLeftPressed
        {
            get { return (newMouseState.LeftButton == ButtonState.Pressed); }
        }

        public static bool MouseMiddlePressed
        {
            get { return (newMouseState.MiddleButton == ButtonState.Pressed); }
        }

        public static bool MouseRightPressed
        {
            get { return (newMouseState.RightButton == ButtonState.Pressed); }
        }

        public static bool MouseLeftClicked
        {
            get { return MouseLeftPressed && (oldMouseState.LeftButton != ButtonState.Pressed); }
        }

        public static bool MouseMiddleClicked
        {
            get { return MouseMiddlePressed && (oldMouseState.MiddleButton != ButtonState.Pressed); }
        }

        public static bool MouseRightClicked
        {
            get { return MouseRightPressed && (oldMouseState.RightButton != ButtonState.Pressed); }
        }

        public static int MouseScrollValue
        {
            get { return newMouseState.ScrollWheelValue; }
        }
    }
}
