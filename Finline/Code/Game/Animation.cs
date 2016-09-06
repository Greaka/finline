using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finline.Code.Game
{
    using System.Timers;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Animation
    {
        private readonly List<Model> animationList;
        private byte index;

        private readonly Timer timer = new Timer();

        public Animation(byte index)
        {
            this.index = index;
            this.animationList = new List<Model>();
            this.timer.Interval = 0.1f;
            this.timer.Enabled = true;
            this.timer.Elapsed += (sender, args) =>
                {
                    ++index;
                };
        }

        public Animation(int anzahl)
        {
            this.animationList = new List<Model>(anzahl);
        }

        public void Add(Model model)
        {
            this.animationList.Add(model);
        }

        public Model CurrentModel => this.animationList[this.index];
    }
}
