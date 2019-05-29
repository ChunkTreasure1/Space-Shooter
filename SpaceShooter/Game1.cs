using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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

    public enum EGameState
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
        private List<Explosion> m_Explosions = new List<Explosion>();
        private Texture2D m_EmptyTexture;

        private Texture2D m_Explosion;
        private bool m_Collision = false;
        private bool m_EscPushed = false;

        private SpriteFont m_Font;
        private static EGameState m_GameState;
        private Camera2D m_Camera;

        private EnemySpawner m_EnemySpawner;
        private Spawner m_Spawner;
        private Random m_Random = new Random();

        private Vector2 m_LastAsteroidCreation;
        private List<Asteroid> m_Asteroids = new List<Asteroid>();
        private Texture2D m_Asteroid;

        private int m_Width;
        private int m_Height;

        private SoundEffect m_ExplosionSound;
        private Background m_Background;

        public static EGameState GetGameState() { return m_GameState; }

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
            m_Camera.Position = new Vector2(0, 0);
            m_GameState = EGameState.eGS_Playing;

            m_Player = new Player(new Vector2(100, 100), 0, 1f, null, new Rectangle(0, 0, 0, 0), m_Graphics, 10, m_Camera);
            m_LastAsteroidCreation = m_Player.GetPosition();

            m_EnemySpawner = new EnemySpawner(ref m_Enemies, 2000, m_Graphics, m_Player);
            m_Spawner = new Spawner(m_Player, 5000, m_Graphics);
            m_Background = new Background(new Vector2(0, 0), m_Player);

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

            m_Spawner.SetHealthTexture(Content.Load<Texture2D>("Images/health"));
            m_Spawner.SetAmmoTexture(Content.Load<Texture2D>("Images/Ammo"));
            m_Asteroid = Content.Load<Texture2D>("Images/Asteroid");
            m_Explosion = Content.Load<Texture2D>("Images/explosion");

            m_Background.SetTexture(Content.Load<Texture2D>("Images/background"));
            m_Background.Start();

            //Audio
            m_ExplosionSound = Content.Load<SoundEffect>("Audio/explosionSound");
            m_Player.SetShootSound(Content.Load<SoundEffect>("Audio/shoot"));
            m_EnemySpawner.SetShootSound(Content.Load<SoundEffect>("Audio/shoot"));

            //Fonts
            m_Font = Content.Load<SpriteFont>("Fonts/Roboto");

            m_Player.SetRectangle(new Rectangle((int)m_Player.GetPosition().X,
                                                (int)m_Player.GetPosition().Y,
                                                m_Player.GetTexture().Width,
                                                m_Player.GetTexture().Height));
            m_Player.LoadTextureData();

            CreateAsteroids();
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

            //Update enemy spawner
            m_EnemySpawner.Update();

            if (m_GameState == EGameState.eGS_Paused || m_GameState == EGameState.eGS_GameOver || m_GameState == EGameState.eGS_Menu)
            {
                return;
            }
            else if (m_GameState == EGameState.eGS_Playing)
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
            GraphicsDevice.Clear(Color.Black);

            m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, m_Camera.GetTransform());

            //Draw the menu if it's in the menu
            if (m_GameState == EGameState.eGS_Menu)
            {

            }

            m_Background.Draw(ref m_SpriteBatch);

            Vector2 middlePos = TransformVector(new Vector2(GraphicsDevice.DisplayMode.Width / 2 - 50, GraphicsDevice.DisplayMode.Height / 2));
            if (m_GameState == EGameState.eGS_Paused)
            {
                m_SpriteBatch.DrawString(m_Font, "PAUSED", middlePos, Color.White);
                return;
            }
            else if (m_GameState == EGameState.eGS_GameOver)
            {
                m_SpriteBatch.DrawString(m_Font, "GAME OVER", middlePos, Color.White);
                return;
            }

            if (m_Enemies.Count > 0)
            {
                for (int i = 0; i < m_Enemies.Count; i++)
                {
                    m_Enemies[i].Draw(ref m_SpriteBatch);
                }
            }

            //Draw pickups
            for (int i = 0; i < m_Spawner.GetHealthPickups().Count; i++)
            {
                m_Spawner.GetHealthPickups()[i].Draw(ref m_SpriteBatch);
            }

            //Draw asteroids
            for (int i = 0; i < m_Asteroids.Count; i++)
            {
                m_Asteroids[i].Draw(ref m_SpriteBatch);
            }

            //Draw explosions
            if (m_Explosions.Count > 0)
            {
                for (int i = 0; i < m_Explosions.Count; i++)
                {
                    m_Explosions[i].Draw(m_SpriteBatch);
                }
            }

            //Draw UI
            m_SpriteBatch.DrawString(m_Font, "FPS: " + 1 / (float)gameTime.ElapsedGameTime.TotalSeconds, TransformVector(new Vector2(100, 100)), Color.White);
            m_SpriteBatch.DrawString(m_Font, "Score: " + m_Player.GetScore(), TransformVector(new Vector2(GraphicsDevice.DisplayMode.Width - 200, 100)), Color.White);
            m_SpriteBatch.DrawString(m_Font, "Level: " + m_EnemySpawner.GetLevel(), TransformVector(new Vector2(GraphicsDevice.DisplayMode.Width - 200, 150)), Color.White);
            m_SpriteBatch.DrawString(m_Font, "Ammo: " + m_Player.GetCurrentAmmo(), TransformVector(new Vector2(100, GraphicsDevice.DisplayMode.Height - 100)), Color.White);

            //Draw the player
            m_Player.Draw(ref m_SpriteBatch);

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
                            m_Enemies[i].GetBullets()[j].Update(gameTime);
                        }
                    }
                }
            }

            //Update the bullets
            if (m_Player.GetBullets().Count > 0)
            {
                for (int i = 0; i < m_Player.GetBullets().Count; i++)
                {
                    if (Vector2.Distance(m_Player.GetPosition(), m_Player.GetBullets()[i].GetPosition()) > 1500)
                    {
                        m_Player.GetBullets().RemoveAt(i);
                        if (i > 0)
                        {
                            i--;
                        }
                    }
                    else
                    {
                        m_Player.GetBullets()[i].Update(gameTime);
                    }
                }
            }

            //Update asteroids
            if (m_Asteroids.Count > 0)
            {
                for (int i = 0; i < m_Asteroids.Count; i++)
                {
                    m_Asteroids[i].Update(gameTime);
                }
            }

            //Update explosions
            if (m_Explosions.Count > 0)
            {
                for (int i = 0; i < m_Explosions.Count; i++)
                {
                    m_Explosions[i].Update(gameTime);
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
                            m_Explosions.Add(new Explosion(m_Explosion, m_Enemies[j].GetPosition()));
                            m_ExplosionSound.Play();
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

                for (int j = 0; j < m_Asteroids.Count; j++)
                {
                    if (i < 0)
                    {
                        i = 0;
                    }
                    if (m_Asteroids.Count == 0 || m_Player.GetBullets().Count == 0)
                    {
                        break;
                    }
                    if (IntersectsPixel(m_Player.GetBullets()[i].GetRectangle(), m_Player.GetBullets()[i].GetTextureData(), m_Asteroids[j].GetRectangle(), m_Asteroids[j].GetTextureData()))
                    {
                        m_Player.GetBullets().RemoveAt(i);

                        m_Explosions.Add(new Explosion(m_Explosion, m_Asteroids[j].GetPosition()));
                        m_ExplosionSound.Play();
                        m_Asteroids.RemoveAt(j);

                        if (m_Player.GetBullets().Count < 1 || m_Asteroids.Count < 1)
                        {
                            break;
                        }
                        else
                        {
                            i--;
                            j--;
                        }
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

            //Check for asteroid collision
            for (int i = 0; i < m_Asteroids.Count; i++)
            {
                if (IntersectsPixel(m_Player.GetRectangle(), m_Player.GetTextureData(), m_Asteroids[i].GetRectangle(), m_Asteroids[i].GetTextureData()))
                {
                    m_Player.SetHealth(m_Player.GetHealth() - 1f);
                    if (m_Player.GetHealth() <= 0)
                    {
                        m_GameState = EGameState.eGS_GameOver;
                    }
                }
            }

            //Check for health collision
            for (int i = 0; i < m_Spawner.GetHealthPickups().Count; i++)
            {
                if (IntersectsPixel(m_Player.GetRectangle(), m_Player.GetTextureData(), m_Spawner.GetHealthPickups()[i].GetRectangle(), m_Spawner.GetHealthPickups()[i].GetTextureData()))
                {
                    m_Spawner.GetHealthPickups()[i].Use(m_Player);
                    m_Spawner.GetHealthPickups().RemoveAt(i);
                    i--;
                }
            }

            //Check for ammo collision
            for (int i = 0; i < m_Spawner.GetAmmoPickups().Count; i++)
            {
                if (IntersectsPixel(m_Player.GetRectangle(), m_Player.GetTextureData(), m_Spawner.GetAmmoPickups()[i].GetRectangle(), m_Spawner.GetAmmoPickups()[i].GetTextureData()))
                {
                    m_Spawner.GetAmmoPickups()[i].Use(m_Player);
                    m_Spawner.GetAmmoPickups().RemoveAt(i);
                    i--;
                }
            }

            //Create new asteroids
            if (Vector2.Distance(m_Player.GetPosition(), m_LastAsteroidCreation) > 4000)
            {
                m_Asteroids.Clear();
                CreateAsteroids();
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

        public Vector2 TransformVector(Vector2 vector)
        {
            return Vector2.Transform(vector, Matrix.Invert(m_Camera.GetTransform()));
        }

        private void CreateAsteroids()
        {
            m_LastAsteroidCreation = m_Player.GetPosition();

            for (int i = 0; i < 200; i++)
            {
                Vector2 pos = new Vector2(m_Player.GetPosition().X + m_Random.Next((int)m_Player.GetPosition().X - 4000, (int)m_Player.GetPosition().X + 4000), m_Player.GetPosition().Y + m_Random.Next((int)m_Player.GetPosition().Y - 4000, (int)m_Player.GetPosition().Y + 4000));

                m_Asteroids.Add(new Asteroid(pos, 0, 1, m_Asteroid, new Rectangle((int)pos.X, (int)pos.Y, m_Asteroid.Width, m_Asteroid.Height), m_Graphics));
                m_Asteroids[m_Asteroids.Count - 1].LoadTextureData();
            }
        }
    }
}
