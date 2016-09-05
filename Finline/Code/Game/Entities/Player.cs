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
    using Microsoft.Xna.Framework.Input;

    public class Player : Entity
    {
        private readonly float unitsPerSecond = 15;
        Model profStand, profLinks, profRechts;
        Model student;
        List<Model> Animation = new List<Model>(4);
        int i = 0;
        float time;
      

        public void Initialize(ContentManager contentManager)
        {
            this._model = contentManager.Load<Model>("prof_stand");
            this.position = new Vector3(4,4,-0.5f);    //Start
            //this.position = new Vector3(90, 240, 0);     // Hörsaal
            student = contentManager.Load<Model>("student");
            profStand = contentManager.Load<Model>("prof_stand");
            profLinks = contentManager.Load<Model>("prof_linkesBein");
            profRechts = contentManager.Load<Model>("prof");
            Animation.Insert(0, profLinks);
            Animation.Insert(1, profStand);
            Animation.Insert(2, profRechts);
            Animation.Insert(3, profStand);

        }

        public void Update(GameTime gameTime, Vector2 moveDirection, Vector2 shootDirection, List<EnvironmentObject> environmentObjects)
        {
            this.SetViewDirection(shootDirection);
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            var pos = this.position;
            float intersection;       
            this.position +=
                new Vector3(moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * this.unitsPerSecond, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (time>0.1f) {
                    i = i + 1;
                    time = 0;
                }
                
                if (i > 3) i = 0;
                this._model = Animation[i];
                
            }



            if (this.IsColliding(environmentObjects, out intersection))
            {
                this.position = pos; // += new Vector3(ControlsHelper.MoveDirection * intersection, 0);
            }
        }
    }
}