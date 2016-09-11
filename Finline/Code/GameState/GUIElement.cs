namespace Finline.Code.GameState
{
    using Finline.Code.Game.Entities.LivingEntity;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    internal class GuiElement
    {
        public delegate void ElementClicked(string element);

        private Rectangle guiRect;
        private Texture2D guiTexture;

        MouseState oldMouseState;
        public static Player.PlayerSelection Ausgewaehlt = Player.PlayerSelection.student;



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
            this.guiTexture = content.Load<Texture2D>("GuiElements/" + this.AssetName);

            this.guiRect = new Rectangle(0, 0, this.guiTexture.Width, this.guiTexture.Height);
        }

        public void Update(ref bool isPressed)
        {
            if (this.guiRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) &&
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

            if (this.guiRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && this.oldMouseState.LeftButton == ButtonState.Released && newMouseState.LeftButton == ButtonState.Pressed)
            {
                if (this.AssetName == "studentprofile") Ausgewaehlt = Player.PlayerSelection.student;
                if (this.AssetName == "profprofile") Ausgewaehlt = Player.PlayerSelection.prof;
            }

            switch (this.AssetName)
            {
                case "LogoTransparent":
                    spriteBatch.Draw(this.guiTexture, new Rectangle(620, 300, 150, 150), null, Color.White);
                    break;
                case "studentprofile":
                    spriteBatch.Draw(this.guiTexture, 
                        new Rectangle(120, 120, this.guiTexture.Width, this.guiTexture.Height), null, 
                        Ausgewaehlt == Player.PlayerSelection.student ? Color.White : Color.DimGray);
                    break;
                case "profprofile":
                    spriteBatch.Draw(this.guiTexture, 
                        new Rectangle(520, 120, this.guiTexture.Width, this.guiTexture.Height), null, 
                        Ausgewaehlt == Player.PlayerSelection.prof ? Color.White : Color.DimGray);
                    break;
                default:
                    spriteBatch.Draw(this.guiTexture, this.guiRect, Color.White);
                    break;
            }

            this.oldMouseState = newMouseState;
        }


        /// <summary>
        ///     Function to center the elements
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void CenterElement(int height, int width)
        {
            this.guiRect = new Rectangle(width / 2 - this.guiTexture.Width / 2, height / 2 - this.guiTexture.Height / 2, this.guiTexture.Width, this.guiTexture.Height);
        }


        /// <summary>
        ///     Function to move the Element
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveElement(int x, int y)
        {
            this.guiRect = new Rectangle(this.guiRect.X += x, this.guiRect.Y += y, this.guiRect.Width, this.guiRect.Height);
        }
    }
}