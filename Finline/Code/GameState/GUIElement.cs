using System.Security.Cryptography.X509Certificates;

namespace Finline.Code.GameState
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    class GUIElement
    {
        private Texture2D GUITexture;

        private Rectangle GUIRect;

        private string assetName;
        

        




        public string AssetName
        {
            get
            {
                return this.assetName;
            }

            private set
            {
                this.assetName = value;
            }
        }

        public delegate void ElementClicked(string element);
        public event ElementClicked ClickEvent;

        /// <summary>
        /// Constructor for GUIElements
        /// </summary>
        /// <param name="assetName"></param>
        public GUIElement(string assetName)
        {
            this.AssetName = assetName;
        }


        public void LoadContent(ContentManager content)
        {
            this.GUITexture = content.Load<Texture2D>(this.AssetName);
            this.GUIRect = new Rectangle(0, 0, this.GUITexture.Width, this.GUITexture.Height);
        }
        
        public void Update()
        {
            if (this.GUIRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                this.ClickEvent(this.assetName);
            }

            if (Mouse.GetState().LeftButton != ButtonState.Pressed)
            {
                MainMenu.isPressed = false;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(this.GUITexture, this.GUIRect, Color.White);
        }


        /// <summary>
        /// Function to center the elements
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void CenterElement(int height, int width)
        {
            this.GUIRect = new Rectangle((width/2) - (this.GUITexture.Width/2) , (height/2) - (this.GUITexture.Height/2), this.GUITexture.Width, this.GUITexture.Height);
            
        }



        /// <summary>
        /// Function to move the Element
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveElement(int x, int y)
        {
            this.GUIRect = new Rectangle(this.GUIRect.X += x, this.GUIRect.Y += y, this.GUIRect.Width, this.GUIRect.Height);
        }

     



    }



}
