using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay.Player
{
    class Player
    {
        private Vector2 m_Position;
        private Texture2D m_Texture;
        private Vector2 m_Size;

        private float m_Rotation;
        
        //Getting
        public Vector2 GetPosition() { return m_Position; }
        public float GetRotation() { return m_Rotation; }

        //Setting
        public void SetPosition(Vector2 pos) { m_Position = pos; }
        public void SetTexture(Texture2D texture) { m_Texture = texture; }
        public void SetRotation(float rot) { m_Rotation = rot; }

        //Constructor
        public Player(Rectangle rect, float rotation)
        {
            m_Position.X = rect.X;
            m_Position.Y = rect.Y;

            m_Size.X = rect.Width;
            m_Size.Y = rect.Height;

            m_Rotation = rotation;
        }

        public Player(Vector2 pos, float rotation)
        {
            m_Position = pos;
            m_Rotation = rotation;
            m_Size = new Vector2(1, 1);
        }

        //Draws the player
        public void Draw(ref SpriteBatch spriteBatch)
        {
            //Draws the texture to the screen
            Rectangle rect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
            Vector2 origin = new Vector2(m_Texture.Width / 2, m_Texture.Height / 2);

            spriteBatch.Draw(m_Texture, m_Position, rect, Color.White, m_Rotation, origin, 1, SpriteEffects.None, 1);
        }
    }
}
