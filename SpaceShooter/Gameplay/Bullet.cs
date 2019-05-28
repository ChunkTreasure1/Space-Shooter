using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceShooter.Gameplay
{
    public class Bullet : Entity
    {
        private float m_Speed;
        private float m_Damage = 30f;

        //Setting
        public void SetDamage(float damage) { m_Damage = damage; }

        //Getting
        public float GetDamage() { return m_Damage; }

        public Bullet(Vector2 pos, float rotation, float scale, Texture2D texture, float speed, Rectangle rect, GraphicsDeviceManager graphics) : 
            base(pos, rotation, scale, texture, rect, graphics)
        {
            m_Position.X = pos.X;
            m_Position.Y = pos.Y;

            m_Scale = scale;
            m_Rotation = rotation;
            m_Texture = texture;

            m_Speed = speed;
            m_Rectangle = rect;
            m_Graphics = graphics;
        }

        public override void Update(GameTime gameTime)
        {
            Move(m_Speed, 2);
            m_Rectangle = new Rectangle((int)m_Position.X, (int)m_Position.Y, m_Texture.Width, m_Texture.Height);
            base.Update(gameTime);
        }

        public override void Move(float speed, float mul)
        {
            Vector2 dir = new Vector2((float)Math.Cos(GetRotation()),
                          (float)Math.Sin(GetRotation()));
            dir.Normalize();
            SetPosition(GetPosition() + dir * speed * mul);

            base.Move(speed, mul);
        }
    }
}
