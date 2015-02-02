#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Threading;
using Newtonsoft.Json;
using GameName1.Objects;
using System.Diagnostics;
#endregion

namespace GameName1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Keystates
        private MouseState mouseState;
        private Point mousePos;
        private KeyboardState keyState;
        //textures
        private Texture2D playerTexture;
        private Texture2D enemyTexture;
        private Texture2D bulletTexture;

        Player player = new Player();
        OtherPlayer Enemy = new OtherPlayer();
        List<OtherPlayer> enemyList = new List<OtherPlayer>();
        List<Bullet> bulletList = new List<Bullet>();

        Thread playerPos;
        Thread enemyPos;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

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
            base.Initialize();

            player.GetName();
            if (ContactServer.ping() == true)
            {
                playerPos = new Thread(new ThreadStart(updatePlayerPos));
                playerPos.Start();
                enemyPos = new Thread(new ThreadStart(getOtherPlayerPos));
                enemyPos.Start();
            }
            //thread
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTexture = Content.Load<Texture2D>("blue.png");
            enemyTexture = Content.Load<Texture2D>("blue.png");
            bulletTexture = Content.Load<Texture2D>("blue.png");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                playerPos.Abort();
                enemyPos.Abort();
                ContactServer.disconnect(player.name);
                Exit();
            }

            mouseState = Mouse.GetState();
            mousePos = new Point(mouseState.X, mouseState.Y);
            keyState = Keyboard.GetState();
            // TODO: Add your update logic here

            string direction = null;
            if (keyState.IsKeyDown(Keys.W))
            {
                player.position.Y -= player.speed;
                direction = "up";
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                player.position.Y += player.speed;
                direction = "down";
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                player.position.X -= player.speed;
                direction = "left";
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                player.position.X += player.speed;
                direction = "right";
            }
            if (keyState.IsKeyDown(Keys.Space))
            {
                Bullet bullet = new Bullet(player.name, player.position, player.position, 10, direction, bulletTexture);
                ContactServer.sendBulletData(bullet);
                bulletList.Add(bullet);
            }

            foreach (var b in bulletList)
            {
                switch (b.direction)
                {
                    case "up":
                        {
                            b.bulletPosition.Y -= b.speed;
                            break;
                        }
                    case "down":
                        {
                            b.bulletPosition.Y += b.speed;
                            break;
                        }
                    case "left":
                        {
                            b.bulletPosition.X -= b.speed;
                            break;
                        }
                    case "right":
                        {
                            b.bulletPosition.X += b.speed;
                            break;
                        }

                }
            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            base.Draw(gameTime);


            //player
            spriteBatch.Draw(playerTexture, new Rectangle(player.position.X, player.position.Y, 32, 32), Color.White);

            foreach (var e in enemyList)
            {
                if (Enemy.position != null && e.name != player.name)
                {
                    spriteBatch.Draw(playerTexture, new Rectangle(e.position.X, e.position.Y, 32, 32), Color.Black);
                }
            }

            //bullets
            foreach (var b in bulletList)
            {
                //draw bullet
                spriteBatch.Draw(bulletTexture, new Rectangle(b.bulletPosition.X, b.bulletPosition.Y, 4, 4), Color.Black);
            }


            spriteBatch.End();
        }
        //threads

        //updating player pos to server
        private void updatePlayerPos()
        {
            while (true)
            {
                ContactServer.sendData(player);
                Thread.Sleep(1);
            }
        }




        //other players
        private void getOtherPlayerPos()
        {
            while (true)
            {
                string data = null;
                data = ContactServer.Receive();
                if (data != null)
                {
                    if(data.Contains("origin") && !data.Contains(player.name))
                    {
                        bulletList.Add(JsonConvert.DeserializeObject<Bullet>(data));
                    }
                    else
                    {
                        try
                        {
                            enemyList = JsonConvert.DeserializeObject<List<OtherPlayer>>(data);
                        }
                        catch (Exception)
                        {

                        }
                        foreach (var enemy in enemyList)
                        {
                            if (enemy.name != player.name)
                            {
                                //Debug.WriteLine(enemy.position.X + "," + enemy.position.Y);
                                Point p = player.position;
                                try
                                {
                                    Enemy.name = enemy.name;
                                    Enemy.position = enemy.position;
                                    Enemy.speed = enemy.speed;
                                }
                                catch (Exception)
                                {

                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
