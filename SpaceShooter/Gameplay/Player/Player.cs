using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System;

namespace SpaceShooter.Gameplay.Player
{
    public class Player : Entity
    {

        private List<Bullet> m_Bullets = new List<Bullet>();

        public List<Bullet> GetBullets() { return m_Bullets; }

        public Player(Vector2 pos, float rotation, float scale, Texture2D texture, Rectangle rect) :
            base(pos, rotation, scale, texture, rect)
        {
            m_Position.X = pos.X;
            m_Position.Y = pos.Y;

            m_Scale = scale;
            m_Rotation = rotation;
            m_Texture = texture;
            m_Rectangle = rect;
        }

        //Overrides the drawing of the player
        public override void Draw(ref SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
            Vector2 origin = new Vector2(m_Texture.Width / 2, m_Texture.Height / 2);

            spriteBatch.Draw(m_Texture, m_Position, rect, Color.White, m_Rotation + MathHelper.ToRadians(90), origin, m_Scale, SpriteEffects.None, 1);

            if (m_Bullets.Count > 0)
            {
                for (int i = 0; i < m_Bullets.Count; i++)
                {
                    m_Bullets[i].Draw(ref spriteBatch);
                }
            }
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
