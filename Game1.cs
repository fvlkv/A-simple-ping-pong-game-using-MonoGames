using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Formats.Asn1.AsnWriter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Zadanie2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D ballAnim;
        private Texture2D fireAnim;
        private Texture2D paddle1;
        private Texture2D paddle2;
        private Texture2D pongBackground;
        private SpriteFont font;
        Vector2 ballSpeed;
        Rectangle paddle1Rect;
        Rectangle paddle2Rect;
        Rectangle ballRect;
        Rectangle[] animationRect2 = new Rectangle[25];
        Rectangle[] animationRect1 = new Rectangle[71];
        Rectangle top, topMid, mid, botMid, bot, top2, topMid2, mid2, botMid2, bot2;
        int animationFrames1 = 0;
        int animationFrames2 = 0;
        int timeSinceLastFrame = 0;
        int hittingTime = 0;
        const int milisecondsPerFrame = 16;
        bool isGamePlaying = false;
        bool isGameOver = false;
        bool hitted;
        bool hitted2;
        bool hitted3;
        int score1 = 0;
        int score2 = 0;
  
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1024;
            Window.AllowUserResizing = false;

        }

        protected override void Initialize()
        {
            ballSpeed = new Vector2(3.0f, 1.5f);
           

            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 16; j++)
                {
                    if (j + (16 * i) < 71)
                        animationRect1[j + (16 * i)] = new Rectangle(j * 64, i * 64, 64, 64);
                    if (j + (16 * i) <= 25)
                        animationRect2[j + (5 * i)] = new Rectangle(j * 64, i * 64, 64, 64);
                }
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballAnim = Content.Load<Texture2D>(@"ball-anim");
            fireAnim = Content.Load<Texture2D>(@"explosion64");
            paddle1 = Content.Load<Texture2D>(@"paddle1a");
            paddle2 = Content.Load<Texture2D>(@"paddle2a");
            pongBackground = Content.Load<Texture2D>(@"pongBackground");
            font = Content.Load<SpriteFont>(@"font");
            ballRect = new Rectangle((GraphicsDevice.Viewport.Width - 64) / 2, (GraphicsDevice.Viewport.Height - 64) / 2, 64, 64);
            paddle1Rect = new Rectangle(new Point(0, (GraphicsDevice.Viewport.Height - 150) / 2),new Point(20,150));
            paddle2Rect = new Rectangle(new Point(GraphicsDevice.Viewport.Width - 20, (GraphicsDevice.Viewport.Height - 150) / 2),new Point(20,150));
        
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                isGamePlaying = true;
            if (isGamePlaying==true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    if (paddle1Rect.Y > 0)
                        paddle1Rect.Y -= 7;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    if (paddle1Rect.Y < (GraphicsDevice.Viewport.Height - 150))
                        paddle1Rect.Y += 7;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    if (paddle2Rect.Y > 0)
                        paddle2Rect.Y -= 7;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    if (paddle2Rect.Y < (GraphicsDevice.Viewport.Height - 150))
                        paddle2Rect.Y += 7;

                }
                Ball();
             
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > milisecondsPerFrame)
                {
                    timeSinceLastFrame -= milisecondsPerFrame;
                    animationFrames1 = (animationFrames1 + 1) % 71;
                    if (isGameOver == true)
                    {
                        animationFrames2 = (animationFrames2 + 1) % 25;
                        if (animationFrames2 == 24)
                        {

                            if (hitted3 == false)
                            {
                                score1 += 1;

                            }
                            else
                            {
                                score2 += 1;
                            }
                            isGamePlaying = false;
                            isGameOver = false;
                            ballRect.Location = new Point((GraphicsDevice.Viewport.Width / 2) - 45, (GraphicsDevice.Viewport.Height / 2) - 45);
                            ballSpeed = new Vector2(2, 2);
                          
                           
                        }
                    }
                }
                if (hitted == true)
                {
                    hittingTime += timeSinceLastFrame;
                    if (hittingTime >= 300)
                    {
                        hittingTime = 0;
                        hitted = false;
                       

                    }
                }
                if (hitted2 == true)
                {
                    hittingTime += timeSinceLastFrame;
                    if (hittingTime >= 300)
                    {
                        hittingTime = 0;
                        hitted2 = false;
                        

                    }
                }

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(pongBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.Draw(paddle1, paddle1Rect, hitted ? Color.Green : Color.White);
            _spriteBatch.Draw(paddle2, paddle2Rect, hitted2 ? Color.Green : Color.White);
            _spriteBatch.Draw(isGameOver ? fireAnim : ballAnim, ballRect, isGameOver ? animationRect2[animationFrames2] : animationRect1[animationFrames1], Color.White);
            _spriteBatch.DrawString(font, "Score: " + score1.ToString(), new Vector2((GraphicsDevice.Viewport.Width / 2) - 100, 0), Color.White);
            _spriteBatch.DrawString(font, "Score: " + score2.ToString(), new Vector2((GraphicsDevice.Viewport.Width/2)+30,0), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        void Ball()
        {
            
            
            ballRect.X += (int)ballSpeed.X;
            ballRect.Y+=(int)ballSpeed.Y;

            if (ballRect.Y <= 0 || (ballRect.Y >= (GraphicsDevice.Viewport.Height - 64)))
            {
                ballSpeed.Y *= -1;
            }
            if (ballRect.Intersects(paddle1Rect))
            {
                if (ballRect.Intersects(new Rectangle(paddle1Rect.Location,new Point(paddle1Rect.Width,paddle1Rect.Height))))
                {
                  
                    ballSpeed.X *= -1.3f;
                    ballSpeed.Y *= 1.1f;
                    hitted3 = false;

                }
                else if (ballRect.Intersects(new Rectangle(new Point(paddle1Rect.Left,paddle1Rect.Top-30), new Point(paddle1Rect.Width, paddle1Rect.Height))))
                {
                 
                    ballSpeed.X *= -1.2f;
                    ballSpeed.Y *= 1.2f;
                    hitted3 = false;

                }
                else if (ballRect.Intersects(new Rectangle(new Point(paddle1Rect.Left, paddle1Rect.Top - 60), new Point(paddle1Rect.Width, paddle1Rect.Height))))
                {
                   
                    ballSpeed.X *= -1.5f;
                    ballSpeed.Y *= 0;
                    hitted3 = false;

                }
                else if (ballRect.Intersects(new Rectangle(new Point(paddle1Rect.Left, paddle1Rect.Top - 90), new Point(paddle1Rect.Width, paddle1Rect.Height))))
                {
                   
                    ballSpeed.X *= -1.3f;
                    ballSpeed.Y /= 1.1f;
                    hitted3 = false;

                }
                else if (ballRect.Intersects(new Rectangle(new Point(paddle1Rect.Left, paddle1Rect.Top - 120), new Point(paddle1Rect.Width, paddle1Rect.Height))))
                {
                   
                    ballSpeed.X *= -1.2f;
                    ballSpeed.Y /= 1.2f;
                    hitted3 = false;

                }
                hitted = true;
                //score1 += 1;
            }
            if (ballRect.Intersects(paddle2Rect))
            {

                if (ballRect.Intersects(new Rectangle(paddle2Rect.Location, new Point(paddle2Rect.Width, paddle2Rect.Height))))
                {

                    ballSpeed.X *= -1.3f;
                    ballSpeed.Y *= 1.7f;
                    hitted3 = true;

                }
                else if (ballRect.Intersects(new Rectangle(new Point(paddle2Rect.Left, paddle2Rect.Top - 30), new Point(paddle2Rect.Width, paddle2Rect.Height))))
                {

                    ballSpeed.X *= -1.2f;
                    ballSpeed.Y *= 1.5f;
                    hitted3 = true;

                }
                else if (ballRect.Intersects(new Rectangle(new Point(paddle2Rect.Left, paddle2Rect.Top - 60), new Point(paddle2Rect.Width, paddle2Rect.Height))))
                {

                    ballSpeed.X *= -1.7f;
                    ballSpeed.Y *= 0;
                    hitted3 = true;

                }
                else if (ballRect.Intersects(new Rectangle(new Point(paddle2Rect.Left, paddle2Rect.Top - 90), new Point(paddle2Rect.Width, paddle2Rect.Height))))
                {

                    ballSpeed.X *= -1.3f;
                    ballSpeed.Y /= 2.5f;
                    hitted3 = true;

                }
                else if (ballRect.Intersects(new Rectangle(new Point(paddle2Rect.Left, paddle2Rect.Top - 120), new Point(paddle2Rect.Width, paddle2Rect.Height))))
                {

                    ballSpeed.X *= -1.2f;
                    ballSpeed.Y /= 2.7f;
                    hitted3 = true;

                }
               
                hitted2 = true;
                //score2 += 1;
               
            }
            if (ballRect.X <= 0||(ballRect.X >= GraphicsDevice.Viewport.Width - 64))
            {
                isGameOver = true;
                ballSpeed =new Vector2(0, 0);

            }
        }
    
    }
}