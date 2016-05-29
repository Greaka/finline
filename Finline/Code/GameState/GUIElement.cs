using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Finline
{
    class GUIElement
    {
        private Texture2D GUITexture;

        private Rectangle GUIRect;

        private string assetName; 



        public string AssetName
        {
            get { return assetName;}
            set { assetName = value; }
        }




        public delegate void ElementClicked(string element);




        public event ElementClicked clickEvent;

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
            GUITexture = content.Load<Texture2D>(AssetName);
            GUIRect = new Rectangle(0,0,GUITexture.Width,GUITexture.Height);
        }


        public void Update()
        {
            if (GUIRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                clickEvent(assetName);
            }

            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GUITexture,GUIRect, Color.White);
        }


        /// <summary>
        /// Function to center the elements
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void CenterElement(int height, int width)
        {
            GUIRect = new Rectangle((width/2) - (this.GUITexture.Width/2) , (height/2) - (this.GUITexture.Height/2),this.GUITexture.Width,this.GUITexture.Height);
            
        }



        /// <summary>
        /// Function to move the Element
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveElement(int x, int y)
        {
            GUIRect = new Rectangle(GUIRect.X += x, GUIRect.Y += y, GUIRect.Width, GUIRect.Height);
        }

        
    }
}
