using System;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Windows;

namespace PGZ_Desert_Battle
{
    class InputController : IDisposable
    {
        private DirectInput _directInput;

        private RenderForm _renderForm;
        public RenderForm RenderForm { get => _renderForm; }

        private Keyboard _keyboard;
        private KeyboardState _keyboardState;
        private bool _keyboardUpdated = false;
        public bool KeyboardUpdated { get => _keyboardUpdated; }
        private bool _keyboardAcquired;

        private Mouse _mouse;
        private MouseState _mouseState;
        private bool _mouseUpdated = false;
        public bool MouseUpdated { get => _mouseUpdated; }
        private bool _mouseAcquired;

        private bool[] _mouseButtons = new bool[8];
        public bool[] MouseButtons { get => _mouseButtons; }

        private int _mouseRelativePositionX;
        public int MouseRelativePositionX { get => _mouseRelativePositionX; }
        private int _mouseRelativePositionY;
        public int MouseRelativePositionY { get => _mouseRelativePositionY; }
        private int _mouseRelativePositionZ;
        public int MouseRelativePositionZ { get => _mouseRelativePositionZ; }

        private static Key[] _keyFuncCodes = new Key[10] {                          // __
            Key.F1, Key.F2, Key.F3, Key.F4, Key.F5, Key.F6, Key.F7, Key.F8, Key.F9, Key.F10         // __
        };                                                                          // __
        private bool[] _keyFuncPreviousPressed = new bool[10];                      // __
        private bool[] _keyFuncCurrentPressed = new bool[10];                       // __
        private bool[] _keyFunc = new bool[10];                                     // __
        public bool[] KeyFunc { get => _keyFunc; }                                  // __

        public bool KeyEsc { get => _keyboardState.IsPressed(Key.Escape); }
        public bool KeyW { get => _keyboardState.IsPressed(Key.W); }
        public bool KeyA { get => _keyboardState.IsPressed(Key.A); }
        public bool KeyS { get => _keyboardState.IsPressed(Key.S); }
        public bool KeyD { get => _keyboardState.IsPressed(Key.D); }

        private bool mousePressed;
        public bool MouseLeft
        {
            get
            {
                if (!mousePressed && _mouseButtons[0])
                {
                    mousePressed = true;
                    return true;
                }
                else if (mousePressed && !_mouseButtons[0])
                {
                    mousePressed = false;
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public InputController(RenderForm renderForm)
        {
            _directInput = new DirectInput();

            _keyboard = new Keyboard(_directInput);
            _keyboard.SetCooperativeLevel(renderForm.Handle,
                CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);      // __ NonExclusive. With excluzive Alt-F4 don't work.
            AcquireKeyboard();
            _keyboardState = new KeyboardState();

            _mouse = new Mouse(_directInput);
            _mouse.SetCooperativeLevel(renderForm.Handle,
                CooperativeLevel.Foreground | CooperativeLevel.Exclusive);      // __ NonExclusive (after add FXX frizes). Cursor hide with Excluzive.
            AcquireMouse();
            _mouseState = new MouseState();

            _renderForm = renderForm;
            mousePressed = false;
        }

        private void AcquireKeyboard()
        {
            try
            {
                _keyboard.Acquire();
                _keyboardAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    _keyboardAcquired = false;
            }
        }

        private void AcquireMouse()
        {
            try
            {
                _mouse.Acquire();
                _mouseAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    _mouseAcquired = false;
            }
        }

        private bool TriggerByKeyUp(Key key, ref bool previous, ref bool current)
        {
            previous = current;
            current = _keyboardState.IsPressed(key);
            return previous && !current;
        }

        private bool TriggerByKeyDown(Key key, ref bool previous, ref bool current)
        {
            previous = current;
            current = _keyboardState.IsPressed(key);
            return !previous && current;
        }

        private void ProcessKeyboardState()
        {
            for (int i = 0; i <= 9; ++i)                                                                                        // __
                _keyFunc[i] = TriggerByKeyUp(_keyFuncCodes[i], ref _keyFuncPreviousPressed[i], ref _keyFuncCurrentPressed[i]);  // __
        }

        public void UpdateKeyboardState()
        {
            if (!_keyboardAcquired) AcquireKeyboard();
            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                _keyboard.GetCurrentState(ref _keyboardState);
                ProcessKeyboardState();
                _keyboardUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                _keyboardUpdated = false;
            }
            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _keyboardAcquired = false;
        }

        private void ProcessMouseState()
        {
            for (int i = 0; i <= 7; ++i)
                _mouseButtons[i] = _mouseState.Buttons[i];
            _mouseRelativePositionX = _mouseState.X;
            _mouseRelativePositionY = _mouseState.Y;
            _mouseRelativePositionZ = _mouseState.Z;
        }

        public void UpdateMouseState()
        {
            if (!_mouseAcquired) AcquireMouse();
            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                _mouse.GetCurrentState(ref _mouseState);
                ProcessMouseState();
                _mouseUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                _mouseUpdated = false;
            }
            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _mouseAcquired = false;
        }

        public void Dispose()
        {
            _mouse.Unacquire();
            Utilities.Dispose(ref _mouse);
            _keyboard.Unacquire();
            Utilities.Dispose(ref _keyboard);
            Utilities.Dispose(ref _directInput);
        }
    }
}
