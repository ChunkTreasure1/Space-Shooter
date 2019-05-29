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
        Player.Player m_Player;
        int m_Time;
        List<HealthPickup> m_HealthPickups = new List<HealthPickup>();
        List<AmmoPickup> m_AmmoPickups = new List<AmmoPickup>();
        Timer m_Timer;

        Texture2D m_HealthTexture;
        Texture2D m_AmmoTexture;
        GraphicsDeviceManager m_Graphics;

        private void SetTimer(int time)
        {
            m_Timer = new Timer(time);
            m_Timer.Elapsed += OnTimerEnd;
            m_Timer.AutoReset = false;
            m_Timer.Enabled = true;
        }
        public void SetHealthTexture(Texture2D texture) { m_HealthTexture = texture; }
        public void SetAmmoTexture(Texture2D texture) { m_AmmoTexture = texture; }

        public List<HealthPickup> GetHealthPickups() { return m_HealthPickups; }
        public List<AmmoPickup> GetAmmoPickups() { return m_AmmoPickups; }

        public Spawner(Player.Player player, int time, GraphicsDeviceManager graphics)
        {
            m_Player = player;
            m_Graphics = graphics;
            m_Time = time;

            SetTimer(time);
        }

        private void OnTimerEnd(Object source, ElapsedEventArgs e)
        {
            Spawn();
        }

        private void Spawn()
        {
            Vector2 pos = GetRandomPosition();
            m_HealthPickups.Add(new HealthPickup(pos, m_HealthTexture, new Rectangle((int)pos.X, (int)pos.Y, m_HealthTexture.Width, m_HealthTexture.Height), m_Graphics));
            m_HealthPickups[m_HealthPickups.Count - 1].LoadTextureData();

            pos = GetRandomPosition();
            m_AmmoPickups.Add(new AmmoPickup(pos, m_AmmoTexture, new Rectangle((int)pos.X, (int)pos.Y, m_AmmoTexture.Width, m_AmmoTexture.Height), m_Graphics));
            m_AmmoPickups[m_AmmoPickups.Count - 1].LoadTextureData();
        }

        private Vector2 GetRandomPosition()
        {
            Random random = new Random();
            int X = random.Next((int)m_Player.GetPosition().X - 1000, (int)m_Player.GetPosition().X + 1000);
            int Y;

            if (random.Next(0, 2) == 0)
            {
                Y = random.Next((int)m_Player.GetPosition().Y - 1000, (int)m_Player.GetPosition().Y - 500);
            }
            else
            {
                Y = random.Next((int)m_Player.GetPosition().Y + 500, (int)m_Player.GetPosition().Y + 1000);
            }

            return new Vector2(X, Y);
        }
    }
}
