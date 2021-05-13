using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class LightController : AController
    {
        Camera camera;
        Renderer renderer;
        Renderer.IlluminationProperties illumination;

        public LightController(Renderer renderer, Scene scene, Game game)
            : base(scene, game)
        {
            camera = scene.Camera;
            this.renderer = renderer;

            illumination = new Renderer.IlluminationProperties();
            Renderer.LightSource lightSource = new Renderer.LightSource();
            illumination.globalAmbient = new Vector3(0.02f);
            lightSource.lightSourceType = Renderer.LightSourceType.Base;
            lightSource.constantAttenuation = 0.01f;
            lightSource.color = Vector3.Zero;
            for (int i = 0; i < Renderer.MaxLights; i++)
                illumination[i] = lightSource;

            illumination[0] = GenerateDefaultLight();
        }

        public override void Update(float delta)
        {
            illumination.eyePosition = (Vector4)camera.Position;
            renderer.UpdateIllumination(illumination);
        }

        public void ChangeMainLight(Renderer.LightSource lightSource)
        {
            illumination[0] = lightSource;
        }

        public Renderer.LightSource GenerateDefaultLight()
        {
            Renderer.LightSource lightSource = new Renderer.LightSource();
            lightSource.lightSourceType = Renderer.LightSourceType.Directional;
            lightSource.constantAttenuation = 0.01f;
            lightSource.color = new Vector3(0.99f, 0.84f, 0.69f);
            lightSource.direction = Vector3.Normalize(new Vector3(0.5f, -2.0f, 1.0f));

            return lightSource;
        }
    }
}
