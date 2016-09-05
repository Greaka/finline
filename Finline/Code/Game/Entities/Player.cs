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
    using GameState;

    public class Player : Entity
    {
        private readonly float unitsPerSecond = 15;
        Model profStand, profLinks, profRechts;
        Model studentStand, studentLinks, studentRechts;
        List<Model> AnimationProf = new List<Model>(4);
        List<Model> AnimationStudent = new List<Model>(4);
        int i = 0;
        float time;
        
      

        public void Initialize(ContentManager contentManager)
        {
            //    this._model = contentManager.Load<Model>("student_stand");
            if (GuiElement.ausgewaehlt == 1) this._model = contentManager.Load<Model>("student_stand"); else
                this._model = contentManager.Load<Model>("prof_stand");
            this.position = new Vector3(4,4,-0.5f);    //Start
           // this.position = new Vector3(90, 240, 0);     // Hörsaal
            
            studentStand = contentManager.Load<Model>("student_stand");
            studentLinks = contentManager.Load<Model>("student_linkesBein");
            studentRechts = contentManager.Load<Model>("student");

            profStand = contentManager.Load<Model>("prof_stand");
            profLinks = contentManager.Load<Model>("prof_linkesBein");
            profRechts = contentManager.Load<Model>("prof");


            AnimationStudent.Insert(0, studentLinks);
            AnimationStudent.Insert(1, studentStand);
            AnimationStudent.Insert(2, studentRechts);
            AnimationStudent.Insert(3, studentStand);

            AnimationProf.Insert(0, profLinks);
            AnimationProf.Insert(1, profStand);
            AnimationProf.Insert(2, profRechts);
            AnimationProf.Insert(3, profStand);

        }

        public void Update(GameTime gameTime, Vector2 moveDirection, Vector2 shootDirection, List<EnvironmentObject> environmentObjects)
        {
            this.SetViewDirection(shootDirection);
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            var pos = this.position;
            float intersection;       
            this.position +=
                new Vector3(moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * this.unitsPerSecond, 0);

            if (GuiElement.ausgewaehlt == 2 && (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D)))
            {
                if (time>0.1f)
                {
                    i = i + 1;
                    time = 0;
                }
                
                if (i > 3) i = 0;
                this._model = AnimationProf[i];
            }

            if (GuiElement.ausgewaehlt == 1 && (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D)))
            {
                if (time > 0.1f)
                {
                    i = i + 1;
                    time = 0;
                }

                if (i > 3) i = 0;
                this._model = AnimationStudent[i];
            }



            if (this.IsColliding(environmentObjects, out intersection))
            {
                this.position = pos; // += new Vector3(ControlsHelper.MoveDirection * intersection, 0);
            }
        }
    }
}