using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class TurretEnemy : AEnemy
    {
        public TurretEnemy(Vector3 position, Vector3 rotation, float detectionRadius)
            : base(position, rotation, detectionRadius)
        {
            shootInterval = 0.8f;
            shootHeight = 1.8f;
        }
    }
}
