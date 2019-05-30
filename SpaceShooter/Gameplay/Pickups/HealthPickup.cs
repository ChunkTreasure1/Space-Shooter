using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay.Pickups
{
    class HealthPickup : Pickup
    {
        //The amount to add to the player
        private float m_HealthAdd;

        //Constructor sets the start values
        public HealthPickup(Vector2 pos, Texture2D texture, Rectangle rect, GraphicsDeviceManager graphics) : 
            base(pos, texture, rect, graphics)
        {
            m_Position = pos;
            m_Texture = texture;
            m_Rectangle = rect;

            m_Graphics = graphics;
            m_HealthAdd = 100;
        }

        //Called when the pickup is used
        public void Use(Player.Player player)
        {
            //Add the health to the player
            player.SetHealth(player.GetHealth() + m_HealthAdd);

            //Check if the players health is more than the max health
            //if it is set the health to the max health
            if (player.GetHealth() > player.GetMaxHealth())
            {
                player.SetHealth(player.GetMaxHealth());
            }
        }
    }
}
