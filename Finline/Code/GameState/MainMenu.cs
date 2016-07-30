using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finline.Code.GameState
{
    internal class MainMenu : DrawableGameComponent
    {
        public delegate void GetIngame();

        private bool isPressed;

        private readonly Dictionary<EMenuState, List<GuiElement>> guiElements = new Dictionary<EMenuState, List<GuiElement>>();

        private readonly SpriteBatch spriteBatch;

        private EMenuState menuState;
        private SpriteFont font;

        private bool paused = false;
       

        /// <summary>
        /// Constructor to use the GUIElementlist to select the element
        /// </summary>
        public MainMenu(StateManager game, SpriteBatch sprite)
            : base(game)
        {
            // The Lists with all the elements
            this.guiElements.Add(EMenuState.TitleScreen, new List<GuiElement>());
            this.guiElements.Add(EMenuState.MainMenu, new List<GuiElement>());
            this.guiElements.Add(EMenuState.CharacterScreen, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Option, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Credits, new List<GuiElement>());

            this.spriteBatch = sprite;
            this.menuState = EMenuState.TitleScreen;

            this.guiElements[EMenuState.TitleScreen].Add(new GuiElement("Logo 2")); // Logo in the state Titlescreen
            
            // here are the elements in the state MainMenu
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("MenuFrame"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("NewGame2"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("Option2"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("Credits2"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("End2"));
            
            // elements in the state characterScreen
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("StartGame2"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("Back_to_MainMenu2"));

            // here are the elements in the state Option
            this.guiElements[EMenuState.Option].Add(new GuiElement("Back_to_MainMenu2"));

            // here are the elements in the state Credits
            this.guiElements[EMenuState.Credits].Add(new GuiElement("Back_to_MainMenu2"));
        }

        protected override void LoadContent()
        {
            this.font = this.Game.Content.Load<SpriteFont>("font");
            foreach (var elementList in this.guiElements.Values)
            {
                foreach (var element in elementList)
                {
                    element.LoadContent(this.Game.Content);
                    element.CenterElement(600, 800);
                    element.ClickEvent += this.OnClick;
                }
            }
            
            this.guiElements[EMenuState.TitleScreen].Find(x => x.AssetName == "Logo 2").MoveElement(0, -50); // move the logo up in y-direction
            
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "MenuFrame").MoveElement(0, -50); // frame Movement
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "NewGame2").MoveElement(0, -200); // move the "newgame" button up in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "Option2").MoveElement(0, -50); // move the "option" button down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "Credits2").MoveElement(220 , 50); // move the "credits" button 200 in x-direction and 50 down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "End2").MoveElement(0, 100); // move the "end" button down in y-direction
            
            this.guiElements[EMenuState.CharacterScreen].Find(x=> x.AssetName =="StartGame2").MoveElement(0, -30); // move the "StartGame" button up in y-direction
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "Back_to_MainMenu2").MoveElement(0, 100); // move the "Back_to_MainMenu" button down in y-direction
            
            this.guiElements[EMenuState.Option].Find(x => x.AssetName == "Back_to_MainMenu2").MoveElement(0, 50); // move the "Back_to_MainMenu" button down in y-direction
            
            this.guiElements[EMenuState.Credits].Find(x => x.AssetName == "Back_to_MainMenu2").MoveElement(0, 50); // move the "Back_to_MainMenu" button down in y-direction

            
        }

        public override void Update(GameTime gameTime)
        {
            if (this.menuState == EMenuState.TitleScreen)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    this.menuState = EMenuState.MainMenu;
            }

            
            
            foreach (var element in this.guiElements[this.menuState])
            {
                element.Update(ref this.isPressed);
            }


          
        }


        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
         
            
            if (this.menuState == EMenuState.Credits)
            {
                this.spriteBatch.DrawString(this.font, 
                    "Minh Vuong Pham\n" + "Michl Steglich\n" + "Tim Stadelmann\n" + "Tino Nagelmueller\n", 
                    new Vector2(300, 100), Color.Black);
            }

            if (this.menuState != EMenuState.None)
            foreach (var element in this.guiElements[this.menuState])
            {
                element.Draw(this.spriteBatch);
            }

           

            this.spriteBatch.End();
        }

        /// <summary>
        /// when click on the element change to next state
        /// </summary>
        /// <param name="element"></param>
        private void OnClick(string element)
        {
            if (this.isPressed) return;

            this.isPressed = true;
            if (element == "MenuFrame") this.isPressed = false;

            if (this.menuState == EMenuState.TitleScreen)
            {
                this.menuState = EMenuState.MainMenu;
            }

            if (element == "NewGame2")
            {
                this.menuState = EMenuState.CharacterScreen;
            }

            if (element == "StartGame2")
            {
                this.menuState = EMenuState.None;
                this.GoIngame?.Invoke();

            }

            if (element == "Option2")
            {
                this.menuState = EMenuState.Option;
            }

            if (element == "Credits2")
            {
                this.menuState = EMenuState.Credits;
            }

            if (element == "End2")
            {
                this.Game.Exit();
            }


            if (element == "Back_to_MainMenu2")
            {
                this.menuState = EMenuState.MainMenu;
            }
        }

        public event GetIngame GoIngame;

        /// <summary>
        ///     MenuState to manage all States
        /// </summary>
        private enum EMenuState
        {
            None, 

            TitleScreen, 

            MainMenu, 

            CharacterScreen, 

            Option, 

            Credits


        }
    }
}