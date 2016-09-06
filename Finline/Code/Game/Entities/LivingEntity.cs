using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finline.Code.Game.Entities
{
    using Microsoft.Xna.Framework;

    public class LivingEntity : Entity
    {
        protected Animation modelAnimation;

        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            base.Draw(viewMatrix, projectionMatrix, this.modelAnimation.CurrentModel);
        }
    }
}
