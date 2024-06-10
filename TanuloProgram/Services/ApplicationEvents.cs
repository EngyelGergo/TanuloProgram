using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TanuloProgram.Services
{
    public static class LanguageEvent
    {
        public static event CollectionAddedElementEventHandler ElementAddedEvent;

        public static event RefreshEventHandler? RefresfhMainWindowEvent;
        public static event HighlightEventHandler? HighlightEvent;
        public static EventHandler? OnlyWordEvent;
        public static EventHandler? OnlySentenceEvent;
        public static EventHandler? NonSelectedElementEvent;

        public delegate void CollectionAddedElementEventHandler(object sender, StringElementEventArg e);
        public delegate void RefreshEventHandler(object sender, RefreshEventArgs e);
        public delegate void HighlightEventHandler(object sender, HighlightEventArgs e);

        public static void TriggerCollenctionAddEvent(object sender, string element)
        {
            ElementAddedEvent?.Invoke(sender, new StringElementEventArg(element));
        }

        public static void TriggerRefreshEvent(object sender, double height, double width)
        {
            RefresfhMainWindowEvent?.Invoke(sender, new RefreshEventArgs(height,width));
        }

        public static void TriggerHighlightEvent(object sender, SolidColorBrush colorCode)
        {
            HighlightEvent?.Invoke(sender, new HighlightEventArgs(colorCode));
        }
    }

    public class StringElementEventArg(string element) : EventArgs
    {
        public string Element { get; } = element;
    }

    public class HighlightEventArgs(SolidColorBrush colorCode) : EventArgs
    {
        public SolidColorBrush ColorCode { get; } = colorCode;

    }
    public class RefreshEventArgs : EventArgs
    {
        public double Height { get;}
        public double Width { get;}
        public WindowState State { get;}
        public WindowStyle Style { get;}

        public RefreshEventArgs(double height, double width)
        {
            Height = height;
            Width = width;
        }
        public RefreshEventArgs(WindowState state, WindowStyle style)
        {
            State = state;
            Style = style;
        }
        public RefreshEventArgs(double height, double width, WindowState state, WindowStyle style)
        {
            Height = height;
            Width = width;
            State = state;
            Style = style;
        }   
    }

}
