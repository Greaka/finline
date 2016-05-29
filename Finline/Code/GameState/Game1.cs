using System.Collections;
using Finline;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finline.Code.GameState

{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MainMenu main = new MainMenu();
        private EGameState currentGameState;
        private EGameState nextGameState;
        private IGameState GameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

  
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 1024;
            nextGameState = EGameState.MainMenu;
            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            main.LoadContent(Content);
           
            
        }

       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (nextGameState != currentGameState)
                HandleGameState();
            GameState.Update(gameTime);
            
           
            base.Update(gameTime);
        }

      
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            GameState.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void HandleGameState()
        {
            switch (nextGameState)
            {
                case EGameState.MainMenu:
                    GameState = new MainMenu();
                    GameState.initialize(Content);
                    break;

            }

            currentGameState = nextGameState;

        }


        
    }


}
