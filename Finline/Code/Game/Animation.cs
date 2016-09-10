namespace Finline.Code.Game
{
    using System.Timers;

    using Microsoft.Xna.Framework.Graphics;

    public class Animation
    {
        private readonly Model[] animationList;
        private byte index;

        public bool Active;

        private readonly Timer timer = new Timer();
        
        public Animation(int anzahl, bool aktiv = true)
        {
            this.Active = aktiv;
            this.animationList = new Model[anzahl];
            this.timer.Interval = 100f;
            this.timer.Enabled = false;
            this.timer.Elapsed += (sender, args) =>
                {
                    if (!this.Active) return;
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

        public bool LastModel => this.index == this.animationList.Length - 1;
    }
}
