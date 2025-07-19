#region File Description
//-----------------------------------------------------------------------------
// InputManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace AzureAcres
{
    /// <summary>
    /// This class handles all keyboard and gamepad actions in the game.
    /// </summary>
    public static class InputManager
    {
        #region Action Enumeration


        /// <summary>
        /// The actions that are possible within the game.
        /// </summary>
        public enum Action
        {
            MainMenu,
            Ok,
            Back,
            CharacterManagement,
            ExitGame,
            TakeView,
            DropUnEquip,
            MoveCharacterUp,
            MoveCharacterDown,
            MoveCharacterLeft,
            MoveCharacterRight,
            CursorUp,
            CursorDown,
            DecreaseAmount,
            IncreaseAmount,
            PageLeft,
            PageRight,
            TargetUp,
            TargetDown,
            ActiveCharacterLeft,
            ActiveCharacterRight,
            TotalActionCount
    }


        /// <summary>
        /// Readable names of each action.
        /// </summary>
        private static readonly string[] actionNames = 
            {
                "Main Menu",
                "Ok",
                "Back",
                "Character Management",
                "Exit Game",
                "Take / View",
                "Drop / Unequip",
                "Move Character - Up",
                "Move Character - Down",
                "Move Character - Left",
                "Move Character - Right",
                "Move Cursor - Up",
                "Move Cursor - Down",
                "Decrease Amount",
                "Increase Amount",
                "Page Screen Left",
                "Page Screen Right",
                "Select Target -Up",
                "Select Target - Down",
                "Select Active Character - Left",
                "Select Active Character - Right",
            };

        /// <summary>
        /// Returns the readable name of the given action.
        /// </summary>
        public static string GetActionName(Action action)
        {
            int index = (int)action;

            if ((index < 0) || (index > actionNames.Length))
            {
                throw new ArgumentException("action");
            }

            return actionNames[index];
        }


        #endregion


        #region Support Types


        /// <summary>
        /// GamePad controls expressed as one type, unified with button semantics.
        /// </summary>
        public enum GamePadButtons
        {
            Start,
            Back,
            A,
            B,
            X,
            Y,
            Up,
            Down,
            Left,
            Right,
            LeftShoulder,
            RightShoulder,
            LeftTrigger,
            RightTrigger,
        }


        /// <summary>
        /// A combination of gamepad and keyboard keys mapped to a particular action.
        /// </summary>
        public class ActionMap
        {
            /// <summary>
            /// List of GamePad controls to be mapped to a given action.
            /// </summary>
            public List<GamePadButtons> gamePadButtons = new List<GamePadButtons>();


            /// <summary>
            /// List of Keyboard controls to be mapped to a given action.
            /// </summary>
            public List<Keys> keyboardKeys = new List<Keys>();
        }


        #endregion


        #region Constants


        /// <summary>
        /// The value of an analog control that reads as a "pressed button".
        /// </summary>
        const float analogLimit = 0.5f;


        #endregion


        #region Keyboard Data


        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        private static KeyboardState currentKeyboardState;

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        public static KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
        }

        /// <summary>
        /// The state of the mouse as of the last update.
        /// </summary>
        private static MouseState currentMouseState;

        /// <summary>
        /// The state of the mouse as of the last update.
        /// </summary>
        public static MouseState CurrentMouseState
        {
            get { return currentMouseState; }
        }


        /// <summary>
        /// The state of the keyboard as of the previous update.
        /// </summary>
        private static KeyboardState previousKeyboardState;

        /// <summary>
        /// The state of the mouse as of the previous update.
        /// </summary>
        private static MouseState previousMouseState;

        // Состояние указателя (мышь/сенсорный экран)
        private static Point pointerPosition;
        private static Point previousPointerPosition;
        private static bool isPointerPressed;
        private static bool wasPointerPressed;
        private static bool isInteractionRequested;
        private static bool wasInteractionRequested;
        private static bool isInventoryRequested;
        private static bool wasInventoryRequested;
        private static Vector2 pointerMovement;
        private static Vector2 previousPointerMovement;


        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }


        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        public static bool IsKeyTriggered(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key)) &&
                (!previousKeyboardState.IsKeyDown(key));
        }


        #endregion


        #region GamePad Data


        /// <summary>
        /// The state of the gamepad as of the last update.
        /// </summary>
        private static GamePadState currentGamePadState;

        /// <summary>
        /// The state of the gamepad as of the last update.
        /// </summary>
        public static GamePadState CurrentGamePadState
        {
            get { return currentGamePadState; }
        }


        /// <summary>
        /// The state of the gamepad as of the previous update.
        /// </summary>
        private static GamePadState previousGamePadState;


        #region GamePadButton Pressed Queries


        /// <summary>
        /// Check if the gamepad's Start button is pressed.
        /// </summary>
        public static bool IsGamePadStartPressed()
        {
            return (currentGamePadState.Buttons.Start == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's Back button is pressed.
        /// </summary>
        public static bool IsGamePadBackPressed()
        {
            return (currentGamePadState.Buttons.Back == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's A button is pressed.
        /// </summary>
        public static bool IsGamePadAPressed()
        {
            return (currentGamePadState.Buttons.A == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's B button is pressed.
        /// </summary>
        public static bool IsGamePadBPressed()
        {
            return (currentGamePadState.Buttons.B == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's X button is pressed.
        /// </summary>
        public static bool IsGamePadXPressed()
        {
            return (currentGamePadState.Buttons.X == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's Y button is pressed.
        /// </summary>
        public static bool IsGamePadYPressed()
        {
            return (currentGamePadState.Buttons.Y == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's LeftShoulder button is pressed.
        /// </summary>
        public static bool IsGamePadLeftShoulderPressed()
        {
            return (currentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed);
        }


        /// <summary>
        /// <summary>
        /// Check if the gamepad's RightShoulder button is pressed.
        /// </summary>
        public static bool IsGamePadRightShoulderPressed()
        {
            return (currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Up on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadUpPressed()
        {
            return (currentGamePadState.DPad.Up == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Down on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadDownPressed()
        {
            return (currentGamePadState.DPad.Down == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Left on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadLeftPressed()
        {
            return (currentGamePadState.DPad.Left == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Right on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadRightPressed()
        {
            return (currentGamePadState.DPad.Right == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's left trigger is pressed.
        /// </summary>
        public static bool IsGamePadLeftTriggerPressed()
        {
            return (currentGamePadState.Triggers.Left > analogLimit);
        }


        /// <summary>
        /// Check if the gamepad's right trigger is pressed.
        /// </summary>
        public static bool IsGamePadRightTriggerPressed()
        {
            return (currentGamePadState.Triggers.Right > analogLimit);
        }


        /// <summary>
        /// Check if Up on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickUpPressed()
        {
            return (currentGamePadState.ThumbSticks.Left.Y > analogLimit);
        }


        /// <summary>
        /// Check if Down on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickDownPressed()
        {
            return (-1f * currentGamePadState.ThumbSticks.Left.Y > analogLimit);
        }


        /// <summary>
        /// Check if Left on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickLeftPressed()
        {
            return (-1f * currentGamePadState.ThumbSticks.Left.X > analogLimit);
        }


        /// <summary>
        /// Check if Right on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickRightPressed()
        {
            return (currentGamePadState.ThumbSticks.Left.X > analogLimit);
        }


        /// <summary>
        /// Check if the GamePadKey value specified is pressed.
        /// </summary>
        private static bool IsGamePadButtonPressed(GamePadButtons gamePadKey)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartPressed();

                case GamePadButtons.Back:
                    return IsGamePadBackPressed();

                case GamePadButtons.A:
                    return IsGamePadAPressed();

                case GamePadButtons.B:
                    return IsGamePadBPressed();

                case GamePadButtons.X:
                    return IsGamePadXPressed();

                case GamePadButtons.Y:
                    return IsGamePadYPressed();

                case GamePadButtons.LeftShoulder:
                    return IsGamePadLeftShoulderPressed();

                case GamePadButtons.RightShoulder:
                    return IsGamePadRightShoulderPressed();

                case GamePadButtons.LeftTrigger:
                    return IsGamePadLeftTriggerPressed();

                case GamePadButtons.RightTrigger:
                    return IsGamePadRightTriggerPressed();

                case GamePadButtons.Up:
                    return IsGamePadDPadUpPressed() ||
                        IsGamePadLeftStickUpPressed();

                case GamePadButtons.Down:
                    return IsGamePadDPadDownPressed() ||
                        IsGamePadLeftStickDownPressed();

                case GamePadButtons.Left:
                    return IsGamePadDPadLeftPressed() ||
                        IsGamePadLeftStickLeftPressed();

                case GamePadButtons.Right:
                    return IsGamePadDPadRightPressed() ||
                        IsGamePadLeftStickRightPressed();
            }

            return false;
        }


        #endregion


        #region GamePadButton Triggered Queries


        /// <summary>
        /// Check if the gamepad's Start button was just pressed.
        /// </summary>
        public static bool IsGamePadStartTriggered()
        {
            return ((currentGamePadState.Buttons.Start == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.Start == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Back button was just pressed.
        /// </summary>
        public static bool IsGamePadBackTriggered()
        {
            return ((currentGamePadState.Buttons.Back == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.Back == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's A button was just pressed.
        /// </summary>
        public static bool IsGamePadATriggered()
        {
            return ((currentGamePadState.Buttons.A == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.A == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's B button was just pressed.
        /// </summary>
        public static bool IsGamePadBTriggered()
        {
            return ((currentGamePadState.Buttons.B == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.B == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's X button was just pressed.
        /// </summary>
        public static bool IsGamePadXTriggered()
        {
            return ((currentGamePadState.Buttons.X == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.X == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Y button was just pressed.
        /// </summary>
        public static bool IsGamePadYTriggered()
        {
            return ((currentGamePadState.Buttons.Y == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.Y == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's LeftShoulder button was just pressed.
        /// </summary>
        public static bool IsGamePadLeftShoulderTriggered()
        {
            return (
                (currentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed) &&
                (previousGamePadState.Buttons.LeftShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's RightShoulder button was just pressed.
        /// </summary>
        public static bool IsGamePadRightShoulderTriggered()
        {
            return (
                (currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed) &&
                (previousGamePadState.Buttons.RightShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if Up on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadUpTriggered()
        {
            return ((currentGamePadState.DPad.Up == ButtonState.Pressed) &&
              (previousGamePadState.DPad.Up == ButtonState.Released));
        }


        /// <summary>
        /// Check if Down on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadDownTriggered()
        {
            return ((currentGamePadState.DPad.Down == ButtonState.Pressed) &&
              (previousGamePadState.DPad.Down == ButtonState.Released));
        }


        /// <summary>
        /// Check if Left on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadLeftTriggered()
        {
            return ((currentGamePadState.DPad.Left == ButtonState.Pressed) &&
              (previousGamePadState.DPad.Left == ButtonState.Released));
        }


        /// <summary>
        /// Check if Right on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadRightTriggered()
        {
            return ((currentGamePadState.DPad.Right == ButtonState.Pressed) &&
              (previousGamePadState.DPad.Right == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's left trigger was just pressed.
        /// </summary>
        public static bool IsGamePadLeftTriggerTriggered()
        {
            return ((currentGamePadState.Triggers.Left > analogLimit) &&
                (previousGamePadState.Triggers.Left < analogLimit));
        }


        /// <summary>
        /// Check if the gamepad's right trigger was just pressed.
        /// </summary>
        public static bool IsGamePadRightTriggerTriggered()
        {
            return ((currentGamePadState.Triggers.Right > analogLimit) &&
                (previousGamePadState.Triggers.Right < analogLimit));
        }


        /// <summary>
        /// Check if Up on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickUpTriggered()
        {
            return ((currentGamePadState.ThumbSticks.Left.Y > analogLimit) &&
                (previousGamePadState.ThumbSticks.Left.Y < analogLimit));
        }


        /// <summary>
        /// Check if Down on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickDownTriggered()
        {
            return ((-1f * currentGamePadState.ThumbSticks.Left.Y > analogLimit) &&
                (-1f * previousGamePadState.ThumbSticks.Left.Y < analogLimit));
        }


        /// <summary>
        /// Check if Left on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickLeftTriggered()
        {
            return ((-1f * currentGamePadState.ThumbSticks.Left.X > analogLimit) &&
                (-1f * previousGamePadState.ThumbSticks.Left.X < analogLimit));
        }


        /// <summary>
        /// Check if Right on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickRightTriggered()
        {
            return ((currentGamePadState.ThumbSticks.Left.X > analogLimit) &&
                (previousGamePadState.ThumbSticks.Left.X < analogLimit));
        }


        /// <summary>
        /// Check if the GamePadKey value specified was pressed this frame.
        /// </summary>
        private static bool IsGamePadButtonTriggered(GamePadButtons gamePadKey)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartTriggered();

                case GamePadButtons.Back:
                    return IsGamePadBackTriggered();

                case GamePadButtons.A:
                    return IsGamePadATriggered();

                case GamePadButtons.B:
                    return IsGamePadBTriggered();

                case GamePadButtons.X:
                    return IsGamePadXTriggered();

                case GamePadButtons.Y:
                    return IsGamePadYTriggered();

                case GamePadButtons.LeftShoulder:
                    return IsGamePadLeftShoulderTriggered();

                case GamePadButtons.RightShoulder:
                    return IsGamePadRightShoulderTriggered();

                case GamePadButtons.LeftTrigger:
                    return IsGamePadLeftTriggerTriggered();

                case GamePadButtons.RightTrigger:
                    return IsGamePadRightTriggerTriggered();

                case GamePadButtons.Up:
                    return IsGamePadDPadUpTriggered() ||
                        IsGamePadLeftStickUpTriggered();

                case GamePadButtons.Down:
                    return IsGamePadDPadDownTriggered() ||
                        IsGamePadLeftStickDownTriggered();

                case GamePadButtons.Left:
                    return IsGamePadDPadLeftTriggered() ||
                        IsGamePadLeftStickLeftTriggered();

                case GamePadButtons.Right:
                    return IsGamePadDPadRightTriggered() ||
                        IsGamePadLeftStickRightTriggered();
            }

            return false;
        }


        #endregion


        #endregion


        #region Action Mapping


        /// <summary>
        /// The action mappings for the game.
        /// </summary>
        private static ActionMap[] actionMaps;


        public static ActionMap[] ActionMaps
        {
            get { return actionMaps; }
        }


        /// <summary>
        /// Reset the action maps to their default values.
        /// </summary>
        private static void ResetActionMaps()
        {
            actionMaps = new ActionMap[(int)Action.TotalActionCount];

            actionMaps[(int)Action.MainMenu] = new ActionMap();
            actionMaps[(int)Action.MainMenu].keyboardKeys.Add(
                Keys.Tab);
            actionMaps[(int)Action.MainMenu].gamePadButtons.Add(
                GamePadButtons.Start);

            actionMaps[(int)Action.Ok] = new ActionMap();
            actionMaps[(int)Action.Ok].keyboardKeys.Add(
                Keys.E);
            actionMaps[(int)Action.Ok].gamePadButtons.Add(
                GamePadButtons.A);

            actionMaps[(int)Action.Back] = new ActionMap();
            actionMaps[(int)Action.Back].keyboardKeys.Add(
                Keys.Escape);
            actionMaps[(int)Action.Back].gamePadButtons.Add(
                GamePadButtons.B);

            actionMaps[(int)Action.CharacterManagement] = new ActionMap();
            actionMaps[(int)Action.CharacterManagement].keyboardKeys.Add(
                Keys.Space);
            actionMaps[(int)Action.CharacterManagement].gamePadButtons.Add(
                GamePadButtons.Y);

            actionMaps[(int)Action.ExitGame] = new ActionMap();
            actionMaps[(int)Action.ExitGame].keyboardKeys.Add(
                Keys.Escape);
            actionMaps[(int)Action.ExitGame].gamePadButtons.Add(
                GamePadButtons.Back);

            actionMaps[(int)Action.TakeView] = new ActionMap();
            actionMaps[(int)Action.TakeView].keyboardKeys.Add(
                Keys.LeftControl);
            actionMaps[(int)Action.TakeView].gamePadButtons.Add(
                GamePadButtons.Y);

            actionMaps[(int)Action.DropUnEquip] = new ActionMap();
            actionMaps[(int)Action.DropUnEquip].keyboardKeys.Add(
                Keys.D);
            actionMaps[(int)Action.DropUnEquip].gamePadButtons.Add(
                GamePadButtons.X);

            actionMaps[(int)Action.MoveCharacterUp] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterUp].keyboardKeys.Add(
                Keys.W);
            actionMaps[(int)Action.MoveCharacterUp].gamePadButtons.Add(
                GamePadButtons.Up);

            actionMaps[(int)Action.MoveCharacterDown] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterDown].keyboardKeys.Add(
                Keys.S);
            actionMaps[(int)Action.MoveCharacterDown].gamePadButtons.Add(
                GamePadButtons.Down);

            actionMaps[(int)Action.MoveCharacterLeft] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterLeft].keyboardKeys.Add(
                Keys.A);
            actionMaps[(int)Action.MoveCharacterLeft].gamePadButtons.Add(
                GamePadButtons.Left);

            actionMaps[(int)Action.MoveCharacterRight] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterRight].keyboardKeys.Add(
                Keys.D);
            actionMaps[(int)Action.MoveCharacterRight].gamePadButtons.Add(
                GamePadButtons.Right);

            actionMaps[(int)Action.CursorUp] = new ActionMap();
            actionMaps[(int)Action.CursorUp].keyboardKeys.Add(
                Keys.Up);
            actionMaps[(int)Action.CursorUp].gamePadButtons.Add(
                GamePadButtons.Up);

            actionMaps[(int)Action.CursorDown] = new ActionMap();
            actionMaps[(int)Action.CursorDown].keyboardKeys.Add(
                Keys.Down);
            actionMaps[(int)Action.CursorDown].gamePadButtons.Add(
                GamePadButtons.Down);

            actionMaps[(int)Action.DecreaseAmount] = new ActionMap();
            actionMaps[(int)Action.DecreaseAmount].keyboardKeys.Add(
                Keys.Left);
            actionMaps[(int)Action.DecreaseAmount].gamePadButtons.Add(
                GamePadButtons.Left);

            actionMaps[(int)Action.IncreaseAmount] = new ActionMap();
            actionMaps[(int)Action.IncreaseAmount].keyboardKeys.Add(
                Keys.Right);
            actionMaps[(int)Action.IncreaseAmount].gamePadButtons.Add(
                GamePadButtons.Right);

            actionMaps[(int)Action.PageLeft] = new ActionMap();
            actionMaps[(int)Action.PageLeft].keyboardKeys.Add(
                Keys.LeftShift);
            actionMaps[(int)Action.PageLeft].gamePadButtons.Add(
                GamePadButtons.LeftTrigger);

            actionMaps[(int)Action.PageRight] = new ActionMap();
            actionMaps[(int)Action.PageRight].keyboardKeys.Add(
                Keys.RightShift);
            actionMaps[(int)Action.PageRight].gamePadButtons.Add(
                GamePadButtons.RightTrigger);

            actionMaps[(int)Action.TargetUp] = new ActionMap();
            actionMaps[(int)Action.TargetUp].keyboardKeys.Add(
                Keys.Up);
            actionMaps[(int)Action.TargetUp].gamePadButtons.Add(
                GamePadButtons.Up);

            actionMaps[(int)Action.TargetDown] = new ActionMap();
            actionMaps[(int)Action.TargetDown].keyboardKeys.Add(
                Keys.Down);
            actionMaps[(int)Action.TargetDown].gamePadButtons.Add(
                GamePadButtons.Down);

            actionMaps[(int)Action.ActiveCharacterLeft] = new ActionMap();
            actionMaps[(int)Action.ActiveCharacterLeft].keyboardKeys.Add(
                Keys.Left);
            actionMaps[(int)Action.ActiveCharacterLeft].gamePadButtons.Add(
                GamePadButtons.Left);

            actionMaps[(int)Action.ActiveCharacterRight] = new ActionMap();
            actionMaps[(int)Action.ActiveCharacterRight].keyboardKeys.Add(
                Keys.Right);
            actionMaps[(int)Action.ActiveCharacterRight].gamePadButtons.Add(
                GamePadButtons.Right);
        }


        /// <summary>
        /// Check if an action has been pressed.
        /// </summary>
        public static bool IsActionPressed(Action action)
        {
            return IsActionMapPressed(actionMaps[(int)action]);
        }


        /// <summary>
        /// Check if an action was just performed in the most recent update.
        /// </summary>
        public static bool IsActionTriggered(Action action)
        {
            return IsActionMapTriggered(actionMaps[(int)action]);
        }


        /// <summary>
        /// Check if an action map has been pressed.
        /// </summary>
        private static bool IsActionMapPressed(ActionMap actionMap)
        {
            // Проверка клавиатуры
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
            {
                if (IsKeyPressed(actionMap.keyboardKeys[i]))
                {
                    return true;
                }
            }
            
            // Проверка геймпада
            if (currentGamePadState.IsConnected)
            {
                for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonPressed(actionMap.gamePadButtons[i]))
                    {
                        return true;
                    }
                }
            }
            
            // Проверка мыши/сенсорного экрана для определенных действий
            Action action = Action.MainMenu;
            for (int i = 0; i < (int)Action.TotalActionCount; i++)
            {
                if (actionMaps[i] == actionMap)
                {
                    action = (Action)i;
                    break;
                }
            }
            
            // Проверяем ввод с мыши/сенсорного экрана
            // Проверяем движение персонажа (не требует нажатия кнопки)
            if (action == Action.MoveCharacterLeft && pointerMovement.X < -5)
            {
                
                System.Diagnostics.Debug.WriteLine($"Движение влево: pointerMovement.X={pointerMovement.X}");
                return true;
            }
            if (action == Action.MoveCharacterRight && pointerMovement.X > 5)
            {
                System.Diagnostics.Debug.WriteLine($"Движение вправо: pointerMovement.X={pointerMovement.X}");
                return true;
            }
            if (action == Action.MoveCharacterUp && pointerMovement.Y < -5)
            {
                action = Action.MoveCharacterUp;
                System.Diagnostics.Debug.WriteLine($"Движение вверх: pointerMovement.Y={pointerMovement.Y}");
                return true;
            }
            if (action == Action.MoveCharacterDown && pointerMovement.Y > 5)
            {
                System.Diagnostics.Debug.WriteLine($"Движение вниз: pointerMovement.Y={pointerMovement.Y}");
                return true;
            }
            
            if (isPointerPressed)
            {
                // Проверяем взаимодействие
                if (action == Action.Ok && isInteractionRequested)
                {
                    System.Diagnostics.Debug.WriteLine("Действие Ok pressed");
                    return true;
                }
                
                // Проверяем вызов инвентаря
                if (action == Action.CharacterManagement && isInventoryRequested)
                {
                    System.Diagnostics.Debug.WriteLine("Действие CharacterManagement pressed");
                    return true;
                }
                
                // Проверяем вызов главного меню
                if (action == Action.MainMenu && isInventoryRequested)
                {
                    System.Diagnostics.Debug.WriteLine("Действие MainMenu pressed");
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Check if an action map has been triggered this frame.
        /// </summary>
        private static bool IsActionMapTriggered(ActionMap actionMap)
        {
            // Проверка клавиатуры
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
            {
                if (IsKeyTriggered(actionMap.keyboardKeys[i]))
                {
                    return true;
                }
            }
            
            // Проверка геймпада
            if (currentGamePadState.IsConnected)
            {
                for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonTriggered(actionMap.gamePadButtons[i]))
                    {
                        return true;
                    }
                }
            }
            
            // Проверка мыши/сенсорного экрана для определенных действий
            Action action = Action.MainMenu;
            for (int i = 0; i < (int)Action.TotalActionCount; i++)
            {
                if (actionMaps[i] == actionMap)
                {
                    action = (Action)i;
                    break;
                }
            }
            
            // Проверяем ввод с мыши/сенсорного экрана
            if (action == Action.Ok && isInteractionRequested && isPointerPressed)
            {
                System.Diagnostics.Debug.WriteLine($"Действие Ok триггер: isInteractionRequested={isInteractionRequested}, wasInteractionRequested={wasInteractionRequested}, isPointerPressed={isPointerPressed}");
                return true;
            }
            
            if (action == Action.CharacterManagement && isInventoryRequested && isPointerPressed)
            {
                System.Diagnostics.Debug.WriteLine($"Действие CharacterManagement триггер: isInventoryRequested={isInventoryRequested}, wasInventoryRequested={wasInventoryRequested}, isPointerPressed={isPointerPressed}");
                return true;
            }
            
            // Проверяем триггеры для действий MainMenu
            if (action == Action.MainMenu && isInventoryRequested && isPointerPressed)
            {
                System.Diagnostics.Debug.WriteLine($"Действие MainMenu триггер: isInventoryRequested={isInventoryRequested}, isPointerPressed={isPointerPressed}");
                return true;
            }
            
            return false;
        }


        #endregion


        #region Initialization


        /// <summary>
        /// Initializes the default control keys for all actions.
        /// </summary>
        public static void Initialize()
        {
            ResetActionMaps();
        }


        #endregion


        #region Updating


        /// <summary>
        /// Updates the keyboard and gamepad control states.
        /// </summary>
        public static void Update()
        {
            // update game input state
            previousKeyboardState = currentKeyboardState;
            previousGamePadState = currentGamePadState;
            previousMouseState = currentMouseState;

            previousPointerPosition = pointerPosition;
            wasPointerPressed = isPointerPressed;
            wasInteractionRequested = isInteractionRequested;
            wasInventoryRequested = isInventoryRequested;
            previousPointerMovement = pointerMovement;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            currentMouseState = Mouse.GetState();
            
            // Сбрасываем запросы взаимодействия и инвентаря после обработки
            isInteractionRequested = false;
            isInventoryRequested = false;

            // Сбрасываем движение указателя после каждого кадра, чтобы персонаж не продолжал двигаться
            // Это заставит GamePage.xaml.cs устанавливать новое значение при каждом движении указателя
            //pointerMovement = Vector2.Zero;
            pointerMovement = 
                new Vector2(
                     currentMouseState.Position.X - previousMouseState.Position.X, 
                     currentMouseState.Position.Y - previousMouseState.Position.Y
                   );

            // Выводим отладочную информацию
            if (isPointerPressed)
              System.Diagnostics.Debug.WriteLine($"Update: isPointerPressed={isPointerPressed}, pointerMovement={pointerMovement}");
        }


        #endregion
        
        #region Pointer Input Methods
        
        // Методы для установки состояния указателя (мышь/сенсорный экран)
        public static void SetPointerPosition(Point position)
        {
            pointerPosition = position;
        }
        
        public static void SetPointerPressed(bool pressed)
        {
            isPointerPressed = pressed;
        }
        
        public static void SetInteractionRequested(bool requested)
        {
            isInteractionRequested = requested;
        }
        
        public static void SetInventoryRequested(bool requested)
        {
            isInventoryRequested = requested;
        }
        
        public static void SetPointerMovement(Vector2 movement)
        {
            pointerMovement = movement;
        }
        
        // Методы для получения состояния указателя
        public static Point GetPointerPosition()
        {
            return pointerPosition;
        }
        
        public static bool IsPointerPressed()
        {
            return isPointerPressed;
        }
        
        public static bool IsPointerTriggered()
        {
            return isPointerPressed && !wasPointerPressed;
        }
        
        public static Vector2 GetPointerMovement()
        {
            return pointerMovement;
        }
        
        public static bool GetInteractionRequested()
        {
            return isInteractionRequested;
        }
        
        public static bool GetInventoryRequested()
        {
            return isInventoryRequested;
        }
        
        #endregion
    }
}
