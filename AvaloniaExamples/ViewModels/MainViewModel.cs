using AvaloniaExamples.Managers;
using AvaloniaExamples.Windows;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ICommand]
        public void CreateChildWindow()
        {
            WindowsManager.CreateNewWindow<BorderlessWindow>();
        }


        [ICommand]
        public void CreateChildFullSizeWindowCommand()
        {
            var window = WindowsManager.CreateNewWindow<FullScreenBorderlessWindow>();
            window.WindowState = Avalonia.Controls.WindowState.Maximized;
        }
    }
}
