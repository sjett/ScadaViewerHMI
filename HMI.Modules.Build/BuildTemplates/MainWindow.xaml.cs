// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace [PROJECTNAMESPACE];

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs args)
    {
        Content = new [PROJECTSTARTPAGE]();
    }
}
