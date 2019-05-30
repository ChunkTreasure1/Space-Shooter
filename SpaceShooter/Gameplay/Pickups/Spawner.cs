using System.Timers;
using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SpaceShooter.Gameplay.Pickups;

namespace SpaceShooter.Gameplay
{
    class Spawner
    {
        //Member vars
        private Player.Player m_Player;
        private Timer m_Timer;
        private int m_Time;

        private List<HealthPickup> m_HealthPickups = new List<HealthPickup>();
        private List<AmmoPickup> m_AmmoPickups = new List<AmmoPickup>();

        private Texture2D m_HealthTexture;
        private Texture2D m_AmmoTexture;
        private GraphicsDeviceManager m_Graphics;

        //Setting
        private void SetTimer(int time)
        {
            //Creates a new spawn timer
            m_Timer = new Timer(time);
            m_Timer.Elapsed += OnTimerEnd;
            m_Timer.AutoReset = false;
            m_Timer.Enabled = true;
        }
        public void SetHealthTexture(Texture2D texture) { m_HealthTexture = texture; }
        public void SetAmmoTexture(Texture2D texture) { m_AmmoTexture = texture; }

        //Getting
        public List<HealthPickup> GetHealthPickups() { return m_HealthPickups; }
        public List<AmmoPickup> GetAmmoPickups() { return m_AmmoPickups; }
        //Returns a random position for the pickup to spawn at
        private Vector2 GetRandomPosition()
        {
            /*
             * Randomly creates a position at which the pickup can spawn at.
             * This uses the players position to get the spawn position
             */
            Random random = new Random();
            int X = random.Next((int)m_Player.GetPosition().X - 500, (int)m_Player.GetPosition().X + 500);
            int Y;

            if (random.Next(0, 2) == 0)
            {
                Y = random.Next((int)m_Player.GetPosition().Y - 500, (int)m_Player.GetPosition().Y - 250);
            }
            else
            {
                Y = random.Next((int)m_Player.GetPosition().Y + 250, (int)m_Player.GetPosition().Y + 500);
            }

            return new Vector2(X, Y);
        }

        //Constructor sets all the start values
        public Spawner(Player.Player player, int time, GraphicsDeviceManager graphics)
        {
            m_Player = player;
            m_Graphics = graphics;
            m_Time = time;

            SetTimer(time);
        }
        
        //Called when the timer ends
        private void OnTimerEnd(Object source, ElapsedEventArgs e)
        {
            //Spawns the pickups
            Spawn();
        }

        //Called when pickups should be spawned
        private void Spawn()
        {
            //Gets a random position, creates the pickup and loads the texture data
            Vector2 pos = GetRandomPosition();
            m_HealthPickups.Add(new HealthPickup(pos, m_HealthTexture, new Rectangle((int)pos.X, (int)pos.Y, m_HealthTexture.Width, m_HealthTexture.Height), m_Graphics));
            m_HealthPickups[m_HealthPickups.Count - 1].LoadTextureData();

            pos = GetRandomPosition();
            m_AmmoPickups.Add(new AmmoPickup(pos, m_AmmoTexture, new Rectangle((int)pos.X, (int)pos.Y, m_AmmoTexture.Width, m_AmmoTexture.Height), m_Graphics));
            m_AmmoPickups[m_AmmoPickups.Count - 1].LoadTextureData();
        }
    }
}
