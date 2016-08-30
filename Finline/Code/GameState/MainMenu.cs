﻿using System.Collections.Generic;
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
            // The Lists with all the GUIElements
            this.guiElements.Add(EMenuState.TitleScreen, new List<GuiElement>());
            this.guiElements.Add(EMenuState.MainMenu, new List<GuiElement>());
            this.guiElements.Add(EMenuState.CharacterScreen, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Controls, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Records, new List<GuiElement>());
            this.guiElements.Add(EMenuState.Credits, new List<GuiElement>());

            this.spriteBatch = sprite;
            this.menuState = EMenuState.TitleScreen;

            this.guiElements[EMenuState.TitleScreen].Add(new GuiElement("TitleScreen")); //Texture in the Titlescreen

#region MainMenu  // here are the elements in the state MainMenu
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("NewGame"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("ControlsButton"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("RecordsButton"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("CreditsButton"));
            this.guiElements[EMenuState.MainMenu].Add(new GuiElement("EndButton"));
            #endregion

#region CharacterScreen           // elements in the state characterScreen
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("StartGame"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("LogoTransparent"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("Ashe"));
            this.guiElements[EMenuState.CharacterScreen].Add(new GuiElement("Yasuo"));
            #endregion

#region Controls           // here are the elements in the state Option
            this.guiElements[EMenuState.Controls].Add(new GuiElement("ControlScreen"));
            this.guiElements[EMenuState.Controls].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Controls].Add(new GuiElement("LogoTransparent"));
            #endregion

#region Records            // here are the elements in the state Records
            this.guiElements[EMenuState.Records].Add(new GuiElement("RecordTexture"));
            this.guiElements[EMenuState.Records].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Records].Add(new GuiElement("LogoTransparent"));
            #endregion

#region Credits            // here are the elements in the state Credits 
            this.guiElements[EMenuState.Credits].Add(new GuiElement("Back2MainMenu"));
            this.guiElements[EMenuState.Credits].Add(new GuiElement("CreditscreenTexture2"));
            #endregion
        }

        protected override void LoadContent()
        {
            //this.font = this.Game.Content.Load<SpriteFont>("font");
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

#region Moved Elements From MainMenu
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "NewGame").MoveElement(-200, -200); // move the "newgame" button up in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "ControlsButton").MoveElement(-200, -125); // move the "option" button down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "RecordsButton").MoveElement(-200, -50);
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "CreditsButton").MoveElement(-200 , 25); // move the "credits" button 200 in x-direction and 50 down in y-direction
            this.guiElements[EMenuState.MainMenu].Find(x => x.AssetName == "EndButton").MoveElement(-200, 100); // move the "end" button down in y-direction
            #endregion
#region Moved Elements From CharacterScreen
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "Ashe").MoveElement(-200, -100);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "Yasuo").MoveElement(200, -100);
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName =="StartGame").MoveElement(0, 25); // move the "StartGame" button up in y-direction
            this.guiElements[EMenuState.CharacterScreen].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 100); // move the "Back_to_MainMenu" button down in y-direction
            #endregion
#region Moved Elements From Controls
            this.guiElements[EMenuState.Controls].Find(x => x.AssetName == "ControlScreen").MoveElement(50, -150);
            this.guiElements[EMenuState.Controls].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 80); // move the "Back_to_MainMenu" button down in y-direction
            #endregion
#region Moved Elements From Records
            this.guiElements[EMenuState.Records].Find(x => x.AssetName == "RecordTexture").MoveElement(0, -265); // move the recordtexture up in y-direction
            this.guiElements[EMenuState.Records].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 70); // move the button down in y-direction
            #endregion
#region Moved Elements From Credits
            this.guiElements[EMenuState.Credits].Find(x => x.AssetName == "CreditscreenTexture2").MoveElement(0, -60);
            this.guiElements[EMenuState.Credits].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 50); // move the "Back_to_MainMenu" button down in y-direction
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
            menuState = EMenuState.MainMenu;
        }

        /// <summary>
        /// when click on the element change to next state
        /// </summary>
        /// <param name="element"></param>
        private void OnClick(string element)
        {
            if (this.isPressed) return;
            this.isPressed = true;
#region Clickable Elements              
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

            if (element == "ControlsButton")
            {
                this.menuState = EMenuState.Controls;
            }

            if (element == "CreditsButton")
            {
                this.menuState = EMenuState.Credits;
            }

            if (element == "RecordsButton")
            {
                this.menuState = EMenuState.Records;
            }

            if (element == "Back2MainMenu")
            {
                this.menuState = EMenuState.MainMenu;
            }

            if (element == "EndButton")
            {
                this.Game.Exit();
            }
#endregion
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

            Credits
        }

    }
}