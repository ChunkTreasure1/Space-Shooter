using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay.Pickups
{
    class AmmoPickup : Pickup
    {
        private int m_AmmoAdd;

        public AmmoPickup(Vector2 pos, Texture2D texture, Rectangle rect, GraphicsDeviceManager graphics) :
            base(pos, texture, rect, graphics)
        {
            m_Position = pos;
            m_Texture = texture;
            m_Rectangle = rect;

            m_Graphics = graphics;
            m_AmmoAdd = 30;
        }

        public void Use(Player.Player player)
        {
            player.SetCurrentAmmo(player.GetCurrentAmmo() + m_AmmoAdd);
            if (player.GetCurrentAmmo() > player.GetMaxAmmo())
            {
                player.SetCurrentAmmo(player.GetMaxAmmo());
            }
        }
    }
}
