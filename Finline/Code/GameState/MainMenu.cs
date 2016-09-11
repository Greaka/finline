using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finline.Code.GameState
{
    using Finline.Code.Game.Entities.LivingEntity;

    using Game = Microsoft.Xna.Framework.Game;

    public class MainMenu : DrawableGameComponent
    {
        public delegate void GetIngame();

        private bool isPressed;

        private readonly Dictionary<EMenuState, List<GuiElement>> guiElements = new Dictionary<EMenuState, List<GuiElement>>();
        

        private readonly SpriteBatch spriteBatch;

        public EMenuState MenuState;

        /// <summary>
        /// Constructor to use the GUIElementlist to select the element
        /// </summary>
        public MainMenu(Game game, SpriteBatch sprite)
            : base(game)
        {
            // The Lists with all the GUIElements
            this.guiElements.Add(EMenuState.TitleScreen, new List<GuiElement>());
            this.guiElements.Add(EMenuState.MainMenu, new List<GuiElement>());
            this.guiElements.Add(EMenuState.CharacterScreen, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Controls, new List<GuiElement>());

            // this.guiElements.Add(EMenuState.Records, new List<GuiElement>());  TODO: wird vielleicht in der letzten Woche vor der Prüfung nochmal eingesetzt, wenn winningscreen nochmal bearbeitet wird
            this.guiElements.Add(EMenuState.Credits, new List<GuiElement>());
            this.guiElements.Add(EMenuState.GameOver, new List<GuiElement>());
            this.guiElements.Add(EMenuState.WinningScreen, new List<GuiElement>()); // vorläufige Abgabelösung

            this.spriteBatch = sprite;
            this.MenuState = EMenuState.TitleScreen;

            this.guiElements[EMenuState.TitleScreen].Add(new GuiElement("TitleScreen")); // Texture in the Titlescreen

            
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("NewGame"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("ControlsButton"));
            // this.guiElements[EMenuState.MainMenu].Add(new GuiElement("RecordsButton")); siehe oben bei TODO
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("CreditsButton"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("EndButton"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("EnemySmaller"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("MainMenu"));





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

            

            #region Records

            // this.guiElements[EMenuState.Records].Add(new GuiElement("RecordTexture")); 
            // this.guiElements[EMenuState.Records].Add(new GuiElement("Back2MainMenu"));  wird vllt in der letzten Woche vor der Prüfung nochmal eingesetzt
            // this.guiElements[EMenuState.Records].Add(new GuiElement("LogoTransparent"));
            #endregion

            #region Credits            // here are the elements in the state Credits 

            this.guiElements[EMenuState.Credits].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Credits].Add(new GuiElement("CreditscreenTexture2"));

            #endregion

            #region GameOver

            this.guiElements[EMenuState.GameOver].Add(new GuiElement("TryAgain"));
            this.guiElements[EMenuState.GameOver].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.GameOver].Add(new GuiElement("GameOverScreen"));

            #endregion

            #region WinningScreen

            this.guiElements[EMenuState.WinningScreen].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.WinningScreen].Add(new GuiElement("WinningScreen"));

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

            this.guiElements[EMenuState.TitleScreen].Find(x => x.AssetName == "TitleScreen").MoveElement(0, -60);// move the logo up in y-direction

            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "NewGame").MoveElement(-200, -175);// move the "newgame" button up in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "ControlsButton").MoveElement(-200, -100);
            // this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "RecordsButton").MoveElement(-200, -50); siehe oben bei TODO 
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "CreditsButton").MoveElement(-200, -25); // move the "credits" button 200 in x-direction and 50 down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "EndButton").MoveElement(-200, 50); // move the "end" button down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "EnemySmaller").MoveElement(150, -20);
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "MainMenu").MoveElement(0,-265);


            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "ChooseText").MoveElement(0, -265);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "studentprofile").MoveElement(-200, -100);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "profprofile").MoveElement(200, -100);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "StartGame").MoveElement(-270, 80);// move the "StartGame" button up in y-direction
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "ControlsButton").MoveElement(-70, 80);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "Back2MainMenu").MoveElement(130, 80);// move the "Back_to_MainMenu" button down in y-direction
            

            

            this.guiElements[EMenuState.Controls].Find(x => x.AssetName == "ControlScreen").MoveElement(50, -150);
            this.guiElements[EMenuState.Controls].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 80);

                // move the "Back_to_MainMenu" button down in y-direction
            

            #region RecordState

            // this.guiElements[EMenuState.Records].Find(x => x.AssetName == "RecordTexture").MoveElement(0, -265); // move the recordtexture up in y-direction , siehe oben bei TODO
            // this.guiElements[EMenuState.Records].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 70); // move the button down in y-direction
            #endregion

            #region Moved Elements From Credits

            this.guiElements[EMenuState.Credits].Find(x => x.AssetName == "CreditscreenTexture2").MoveElement(0, -60);
            this.guiElements[EMenuState.Credits].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 50);

                // move the "Back_to_MainMenu" button down in y-direction
            #endregion

            #region Game Over

            this.guiElements[EMenuState.GameOver].Find(x => x.AssetName == "GameOverScreen").MoveElement(0, -60);
            this.guiElements[EMenuState.GameOver].Find(x => x.AssetName == "TryAgain").MoveElement(0, -10);
            this.guiElements[EMenuState.GameOver].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 75);

            #endregion

            #region WinningScreen

            this.guiElements[EMenuState.WinningScreen].Find(x => x.AssetName == "WinningScreen").MoveElement(0, -60);
            this.guiElements[EMenuState.WinningScreen].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 90);

            #endregion
        }

        public void GameOver(LivingEntity me)
        {
            this.MenuState = EMenuState.GameOver;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.MenuState == EMenuState.TitleScreen)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    this.MenuState = EMenuState.MainMenu;
            }

          
            foreach (var element in this.guiElements[this.MenuState])
            {
                element.Update(ref this.isPressed);
            }
        }


        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            if (this.MenuState != EMenuState.None)
            foreach (var element in this.guiElements[this.MenuState])
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
            this.MenuState = EMenuState.MainMenu;
        }

        /// <summary>
        /// when click on the element change to next state
        /// </summary>
        /// <param name="element"></param>
        private void OnClick(string element)
        {
            if (this.isPressed) return;
            this.isPressed = true;

            if (this.MenuState == EMenuState.TitleScreen)
            {
                this.MenuState = EMenuState.MainMenu;
            }

            switch (element)
            {
                case "NewGame":
                    this.MenuState = EMenuState.CharacterScreen;
                    GuiElement.Ausgewaehlt = Player.PlayerSelection.student;
                    break;
                case "StartGame":
                    this.MenuState = EMenuState.None;
                    this.GoIngame?.Invoke();
                    break;
                case "ControlsButton":
                    this.MenuState = EMenuState.Controls;
                    break;
                case "CreditsButton":
                    this.MenuState = EMenuState.Credits;
                    break;

                // case "RecordsButton":
                // this.MenuState = EMenuState.Records;
                // break; siehe oben bei TODO
                case "Back2MainMenu":
                    this.MenuState = EMenuState.MainMenu;
                    GuiElement.Ausgewaehlt = Player.PlayerSelection.student;
                    break;
                case "EndButton":
                    this.Game.Exit();
                    break;
                case "TryAgain":
                    this.MenuState = EMenuState.CharacterScreen;
                    break;
            }
        }

        public event GetIngame GoIngame;

        /// <summary>
        ///     MenuState to manage all States
        /// </summary>
        public enum EMenuState
        {
            None, 

            TitleScreen, 

            MainMenu, 

            CharacterScreen, 

            Controls, 

            Records, 

            Credits, 

            GameOver, 

            WinningScreen


        }

    }
}