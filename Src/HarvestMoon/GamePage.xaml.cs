using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AzureAcres;
using Microsoft.Xna.Framework;
using MonoGame.Framework;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml.Markup;
using Windows.UI.Input;

namespace AzureAcres
{
  public sealed partial class GamePage : Page
  {
        readonly AzureAcres _game;
        private Point _lastTouchPosition;
        private bool _isTouching;

        public GamePage()
        {
            this.InitializeComponent();

            // Create the game.
            var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<AzureAcres>.Create(
                launchArguments,
                Window.Current.CoreWindow,
                swapChainPanel);

            // Регистрируем обработчики событий мыши
            swapChainPanel.PointerPressed += SwapChainPanel_PointerPressed;
            swapChainPanel.PointerReleased += SwapChainPanel_PointerReleased;
            swapChainPanel.PointerMoved += SwapChainPanel_PointerMoved;
        }

        private void SwapChainPanel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // Получаем текущую позицию указателя
            PointerPoint pointerPoint = e.GetCurrentPoint(swapChainPanel);
            _lastTouchPosition = new Point((int)pointerPoint.Position.X, (int)pointerPoint.Position.Y);
            _isTouching = true;

            // Передаем информацию в InputManager
            InputManager.SetPointerPosition(_lastTouchPosition);
            InputManager.SetPointerPressed(true);

            // Если это правая кнопка мыши или долгое нажатие на сенсорном экране, вызываем инвентарь
            if (pointerPoint.Properties.IsRightButtonPressed)
            {
                InputManager.SetInventoryRequested(true);
                Debug.WriteLine("Запрошен инвентарь (правая кнопка мыши)");
                Debug.WriteLine($"isInventoryRequested: {InputManager.GetInventoryRequested()}");
            }
            else
            {
                // Левая кнопка мыши или обычное касание - взаимодействие
                InputManager.SetInteractionRequested(true);
                Debug.WriteLine("Запрошено взаимодействие (левая кнопка мыши)");
                Debug.WriteLine($"isInteractionRequested: {InputManager.GetInteractionRequested()}");
            }

            e.Handled = true;
        }

        private void SwapChainPanel_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _isTouching = false;
            InputManager.SetPointerPressed(false);
            InputManager.SetInteractionRequested(false);
            InputManager.SetInventoryRequested(false);
            e.Handled = true;
        }

        private void SwapChainPanel_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_isTouching)
            {
                PointerPoint pointerPoint = e.GetCurrentPoint(swapChainPanel);
                Point currentPosition = new Point((int)pointerPoint.Position.X, (int)pointerPoint.Position.Y);
                
                // Передаем новую позицию в InputManager
                InputManager.SetPointerPosition(currentPosition);
                
                // Рассчитываем направление движения
                int deltaX = currentPosition.X - _lastTouchPosition.X;
                int deltaY = currentPosition.Y - _lastTouchPosition.Y;
                
                // Проверяем, достаточно ли большое смещение для свайпа (минимум 5 пикселей)
                if (Math.Abs(deltaX) > 5 || Math.Abs(deltaY) > 5)
                {
                    // Определяем основное направление свайпа
                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                    {
                        // Горизонтальный свайп
                        if (deltaX > 0)
                        {
                            // Свайп вправо
                            InputManager.SetPointerMovement(new Vector2(20, 0));
                            Debug.WriteLine("Свайп вправо");
                        }
                        else
                        {
                            // Свайп влево
                            InputManager.SetPointerMovement(new Vector2(-20, 0));
                            Debug.WriteLine("Свайп влево");
                        }
                    }
                    else
                    {
                        // Вертикальный свайп
                        if (deltaY > 0)
                        {
                            // Свайп вниз
                            InputManager.SetPointerMovement(new Vector2(0, 20));
                            Debug.WriteLine("Свайп вниз");
                        }
                        else
                        {
                            // Свайп вверх
                            InputManager.SetPointerMovement(new Vector2(0, -20));
                            Debug.WriteLine("Свайп вверх");
                        }
                    }
                    
                    Debug.WriteLine($"Свайп: deltaX={deltaX}, deltaY={deltaY}, pointerMovement={InputManager.GetPointerMovement()}");
                }
                else
                {
                    // Если движение слишком маленькое, считаем его как отсутствие движения
                    InputManager.SetPointerMovement(Vector2.Zero);
                }
                
                // Обновляем последнюю позицию
                _lastTouchPosition = currentPosition;
            }
            e.Handled = true;
        }
    }
}


