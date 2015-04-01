using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Gravity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private const bool DEBUG = true;
        private const int NUM_LEVELS = 2;

        private enum GameState
        {
            MainMenu,
            Options,
            Play,
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ObjectCollection objectCollection;
        Player player;
        GameState currentGameState = GameState.MainMenu;
        int screenWidth = 800, screenHeight = 600;
        cButton buttonPlay;
        Texture2D menuTexture;
        Body body;
        Cursor cursor;
        Level level;
        int currentLevel;

        public Game1()
        {
            //GraphicsAdapter.UseReferenceDevice = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            currentLevel = 1;

            if (DEBUG)
            {
                currentGameState = GameState.Play;
                currentLevel = 1;
            }

            cursor = new Cursor(screenWidth / 2f, screenHeight / 2f);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {  
            StartLevel(currentLevel);

            base.Initialize();
        }

        void StartLevel(int lvl)
        {
            // Load level
            StreamReader sr = new StreamReader("Levels/lvl" + lvl + ".xml");
            level = new Level(sr);
            level.Load();
            sr.Close();

            // Initialize Player
            player = level.P;
            objectCollection = level.OC;

            // Listen for player death
            player.Died += player_Died;
            player.Won += player_Won;
        }

        void player_Won(object sender, EventArgs e)
        {
            // Start Next Level
            currentLevel++;
            if (currentLevel <= NUM_LEVELS)
                StartLevel(currentLevel);
            else
            {
                currentGameState = GameState.MainMenu;
                currentLevel = 1;
                StartLevel(currentLevel);
            }
        }

        void player_Died(object sender, EventArgs e)
        {
            // Show Game Over, Reset Level
            StartLevel(currentLevel);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            // Setup Main Menu
            buttonPlay = new cButton(Content.Load<Texture2D>("play"), graphics.GraphicsDevice);
            buttonPlay.SetPosition(new Vector2(300, 300));
            WinArea.WinTexture = menuTexture = Content.Load<Texture2D>("MainMenu");

            // Set Textures
            Box.BoxTexture = Content.Load<Texture2D>("Player");
            Player.PlayerTexture = Content.Load<Texture2D>("star_man");
            FloorSegment.FloorTexture = Content.Load<Texture2D>("Floor");
            Laser.LaserBaseTexture = Content.Load<Texture2D>("LaserBase");
            Laser.LaserBeamTexture = Content.Load<Texture2D>("LaserBeam");
            Cursor.CursorNormalTexture = Content.Load<Texture2D>("reticule");
            Cursor.CursorHighlightTexture = Content.Load<Texture2D>("reticule_highlight");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mouse = Mouse.GetState();

            if (currentGameState == GameState.MainMenu)
            {
                buttonPlay.Update(mouse);
                if (buttonPlay.isClicked == true) 
                    currentGameState = GameState.Play;
                return;
            }

            KeyboardState keyBoard = Keyboard.GetState();
            if (keyBoard.IsKeyDown(Keys.W)) { player.Jump(); }
            if (keyBoard.IsKeyDown(Keys.A)) { player.MoveLeft(); }
            if (keyBoard.IsKeyDown(Keys.D)) { player.MoveRight(); }

            if (keyBoard.IsKeyDown(Keys.Up))    { cursor.Up(); }
            if (keyBoard.IsKeyDown(Keys.Down))  { cursor.Down(); }
            if (keyBoard.IsKeyDown(Keys.Left))  { cursor.Left(); }
            if (keyBoard.IsKeyDown(Keys.Right)) { cursor.Right(); }

            if (keyBoard.IsKeyDown(Keys.R)) { StartLevel(currentLevel); } //restart

            body = objectCollection.CheckCursorOverObject(cursor);
            cursor.Highlighting = body != null;

            if (keyBoard.IsKeyDown(Keys.Space)) //(mouse.LeftButton == ButtonState.Pressed)
            {
                if (!player.Pulling && body != null)
                {
                    objectCollection.AvoidCollision(player, body);
                    player.CaptureObject(body);
                }
                
                if (player.Pulling)
                    player.PullObject(cursor);
            }
            else
            {
                Body b = player.ReleaseObject(cursor, keyBoard.IsKeyDown(Keys.LeftShift));
                if (b != null)
                {
                    //add manifold back
                    objectCollection.ReinstateCollision(player, b);
                }
            }


            objectCollection.UpdateAll(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(menuTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    buttonPlay.Draw(spriteBatch);
                    break;

                case GameState.Play:
                    objectCollection.DrawAll(spriteBatch);
                    cursor.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
