using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SpaceShooter.Gameplay.Enemies
{
    class EnemySpawner
    {
        //Private vars
        int m_Level;
        int m_Time;
        bool m_TimerStarted;
        List<Enemy> m_EnemyList;
        Timer m_Timer;

        Texture2D m_EnemyTexture;
        GraphicsDeviceManager m_Graphics;

        public EnemySpawner(ref List<Enemy> enemies, int time, Texture2D enemyTexture, GraphicsDeviceManager graphics)
        {
            m_EnemyList = enemies;
            m_Level = 0;
            m_Time = time;

            m_TimerStarted = false;
            m_EnemyTexture = enemyTexture;
            m_Graphics = graphics;

            SetTimer(time);
            m_TimerStarted = true;
        }
        public void SetTexture(Texture2D texture) { m_EnemyTexture = texture; }
        private void SetTimer(int time)
        {
            m_Timer = new Timer(time);
            m_Timer.Elapsed += OnTimerEnd;
            m_Timer.AutoReset = false;
            m_Timer.Enabled = true;
        }

        public void Update()
        {
            if (!m_TimerStarted)
            {
                SetTimer(m_Time);
                m_TimerStarted = true;
            }
        }
        private void OnTimerEnd(Object source, ElapsedEventArgs e)
        {
            Vector2 pos = GetRandomPosition();

            //Spawn
            m_EnemyList.Add(new Enemy(pos, 0, 1, m_EnemyTexture, 4, new Rectangle((int)pos.X, (int)pos.Y, m_EnemyTexture.Width, m_EnemyTexture.Height), m_Graphics));
            m_EnemyList[m_EnemyList.Count - 1].LoadTextureData();
            m_TimerStarted = false;
        }

        private Vector2 GetRandomPosition()
        {
            Random random = new Random();

            int X = 2000;
            int Y = random.Next(0, 1100);

            return new Vector2(X, Y);
        }
    }
}
