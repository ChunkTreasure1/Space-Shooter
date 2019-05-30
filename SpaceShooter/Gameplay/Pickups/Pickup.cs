using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay
{
    public class Pickup
    {
        //Member vars
        protected Vector2 m_Position;
        protected Texture2D m_Texture;
        protected Rectangle m_Rectangle;

        protected Color[] m_TextureData;
        protected GraphicsDeviceManager m_Graphics;
        protected float m_Rotation;

        protected int m_RotationFrames = 1;
        protected int m_CurrFrames = 0;

        //Getting
        public Vector2 GetPosition() { return m_Position; }
        public Rectangle GetRectangle() { return m_Rectangle; }
        public Texture2D GetTexture() { return m_Texture; }
        public Color[] GetTextureData() { return m_TextureData; }
        public float GetRotation() { return m_Rotation; }

        //Setting
        public void SetPosition(Vector2 pos)
        {
            m_Position = pos;
            m_Rectangle.X = (int)pos.X;
            m_Rectangle.Y = (int)pos.Y;
        }
        public void SetTexture(Texture2D texture) { m_Texture = texture; }
        public void SetRectangle(Rectangle rectangle) { m_Rectangle = rectangle; }
        public void SetRotation(float rot) { m_Rotation = rot; }

        //Methods
        public Pickup(Vector2 pos, Texture2D texture, Rectangle rect, GraphicsDeviceManager graphics)
        {
            m_Position = pos;
            m_Texture = texture;
            m_Rectangle = rect;

            m_Graphics = graphics;
        }

        //Draws the pickup if the draw method isn't overridden
        public virtual void Draw(ref SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
            Vector2 origin = new Vector2(m_Texture.Width / 2, m_Texture.Height / 2);

            spriteBatch.Draw(m_Texture, m_Position, rect, Color.White, m_Rotation, origin, 1, SpriteEffects.None, 1);
        }
        
        //Loads the texture data of the pickup(used for collision)
        public void LoadTextureData()
        {
            //Create an array of colors using the texture size
            m_TextureData = new Color[m_Texture.Width * m_Texture.Height];
            //Fill the array using data from the texture
            m_Texture.GetData(m_TextureData);
        }
    }
}
