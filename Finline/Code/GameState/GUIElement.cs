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

        /// <summary>
        ///     Constructor for GUIElements
        /// </summary>
        /// <param name="assetName"></param>
        public GuiElement(string assetName)
        {
            AssetName = assetName;
        }


        public string AssetName { get; }

        public event ElementClicked ClickEvent;


        public void LoadContent(ContentManager content)
        {
            
            _guiTexture = content.Load<Texture2D>(AssetName);

            _guiRect = new Rectangle(0, 0, _guiTexture.Width, _guiTexture.Height);
        }

        public void Update()
        {
            if (_guiRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                ClickEvent?.Invoke(AssetName);
            }

            if (Mouse.GetState().LeftButton != ButtonState.Pressed)
            {
                MainMenu.IsPressed = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (AssetName == "Logo 2")
                spriteBatch.Draw(_guiTexture, new Rectangle(30, 10, 700, 440), null, Color.White);
            else
                spriteBatch.Draw(_guiTexture, _guiRect, Color.White);
        }


        /// <summary>
        ///     Function to center the elements
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void CenterElement(int height, int width)
        {
            _guiRect = new Rectangle(width/2 - _guiTexture.Width/2, height/2 - _guiTexture.Height/2, _guiTexture.Width,
                _guiTexture.Height);
        }


        /// <summary>
        ///     Function to move the Element
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveElement(int x, int y)
        {
            _guiRect = new Rectangle(_guiRect.X += x, _guiRect.Y += y, _guiRect.Width, _guiRect.Height);
        }
    }
}