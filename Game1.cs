using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Conway
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random rand = new Random();

        private Texture2D Texture;

        private bool[,] state;
        private bool[,] next_state;

        const int WIDTH = 200;
        const int HEIGHT = 200;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture = new Texture2D(GraphicsDevice, WIDTH, HEIGHT);

            state = new bool[HEIGHT, WIDTH];
            next_state = new bool[HEIGHT, WIDTH];
        }

        bool r_pressed;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // UPDATE LOGIC
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.R) && r_pressed == false)
            {
                // runs once when r is pressed

                RandomizeCells();
                SetTexture();

                r_pressed = true;
            }
            else if (keyState.IsKeyUp(Keys.R) && r_pressed == true) r_pressed = false;

            if (keyState.IsKeyDown(Keys.Enter))
            {
                GetNextState();
                SetTexture();
            }

            base.Update(gameTime);
        }

        int rand_num;
        private void RandomizeCells()
        {
            for (int r = 0; r < HEIGHT; r++)
            {
                // loop rows
                for (int c = 0; c < WIDTH; c++)
                {
                    rand_num = rand.Next(2);

                    if (rand_num == 0)
                    {
                        state[r, c] = true;
                        next_state[r, c] = true;
                    }
                    else
                    {
                        state[r, c] = false;
                        next_state[r, c] = false;
                    }

                    // loop columns
                }
            }
        }

        int iter = 0;
        Color[] colors = new Color[WIDTH * HEIGHT];
        private void SetTexture()
        {
            iter = 0;

            for (int r = 0; r < HEIGHT; r++)
            {
                for (int c = 0; c < WIDTH; c++)
                {
                    if (state[r, c] == true) colors[iter] = Color.DarkOliveGreen;
                    else colors[iter] = Color.AntiqueWhite;

                    iter++;
                }
            }

            Texture.SetData(colors);
        }

        private void GetNextState()
        {
            for (int r = 0; r < HEIGHT; r++)
            {
                for (int c = 0; c < WIDTH; c++)
                {
                    int neighbors = 0;

                    if (r < HEIGHT - 1 && state[r + 1, c] == true) neighbors++;
                    if (r > 0 && state[r - 1, c] == true) neighbors++;
                    if (c < WIDTH - 1 && state[r, c + 1] == true) neighbors++;
                    if (c > 0 && state[r, c - 1] == true) neighbors++;

                    if (r < HEIGHT - 1 && c < WIDTH - 1 && state[r + 1, c + 1] == true) neighbors++;
                    if (r > 0 && c > 0 && state[r - 1, c - 1] == true) neighbors++;
                    if (r < HEIGHT - 1 && c > 0 && state[r + 1, c - 1] == true) neighbors++;
                    if (r > 0 && c < WIDTH - 1 && state[r - 1, c + 1] == true) neighbors++;

                    // RULE: alive: stays alive if neighbor >< 2-3;
                    // RULE: dead: becomes alive if neighbors == 3;

                    if (state[r, c] == true)
                    {
                        if (neighbors > 3 || neighbors < 2) next_state[r, c] = false;
                    }
                    else
                    {
                        if (neighbors == 3) next_state[r, c] = true;
                    }

                }
            }

            state = (bool[,])next_state.Clone();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            // PLACE TO DRAW STUFF

            _spriteBatch.Draw(Texture, new Rectangle(0, 0, WIDTH * 3, HEIGHT * 3), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}