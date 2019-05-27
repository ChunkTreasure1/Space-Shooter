using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using SpaceShooter.Gameplay.Player;
using SpaceShooter.Gameplay.Enemies;
using SpaceShooter.Gameplay;
using System;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    enum EGameState
    {
        eGS_Menu,
        eGS_Playing,
        eGS_Paused,
        eGS_GameOver
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private Player m_Player;

        private List<Enemy> m_Enemies = new List<Enemy>();
        private Texture2D m_EmptyTexture;

        private bool m_Collision = false;
        private bool m_CreatedEnemy = false;
        private bool m_EscPushed = false;

        private SpriteFont m_Font;
        private EGameState m_GameState;
        private Camera2D m_Camera;
        private EnemySpawner m_EnemySpawner;

        private int m_Width;
        private int m_Height;

        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Create player
            m_Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            m_Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;

            m_Width = GraphicsDevice.DisplayMode.Width;
            m_Height = GraphicsDevice.DisplayMode.Height;
            //m_Graphics.IsFullScreen = true;
            m_Graphics.ApplyChanges();

            m_Camera = new Camera2D(m_Width, m_Height);
            m_GameState = EGameState.eGS_Playing;
            m_Player = new Player(new Vector2(100, 100), 0, 1f, null, new Rectangle(0, 0, 0, 0), m_Graphics, 10, m_Camera);
            m_EnemySpawner = new EnemySpawner(ref m_Enemies, 2000, m_Graphics);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D bullet = Content.Load<Texture2D>("Images/bullet");

            //Add textures
            m_EmptyTexture = Content.Load<Texture2D>("Images/EmptyTexture");
            m_Player.SetTexture(Content.Load<Texture2D>("Images/PlayerShip"));
            m_Player.SetEmptyTexture(m_EmptyTexture);
            m_Player.SetBulletTexture(bullet);
            m_EnemySpawner.SetTexture(Content.Load<Texture2D>("Images/EnemyShip"));
            m_EnemySpawner.SetEmptyTexture(m_EmptyTexture);
            m_EnemySpawner.SetBulletTexture(bullet);

            //Fonts
            m_Font = Content.Load<SpriteFont>("Fonts/Roboto");

            m_Player.SetRectangle(new Rectangle((int)m_Player.GetPosition().X,
                                                (int)m_Player.GetPosition().Y,
                                                m_Player.GetTexture().Width,
                                                m_Player.GetTexture().Height));

            m_Player.LoadTextureData();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GetInput(gameTime);

            if (m_GameState == EGameState.eGS_Paused || m_GameState == EGameState.eGS_GameOver || m_GameState == EGameState.eGS_Menu)
            {
                return;
            }
            else if(m_GameState == EGameState.eGS_Playing)
            {
                UpdateEntities(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (m_Collision)
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
            }

            Vector3 screenScale = GetScreenScale();
            Matrix viewMatrix = m_Camera.GetTransform();

            m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));

            if (m_GameState == EGameState.eGS_Paused)
            {
                m_SpriteBatch.DrawString(m_Font, "PAUSED", new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2), Color.White);
            }
            else if(m_GameState == EGameState.eGS_GameOver)
            {
                m_SpriteBatch.DrawString(m_Font, "GAME OVER", new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2), Color.White);
            }

            //Draw the player
            m_Player.Draw(ref m_SpriteBatch);
            if (m_Enemies.Count > 0)
            {
                for (int i = 0; i < m_Enemies.Count; i++)
                {
                    m_Enemies[i].Draw(ref m_SpriteBatch);
                }
            }

            m_SpriteBatch.DrawString(m_Font, "FPS: " + 1 / (float)gameTime.ElapsedGameTime.TotalSeconds, new Vector2(100, 100), Color.White);
            m_SpriteBatch.DrawString(m_Font, "Score: " + m_Player.GetScore(), new Vector2(GraphicsDevice.DisplayMode.Width - 200, 100), Color.White);
            m_SpriteBatch.DrawString(m_Font, "Level: " + m_EnemySpawner.GetLevel(), new Vector2(GraphicsDevice.DisplayMode.Width - 200, 150), Color.White);

            m_SpriteBatch.End();
            base.Draw(gameTime);
        }

        //Gets the players input
        private void GetInput(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (!m_EscPushed)
                {
                    m_EscPushed = true;
                    if (m_GameState == EGameState.eGS_Paused)
                    {
                        m_GameState = EGameState.eGS_Playing;
                    }
                    else
                    {
                        m_GameState = EGameState.eGS_Paused;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                m_EscPushed = false;
            }
        }

        //Updates all the entities
        private void UpdateEntities(GameTime gameTime)
        {
            m_Player.Update(gameTime);

            //Update the enemies
            if (m_Enemies.Count > 0)
            {
                for (int i = 0; i < m_Enemies.Count; i++)
                {
                    m_Enemies[i].Update(gameTime);
                    m_Enemies[i].SetPlayerPosition(m_Player.GetPosition());

                    if (m_Enemies[i].GetBullets().Count > 0)
                    {
                        for (int j = 0; j < m_Enemies[i].GetBullets().Count; j++)
                        {
                            if (m_Enemies[i].GetBullets()[j].GetPosition().X > m_Graphics.PreferredBackBufferWidth || m_Enemies[i].GetBullets()[j].GetPosition().X < 0 ||
                                m_Enemies[i].GetBullets()[j].GetPosition().Y > m_Graphics.PreferredBackBufferHeight || m_Enemies[i].GetBullets()[j].GetPosition().Y < 0)
                            {
                                m_Enemies[i].GetBullets().RemoveAt(j);
                                j--;
                            }
                            else
                            {
                                m_Enemies[i].GetBullets()[j].Update(gameTime);
                            }
                        }
                    }
                }
            }

            //Update the bullets
            if (m_Player.GetBullets().Count > 0)
            {
                for (int i = 0; i < m_Player.GetBullets().Count; i++)
                {
                    if (m_Player.GetBullets()[i].GetPosition().X > m_Graphics.PreferredBackBufferWidth || m_Player.GetBullets()[i].GetPosition().X < 0 ||
                        m_Player.GetBullets()[i].GetPosition().Y > m_Graphics.PreferredBackBufferHeight || m_Player.GetBullets()[i].GetPosition().Y < 0)
                    {
                        m_Player.GetBullets().RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        m_Player.GetBullets()[i].Update(gameTime);
                    }
                }
            }

            //Check for bullet collision
            for (int i = 0; i < m_Player.GetBullets().Count; i++)
            {
                if (m_Player.GetBullets().Count == 0)
                {
                    break;
                }
                for (int j = 0; j < m_Enemies.Count; j++)
                {
                    if (i < 0)
                    {
                        i = 0;
                    }
                    if (IntersectsPixel(m_Player.GetBullets()[i].GetRectangle(), m_Player.GetBullets()[i].GetTextureData(), m_Enemies[j].GetRectangle(), m_Enemies[j].GetTextureData()))
                    {
                        m_Enemies[j].SetHealth(m_Enemies[j].GetHealth() - m_Player.GetBullets()[i].GetDamage());
                        if (m_Enemies[j].GetHealth() <= 0)
                        {
                            m_Player.SetKillCount(m_Player.GetKills() + 1);
                            m_Player.SetScore(m_Player.GetScore() + m_Enemies[j].GetKillScore());
                            m_Enemies.RemoveAt(j);
                        }
                        m_Player.GetBullets().RemoveAt(i);
                        if (m_Enemies.Count < 1 || m_Player.GetBullets().Count < 1)
                        {
                            break;
                        }
                        else
                        {
                            j--;
                            i--;
                        }
                        m_Collision = true;
                    }
                    else
                    {
                        m_Collision = false;
                    }
                }
            }

            //Check for enemy bullet collision
            for (int i = 0; i < m_Enemies.Count; i++)
            {
                for (int j = 0; j < m_Enemies[i].GetBullets().Count; j++)
                {
                    if (m_Enemies[i].GetBullets().Count == 0)
                    {
                        break;
                    }

                    if (IntersectsPixel(m_Enemies[i].GetBullets()[j].GetRectangle(), m_Enemies[i].GetBullets()[j].GetTextureData(), m_Player.GetRectangle(), m_Player.GetTextureData()))
                    {
                        m_Player.SetHealth(m_Player.GetHealth() - m_Enemies[i].GetBullets()[j].GetDamage());
                        if (m_Player.GetHealth() <= 0)
                        {
                            m_GameState = EGameState.eGS_GameOver;
                        }

                        m_Enemies[i].GetBullets().RemoveAt(j);
                        if (m_Enemies[i].GetBullets().Count < 1)
                        {
                            break;
                        }
                        else
                        {
                            j--;
                        }
                    }
                }
            }
        }
        static bool IntersectsPixel(Rectangle rect1, Color[] data1,
                                    Rectangle rect2, Color[] data2)
        {
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);

            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color color1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                    Color color2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                    if (color1.A != 0 && color2.A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Vector3 GetScreenScale()
        {
            var scaleX = (float)GraphicsDevice.DisplayMode.Width / (float)m_Width;
            var scaleY = (float)GraphicsDevice.DisplayMode.Height / (float)m_Height;
            return new Vector3(scaleX, scaleY, 1.0f);
        }
    }
}
