using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Managers
{
    public class WindowsManager
    {
        public static Window CreateNewWindow<T>() where T : Window
        {
            var window = (Window)Activator.CreateInstance<T>();
            window.Show();

            return window;
        }
    }
}
