using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay
{
    public class Bullet : Entity
    {
        private float m_Speed;
        public Bullet(Vector2 pos, float rotation, float scale, Texture2D texture, float speed) : 
            base(pos, rotation, scale, texture)
        {
            m_Position.X = pos.X;
            m_Position.Y = pos.Y;

            m_Scale = scale;
            m_Rotation = rotation;
            m_Texture = texture;
            m_Speed = speed;
        }

        public override void Update(GameTime gameTime)
        {
            Move(m_Speed, 2);
            base.Update(gameTime);
        }
    }
}
