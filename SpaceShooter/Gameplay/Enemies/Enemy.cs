using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay.Enemies
{
    public class Enemy : Entity
    {
        public Enemy(Vector2 pos, float rotation, float scale, Texture2D texture) :
            base(pos, rotation, scale, texture)
        {
            m_Position.X = pos.X;
            m_Position.Y = pos.Y;

            m_Scale = scale;
            m_Rotation = rotation;
            m_Texture = texture;
        }


    }
}
