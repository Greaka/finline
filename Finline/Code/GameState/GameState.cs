namespace Finline.Code.GameState
{
    using Microsoft.Xna.Framework;

    using Game = Microsoft.Xna.Framework.Game;

    enum EGameState 
    {
        None,
        MainMenu,
        InGame
    }

/*
    public abstract class GameState : DrawableGameComponent
    {
        public virtual void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public virtual void Initialize()
        {
            // base.Initialize();
        }

        public GameState(Game game)
            : base(game)
        {
        }
    }
    */
}
