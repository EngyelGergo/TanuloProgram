using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TanuloProgram.Services
{
    public static class LanguageEvent
    {
        public static event HighlightEventHandler HighlightEvent;
        public static EventHandler OnlyWordEvent;
        public static EventHandler OnlySentenceEvent;
        public static EventHandler NonSelectedElementEvent;

        public static void TriggerHighlightEvent(object sender, SolidColorBrush colorCode)
        {
            HighlightEvent?.Invoke(sender, new HighlightEventArgs(colorCode));
        }
    }
    public class HighlightEventArgs : EventArgs
    {
        public SolidColorBrush ColorCode { get; }

        public HighlightEventArgs(SolidColorBrush colorCode)
        {
            ColorCode = colorCode;
        }
    }

    public delegate void HighlightEventHandler(object sender, HighlightEventArgs e);

}
