using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Gameplay
{
    public class Background
    {
        private Texture2D m_Background;
        private Vector2 m_StartPos;
        private Vector2 m_LastPos;

        private Player.Player m_Player;

        List<Vector2> backgroundPositions = new List<Vector2>();

        public void SetTexture(Texture2D texture) { m_Background = texture; }

        public Background(Vector2 startPos, Player.Player player)
        {
            m_StartPos = startPos;
            m_Player = player;

            backgroundPositions.Add(m_StartPos);
            m_LastPos = m_StartPos;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            for (int i = 0; i < backgroundPositions.Count; i++)
            {
                spriteBatch.Draw(m_Background, backgroundPositions[i], Color.White);
            }
        }

        public void Start()
        {
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
