using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finline.Code.GameState
{
    internal class GuiElement
    {
        public delegate void ElementClicked(string element);

        private Rectangle _guiRect;
        private Texture2D _guiTexture;

        MouseState oldMouseState;
        public int ausgewaehlt = 0;



        /// <summary>
        ///     Constructor for GUIElements
        /// </summary>
        /// <param name="assetName"></param>
        public GuiElement(string assetName)
        {
            this.AssetName = assetName;
        }


        public string AssetName { get; }

        public event ElementClicked ClickEvent;


        public void LoadContent(ContentManager content)
        {
            this._guiTexture = content.Load<Texture2D>(this.AssetName);

            this._guiRect = new Rectangle(0, 0, this._guiTexture.Width, this._guiTexture.Height);
        }

        public void Update(ref bool isPressed)
        {
            if (this._guiRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                this.ClickEvent?.Invoke(this.AssetName);
            }

            if (Mouse.GetState().LeftButton != ButtonState.Pressed)
            {
                isPressed = false;
            }



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var newMouseState = Mouse.GetState();
          
            if (this.AssetName == "LogoTransparent")
                spriteBatch.Draw(this._guiTexture, new Rectangle(620, 300, 150, 150), null, Color.White);
            else spriteBatch.Draw(this._guiTexture, this._guiRect, Color.White);


            if (this._guiRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) &&
                        oldMouseState.LeftButton == ButtonState.Released && newMouseState.LeftButton == ButtonState.Pressed)
            {
                ausgewaehlt = (ausgewaehlt + 1) % 2;
            }

            if (this.AssetName == "Ashe")
            {
                spriteBatch.Draw(this._guiTexture,
                    new Rectangle(150, 150, this._guiTexture.Width, this._guiTexture.Height), null,
                    ausgewaehlt == 1 ? Color.Goldenrod : Color.White);
            }
            if (this.AssetName == "Yasuo")
            {
                spriteBatch.Draw(this._guiTexture,
                    new Rectangle(550, 150, this._guiTexture.Width, this._guiTexture.Height), null,
                    ausgewaehlt == 1 ? Color.Goldenrod : Color.White);
            }

            oldMouseState = newMouseState;
        }


        /// <summary>
        ///     Function to center the elements
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void CenterElement(int height, int width)
        {
            this._guiRect = new Rectangle(width / 2 - this._guiTexture.Width / 2, height / 2 - this._guiTexture.Height / 2, this._guiTexture.Width, this._guiTexture.Height);
        }


        /// <summary>
        ///     Function to move the Element
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveElement(int x, int y)
        {
            this._guiRect = new Rectangle(this._guiRect.X += x, this._guiRect.Y += y, this._guiRect.Width, this._guiRect.Height);
        }
    }
}