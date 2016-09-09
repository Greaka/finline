using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finline.Code.GameState
{
    using Finline.Code.Game.Entities;

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
            // The Lists with all the GUIElements
            this.guiElements.Add(EMenuState.TitleScreen, new List<GuiElement>());
            this.guiElements.Add(EMenuState.MainMenu, new List<GuiElement>());
            this.guiElements.Add(EMenuState.CharacterScreen, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Controls, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Records, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Credits, new List<GuiElement>());
            this.guiElements.Add(EMenuState.GameOver, new List<GuiElement>());

            this.spriteBatch = sprite;
            this.menuState = EMenuState.TitleScreen;

            this.guiElements[EMenuState.TitleScreen].Add(new GuiElement("TitleScreen")); // Texture in the Titlescreen


            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("NewGame"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("ControlsButton"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("RecordsButton"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("CreditsButton"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("EndButton"));
            


            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("ChooseText"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("StartGame"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("ControlsButton"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("LogoTransparent"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("studentprofile"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("profprofile"));

            


            this.guiElements[EMenuState.Controls].Add(new GuiElement("ControlScreen"));
            this.guiElements[EMenuState.Controls].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Controls].Add(new GuiElement("LogoTransparent"));
            


            this.guiElements[EMenuState.Records].Add(new GuiElement("RecordTexture"));
            this.guiElements[EMenuState.Records].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Records].Add(new GuiElement("LogoTransparent"));
            

#region Credits            // here are the elements in the state Credits 
            this.guiElements[EMenuState.Credits].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Credits].Add(new GuiElement("CreditscreenTexture2"));
            #endregion

            #region GameOver
            this.guiElements[EMenuState.GameOver].Add(new GuiElement("TryAgain"));
            this.guiElements[EMenuState.GameOver].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.GameOver].Add(new GuiElement("GameOverScreen"));
            #endregion
        }

        protected override void LoadContent()
        {
            // this.font = this.Game.Content.Load<SpriteFont>("font");
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
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "ControlsButton").MoveElement(-200, -125); // move the "option" button down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "RecordsButton").MoveElement(-200, -50);
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "CreditsButton").MoveElement(-200 , 25); // move the "credits" button 200 in x-direction and 50 down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "EndButton").MoveElement(-200, 100); // move the "end" button down in y-direction
            

            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "ChooseText").MoveElement(0, -265);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "studentprofile").MoveElement(-200, -100);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "profprofile").MoveElement(200, -100);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "StartGame").MoveElement(-270, 80); // move the "StartGame" button up in y-direction
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "ControlsButton").MoveElement(-70, 80);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "Back2MainMenu").MoveElement(130, 80); // move the "Back_to_MainMenu" button down in y-direction
            

            this.guiElements[EMenuState.Controls].Find(x => x.AssetName == "ControlScreen").MoveElement(50, -150);
            this.guiElements[EMenuState.Controls].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 80); // move the "Back_to_MainMenu" button down in y-direction
            

            this.guiElements[EMenuState.Records].Find(x => x.AssetName == "RecordTexture").MoveElement(0, -265); // move the recordtexture up in y-direction
            this.guiElements[EMenuState.Records].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 70); // move the button down in y-direction
            
#region Moved Elements From Credits
            this.guiElements[EMenuState.Credits].Find(x => x.AssetName == "CreditscreenTexture2").MoveElement(0, -60);
            this.guiElements[EMenuState.Credits].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 50); // move the "Back_to_MainMenu" button down in y-direction
            #endregion

            #region Game Over
            this.guiElements[EMenuState.GameOver].Find(x => x.AssetName == "GameOverScreen").MoveElement(0, -60);
            this.guiElements[EMenuState.GameOver].Find(x => x.AssetName == "TryAgain").MoveElement(0, -10);
            this.guiElements[EMenuState.GameOver].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 75);


            #endregion
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
            if (this.menuState != EMenuState.None)
            foreach (var element in this.guiElements[this.menuState])
            {
                element.Draw(this.spriteBatch);
            }

            this.spriteBatch.End();
        }

        /// <summary>
        /// put menuState to EMenuState MainMenu
        /// </summary>
        public void MakeHeile()
        {
            this.menuState = EMenuState.MainMenu;
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

            switch (element)
           {
               case "NewGame":
                   this.menuState = EMenuState.CharacterScreen;
                    GuiElement.Ausgewaehlt = Player.PlayerSelection.student;
                    break;
               case "StartGame":
                        this.menuState = EMenuState.None;
                        this.GoIngame?.Invoke();
                   break;
               case "ControlsButton":
                   this.menuState = EMenuState.Controls;
                   break;
               case "CreditsButton":
                   this.menuState = EMenuState.Credits;
                   break;
               case "RecordsButton":
                   this.menuState = EMenuState.Records;
                   break;
               case "Back2MainMenu":
                   this.menuState = EMenuState.MainMenu;
                   GuiElement.Ausgewaehlt = Player.PlayerSelection.student;
                   break;
               case "EndButton":
                   this.Game.Exit();
                   break;
               case "TryAgain":
                   this.menuState = EMenuState.CharacterScreen;
                   break;
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

            Controls, 

            Records, 

            Credits, 

            GameOver, 


        }

    }
}