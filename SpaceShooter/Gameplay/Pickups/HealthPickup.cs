using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay.Pickups
{
    class HealthPickup : Pickup
    {
        private float m_HealthAdd;

        public HealthPickup(Vector2 pos, Texture2D texture, Rectangle rect, GraphicsDeviceManager graphics) : 
            base(pos, texture, rect, graphics)
        {
            m_Position = pos;
            m_Texture = texture;
            m_Rectangle = rect;

            m_Graphics = graphics;
            m_HealthAdd = 100;
        }

        public void Use(Player.Player player)
        {
            player.SetHealth(player.GetHealth() + m_HealthAdd);

            if (player.GetHealth() > player.GetMaxHealth())
            {
                player.SetHealth(player.GetMaxHealth());
            }
        }
    }
}
