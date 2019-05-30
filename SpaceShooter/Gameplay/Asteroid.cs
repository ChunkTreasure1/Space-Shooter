using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceShooter.Gameplay
{
    public class Asteroid : Entity
    {
        //Member vars
        private Vector2 m_SpawnPosition;
        private Vector2 m_DesiredPosition;
        private Random m_Random = new Random();

        private int m_RotationFrames = 1;
        private int m_CurrFrames = 0;

        //Constructor sets all the standard values
        public Asteroid(Vector2 pos, float rotation, float scale, Texture2D texture, Rectangle rect, GraphicsDeviceManager graphics) : 
            base(pos, rotation, scale, texture, rect, graphics)
        {
            m_SpawnPosition = pos;
            m_DesiredPosition = pos;

            m_Position = pos;
            m_Rotation = rotation;
            m_Scale = scale;

            m_Texture = texture;
            m_Rectangle = rect;
            m_Graphics = graphics;
        }

        //Updates the asteroid
        public override void Update(GameTime gameTime)
        {
            //Checks if the current frames have reached the rotation frames
            if (m_CurrFrames == m_RotationFrames)
            {
                //Reset the current frames and change the rotation by 3 degrees
                m_CurrFrames = 0;
                SetRotation(MathHelper.ToDegrees(GetRotation()) + 2f);
            }
            else
            {
                //Add one to the frames
                m_CurrFrames++;
            }

            //Check if the rotation has reached 360 degrees
            //If so set the rotation back to 0 for simpliness
            if (MathHelper.ToDegrees(GetRotation()) == 360)
            {
                SetRotation(0);
            }

            base.Update(gameTime);
        }
    }
}
