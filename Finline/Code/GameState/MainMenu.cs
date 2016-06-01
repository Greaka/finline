using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finline.Code.GameState
{
    internal class MainMenu : DrawableGameComponent
    {
        public delegate void GetIngame();

        public static bool IsPressed;
        private readonly List<GuiElement> _main = new List<GuiElement>();
        private readonly List<GuiElement> _option = new List<GuiElement>();
        private readonly List<GuiElement> _credits = new List<GuiElement>();

        private readonly SpriteBatch _spriteBatch;


        // The Lists with all the elements
        private readonly List<GuiElement> _title = new List<GuiElement>();
        private EMenuState _menuState;
        private SpriteFont _font;

        /// <summary>
        ///     Constructor to use the GUIElementlist to select the element
        /// </summary>
        public MainMenu(Microsoft.Xna.Framework.Game game, SpriteBatch sprite)
            : base(game)
        {
            _spriteBatch = sprite;
            _menuState = EMenuState.TitleScreen;

            _title.Add(new GuiElement("Logo 2")); //Logo in the state Titlescreen


            //here are the elements in the state MainMenu

            _main.Add(new GuiElement("MenuFrame"));
            _main.Add(new GuiElement("NewGame"));
            _main.Add(new GuiElement("Option"));
            _main.Add(new GuiElement("Credits"));
            _main.Add(new GuiElement("End"));

            //here are the elements in the state Option
            _option.Add(new GuiElement("Back_to_MainMenu"));


            _credits.Add(new GuiElement("Back_to_MainMenu"));
        }

        protected override void LoadContent()
        {
            _font = Game.Content.Load<SpriteFont>("font");

            foreach (var element in _title)
            {
                element.LoadContent(Game.Content);
                element.CenterElement(600, 800);
                element.ClickEvent += OnClick;
            }
            _title.Find(x => x.AssetName == "Logo 2").MoveElement(0, -50); //move the logo up in y-direction


            foreach (var element in _main)
            {
                element.LoadContent(Game.Content);
                element.CenterElement(600, 800);
                element.ClickEvent += OnClick;
            }
            _main.Find(x => x.AssetName == "MenuFrame").MoveElement(0, -50); // frame Movement
            _main.Find(x => x.AssetName == "NewGame").MoveElement(0, -200); // move the "newgame" button up in y-direction
            _main.Find(x => x.AssetName == "Option").MoveElement(0, -50); // move the "option" button down in y-direction
            _main.Find(x => x.AssetName == "Credits").MoveElement(200 , 50); //move the "credits" button 200 in x-direction and 50 down in y-direction
            _main.Find(x => x.AssetName == "End").MoveElement(0, 100); //move the "end" button down in y-direction


            foreach (var element in _option)
            {
                element.LoadContent(Game.Content);
                element.CenterElement(600, 800);
                element.ClickEvent += OnClick;
            }

            _option.Find(x => x.AssetName == "Back_to_MainMenu").MoveElement(0, 50);
                // move the "Back_to_MainMenu" button down in y-direction


            foreach (var element in _credits)
            {
                element.LoadContent(Game.Content);
                element.CenterElement(600,800);
                element.ClickEvent += OnClick;
            }

            _credits.Find(x => x.AssetName == "Back_to_MainMenu").MoveElement(0, 50);
            

        }

        public override void Update(GameTime gameTime)
        {
            switch (_menuState)
            {
                case EMenuState.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        _menuState = EMenuState.MainMenu;
                    break;


                case EMenuState.MainMenu:
                    foreach (var element in _main)
                    {
                        element.Update();
                    }

                    break;

                case EMenuState.Option:
                    foreach (var element in _option)
                    {
                        element.Update();
                    }
                    break;

                case EMenuState.Credits:
                    foreach (var element in _credits)
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
            _spriteBatch.Begin();

            switch (_menuState)
            {
                case EMenuState.TitleScreen:
                    foreach (var element in _title)
                    {
                        element.Draw(_spriteBatch);
                    }


                    break;
                case EMenuState.MainMenu:
                    foreach (var element in _main)
                    {
                        element.Draw(_spriteBatch);
                    }

                    break;
                case EMenuState.Option:
                    foreach (var element in _option)
                    {
                        element.Draw(_spriteBatch);
                    }

                    break;
                case EMenuState.Credits:
                    foreach (var element in _credits)
                    {
                        element.Draw(_spriteBatch);
                    }
                    _spriteBatch.DrawString(_font,
                        "Minh Vuong Pham\n" + "Michl Steglich\n" + "Tim Stadelmann\n" + "Tino Nagelmueller\n",
                        new Vector2(300, 100), Color.Black);
                    
                    break;
                case EMenuState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _spriteBatch.End();
        }

        /// <summary>
        ///     when click on the element change to next state
        /// </summary>
        /// <param name="element"></param>
        private void OnClick(string element)
        {
            if (IsPressed) return;

            IsPressed = true;
            if (element == "MenuFrame")
                IsPressed = false;

            if (_menuState == EMenuState.TitleScreen)
            {
                _menuState = EMenuState.MainMenu;
            }

            if (element == "NewGame")
            {
                _menuState = EMenuState.None;
                GoIngame?.Invoke();
            }

            if (element == "Option")
            {
                _menuState = EMenuState.Option;
            }

            if (element == "Credits")
            {
                _menuState = EMenuState.Credits;
            }

            if (element == "End")
            {
                _menuState = EMenuState.TitleScreen;
            }


            if (element == "Back_to_MainMenu")
            {
                _menuState = EMenuState.MainMenu;
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

            Option,

            Credits


        }
    }
}