using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finline.Code.Game
{
    using System.Runtime.CompilerServices;
    using System.Timers;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Animation
    {
        private Model[] animationList;
        private byte index;

        public bool active;

        private readonly Timer timer = new Timer();
        
        public Animation(int anzahl, bool aktiv = true)
        {
            this.active = aktiv;
            this.animationList = new Model[anzahl];
            this.timer.Interval = 100f;
            this.timer.Enabled = false;
            this.timer.Elapsed += (sender, args) =>
                {
                    if (!this.active) return;
                    if (this.index == this.animationList.Length - 1)
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
            if (this.animationList.Length == this.index)
            {
                this.index = 0;
                this.timer.Enabled = true;
            }
        }

        public Model CurrentModel => this.animationList[this.index];

        public bool LastModel => this.index == this.animationList.Length;
    }
}
