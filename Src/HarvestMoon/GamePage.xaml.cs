using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
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

namespace AzureAcres
{
  public sealed partial class GamePage : Page
  {
        readonly AzureAcres _game;

        public GamePage()
        {
            this.InitializeComponent();

            // Create the game.
            var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<AzureAcres>.Create(
                launchArguments,
                Window.Current.CoreWindow,
                swapChainPanel);

        }
    }
}


