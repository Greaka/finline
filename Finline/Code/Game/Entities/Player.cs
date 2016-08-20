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
            this._model = contentManager.Load<Model>("player");
            this.position = new Vector3(4,4,0);
        }

        public void Update(GameTime gameTime, Vector2 moveDirection, Vector2 shootDirection, List<EnvironmentObject> environmentObjects)
        {
            this.SetViewDirection(shootDirection);
            var pos = moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * this.unitsPerSecond;
            var collisionResult = this.IsColliding(environmentObjects, pos);
            this.position += new Vector3(pos, 0);
            if (collisionResult.HasValue)
            {
                this.position += new Vector3(collisionResult.Value, 0);
            }
        }
    }
}