// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="">
// </copyright>
// <summary>
//   Defines the Player type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Game.Entities
{
    using System.Collections.Generic;

    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Player : Entity
    {
        private readonly float unitsPerSecond = 15; 

        public void Initialize(ContentManager contentManager)
        {
            this._model = contentManager.Load<Model>("prof");
            this.position = new Vector3(4,4,-0.5f);
        }

        public void Update(GameTime gameTime, Vector2 moveDirection, Vector2 shootDirection, List<EnvironmentObject> environmentObjects)
        {
            this.SetViewDirection(shootDirection);
            var pos = this.position;
            float intersection;
            this.position +=
                new Vector3(moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * this.unitsPerSecond, 0);

            if (this.IsColliding(environmentObjects, out intersection))
            {
                this.position = pos; // += new Vector3(ControlsHelper.MoveDirection * intersection, 0);
            }
        }
    }
}