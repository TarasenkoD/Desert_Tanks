using System;
using SharpDX;
using SharpDX.Windows;

namespace PGZ_Desert_Battle
{

    class Game : IDisposable
    {
        private RenderForm renderForm;

        private Scene currentScene;

        private DirectX3DGraphics directX3DGraphics;
        private DirectX2DGraphics directX2DGraphics;
        private Renderer renderer;

        private bool _firstRun = true;

        private TimeHelper timeHelper;
        private InputController inputController;
        private SharpAudioDevice audioDevice;

        Loader loader;

        public Game()
        {
            renderForm = new RenderForm();
            directX3DGraphics = new DirectX3DGraphics(renderForm);
            directX2DGraphics = new DirectX2DGraphics(this, directX3DGraphics);
            renderer = new Renderer(directX3DGraphics);
            renderer.CreateConstantBuffers();
            inputController = new InputController(renderForm);

            loader = new Loader(directX3DGraphics);
            timeHelper = new TimeHelper();
            audioDevice = new SharpAudioDevice();

            SceneDirector director = new SceneDirector();
            MenuSceneBuilder builder = new MenuSceneBuilder();
            director.Builder = builder;

            director.BuildScene(this, renderer, loader, audioDevice, inputController, directX2DGraphics);
            currentScene = builder.GetResult();

            renderForm.UserResized += RenderFormResizedCallback;
        }

        public event EventHandler SwapChainResizing;
        public event EventHandler SwapChainResized;

        public void RenderFormResizedCallback(object sender, EventArgs args)
        {
            SwapChainResizing?.Invoke(this, null);
            directX3DGraphics.Resize();
            currentScene.Camera.Aspect = renderForm.ClientSize.Width /
                (float)renderForm.ClientSize.Height;
            SwapChainResized?.Invoke(this, null);
        }

        public void ChangeScene()
        {
            SceneDirector director = new SceneDirector();
            ISceneBuilder builder;
            if (currentScene is GameScene)
            {
                builder = new MenuSceneBuilder();
            }
            else
            {
                builder = new GameSceneBuilder();
            }
            director.Builder = builder;
            director.BuildScene(this, renderer, loader, audioDevice, inputController, directX2DGraphics);

            currentScene = builder.GetResult();
            RenderFormResizedCallback(this, null);
        }

        public void RenderLoopCallback()
        {
            if (_firstRun)
            {
                RenderFormResizedCallback(this, EventArgs.Empty);
                _firstRun = false;
            }

            timeHelper.Update();
            renderer.UpdatePerFrameConstantBuffers(timeHelper.Time);
            currentScene.Player?.Update(timeHelper.DeltaT);
            currentScene.Camera?.Update(timeHelper.DeltaT);
            currentScene.UpdateControllers(timeHelper.DeltaT);

            Matrix viewMatrix = currentScene.Camera.GetViewMatrix();
            Matrix projectionMatrix = currentScene.Camera.GetPojectionMatrix();

            renderer.BeginRender();

            for (int i = 0; i < currentScene.Meshes.Count; i++)
            {
                MeshObject mesh = currentScene.Meshes[i];
                renderer.UpdatePerObjectConstantBuffers(
                    mesh.GetWorldMatrix(), viewMatrix, projectionMatrix, 0);
                renderer.RenderMeshObject(mesh);
                mesh.Update(timeHelper.DeltaT);
            }

            currentScene.UserInterface.Render();

            renderer.EndRender();
        }

        public void Run()
        {
            RenderLoop.Run(renderForm, RenderLoopCallback);
        }

        public void Dispose()
        {
            inputController.Dispose();
            currentScene.Dispose();
            renderer.Dispose();
            directX3DGraphics.Dispose();
        }
    }
}
