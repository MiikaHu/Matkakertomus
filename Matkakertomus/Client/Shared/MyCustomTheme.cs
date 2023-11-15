using MudBlazor;

namespace Matkakertomus.Client.Shared
{
    public class MyCustomTheme : MudTheme
    {
        public MyCustomTheme()
        {
            Palette = new Palette
            {
                Primary = Colors.Grey.Darken3,
                Secondary = Colors.Grey.Darken4,
                Background = Colors.Brown.Default,
                AppbarBackground = Colors.Brown.Lighten1,
                DrawerBackground = Colors.Brown.Lighten1,
                DrawerText = Colors.Grey.Darken4,
                TextPrimary = Colors.Grey.Darken4,
                TextSecondary = Colors.Grey.Darken4,
                DrawerIcon = Colors.Grey.Darken4,
                
            };


            Typography = new Typography
            {
                Default = new Default()
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "16px",
                    LineHeight = 1.5,
                },
            };
        }
    }
}
