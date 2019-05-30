using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Gameplay
{
    class Explosion
    {
        private Texture2D m_Texture;
        private Vector2 m_Position;
        private float m_Timer;

        private float m_Interval;
        private Vector2 m_Origin;
        private int m_CurrentFrame;

        private int m_SpriteWidth;
        private int m_SpriteHeight;
        private Rectangle m_SourceRectangle;

        private bool m_IsVisible;

        public Explosion(Texture2D texture, Vector2 position)
        {
            m_Position = position;
            m_Texture = texture;
            m_Timer = 0f;

            m_Interval = 20f;
            m_CurrentFrame = 1;
            m_SpriteWidth = 128;
            m_SpriteHeight = 128;
            m_IsVisible = true;
        }
        
        //Updates the explosion
        public void Update(GameTime gameTime)
        {

            //Add to the timer 
            m_Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Check if the timer has ended (interval is end time)
            if (m_Timer > m_Interval)
            {
                //Add to the current frame of the animation and reset the timer
                m_CurrentFrame++;
                m_Timer = 0f;
            }

            //Check the current frame and if it is equal to 6, reset the frames and set visible to false
            if (m_CurrentFrame == 6)
            {
                m_IsVisible = false;
                m_CurrentFrame = 0;
            }

            //Change the position of the source rectangle depending on where on the spritesheet we want to be
            //Also change to origin to the new rectangle
            m_SourceRectangle = new Rectangle(m_CurrentFrame * m_SpriteWidth, 0, m_SpriteWidth, m_SpriteHeight);
            m_Origin = new Vector2(m_SourceRectangle.Width / 2, m_SourceRectangle.Height / 2);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the explosion if it's visible
            if (m_IsVisible)
            {
                spriteBatch.Draw(m_Texture, m_Position, m_SourceRectangle, Color.White, 0f, m_Origin, 1f, SpriteEffects.None, 0);
            }
        }
    }
}
