using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace RejTech.Drawing
{
    /// <summary>
    /// Wrapper for graphics engine.
    /// 
    /// Current default engine is MonoGame.  However, this wrapper is designed to make it semi-easy to port a MonoGame-like interface to other 
    /// more bare-to-the-metal engines (e.g. Direct3D or OpenGL calls) since only simple graphics is currently being done, such as 
    /// lines, text, boxes, graphics, etc, used by motion tests such as offline versions.   This is a wrapper to allow the option to 
    /// reduce dependancy on MonoGame in the future.
    /// </summary>
    public class Graphics : System.IDisposable
    {
        public delegate void Initialize();
        public delegate void LoadContent();
        public delegate void UnloadContent();
        public delegate void Update(TimeSpan totalGameTime, TimeSpan elapsedGameTime);
        public delegate void Draw(TimeSpan totalGameTime, TimeSpan elapsedGameTime);

        public Initialize OnInitialize;
        public LoadContent OnLoadContent;
        public UnloadContent OnUnloadContent;
        public Update OnUpdate;
        public Draw OnDraw;

        /// <summary>XNA/MonoGame interface, kept private from rest of application for easier future engine-switching</summary>
        private class Engine : Game
        {
            Graphics graphics;

            public Engine(Graphics useGraphics)
            {
                graphics = useGraphics;
            }

            protected override void Initialize()
            {
                graphics.OnInitialize?.Invoke();
                base.Initialize();
            }

            protected override void LoadContent()
            {
                graphics.OnLoadContent?.Invoke();
                base.LoadContent();
            }

            protected override void UnloadContent()
            {
                graphics.OnUnloadContent?.Invoke();
                base.UnloadContent();
            }

            protected override void Update(GameTime gameTime)
            {
                //graphics.OnUpdate(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                graphics.OnUpdate?.Invoke(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                base.Update(gameTime);
            }

            protected override void Draw(GameTime gameTime)
            {
                //graphics.OnDraw(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                graphics.OnDraw?.Invoke(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                base.Draw(gameTime);
            }

            new protected void Exit()
            {
                base.Dispose();
                base.Exit();
            }

            new protected void Run()
            {
                base.Run();
            }
        }

        internal Game game = null;
        internal Form gameForm = null;
        internal ContentManager content = null;
        internal GraphicsDeviceManager manager = null;
        internal SpriteBatch spriteBatch = null;
                                      
        private double frameRateCap = 0;
        private Color backgroundColor = Color.Black;
        private bool running = false;

        public bool LoadContentFromResources { get; set; } = true;

        /// <summary>Monogame style interfaces</summary>
        public Graphics()
        {
            game = new Engine(this);
            manager = new GraphicsDeviceManager(game);
            manager.PreferredBackBufferWidth = ScreenWidth;
            manager.PreferredBackBufferHeight = ScreenHeight;
            // Has bugs with MonoGame 3.8.
            // Need to refactor for xnb files embedded into the EXE.
            // Use external .xnb files for now in Content subfolder instead.
            //
            //if (LoadContentFromResources)
            //{
            //    // NOTE: You can automate loading xnb files into resources: https://gist.github.com/tgjones/8c1c46e1950a7b9a4769
            //    game.Content = new ResourceContentManager(game.Services, RejTech.Properties.Resources.ResourceManager);
            //}
            //else
            //{
                game.Content.RootDirectory = "Content";
            //}
            gameForm = Control.FromHandle(game.Window.Handle) as Form;
            game.InactiveSleepTime = new TimeSpan(0);
        }

        /// <summary>Run the engine. This blocks until Exit(), so you may want to use a separate thread</summary>
        public void Run()
        {
            manager.ApplyChanges();
            running = true;
            game?.Run();
        }

        /// <summary>Exit the engine</summary>
        public void Exit()
        {
            running = false;
            game?.Exit();
        }

        /// <summary>Call before beginning a cycle</summary>
        public void BeginDraw()
        {
            if (spriteBatch == null) spriteBatch = new SpriteBatch(manager.GraphicsDevice);
            spriteBatch.Begin();
            manager.GraphicsDevice.Clear(backgroundColor);
        }

        /// <summary>Call after finishing a refresh cycle</summary>
        public void EndDraw()
        {
            spriteBatch?.End();
        }

        /// <summary>Call to reinitialize after changing settings (window size, VSYNC, etc</summary>
        public void Init()
        {
            manager.ApplyChanges();
        }

        /// <summary>Call on destroy</summary>
        public void Dispose()
        {
            // TODO: Port Feb2021 memory optimization into other implementations
            spriteBatch?.Dispose();
            content?.Dispose();
            manager?.Dispose();
            gameForm?.Dispose();
            game?.Dispose();
        }

        /// <summary>Show the engine window</summary>
        public void Show()
        {
            gameForm?.Show();
        }

        /// <summary>Hide the engine window</summary>
        public void Hide()
        {
            gameForm?.Hide();
        }

        /// <summary>Sets focus to engine window</summary>
        public void Focus()
        {
            Window.SetForeground(Handle);
        }

        /// <summary>Raise engine window to top of Z-order without setting focus</summary>
        public void RaiseWithoutFocus()
        {
            Window.SetForegroundWithoutFocus(Handle);
        }

        /// <summary>Hide taskbar icon</summary>
        public void HideTaskbarIcon()
        {
            Window.HideTaskbarIcon(Handle);
        }

        /// <summary>Returns true if engine running</summary>
        public bool IsRunning
        {
            get
            {
                return running;
            }
        }

        /// <summary>Returns true if engine window is visible</summary>
        public bool IsVisible
        {
            get
            {
                return (gameForm != null) ? gameForm.Visible : false;
            }
        }

        /// <summary>Returns true if engine window is focussed</summary>
        public bool IsFocussed
        {
            get
            {
                return (Window.GetForeground() == Handle);
            }
        }

        /// <summary>Handle of engine window (internal use only)</summary>
        private IntPtr Handle
        {
            get
            {
                if ((game == null) || (game.Window == null)) return IntPtr.Zero;
                return game.Window.Handle;
            }
        }

        /// <summary>Sets position of the engine window</summary>
        public void SetWindowPos(int x, int y)
        {
             game.Window.Position = new Point(x, y);
        }

        /// <summary>Sets size of the engine window</summary>
        public void SetWindowSize(uint width, uint height)
        {
            manager.PreferredBackBufferWidth = (int)width;
            manager.PreferredBackBufferHeight = (int)height;
        }

        /// <summary>Screen width of primary monitor</summary>
        public static int ScreenWidth
        {
            get
            {
                return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            }
        }

        /// <summary>Screen height of primary monitor</summary>
        public static int ScreenHeight
        {
            get
            {
                return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
        }

        /// <summary>Width of engine window</summary>
        public int BufferWidth
        {
            get
            {
                return manager.PreferredBackBufferWidth;
            }
        }

        /// <summary>Height of engine window</summary>
        public int BufferHeight
        {
            get
            {
                return manager.PreferredBackBufferHeight;
            }
        }

        /// <summary>Gets/Sets the default blank-buffer background color</summary>
        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
            }
        }

        /// <summary>Gets/sets the VSYNC ON/OFF state. Call Init() to refresh.</summary>
        public bool VSYNC
        {
            get
            {
                return manager.SynchronizeWithVerticalRetrace;
            }
            set
            {
                manager.SynchronizeWithVerticalRetrace = value;
            }
        }

        /// <summary>Gets/sets the exclusive full screen mode state. Call Init() to refresh.</summary>
        public bool FullScreen
        {
            get
            {
                return manager.IsFullScreen;
            }
            set
            {
                manager.IsFullScreen = value;
            }
        }

        /// <summary>Gets/sets the borderless windowed mode state. Call Init() to refresh.</summary>
        public bool Borderless
        {
            get
            {
                return game.Window.IsBorderless;
            }
            set
            {
                game.Window.IsBorderless = value;
            }
        }

        /// <summary>Gets/sets the mouse visibility state.</summary>
        public bool MouseVisible
        {
            get
            {
                return game.IsMouseVisible;
            }
            set
            {
                game.IsMouseVisible = value;
            }
        }

        /// <summary>Gets/sets frame rate cap.  Useful for VSYNC OFF, FreeSync or G-SYNC variable refresh rate displays.  Set to 0 to disable cap.</summary>
        public double FrameRateCap
        {
            get
            {
                return frameRateCap;
            }
            set
            {
                frameRateCap = value;
                game.IsFixedTimeStep = (frameRateCap != 0);
                if (frameRateCap != 0)
                {
                    game.TargetElapsedTime = new TimeSpan((long)((double)TimeSpan.TicksPerSecond / frameRateCap));
                }
            }
        }

        /// <summary>Refresh settings</summary>
        public void ApplyChanges()
        {
            manager.ApplyChanges();
        }
    }
}
