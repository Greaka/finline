namespace Finline.Code.GameState
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    internal class MainMenu: DrawableGameComponent
    {
        /// <summary>
        /// MenuState to manage all States
        /// </summary>
        private enum EMenuState
        {
            None, 

            TitleScreen, 

            MainMenu, 

            Option
        }

        public static bool isPressed = false;
        private EMenuState menuState;


        // The Lists with all the elements
        private readonly List<GUIElement> title = new List<GUIElement>();
        private readonly List<GUIElement> main = new List<GUIElement>();
        private readonly List<GUIElement> option = new List<GUIElement>();

        private readonly SpriteBatch spriteBatch;
       

        /// <summary>
        /// Constructor to use the GUIElementlist to select the element
        /// </summary>
        public MainMenu(Game game, SpriteBatch sprite)
            : base(game)
        {
            this.spriteBatch = sprite;
            this.menuState = EMenuState.TitleScreen;

            this.title.Add(new GUIElement("Logo 2")); //Logo in the state Titlescreen
            

            //here are the elements in the state MainMenu
           
            this.main.Add(new GUIElement("MenuFrame"));
            this.main.Add(new GUIElement("NewGame"));
            this.main.Add(new GUIElement("Option"));
            this.main.Add(new GUIElement("End"));

            //here are the elements in the state Option
            this.option.Add(new GUIElement("Back_to_MainMenu"));
           
        }

        protected override void LoadContent()
        {
            foreach (var element in this.title)
            {
                element.LoadContent(this.Game.Content);
                element.CenterElement(600, 800);
                element.ClickEvent += this.OnClick;
            }
            this.title.Find(x => x.AssetName == "Logo 2").MoveElement(0,-50);
            


            foreach (var element in this.main)
            {
                element.LoadContent(this.Game.Content);
                element.CenterElement(600, 800);
                element.ClickEvent += this.OnClick;
            }
            this.main.Find(x =>x.AssetName =="MenuFrame").MoveElement(0, -50);  // frame Movement
            this.main.Find(x => x.AssetName == "NewGame").MoveElement(0, -200); // move the "newgame" button 100 up in y-direction
            this.main.Find(x => x.AssetName == "Option").MoveElement(0, -50);   // move the "option" button down in y-direction
            this.main.Find(x => x.AssetName == "End").MoveElement(0, 100);


            foreach (var element in this.option)
            {
                element.LoadContent(this.Game.Content);
                element.CenterElement(600, 800);
                element.ClickEvent += this.OnClick;

            }

            this.option.Find(x => x.AssetName == "Back_to_MainMenu").MoveElement(0, 50);
        }

        public override void Update(GameTime gameTime)
        {
            switch (this.menuState) {
                case EMenuState.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        this.menuState = EMenuState.MainMenu;
                    break;
                    

                case EMenuState.MainMenu:
                    foreach (var element in this.main)
                    {
                        element.Update();
                    }

                    break;
                   
                case EMenuState.Option:
                    foreach (var element in this.option)
                    {
                        element.Update();
                    }

                    break;
                case EMenuState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            
            
            switch (this.menuState)
            {
                case EMenuState.TitleScreen:
                    foreach (var element in this.title)
                    {
                        element.Draw(this.spriteBatch);
                    }
                   

                    break;
                case EMenuState.MainMenu:
                    foreach (var element in this.main)
                    {
                        element.Draw(this.spriteBatch);
                    }

                    break;
                case EMenuState.Option:
                    foreach (var element in this.option)
                    {
                        element.Draw(this.spriteBatch);
                    }

                    break;
                case EMenuState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.spriteBatch.End();
        }

        /// <summary>
        /// when click on the element change to next state
        /// </summary>
        /// <param name="element"></param>
        private void OnClick(string element)
        {
            if (isPressed) return;

            isPressed = true;
            if (element == "MenuFrame")
                isPressed = false;

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
                this.menuState = EMenuState.TitleScreen;
            }
            

            if (element == "Back_to_MainMenu")
            {
                this.menuState = EMenuState.MainMenu;
            }
        }

        public delegate void GetIngame();

        public event GetIngame GoIngame;
    }
}
