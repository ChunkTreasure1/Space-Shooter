﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System;

namespace SpaceShooter.Gameplay.Player
{
    public class Player : Entity
    {
        private float m_MaxSpeed;
        private float m_CurrentSpeed;
        private bool m_ShootPressed = false;
        private Camera2D m_Camera;

        private Texture2D m_BulletTexture;

        private List<Bullet> m_Bullets = new List<Bullet>();

        public List<Bullet> GetBullets() { return m_Bullets; }

        public Player(Vector2 pos, float rotation, float scale, Texture2D texture, Rectangle rect, GraphicsDeviceManager graphics, float maxSpeed, Camera2D camera) :
            base(pos, rotation, scale, texture, rect, graphics)
        {
            m_Position.X = pos.X;
            m_Position.Y = pos.Y;

            m_Scale = scale;
            m_Rotation = rotation;
            m_Texture = texture;

            m_Rectangle = rect;
            m_Graphics = graphics;
            m_MaxSpeed = maxSpeed;

            m_Camera = camera;
        }
        public override void SetPosition(Vector2 pos)
        {
            if (pos.X < m_Graphics.PreferredBackBufferWidth && pos.X > 0 &&
                pos.Y < m_Graphics.PreferredBackBufferHeight && pos.Y > 0)
            {
                m_Position = pos;
                m_Rectangle.X = (int)pos.X;
                m_Rectangle.Y = (int)pos.Y;
            }
        }
        public void SetBulletTexture(Texture2D texture) { m_BulletTexture = texture; }
        public float GetMaxSpeed() { return m_MaxSpeed; }

        //Overrides the drawing of the player
        public override void Draw(ref SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
            Vector2 origin = new Vector2(m_Texture.Width / 2, m_Texture.Height / 2);

            spriteBatch.Draw(m_Texture, m_Position, rect, Color.White, m_Rotation + MathHelper.ToRadians(90), origin, m_Scale, SpriteEffects.None, 1);

            if (m_Bullets.Count > 0)
            {
                for (int i = 0; i < m_Bullets.Count; i++)
                {
                    m_Bullets[i].Draw(ref spriteBatch);
                }
            }
        }
        public override void Move(float speed, float mul)
        {
            Vector2 dir = new Vector2((float)Math.Cos(GetRotation()),
                          (float)Math.Sin(GetRotation()));
            dir.Normalize();

            if (m_CurrentSpeed <= m_MaxSpeed)
            {
                m_CurrentSpeed += speed;
            }
            else if (m_CurrentSpeed > m_MaxSpeed)
            {
                m_CurrentSpeed -= speed;
            }

            SetPosition(GetPosition() + dir * m_CurrentSpeed * mul);
            Vector2 pos = GetPosition();

            if (m_Camera.WorldToScreenCoords(ref pos).Y > (m_Graphics.PreferredBackBufferHeight / 1.33) || m_Camera.WorldToScreenCoords(ref pos).Y < (m_Graphics.PreferredBackBufferHeight / 2.33)
                || m_Camera.WorldToScreenCoords(ref pos).X > (m_Graphics.PreferredBackBufferWidth / 1.33) || m_Camera.WorldToScreenCoords(ref pos).X < (m_Graphics.PreferredBackBufferWidth / 2.33)) 
            {
                //m_Camera.Move(dir * m_CurrentSpeed);
            }
            base.Move(speed, mul);
        }
        public override void Update(GameTime gameTime)
        {
            GetInput();
            base.Update(gameTime);
        }
        private void GetInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                m_MaxSpeed = 10;
                Move(0.01f, 1);
            }
            if (Keyboard.GetState().IsKeyUp(Keys.W))
            {
                m_MaxSpeed = 0;
                Move(0.01f, 1);
            }
            //if (Keyboard.GetState().IsKeyDown(Keys.S))
            //{
            //    Move(-0.1f, 1);
            //}
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                SetRotation(MathHelper.ToDegrees(GetRotation()) + 3f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                SetRotation(MathHelper.ToDegrees(GetRotation()) - 3f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !m_ShootPressed)
            {
                m_ShootPressed = true;
                GetBullets().Add(new Bullet(new Vector2(GetPosition().X,
                                                                 GetPosition().Y),
                                                     GetRotation(),
                                                     0.5f,
                                                     m_BulletTexture,
                                                     10f,
                                                     new Rectangle((int)GetPosition().X,
                                                                   (int)GetPosition().Y,
                                                                   m_BulletTexture.Width,
                                                                   m_BulletTexture.Height),
                                                     m_Graphics));

                GetBullets()[GetBullets().Count - 1].LoadTextureData();
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space) && m_ShootPressed)
            {
                m_ShootPressed = false;
            }
        }
    }
}
