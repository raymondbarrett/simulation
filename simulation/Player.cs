using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace simulation
{
    class Player
    {
        public Texture2D PlayerTexture;
        public Vector2 Position;
        public int Direction;
        public Vector2[] playerVerts;
        public int Height {
            get { return PlayerTexture.Height; }
        }
        public int Width
        {
            get { return PlayerTexture.Width; }
        }
        public Vector2[] Verts
        {
            get { return playerVerts; }
        }

        public void Initialize(Texture2D texture, Vector2 position, int direction)

        {
            PlayerTexture = texture;
            Position = position;
            Direction = direction;
            playerVerts = new Vector2[] {
                position,
                new Vector2(position.X + this.Width, position.Y),
                new Vector2(position.X + this.Width, position.Y + this.Height),
                new Vector2(position.X, position.Y + this.Height)
            };
        }



        public void Update()

        {
            
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
