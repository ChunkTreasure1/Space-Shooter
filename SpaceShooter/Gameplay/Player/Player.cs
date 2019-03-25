using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay.Player
{
    class Player
    {
        private Vector2 m_Position;
        private Texture2D m_Texture;
        private Vector2 m_Size;
        
        //Getting
        public Vector2 GetPosition() { return m_Position; }
        public void SetPosition(Vector2 pos) { m_Position = pos; }
        
        //Setting
        public void SetTexture(Texture2D texture) { m_Texture = texture; }

        //Constructor
        public Player(Rectangle rect)
        {
            m_Position.X = rect.X;
            m_Position.Y = rect.Y;

            m_Size.X = rect.Width;
            m_Size.Y = rect.Height;
        }

        public Player(Vector2 pos)
        {
            m_Position = pos;
            m_Size = new Vector2(1, 1);
        }

        //Draws the player
        public void Draw(ref SpriteBatch spriteBatch)
        {
            //Draws the texture to the screen
            Rectangle rect = new Rectangle((int)m_Position.X, (int)m_Position.Y, (int)m_Size.X, (int)m_Size.Y);
            spriteBatch.Draw(m_Texture, rect, Color.White);
        }
    }
}
