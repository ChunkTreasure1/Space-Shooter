using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SpaceShooter.Gameplay.Enemies
{
    class EnemySpawner
    {
        //Private vars
        private int m_Level;
        private int m_Time;
        private int m_KillCount = 0;
        private List<Enemy> m_EnemyList;
        private Timer m_Timer;

        private Texture2D m_EnemyTexture;
        private Texture2D m_BulletTexture;
        private Texture2D m_EmptyTexture;
        private GraphicsDeviceManager m_Graphics;

        private Player.Player m_Player;
        private Random m_Random = new Random();

        //Getting
        public int GetLevel() { return m_Level; }

        //Setting
        public void SetTexture(Texture2D texture) { m_EnemyTexture = texture; }
        public void SetBulletTexture(Texture2D texture) { m_BulletTexture = texture; }
        public void SetEmptyTexture(Texture2D texture) { m_EmptyTexture = texture; }
        private void SetTimer(int time)
        {
            m_Timer = new Timer(time);
            m_Timer.Elapsed += OnTimerEnd;
            m_Timer.AutoReset = false;
            m_Timer.Enabled = true;
        }

        public EnemySpawner(ref List<Enemy> enemies, int time, GraphicsDeviceManager graphics, Player.Player player)
        {
            m_EnemyList = enemies;
            m_Level = 1;
            m_Time = time;

            m_Graphics = graphics;
            m_Player = player;

            SetTimer(time);
        }
        private void OnTimerEnd(Object source, ElapsedEventArgs e)  
        {
            Vector2 pos = GetRandomPosition();

            if (m_KillCount >= m_Level * 10)
            {
                m_Level++;
                m_KillCount = 0;
            }

            if (m_EnemyList.Count < 7)
            {
                //Spawn
                m_EnemyList.Add(new Enemy(pos, 0, 1, m_EnemyTexture, 4, new Rectangle((int)pos.X, (int)pos.Y - 50, m_EnemyTexture.Width, m_EnemyTexture.Height), m_Graphics, m_EmptyTexture));
                m_EnemyList[m_EnemyList.Count - 1].LoadTextureData();
                m_EnemyList[m_EnemyList.Count - 1].SetBulletTexture(m_BulletTexture);
            }
            m_Timer.Enabled = false;
            m_Timer.Dispose();

            SetTimer(m_Time);
        }
        private Vector2 GetRandomPosition()
        {
            int X = m_Random.Next((int)m_Player.GetPosition().X - 1000, (int)m_Player.GetPosition().X + 1000);
            int Y = m_Random.Next((int)m_Player.GetPosition().Y - 1000, (int)m_Player.GetPosition().Y + 1000);

            return new Vector2(X, Y);
        }
    }
}       