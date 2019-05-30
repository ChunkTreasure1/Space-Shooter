using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay
{
    public class Entity
    {
        protected Vector2 m_Position;
        protected Texture2D m_Texture;
        protected Rectangle m_Rectangle;

        protected float m_Rotation;
        protected float m_Scale;
        protected Color[] m_TextureData;

        protected GraphicsDeviceManager m_Graphics;

        //Getting
        public Vector2 GetPosition() { return m_Position; }
        public float GetRotation() { return m_Rotation; }
        public Rectangle GetRectangle() { return m_Rectangle; }

        public Texture2D GetTexture() { return m_Texture; }
        public Color[] GetTextureData() { return m_TextureData; }

        //Setting
        public virtual void SetPosition(Vector2 pos)
        {
            m_Position = pos;
            m_Rectangle.X = (int)pos.X;
            m_Rectangle.Y = (int)pos.Y;
        }
        public void SetTexture(Texture2D texture) { m_Texture = texture; }
        public void SetRotation(float rot) { m_Rotation = MathHelper.ToRadians(rot); }
        public void SetRectangle(Rectangle rect) { m_Rectangle = rect; }

        //Methods
        public Entity(Vector2 pos, float rotation, float scale, Texture2D texture, Rectangle rect, GraphicsDeviceManager graphics)
        {
            m_Position = pos;

            m_Scale = scale;
            m_Rotation = rotation;
            m_Texture = texture;

            m_Rectangle = rect;
            m_Graphics = graphics;
        }
        public virtual void Move(float speed, float mul) { }
        public virtual void Draw(ref SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
            Vector2 origin = new Vector2(m_Texture.Width / 2, m_Texture.Height / 2);

            spriteBatch.Draw(m_Texture, m_Position, rect, Color.White, m_Rotation + MathHelper.ToRadians(90), origin, m_Scale, SpriteEffects.None, 1);
        }
        public virtual void Update(GameTime gameTime)
        {

        }
        public void LoadTextureData()
        {
            m_TextureData = new Color[m_Texture.Width * m_Texture.Height];
            m_Texture.GetData(m_TextureData);
        }
    }
}
