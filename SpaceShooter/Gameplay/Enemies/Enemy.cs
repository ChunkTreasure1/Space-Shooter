using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using System;
using System.Collections.Generic;
using System.Timers;

namespace SpaceShooter.Gameplay.Enemies
{
    public class Enemy : Entity
    {
        private float m_Speed;
        private Vector2 m_PlayerPosition;
        private Timer m_Timer;

        private Texture2D m_BulletTexture;
        private List<Bullet> m_Bullets = new List<Bullet>();
        private bool m_ShootTimerStarted = false;

        private Texture2D m_EmptyTexture;
        private float m_Health = 50f;
        private float m_MaxHealth = 50f;

        private int m_KillScore = 10;
        private SoundEffect m_ShootSound;
        private float m_BulletDamage = 30f;

        //Getting
        public List<Bullet> GetBullets() { return m_Bullets; }
        public float GetHealth() { return m_Health; }
        public int GetKillScore() { return m_KillScore; }

        //Setting
        public void SetBulletTexture(Texture2D texture) { m_BulletTexture = texture; }
        public void SetHealth(float health) { m_Health = health; }
        public void SetMaxHealth(float health) { m_MaxHealth = health; m_Health = health; }
        public void SetKillScore(int i) { m_KillScore = i; }
        public void SetShootSound(SoundEffect sound) { m_ShootSound = sound; }
        public void SetBulletDamage(float amount) { m_BulletDamage = amount; }
        public void SetPlayerPosition(Vector2 pos) { m_PlayerPosition = pos; }
        
        //Constructor sets the start values
        public Enemy(Vector2 pos, float rotation, float scale, Texture2D texture, float speed, Rectangle rect, GraphicsDeviceManager graphics, Texture2D emptyTexture) :
            base(pos, rotation, scale, texture, rect, graphics)
        {
            m_Position.X = pos.X;
            m_Position.Y = pos.Y;

            m_Scale = scale;
            m_Rotation = rotation;
            m_Texture = texture;

            m_Speed = speed;
            m_Graphics = graphics;
            m_EmptyTexture = emptyTexture;
        }

        //Updates the enemy
        public override void Update(GameTime gameTime)
        {
            //Sets the enemys rectangle
            m_Rectangle = new Rectangle((int)m_Position.X, (int)m_Position.Y, m_Texture.Width, m_Texture.Height);

            //If the enemy is less than 1000 l.e away from the player it should start a timer for the shooting
            if (Vector2.Distance(m_PlayerPosition, m_Position) < 1000 && !m_ShootTimerStarted)
            {
                SetTimer(2000);
                m_ShootTimerStarted = true;
            }

            //Move the enemy
            Move(m_Speed, 1);
            base.Update(gameTime);
        }

        //Moves the enemy
        public override void Move(float speed, float mul)
        {
            //Get the direction it should move on and normalize it
            Vector2 dir = m_PlayerPosition - m_Position;
            dir.Normalize();

            //Get the rotation from the direction
            m_Rotation = (float)Math.Atan2(dir.Y, dir.X);

            //If the enemy is further away from the player than 450 l.e it should move
            if (Vector2.Distance(m_PlayerPosition, m_Position) > 450)
            {
                SetPosition(GetPosition() + dir * speed * mul);
            }
            base.Move(speed, mul);
        }

        //Draws the enemy
        public override void Draw(ref SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
            Vector2 origin = new Vector2(m_Texture.Width / 2, m_Texture.Height / 2);

            //Draws the enemy
            spriteBatch.Draw(m_Texture, m_Position, rect, Color.White, m_Rotation + MathHelper.ToRadians(90), origin, m_Scale, SpriteEffects.None, 1);

            //Draws the health bar in different color depending on the value
            if (m_Health > m_MaxHealth / 2)
            {
                spriteBatch.Draw(m_EmptyTexture, new Rectangle((int)m_Position.X - 50, (int)m_Position.Y + m_Texture.Height, (int)(m_Texture.Width * (m_Health / m_MaxHealth)), m_EmptyTexture.Height + 10), Color.Green);
            }

            if (m_Health <= m_MaxHealth / 2)
            {
                spriteBatch.Draw(m_EmptyTexture, new Rectangle((int)m_Position.X - 50, (int)m_Position.Y + m_Texture.Height, (int)(m_Texture.Width * (m_Health / m_MaxHealth)), m_EmptyTexture.Height + 10), Color.Yellow);
            }

            if (m_Health <= m_MaxHealth / 5)
            {
                spriteBatch.Draw(m_EmptyTexture, new Rectangle((int)m_Position.X - 50, (int)m_Position.Y + m_Texture.Height, (int)(m_Texture.Width * (m_Health / m_MaxHealth)), m_EmptyTexture.Height + 10), Color.Red);
            }

            //Draws the enemys bullets
            if (m_Bullets.Count > 0)
            {
                for (int i = 0; i < m_Bullets.Count; i++)
                {
                    m_Bullets[i].Draw(ref spriteBatch);
                }
            }
            base.Draw(ref spriteBatch);
        }
        
        //Shoots a bullet from the enemy
        private void Shoot()
        {
            //Play the shoot sound
            m_ShootSound.Play();

            //Create a new bullet
            GetBullets().Add(new Bullet(new Vector2(GetPosition().X, GetPosition().Y),
                GetRotation(), 0.5f, m_BulletTexture, 10f, new Rectangle((int)GetPosition().X, (int)GetPosition().Y, m_BulletTexture.Width, m_BulletTexture.Height),
                m_Graphics));

            //Load the texture data and set the damage
            GetBullets()[GetBullets().Count - 1].LoadTextureData();
            GetBullets()[GetBullets().Count - 1].SetDamage(m_BulletDamage);
        }

        //Sets the shooting timer
        private void SetTimer(int time)
        {
            m_Timer = new Timer(time);
            m_Timer.Elapsed += OnTimerEnd;
            m_Timer.AutoReset = false;
            m_Timer.Enabled = true;

            m_ShootTimerStarted = true;
        }

        //Called when the timer ends
        private void OnTimerEnd(Object source, ElapsedEventArgs e)
        {
            //Disable and dispose of the timer
            m_Timer.Enabled = false;
            m_Timer.Dispose();

            //Shoots a bullet when the timer runs out
            Shoot();
            m_ShootTimerStarted = false;
        }
    }
}