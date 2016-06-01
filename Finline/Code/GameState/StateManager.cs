using Finline.Code.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finline.Code.GameState

{
    public class StateManager : Microsoft.Xna.Framework.Game
    {
        private DrawableGameComponent _gameState;
        private EGameState _currentGameState;
        private MainMenu _main;
        private EGameState _nextGameState;

        private SpriteBatch _spriteBatch;

        public StateManager()
        {
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        public GraphicsDeviceManager Graphics { get; private set; }

        protected override void Initialize()
        {
            _nextGameState = EGameState.MainMenu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Content.RootDirectory = "Content";
            _main = new MainMenu(this, _spriteBatch);
            _main.GoIngame += StartNewGame;
            _main.Initialize();
        }

        /// <summary>
        ///     The unload content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (_nextGameState != _currentGameState)
                HandleGameState();
            _gameState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _gameState.Draw(gameTime);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            base.Draw(gameTime);
        }

        private void HandleGameState()
        {
            switch (_nextGameState)
            {
                case EGameState.MainMenu:
                    _gameState = _main;
                    break;

                case EGameState.InGame:
                    _gameState = new Ingame(this);
                    break;
            }

            _gameState.Initialize();

            _currentGameState = _nextGameState;
        }

        private void StartNewGame()
        {
            _nextGameState = EGameState.InGame;
        }
    }
}