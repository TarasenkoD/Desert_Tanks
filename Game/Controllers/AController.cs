using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    abstract class AController
    {
        protected Scene scene;
        protected Game game;

        public AController(Scene scene, Game game)
        {
            this.scene = scene;
            this.game = game;
        }

        public abstract void Update(float delta);
    }
}
