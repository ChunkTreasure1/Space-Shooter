using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using SpaceShooter.Gameplay.Player;
using SpaceShooter.Gameplay;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager m_Graphics;
        SpriteBatch m_SpriteBatch;
        Player m_Player;
        Texture2D m_BulletTexture;

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
            m_Graphics.PreferredBackBufferWidth = 1280;
            m_Graphics.PreferredBackBufferHeight = 720;
            m_Graphics.ApplyChanges();

            m_Player = new Player(new Rectangle(100, 100, 64, 64), 0f);

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

            //Add textures
            m_Player.SetTexture(Content.Load<Texture2D>("Images/spaceship"));
            m_BulletTexture = Content.Load<Texture2D>("Images/bullet");

            // TODO: use this.Content to load your game content here
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
            // TODO: Add your update logic here
            UpdatePosition(gameTime);
            GetInput(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            m_SpriteBatch.Begin();

            //Draw the player
            m_Player.Draw(ref m_SpriteBatch);

            m_SpriteBatch.End();
            base.Draw(gameTime);
        }

        //Gets the players input
        private void GetInput(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Vector2 dir = new Vector2((float)Math.Cos(m_Player.GetRotation()),
                                          (float)Math.Sin(m_Player.GetRotation()));
                dir.Normalize();

                m_Player.SetPosition(m_Player.GetPosition() + dir * 10);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Vector2 dir = new Vector2((float)Math.Cos(m_Player.GetRotation()),
                                         (float)Math.Sin(m_Player.GetRotation()));
                dir.Normalize();
                m_Player.SetPosition(m_Player.GetPosition() - dir * 10);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_Player.SetRotation(MathHelper.ToDegrees(m_Player.GetRotation()) + 10f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_Player.SetRotation(MathHelper.ToDegrees(m_Player.GetRotation()) - 10f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                m_Player.GetBullets().Add(new Bullet(new Rectangle(100, 100, 64, 64), 0f, m_BulletTexture));
            }
        }

        //Updates the position of bullets etc
        private void UpdatePosition(GameTime gameTime)
        {
            for (int i = 0; i < m_Player.GetBullets().Count; i++)
            {
                m_Player.GetBullets()[i].Move();
            }
        }
    }
}
