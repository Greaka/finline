using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finline.Code.GameState;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Finline
{
    class MainMenu: IGameState {


        //Gamestate to manage all the states
        enum GameState
            {
             TitleScreen,
             MainMenu,
             Option,
             InGame
             }

         GameState gameState;


        //The Lists with all the elements
        List<GUIElement> title = new List<GUIElement>();
        List<GUIElement> main = new List<GUIElement>(); 
        List<GUIElement> option = new List<GUIElement>();
        private GUIElement.ElementClicked Onclick;


        //Using Texture from the List
        public MainMenu()
        {
            title.Add(new GUIElement("Logo"));

            main.Add(new GUIElement("MenuFrame"));
            main.Add(new GUIElement("NewGame"));
            main.Add(new GUIElement("Option"));

            option.Add(new GUIElement(("End")));
           
        }

        public void LoadContent(ContentManager content)
        {

            foreach (GUIElement element in title)
            {
                element.LoadContent(content);
                element.CenterElement(600,800);
                element.clickEvent += OnClick;
            }

            foreach (GUIElement element in main)
            {
                element.LoadContent(content);
                element.CenterElement(600,800);
                element.clickEvent += OnClick;
            }

            main.Find(x => x.AssetName == "NewGame").MoveElement(0, -100); //move the newgamee button 100 up in y-direction

            foreach (GUIElement element in option)
            {
                element.LoadContent(content);
                element.CenterElement(600, 800);
                element.clickEvent += OnClick;

            }
            main.Find(x => x.AssetName == "Option").MoveElement(0, 100); //move the option button down in y-direction


            foreach (GUIElement element in option)
            {
                element.LoadContent(content);
                element.CenterElement(600, 800);
                element.clickEvent += OnClick;

            }
            option.Find(x => x.AssetName == "End").MoveElement(0, 50); 

        }
        public void Update()
        {
            switch (gameState) {
                case GameState.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        gameState = GameState.MainMenu;
                    break;
                    

                case GameState.MainMenu:
                    foreach (GUIElement element in main)
                    {
                        element.Update();
                    }
                    break;
                   
                case GameState.Option:
                    foreach (GUIElement element in option)
                    {
                        element.Update();
                    }
                    break;
                  

                case GameState.InGame:
                    break;
            }


           

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.TitleScreen:
                    foreach (GUIElement element in title)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
               case GameState.MainMenu:
                    foreach (GUIElement element in main)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                case GameState.Option:
                    foreach (GUIElement element in option)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;

                case GameState.InGame: break;
            }
            


        }


        //When Click on Buttons, Change to the GameState
        public void OnClick(string element)
        {
          

             if (element == "NewGame")
            {
                gameState = GameState.InGame;
            }

            if (element == "Option")
            {
                gameState = GameState.Option;
            }

            if (element == "End")
            {
                gameState = GameState.MainMenu;
            }
        }

        public void initialize(ContentManager content)
        {
           LoadContent(content);
        }

        public EGameState Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.P))
                return EGameState.InGame;
            return EGameState.HauptMenu;
        }
    }
}
