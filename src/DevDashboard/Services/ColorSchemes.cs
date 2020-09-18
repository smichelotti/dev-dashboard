using Terminal.Gui;

namespace DevDashboard.Services
{
    public static class ColorSchemes
    {
        public static ColorScheme Get(Color color) => new ColorScheme { Normal = GetColor(color) };

        private static Attribute GetColor(Color color) => Application.Driver.MakeAttribute(color, Color.Black);
    }
}
