﻿using System;
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
        List<Enemy> m_EnemyList;
        Timer m_Timer;

        Texture2D m_EnemyTexture;
        Texture2D m_BulletTexture;
        GraphicsDeviceManager m_Graphics;

        public EnemySpawner(ref List<Enemy> enemies, int time, GraphicsDeviceManager graphics)
        {
            m_EnemyList = enemies;
            m_Level = 0;
            m_Time = time;

            m_Graphics = graphics;

            SetTimer(time);
        }
        public void SetTexture(Texture2D texture) { m_EnemyTexture = texture; }
        public void SetBulletTexture(Texture2D texture) { m_BulletTexture = texture; }
        private void SetTimer(int time)
        {
            m_Timer = new Timer(time);
            m_Timer.Elapsed += OnTimerEnd;
            m_Timer.AutoReset = false;
            m_Timer.Enabled = true;
        }
        private void OnTimerEnd(Object source, ElapsedEventArgs e)
        {
            Vector2 pos = GetRandomPosition();

            //Spawn
            m_EnemyList.Add(new Enemy(pos, 0, 1, m_EnemyTexture, 4, new Rectangle((int)pos.X, (int)pos.Y - 50, m_EnemyTexture.Width, m_EnemyTexture.Height), m_Graphics));
            m_EnemyList[m_EnemyList.Count - 1].LoadTextureData();
            m_EnemyList[m_EnemyList.Count - 1].SetBulletTexture(m_BulletTexture);
            m_Timer.Enabled = false;
            m_Timer.Dispose();

            SetTimer(m_Time);
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