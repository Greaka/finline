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
        private readonly Model[] animationList;
        private byte index;

        private readonly Timer timer = new Timer();
        
        public Animation(int anzahl)
        {
            this.animationList = new Model[anzahl];
            this.timer.Interval = 0.1f;
            this.timer.Enabled = false;
            this.timer.Elapsed += (sender, args) =>
                {
                    if (this.index > this.animationList.Length)
                    {
                        this.index = 0;
                    }
                    else
                    {
                        ++this.index;
                    }
            };
        }

        public void Add(Model model)
        {
            this.animationList[this.index] = model;
            ++this.index;
            if (this.animationList.Length <= this.index) this.timer.Enabled = true;
        }

        public Model CurrentModel => this.animationList[this.index];
    }
}
