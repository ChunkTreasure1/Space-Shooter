using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace SpaceShooter.Gameplay
{
    class Bullet
    {
        private Vector2 m_Position;
        private float m_Size;

        private Texture2D m_Texture;
        private float m_Rotation;

        //Getting
        public Vector2 GetPosition() { return m_Position; }
        public float GetRotation() { return m_Rotation; }

        //Setting
        public void SetPosition(Vector2 pos) { m_Position = pos; }
        public void SetTexture(Texture2D texture) { m_Texture = texture; }
        public void SetRotation(float rot) { m_Rotation = MathHelper.ToRadians(rot); }

        public Bullet(Vector2 pos, float rotation, float scale, Texture2D texture)
        {
            m_Position.X = pos.X;
            m_Position.Y = pos.Y;

            m_Size = scale;

            m_Rotation = rotation;
            m_Texture = texture;
        }

        public void Move(float speed)
        {
            Vector2 dir = new Vector2((float)Math.Cos(GetRotation()),
                                      (float)Math.Sin(GetRotation()));
            dir.Normalize();
            SetPosition(GetPosition() + dir * speed * 2);
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
            Vector2 origin = new Vector2(m_Texture.Width / 2, m_Texture.Height / 2);

            spriteBatch.Draw(m_Texture, m_Position, rect, Color.White, m_Rotation, origin, m_Size, SpriteEffects.None, 1);
        }
    }
}
