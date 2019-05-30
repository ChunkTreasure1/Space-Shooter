using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay.Pickups
{
    class AmmoPickup : Pickup
    {
        //Member vars
        private int m_AmmoAdd;

        //Constructor sets the start values
        public AmmoPickup(Vector2 pos, Texture2D texture, Rectangle rect, GraphicsDeviceManager graphics) :
            base(pos, texture, rect, graphics)
        {
            m_Position = pos;
            m_Texture = texture;
            m_Rectangle = rect;

            m_Graphics = graphics;
            m_AmmoAdd = 30;
        }

        //Called when the player uses the pickup
        public void Use(Player.Player player)
        {
            //Add the ammo to the player
            player.SetCurrentAmmo(player.GetCurrentAmmo() + m_AmmoAdd);

            //Check if the current ammo of the player is more than the max ammo
            //and if it is, set the current ammo to the max ammo
            if (player.GetCurrentAmmo() > player.GetMaxAmmo())
            {
                player.SetCurrentAmmo(player.GetMaxAmmo());
            }
        }
    }
}
