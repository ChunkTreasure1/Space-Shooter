using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay
{
    public class Background
    {
        //Member vars
        private Texture2D m_Background;
        private Vector2 m_StartPos;
        private Vector2 m_LastPos;

        private Player.Player m_Player;
        List<Vector2> backgroundPositions = new List<Vector2>();

        //Setting
        public void SetTexture(Texture2D texture) { m_Background = texture; }

        //Constructor sets the start values
        public Background(Vector2 startPos, Player.Player player)
        {
            m_StartPos = startPos;
            m_Player = player;

            backgroundPositions.Add(m_StartPos);
            m_LastPos = m_StartPos;
        }

        //Draws the backgrounds
        public void Draw(ref SpriteBatch spriteBatch)
        {
            for (int i = 0; i < backgroundPositions.Count; i++)
            {
                spriteBatch.Draw(m_Background, backgroundPositions[i], Color.White);
            }
        }

        //Called when the background should be created
        public void Start()
        {
            /*
             * Simply get all the positions that there should be backgrounds.
             * This will not create an etern background, but a very large one,
             * this should be changed to only create backgrounds around the player
             */

            //Create the left, right, up and down positions
            for (int i = 0; i < 100; i++)
            {
                backgroundPositions.Add(new Vector2(m_LastPos.X + (i + 1) * m_Background.Width, 0));
            }

            for (int i = 0; i < 100; i++)
            {
                backgroundPositions.Add(new Vector2(0, m_LastPos.Y + (i + 1) * m_Background.Height));
            }

            for (int i = 0; i < 100; i++)
            {
                backgroundPositions.Add(new Vector2(-(m_LastPos.X + (i + 1) * m_Background.Width), 0));
            }

            for (int i = 0; i < 100; i++)
            {
                backgroundPositions.Add(new Vector2(0, -(m_LastPos.Y + (i + 1) * m_Background.Height)));
            }

            //Fill the four squares
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    backgroundPositions.Add(new Vector2(m_LastPos.X + (j + 1) * m_Background.Width, m_LastPos.Y + (i + 1) * m_Background.Height));
                }

                for (int j = 0; j < 100; j++)
                {
                    backgroundPositions.Add(new Vector2(-(m_LastPos.X + (j + 1) * m_Background.Width), m_LastPos.Y + (i + 1) * m_Background.Height));
                }

                for (int j = 0; j < 100; j++)
                {
                    backgroundPositions.Add(new Vector2(m_LastPos.X + (j + 1) * m_Background.Width, -(m_LastPos.Y + (i + 1) * m_Background.Height)));
                }

                for (int j = 0; j < 100; j++)
                {
                    backgroundPositions.Add(new Vector2(-(m_LastPos.X + (j + 1) * m_Background.Width), -(m_LastPos.Y + (i + 1) * m_Background.Height)));
                }
            }
        }
    }
}
