using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using Finline.Code.Game.Entities;
using Finline.Code.Game.Helper;
using Microsoft.Xna.Framework.Content;

namespace Finline.Code.Game.Controls
{
    public class Shooting
    {
        private readonly Stopwatch aStopwatch = new Stopwatch();
        private readonly ContentManager content;
        private ConcurrentBag<int> indices = new ConcurrentBag<int>();
        public Shooting(ContentManager shiny)
        {
            for (var i = 0; i < 1000; i++)
                indices.Add(i);

            content = shiny;
            aStopwatch.Restart();
        }

        private void AddIndex(int index)
        {
            Projectile projectile;
            if (ControlsHelper.Projectiles.TryRemove(index, out projectile))
                indices.Add(index);
        }

        public void Shoot()
        {
            int index;
            indices.TryTake(out index);
            var projectile = new Projectile(aStopwatch.Elapsed, content, index);
            projectile.Destruct += AddIndex;
            ControlsHelper.Projectiles.TryAdd(index, projectile);
        }

        public void Update()
        {
            while (ControlsHelper.Active)
            {
                foreach (var outch in ControlsHelper.Projectiles.Values)
                {
                    outch.Update(aStopwatch.Elapsed);
                }
            }
        }
    }
}
