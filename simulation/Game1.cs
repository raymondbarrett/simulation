using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended.Content;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Tiled.Renderers;


namespace simulation
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        TiledMap map;
        TiledMapRenderer mapRenderer;
        Camera2d camera;
        int[,] collisionGrid;
        //tracking user input
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        //map width and height
        int mapWidth;
        int mapHeight;
        int tileWidth;
        //stores current tile when creating collision array
        TiledMapTile tile;
        //values for character accel, max vel, gravity and friction
        float playerAccel = 22.0f;
        float maxVel = 6.0f;
        float jumpVel = 12.0f;
        float gAccel = 25.0f;
        float frictionAccel = 12.0f;
        float unitConversion = 128.0f;
        //storing current velocity
        Vector2 currentVel;
       

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            camera = new Camera2d();
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = Content.Load<TiledMap>("test");
            mapWidth = map.Width;
            mapHeight = map.Height;
            tileWidth = map.TileWidth;
            collisionGrid = new int[mapWidth, mapHeight];
            var tileLayer = map.GetLayer<TiledMapTileLayer>("Tile Layer 1");
            for(int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    collisionGrid[i,j] = (int)(tileLayer.GetTile((ushort)i, (ushort)j).ToString()[18]);
                }
            }
            mapRenderer = new TiledMapRenderer(GraphicsDevice, map);
            // TODO: use this.Content to load your game content here
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\basicchar"), playerPosition,0);
            camera.Pos = new Vector2(playerPosition.X, playerPosition.Y);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            // TODO: Add your update logic here
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            mapRenderer.Update(gameTime);
            UpdatePlayer(gameTime);
            base.Update(gameTime);
        }
        private void UpdatePlayer(GameTime gameTime) {
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (currentVel.X < maxVel)
                {
                    currentVel.X += ((float)(playerAccel * gameTime.ElapsedGameTime.TotalMilliseconds) / 1000);
                }
                else
                {
                    currentVel.X += (float)((frictionAccel * gameTime.ElapsedGameTime.TotalMilliseconds) / 1000);
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (-currentVel.X < maxVel)
                {
                    currentVel.X -= ((float)(playerAccel * gameTime.ElapsedGameTime.TotalMilliseconds) / 1000);
                }
                else
                {
                    currentVel.X -= (float)((frictionAccel * gameTime.ElapsedGameTime.TotalMilliseconds) / 1000);
                }

            }
            if (currentKeyboardState.IsKeyUp(Keys.Up) && currentVel.Y < 0) {
                currentVel.Y = 0;
            }
            if (3200-player.Height-player.Position.Y > 0)
            {
                currentVel.Y += (float)((gAccel * gameTime.ElapsedGameTime.TotalMilliseconds)/1000);
            }
            else {
                currentVel.Y = 0;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) && player.Position.Y == 3200 - player.Height)
            {
                currentVel.Y = -jumpVel;
            }
            if (Math.Abs(currentVel.X) >= (frictionAccel * gameTime.ElapsedGameTime.TotalMilliseconds / 1000))
            {
                currentVel.X -= (float)(currentVel.X / Math.Abs(currentVel.X) * frictionAccel * gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
            }
            else {
                currentVel.X = 0;
            }

            player.Position.X += unitConversion * (float)((currentVel.X * gameTime.ElapsedGameTime.TotalMilliseconds)/1000);
            player.Position.Y += unitConversion * (float)((currentVel.Y * gameTime.ElapsedGameTime.TotalMilliseconds)/1000);
            camera.Move(unitConversion * currentVel * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, 3200 - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, 3200 - player.Height);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Pink);
            // TODO: Add your drawing code here
            spriteBatch.Begin(transformMatrix: camera.get_transformation(GraphicsDevice), samplerState: SamplerState.PointClamp);
            player.Draw(spriteBatch);
            mapRenderer.Draw(camera.get_transformation(GraphicsDevice));
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
