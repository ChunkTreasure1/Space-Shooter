using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay.Enemies
{
    public class Enemy : Entity
    {
        private float m_Speed;
        private Vector2 m_PlayerPosition;

        public Enemy(Vector2 pos, float rotation, float scale, Texture2D texture, float speed) :
            base(pos, rotation, scale, texture)
        {
            m_Position.X = pos.X;
            m_Position.Y = pos.Y;

            m_Scale = scale;
            m_Rotation = rotation;
            m_Texture = texture;

            m_Speed = speed;
        }

        public void SetPlayerPosition(Vector2 pos) { m_PlayerPosition = pos; }

        public override void Update(GameTime gameTime)
        {
            Move(m_Speed, 1);
            base.Update(gameTime);
        }

        public override void Move(float speed, float mul)
        {
            Vector2 dir = m_PlayerPosition - m_Position;
            dir.Normalize();

            SetPosition(GetPosition() + dir * speed * mul);
            base.Move(speed, mul);
        }
    }
}
