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
        CollisionMap collisionMap;
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
        float gAccel = 1.0f;
        float frictionAccel = 12.0f;
        int unitConversion = 128;
        //storing current velocity
        Vector2 currentVel;
        //storing the initial position before moving the player
        Vector2 initialPos;
       

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
            map = Content.Load<TiledMap>("test1");
            mapWidth = map.Width;
            mapHeight = map.Height;
            tileWidth = map.TileWidth;
            collisionGrid = new int[mapHeight, mapWidth];
            var tileLayer = map.GetLayer<TiledMapTileLayer>("Tile Layer 1");
            for(int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    System.Diagnostics.Debug.Write((int)Char.GetNumericValue(tileLayer.GetTile((ushort)j, (ushort)i).ToString()[18]) + " ,");
                    collisionGrid[i,j] = (int)Char.GetNumericValue(tileLayer.GetTile((ushort)j, (ushort)i).ToString()[18]);
                }
                System.Diagnostics.Debug.WriteLine("");
            }
            //foreach (int i in collisionGrid)
            //{
            //    System.Diagnostics.Debug.Write("{0} ", i.ToString());
            //}
            collisionMap = new CollisionMap(collisionGrid, map.TileHeight);
            mapRenderer = new TiledMapRenderer(GraphicsDevice, map);
            // TODO: use this.Content to load your game content here
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,0);
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
            if (320-player.Height-player.Position.Y > 0)
            {
                currentVel.Y += (float)((gAccel * gameTime.ElapsedGameTime.TotalMilliseconds)/1000);
            }
            else {
                currentVel.Y = 0;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) && player.Position.Y < player.Height)
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
            initialPos = player.Position;
            player.Position = Vector2.Add(new Vector2(unitConversion * (float)((currentVel.X * gameTime.ElapsedGameTime.TotalMilliseconds)/1000), unitConversion * (float)((currentVel.Y * gameTime.ElapsedGameTime.TotalMilliseconds) / 1000)), player.Position);
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, 1280 - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, 320 - player.Height);
            currentVel = collisionMap.calculateCollisions(currentVel, initialPos, player, (float)gameTime.ElapsedGameTime.TotalMilliseconds, unitConversion); ;
            camera.Pos = new Vector2(player.Position.X, player.Position.Y);
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, 1280 - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, 320 - player.Height);
            
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
