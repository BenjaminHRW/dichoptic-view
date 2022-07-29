using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System;
using RejTech.Drawing;

namespace RejTech
{
    /// <summary>
    /// TODO: Embedding https://stackoverflow.com/questions/41908427/embed-dlls-and-contents-into-exe-in-monogame
    /// Alternative: GameControl technique https://jaquadro.com/2013/03/embedding-your-monogame-game-in-a-winforms-control/
    /// </summary>
    public class TestPattern // : Game
    {
        Color TEXT_COLOR_DEFAULT = Color.White;
        const bool DISPLAY_BORDERLESS = true;
        const bool DISPLAY_FULLSCREEN = false;
        const bool DISPLAY_VSYNC = true;
        const bool DISPLAY_VRR = false;

        // Graphics output
        private Drawing.Graphics testGraphics;
        private Drawing.Font font;
        private Drawing.Image background;
        private Drawing.TextFormatter textStats;

        // Motion test;
        private Object thisLock = new Object();
        private const int DEFAULT_FRAME_STEP = 4; //Interesting to change
        private Counter frames = new Counter();
        private int frameStep = DEFAULT_FRAME_STEP;
        private int posX = 0;

        private IntPtr parentHandle = IntPtr.Zero;
        private IntPtr handle = IntPtr.Zero;
        private double currentFrameRate = 0;
        private bool changedVisible = false;
        private bool currentVisible = false;
        private bool newVisible = false;
        private bool changedDisplay = false;
        private DisplayInfo currentDisplay = null;
        private DisplayInfo newDisplay = null;
        
        /// <summary>Thread-safe modifiable display to display test on. Set to null to exit thread</summary>
        public DisplayInfo SetDisplay
        {
            get 
            {
                lock (thisLock)
                {
                    return currentDisplay;
                }
            }
            set
            {
                lock(thisLock)
                {
                    newDisplay = value;
                    changedDisplay = true;
                    frames.Clear();
                }
            }
        }

        /// <summary>True if engine is running</summary>
        public bool IsRunning
        {
            get
            {
                lock (thisLock)
                {
                    return (testGraphics != null) && testGraphics.IsRunning;
                }
            }
        }

        /// <summary>Thread-safe modifiable visibility state</summary>
        public bool Visible
        {
            get
            {
                lock (thisLock)
                {
                    return currentVisible;
                }
            }
            set
            {
                lock (thisLock)         
                {
                    newVisible = value;
                    changedVisible = true;
                    frames.Clear();
                }
            }
        }

        /// <summary>Current frame rate of the</summary>
        public double CurrentFrameRate
        {
            get
            {
                lock (thisLock)
                {
                    return currentFrameRate;
                }
            }
        }

        /// <summary>Constructor</summary>
        /// <param name="setParentWindow">Parent window that controls this motion test</param>
        public TestPattern(IntPtr setParentWindow)
        {
            parentHandle = setParentWindow;
        }

        /// <summary>Show animation</summary>
        private void Show()
        {
            testGraphics.Show();
            lock (thisLock)
            {
                currentVisible = true;
                newVisible = true;
                changedVisible = false;
            }

            // Force form to front of all non-topmost windows, then force-activate our topmost parent
            testGraphics.RaiseWithoutFocus();
            testGraphics.HideTaskbarIcon();
            Window.SetForeground(parentHandle);
        }

        /// <summary>Hide animation</summary>
        private void Hide()
        {
            testGraphics.Hide();
            lock (thisLock)
            {
                currentVisible = false;
                newVisible = false;
                changedVisible = false;
            }
        }

        private void ShowScreenOnDisplay(DisplayInfo display)
        {
            if (display == null)
            {
                testGraphics.SetWindowSize(0, 0);
                return;
            }
            lock (thisLock)
            {
                currentDisplay = display;
                changedVisible = false;
                frames.Clear();
            }
            if (!Visible) Show();
            testGraphics.SetWindowPos(currentDisplay.PositionX, currentDisplay.PositionY);
            testGraphics.SetWindowSize(currentDisplay.Width, currentDisplay.Height);
            UpdateFrameStep(currentDisplay.RefreshRate);
            testGraphics.Init();
            testGraphics.Focus();
            testGraphics.ApplyChanges();
        }

        private void UpdateFrameStep(double refresh) 
        {
            frameStep = ((int)(testGraphics.BufferWidth / 2 / refresh)) & 0xFFFE;
            frames.Clear();
            Console.WriteLine("changed framestep to " + frameStep);


        }

        /// <summary>
        /// Entry point for application
        /// </summary>
        public void Start(DisplayInfo display)
        {
            if ((testGraphics != null) && testGraphics.IsRunning) return;
            currentDisplay = display;
            newDisplay = display;
            testGraphics = new Drawing.Graphics()
            {
                OnInitialize = Initialize,
                OnLoadContent = LoadContent,
                OnUnloadContent = UnloadContent,
                OnUpdate = Update,
                OnDraw = Draw,
                MouseVisible = true
            };
            testGraphics.SetWindowPos(display.PositionX, display.PositionY);
            testGraphics.SetWindowSize(display.Width, display.Height);
            testGraphics.VSYNC = DISPLAY_VSYNC;
            testGraphics.FrameRateCap = 0;
            testGraphics.FullScreen = DISPLAY_FULLSCREEN;
            testGraphics.Borderless = DISPLAY_BORDERLESS;
            testGraphics.ApplyChanges();
            currentVisible = true;
            testGraphics.Run();
        }
        
        public void Stop()
        {
            if (testGraphics.IsRunning) testGraphics.Exit();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected void Initialize()
        {
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected void LoadContent()
        {
            try
            {
                background = new Drawing.Image(testGraphics, "testpatternwall");
                font = new Drawing.Font(testGraphics, "MainText");
                textStats = new Drawing.TextFormatter(testGraphics, font, new Vector2(60, 60), Color.White);
                Console.WriteLine("loaded test pattern");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("nah didn't work");
                UnloadContent();
                throw e;
            }

            //ShowScreenOnDisplay(currentDisplay);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected void UnloadContent()
        {
            if (font != null) font.Dispose();
            if (background != null) background.Dispose();
            if (testGraphics != null) testGraphics.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void Update(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            Console.WriteLine("Called Updated");
            if (testGraphics == null)
            {
                return;
            }
            else if ((parentHandle != IntPtr.Zero) && !Window.Exists(parentHandle))
            {
                // If our parent window is gone, we'll terminate this thread.
                //Console.WriteLine("Exited");
                testGraphics.Exit();
            }
            else if (testGraphics.IsFocussed)
            {
                // We never want to be the foreground window, reject focus to our parent
                if (parentHandle != IntPtr.Zero) Window.SetForeground(parentHandle);
            }
            lock (thisLock)
            {
                if (changedVisible)
                {
                    changedVisible = false;
                    if (newVisible) Show(); else { Hide(); }
                }
                if (changedDisplay)
                {
                    if (newDisplay == null) testGraphics.Exit();
                    changedDisplay = false;
                    ShowScreenOnDisplay(newDisplay);
                }
            }
            if (testGraphics.BufferWidth == 0) return;

            if (testGraphics.IsFocussed)
            {
                Drawing.Input.Poll();
                if (Drawing.Input.MouseLeftClicked)
                {
                    //if (parentHandle != IntPtr.Zero) Window.SetForeground(parentHandle);
                }
                else if (Drawing.Input.KeyDown(27))
                {
                    newDisplay = null;
                    currentDisplay = null;
                    Hide();
                }
            }
            frames++;

            if (!frames.IsRunning) frames.Start();
            if ((frames.Timestamp > 2000) && this.Visible)
            {
                currentFrameRate = frames.Frequency;
                UpdateFrameStep(currentFrameRate);
            }

            posX += frameStep;
            while (posX > 0) posX -= background.Width;
        }

        /// <summary>This is called when the game should draw itself.</summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void Draw(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            if (testGraphics == null) return;
            if (testGraphics.BufferWidth == 0) return;

            // Begin draw
            testGraphics.BeginDraw();

            for (int x = posX; x < testGraphics.BufferWidth; x += background.Width)
            {
                for (int y = 0; y < testGraphics.BufferHeight; y += background.Height)
                {
                    background.DrawImage(new Rectangle(x, y, background.Width, background.Height));
                }
            }

            // Text statistics
            if (currentDisplay != null)
            {
                string displayLine = currentDisplay.FriendlyName + "     " + currentDisplay.Width + "x" + currentDisplay.Height;
                if (!double.IsNaN(currentDisplay.RefreshRate))
                {
                    displayLine += "     " + currentDisplay.RefreshRate.ToString("0.00") + " Hz";
                    displayLine += "     VT" + currentDisplay.VertTotal.ToString();
                }
                textStats.BeginDraw();
                textStats.WriteLine(displayLine);
                textStats.SetPosition(new Vector2(currentDisplay.Width - 120, 60));
                textStats.WriteLine(String.Format("{0:0} fps", currentFrameRate));
                textStats.WriteLine(String.Format("{0:0} framestep", frameStep));
            }

            testGraphics.EndDraw();
        }
    }
}
