using System;
using System.Windows;
using System.Windows.Controls;
using TripleMatch.WPF.Common.ViewManagers.IFrameManagers;

namespace TripleMatch.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IFrameContainer
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Frame GetNavigationFrame()
        {
            return new Frame();
        }
    }
}
