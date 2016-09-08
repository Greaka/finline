#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finline.Code.DebugUtils
{
    using Finline.Code.Game.Entities;
    using Finline.Code.GameState;
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    class HullDrawing : DrawableGameComponent
    {
        private IList<Vector2> bound;

        public HullDrawing(StateManager game, SpriteBatch sb)
            : base(game)
        {
            this.spriteBatch = sb;
        }

        Texture2D t; // base for the line texture

        private SpriteBatch spriteBatch;

        protected override void LoadContent()
        {
            this.effect = new BasicEffect(this.GraphicsDevice);

            // create 1x1 texture for line drawing
            this.t = new Texture2D(this.GraphicsDevice, 1, 1);
            this.t.SetData<Color>(new[] { Color.White });
        }

        private BasicEffect effect;

        private IList<EnvironmentObject> env;

        private IList<Enemy> enemies;

        private IList<Projectile> projectiles;

        private Player player;

        public void LoadEntities(IList<EnvironmentObject> env, IList<Enemy> enemies, IList<Projectile> projectiles, Player player)
        {
            this.LoadContent();
            this.env = env;
            this.enemies = enemies;
            this.projectiles = projectiles;
            this.player = player;
        }

        public void Draw(GameTime gameTime, Matrix projectionMatrix, Matrix viewMatrix)
        {
            this.spriteBatch.Begin();

            const float Multiplikator = 7;

            foreach (var entity in this.enemies)
            {
                for (var i = 0; i < entity.GetBound.Length; i++)
                {
                    var p1 = entity.GetBound[i].Position.Get2D() * Multiplikator;
                    var p2 = (i + 1 >= entity.GetBound.Length ? entity.GetBound[0] : entity.GetBound[i + 1]).Position.Get2D() * Multiplikator;
                    this.DrawLine(this.spriteBatch, p1, p2, Color.Red);
                }
            }

            foreach (var entity in this.env)
            {
                for (var i = 0; i < entity.GetBound.Length; i++)
                {
                    var p1 = entity.GetBound[i].Position.Get2D() * Multiplikator;
                    var p2 = (i + 1 >= entity.GetBound.Length ? entity.GetBound[0] : entity.GetBound[i + 1]).Position.Get2D() * Multiplikator;
                    this.DrawLine(this.spriteBatch, p1, p2, Color.White);
                }
            }

            for (var i = 0; i < this.player.GetBound.Length; i++)
            {
                var p1 = this.player.GetBound[i].Position.Get2D() * Multiplikator;
                var p2 =
                    (i + 1 >= this.player.GetBound.Length ? this.player.GetBound[0] : this.player.GetBound[i + 1])
                        .Position.Get2D() * Multiplikator;
                this.DrawLine(this.spriteBatch, p1, p2, Color.LimeGreen);
            }

            this.spriteBatch.End();
            var pbound = this.player.GetBound;

            // Initialize an array of indices of type short.
            var lineListIndices = new short[pbound.Length + 1];

            // Populate the array with references to indices in the vertex buffer
            for (var i = 0; i < pbound.Length; i++)
            {
                lineListIndices[i] = (short)i;
            }

            lineListIndices[lineListIndices.Length - 1] = 0;

            this.effect.View = viewMatrix;
            this.effect.VertexColorEnabled = true;
            this.effect.Projection = projectionMatrix;
            this.effect.TextureEnabled = true;

            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                /*this.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineStrip,
                    pbound,
                    0,
                    pbound.Length,
                    lineListIndices,
                    0,
                    pbound.Length);*/
            }
        }

        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color)
        {
            var offset = new Vector2(10);
            var edge = end - start;

            // calculate angle to rotate line
            var angle = (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(
                this.t, 
                new Rectangle(

                    // rectangle defines shape of line and position of start of line
                    (int) (start.X + offset.X), 
                    (int) (start.Y + offset.Y), 
                    (int)edge.Length(), 

                    // sb will strech the texture to fill this rectangle
                    1), 

                // width of line, change this to make thicker line
                null, 
                color, 

                // colour of line
                angle, 

                // angle of line (calulated above)
                new Vector2(0, 0), 

                // point in line about which to rotate
                SpriteEffects.None, 
                0);
        }
    }
}
#endif