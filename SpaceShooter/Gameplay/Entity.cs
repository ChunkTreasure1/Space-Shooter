using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace SpaceShooter.Gameplay
{
    public class Entity
    {
        protected Vector2 m_Position;
        protected Texture2D m_Texture;

        protected float m_Rotation;
        protected float m_Scale;

        //Getting
        public Vector2 GetPosition() { return m_Position; }
        public float GetRotation() { return m_Rotation; }

        //Setting
        public void SetPosition(Vector2 pos) { m_Position = pos; }
        public void SetTexture(Texture2D texture) { m_Texture = texture; }
        public void SetRotation(float rot) { m_Rotation = MathHelper.ToRadians(rot); }

        public Entity(Vector2 pos, float rotation, float scale, Texture2D texture)
        {
            m_Position.X = pos.X;
            m_Position.Y = pos.Y;

            m_Scale = scale;
            m_Rotation = rotation;
            m_Texture = texture;
        }
        public virtual void Move(float speed, float mul)
        {
            Vector2 dir = new Vector2((float)Math.Cos(GetRotation()),
                                      (float)Math.Sin(GetRotation()));
            dir.Normalize();
            SetPosition(GetPosition() + dir * speed * mul);
        }
        public virtual void Draw(ref SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
            Vector2 origin = new Vector2(m_Texture.Width / 2, m_Texture.Height / 2);

            spriteBatch.Draw(m_Texture, m_Position, rect, Color.White, m_Rotation, origin, m_Scale, SpriteEffects.None, 1);
        }
        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
