using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceShooter.Gameplay
{
    public class Asteroid : Entity
    {
        private Vector2 m_SpawnPosition;
        private Vector2 m_DesiredPosition;
        private Random m_Random = new Random();

        private int m_RotationFrames = 1;
        private int m_CurrFrames = 0;

        public Asteroid(Vector2 pos, float rotation, float scale, Texture2D texture, Rectangle rect, GraphicsDeviceManager graphics) : 
            base(pos, rotation, scale, texture, rect, graphics)
        {
            m_SpawnPosition = pos;
            m_DesiredPosition = pos;

            m_Position = pos;
            m_Rotation = rotation;
            m_Scale = scale;

            m_Texture = texture;
            m_Rectangle = rect;
            m_Graphics = graphics;
        }

        public override void Move(float speed, float mul)
        {
            if (m_DesiredPosition == m_Position)
            {
                m_DesiredPosition = m_SpawnPosition += new Vector2(m_Random.Next(-100, 100), m_Random.Next(-100, 100));
            }

            Vector2 dir = m_DesiredPosition - m_Position;
            dir.Normalize();

            m_Rotation = (float)Math.Atan2(dir.Y, dir.X);
            SetPosition(GetPosition() + dir * speed);

            base.Move(speed, mul);
        }

        public override void Update(GameTime gameTime)
        {
            if (m_CurrFrames == m_RotationFrames)
            {
                m_CurrFrames = 0;
                SetRotation(MathHelper.ToDegrees(GetRotation()) + 2f);
            }
            else
            {
                m_CurrFrames++;
            }

            if (MathHelper.ToDegrees(GetRotation()) == 360)
            {
                SetRotation(0);
            }

            //Move(2, 0);
            base.Update(gameTime);
        }
    }
}
