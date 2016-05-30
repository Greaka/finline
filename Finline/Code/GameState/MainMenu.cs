namespace Finline.Code.GameState
{
    using System.Collections.Generic;

    using Finline.Code.Game;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    class MainMenu: DrawableGameComponent
    {
        //Gamestate to manage all the states
        private enum EMenuState
        {
            None,

            TitleScreen,

            MainMenu,

            Option
        }

        private EMenuState menuState;


        //The Lists with all the elements
        List<GUIElement> title = new List<GUIElement>();
        List<GUIElement> main = new List<GUIElement>(); 
        List<GUIElement> option = new List<GUIElement>();
        private GUIElement.ElementClicked Onclick;

        private SpriteBatch spriteBatch;

        /// <summary>
        /// Constructor to use the GUIElementlist to select the element
        /// </summary>
        public MainMenu(Game game, SpriteBatch sprite)
            : base(game)
        {
            this.spriteBatch = sprite;
            this.menuState = EMenuState.TitleScreen;
            this.title.Add(new GUIElement("Logo"));

            this.main.Add(new GUIElement("MenuFrame"));
            this.main.Add(new GUIElement("NewGame"));
            this.main.Add(new GUIElement("Option"));

            this.option.Add(new GUIElement(("End")));
           
        }

        protected override void LoadContent()
        {
            foreach (GUIElement element in this.title)
            {
                element.LoadContent(this.Game.Content);
                element.CenterElement(600,800);
                element.clickEvent += this.OnClick;
            }

            foreach (GUIElement element in this.main)
            {
                element.LoadContent(this.Game.Content);
                element.CenterElement(600,800);
                element.clickEvent += this.OnClick;
            }

            this.main.Find(x => x.AssetName == "NewGame").MoveElement(0, -100); //move the "newgame" button 100 up in y-direction

            foreach (GUIElement element in this.option)
            {
                element.LoadContent(this.Game.Content);
                element.CenterElement(600, 800);
                element.clickEvent += this.OnClick;

            }
            this.main.Find(x => x.AssetName == "Option").MoveElement(0, 100); //move the "option" button down in y-direction


            foreach (GUIElement element in this.option)
            {
                element.LoadContent(this.Game.Content);
                element.CenterElement(600, 800);
                element.clickEvent += this.OnClick;

            }
            this.option.Find(x => x.AssetName == "End").MoveElement(0, 50);
        }

        public override void Update(GameTime gameTime)
        {
            switch (this.menuState) {
                case EMenuState.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        this.menuState = EMenuState.MainMenu;
                    break;
                    

                case EMenuState.MainMenu:
                    foreach (GUIElement element in this.main)
                    {
                        element.Update();
                    }
                    break;
                   
                case EMenuState.Option:
                    foreach (GUIElement element in this.option)
                    {
                        element.Update();
                    }
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            switch (this.menuState)
            {
                case EMenuState.TitleScreen:
                    foreach (GUIElement element in this.title)
                    {
                        element.Draw(this.spriteBatch);
                    }
                    break;
               case EMenuState.MainMenu:
                    foreach (GUIElement element in this.main)
                    {
                        element.Draw(this.spriteBatch);
                    }
                    break;
                case EMenuState.Option:
                    foreach (GUIElement element in this.option)
                    {
                        element.Draw(this.spriteBatch);
                    }
                    break;
            }
            


        }


        /// <summary>
        /// when click on the element change to next state
        /// </summary>
        /// <param name="element"></param>
        public void OnClick(string element)
        {
            if (this.menuState == EMenuState.TitleScreen)
            {
                this.menuState = EMenuState.MainMenu;
            }

            if (element == "NewGame")
            {
                this.menuState = EMenuState.None;
                this.GoIngame?.Invoke();
            }

            if (element == "Option")
            {
                this.menuState = EMenuState.Option;
            }

            if (element == "End")
            {
                this.menuState = EMenuState.MainMenu;
            }
        }

        public delegate void GetIngame();

        public event GetIngame GoIngame;
    }
}
