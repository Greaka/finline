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

    using GameState;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Player : Entity
    {
        private readonly float unitsPerSecond = 15;
        Model profStand;

        Model profLinks;

        Model profRechts;

        Model studentStand;

        Model studentLinks;

        Model studentRechts;

        List<Model> AnimationProf = new List<Model>(4);
        List<Model> AnimationStudent = new List<Model>(4);
        int i = 0;
        float time;
        
      

        public void Initialize(ContentManager contentManager)
        {
            // this._model = contentManager.Load<Model>("student_stand");
            if (GuiElement.ausgewaehlt == 1) this._model = contentManager.Load<Model>("student_stand");
            else this._model = contentManager.Load<Model>("prof_stand");
            this.position = new Vector3(4, 4, -0.5f); // Standard

            // this.position = new Vector3(90, 240, 0);   // Hörsaal
            // this.position = new Vector3(26, 90, 0);      // 333
            this.studentStand = contentManager.Load<Model>("student_stand");
            this.studentLinks = contentManager.Load<Model>("student_linkesBein");
            this.studentRechts = contentManager.Load<Model>("student");

            this.profStand = contentManager.Load<Model>("prof_stand");
            this.profLinks = contentManager.Load<Model>("prof_linkesBein");
            this.profRechts = contentManager.Load<Model>("prof");

            this.AnimationStudent.Insert(0, this.studentLinks);
            this.AnimationStudent.Insert(1, this.studentStand);
            this.AnimationStudent.Insert(2, this.studentRechts);
            this.AnimationStudent.Insert(3, this.studentStand);

            this.AnimationProf.Insert(0, this.profLinks);
            this.AnimationProf.Insert(1, this.profStand);
            this.AnimationProf.Insert(2, this.profRechts);
            this.AnimationProf.Insert(3, this.profStand);
        }

        public void Update(GameTime gameTime, Vector2 moveDirection, Vector2 shootDirection, List<EnvironmentObject> environmentObjects)
        {
            this.SetViewDirection(shootDirection);
            this.time += (float)gameTime.ElapsedGameTime.TotalSeconds;            if (GuiElement.ausgewaehlt == 2 && (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D)))
            {
                if (this.time>0.1f)
                {
                    this.i = this.i + 1;
                    this.time = 0;
                }
                
                if (this.i > 3) this.i = 0;
                this._model = this.AnimationProf[this.i];
            }

            if (GuiElement.ausgewaehlt == 1 && (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D)))
            {
                if (this.time > 0.1f)
                {
                    this.i = this.i + 1;
                    this.time = 0;
                }

                if (this.i > 3) this.i = 0;
                this._model = this.AnimationStudent[this.i];
            }
            var pos = moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * this.unitsPerSecond;
            var collisionResult = this.IsColliding(environmentObjects, pos);
            this.position += new Vector3(pos, 0);
            if (collisionResult.HasValue)            {
                this.position += new Vector3(collisionResult.Value, 0);
            }
        }
    }
}