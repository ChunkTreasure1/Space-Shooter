using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter.Gameplay.Enemies
{
    class EnemySpawner
    {
        //Private vars
        private int m_Level;
        private int m_Time;
        private List<Enemy> m_EnemyList;
        private Timer m_Timer;

        private Texture2D m_EnemyTexture;
        private Texture2D m_BulletTexture;
        private Texture2D m_EmptyTexture;
        private GraphicsDeviceManager m_Graphics;

        private Player.Player m_Player;
        private Random m_Random = new Random();
        private SoundEffect m_ShootSound;
        private EGameState m_LastState;

        private bool m_Paused = false;

        //Getting
        public int GetLevel() { return m_Level; }
        //Returns a random position for the enemy to spawn at
        private Vector2 GetRandomPosition()
        {
            //Get a random X and Y position based on where the player is currently located
            int X = m_Random.Next((int)m_Player.GetPosition().X - 1000, (int)m_Player.GetPosition().X + 1000);
            int Y = m_Random.Next((int)m_Player.GetPosition().Y - 1000, (int)m_Player.GetPosition().Y + 1000);

            return new Vector2(X, Y);
        }

        //Setting
        public void SetTexture(Texture2D texture) { m_EnemyTexture = texture; }
        public void SetBulletTexture(Texture2D texture) { m_BulletTexture = texture; }
        public void SetEmptyTexture(Texture2D texture) { m_EmptyTexture = texture; }
        public void SetShootSound(SoundEffect sound) { m_ShootSound = sound; }
        
        //Starts the timer that spawns an enemy
        private void SetTimer(int time)
        {
            m_Timer = new Timer(time);
            m_Timer.Elapsed += OnTimerEnd;
            m_Timer.AutoReset = false;
            m_Timer.Enabled = true;
        }

        //Constructor sets all the start values
        public EnemySpawner(ref List<Enemy> enemies, int time, GraphicsDeviceManager graphics, Player.Player player)
        {
            m_EnemyList = enemies;
            m_Level = 1;
            m_Time = time;

            m_Graphics = graphics;
            m_Player = player;
            m_LastState = Game1.GetGameState();

            SetTimer(time);
        }

        //Called when the timer ends
        private void OnTimerEnd(Object source, ElapsedEventArgs e)  
        {
            //If the game is paused simply return
            if (m_Paused)
            {
                return;
            }

            //Get a random position for the enemy to spawn
            Vector2 pos = GetRandomPosition();

            //If the amount of player kills is equal to the current level
            //it should increase the level and set the kill count back to zero
            if (m_Player.GetKills() >= m_Level)
            {
                m_Level++;
                m_Player.SetKillCount(0);
            }

            //If the amount of enemies currenly alive is less than
            //the current level, spawn another one
            if (m_EnemyList.Count < m_Level)
            {
                //Spawn an enemy and set the values
                m_EnemyList.Add(new Enemy(pos, 0, 1, m_EnemyTexture, 4, new Rectangle((int)pos.X, (int)pos.Y - 50, m_EnemyTexture.Width, m_EnemyTexture.Height), m_Graphics, m_EmptyTexture));
                m_EnemyList[m_EnemyList.Count - 1].LoadTextureData();
                m_EnemyList[m_EnemyList.Count - 1].SetBulletTexture(m_BulletTexture);

                m_EnemyList[m_EnemyList.Count - 1].SetShootSound(m_ShootSound);
                m_EnemyList[m_EnemyList.Count - 1].SetBulletDamage(m_Level * 10);
                m_EnemyList[m_EnemyList.Count - 1].SetMaxHealth(m_Level * 30);
            }

            //Turn off the timer and dispose of it
            m_Timer.Enabled = false;
            m_Timer.Dispose();

            //Set a new timer to keep the loop going
            SetTimer(m_Time);
        }

        //Updates the spawner
        public void Update()
        {
            //Check if the game stae has changed
            if (m_LastState != Game1.GetGameState())
            {
                //If the game state is one that should be paused set m_Paused to true
                //and set the last game state
                if (Game1.GetGameState() == EGameState.eGS_GameOver || Game1.GetGameState() == EGameState.eGS_Menu || Game1.GetGameState() == EGameState.eGS_Paused)
                {
                    m_Paused = true;
                    m_LastState = Game1.GetGameState();
                }
                else
                {
                    //Set m_Paused to false, set the last game state and start a new timer for an enemy to be spawned
                    m_Paused = false;
                    m_LastState = Game1.GetGameState();
                    SetTimer(2000);
                }
            }
        }
    }
}       