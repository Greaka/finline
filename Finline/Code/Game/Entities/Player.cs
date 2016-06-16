// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="">
// </copyright>
// <summary>
//   Defines the Player type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Game.Entities
{
    using Finline.Code.Game.Helper;
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Player : Entity
    {
        private readonly float unitsPerSecond = 15;

        public Vector3 Position
        {
            get { return this._position; }
            private set
            {
                var pos = this._position;
                this._position = value;
                float intersection;
                if (this.IsColliding(ControlsHelper.EnvironmentObjects, out intersection))
                {
                    this._position = pos; // += new Vector3(ControlsHelper.MoveDirection * intersection, 0);
                }
            }
        }

        public void Initialize(ContentManager contentManager)
        {
            this._model = contentManager.Load<Model>("Undead");
            this._position = Vector3.Zero;
        }

        public void Update(GameTime gameTime)
        {
            this.SetViewDirection(ControlsHelper.MoveDirection);
            this.Position += new Vector3(ControlsHelper.MoveDirection * 
               (float)gameTime.ElapsedGameTime.TotalSeconds * this.unitsPerSecond, 
               0);
            ControlsHelper.PlayerPosition = this._position;
        }
    }
}