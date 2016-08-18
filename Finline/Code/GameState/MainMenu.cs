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
            this.guiElements.Add(EMenuState.Records, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Credits, new List<GuiElement>());

            this.spriteBatch = sprite;
            this.menuState = EMenuState.TitleScreen;

            this.guiElements[EMenuState.TitleScreen].Add(new GuiElement("TitleScreen")); //Texture in the Titlescreen
            
            // here are the elements in the state MainMenu
            
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("NewGame"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("Options"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("Records"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("Credits"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("End"));
            
            // elements in the state characterScreen
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("StartGame"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("Back2MainMenu"));

            // here are the elements in the state Option
            this.guiElements[EMenuState.Option].Add(new GuiElement("Controls"));
            this.guiElements[EMenuState.Option].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Option].Add(new GuiElement("LogoTransparent"));

            // here are the elements in the state Records
            this.guiElements[EMenuState.Records].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Records].Add(new GuiElement("LogoTransparent"));

            // here are the elements in the state Credits
            
            this.guiElements[EMenuState.Credits].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Credits].Add(new GuiElement("CreditscreenTexture2"));


           

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
            
            this.guiElements[EMenuState.TitleScreen].Find(x => x.AssetName == "TitleScreen").MoveElement(0, -60); // move the logo up in y-direction

            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "NewGame").MoveElement(-200, -200); // move the "newgame" button up in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "Options").MoveElement(-200, -125); // move the "option" button down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "Records").MoveElement(-200, -50);
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "Credits").MoveElement(-200 , 25); // move the "credits" button 200 in x-direction and 50 down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "End").MoveElement(-200, 100); // move the "end" button down in y-direction
            
            this.guiElements[EMenuState.CharacterScreen].Find(x=> x.AssetName =="StartGame").MoveElement(0, 25); // move the "StartGame" button up in y-direction
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 100); // move the "Back_to_MainMenu" button down in y-direction
            
            this.guiElements[EMenuState.Option].Find(x => x.AssetName == "Controls").MoveElement(0, -185);
            this.guiElements[EMenuState.Option].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 80); // move the "Back_to_MainMenu" button down in y-direction
            
            this.guiElements[EMenuState.Records].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 70); // move the button down in y-direction

            this.guiElements[EMenuState.Credits].Find(x => x.AssetName == "CreditscreenTexture2").MoveElement(0, -60);
            this.guiElements[EMenuState.Credits].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 50); // move the "Back_to_MainMenu" button down in y-direction
            
            
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

            if (this.menuState == EMenuState.Records)
            {
                this.spriteBatch.DrawString(this.font, "Records", new Vector2(350, 0),Color.White);
                this.spriteBatch.DrawString(this.font, "Placement", new Vector2(80, 50), Color.White);
                this.spriteBatch.DrawString(this.font,"Name",new Vector2(300, 50),Color.White);
                this.spriteBatch.DrawString(this.font, "Time", new Vector2(550, 50), Color.White);
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

            if (this.menuState == EMenuState.TitleScreen)
            {
                this.menuState = EMenuState.MainMenu;
            }

            if (element == "NewGame")
            {
                this.menuState = EMenuState.CharacterScreen;
            }

            if (element == "StartGame")
            {
                this.menuState = EMenuState.None;
                this.GoIngame?.Invoke();

            }

            if (element == "Options")
            {
                this.menuState = EMenuState.Option;
            }

            if (element == "Credits")
            {
                this.menuState = EMenuState.Credits;
            }

            if (element == "Records")
            {
                this.menuState = EMenuState.Records;
            }

            if (element == "Back2MainMenu")
            {
                this.menuState = EMenuState.MainMenu;
            }

            if (element == "End")
            {
                this.Game.Exit();
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

            Records,

            Credits


        }
    }
}