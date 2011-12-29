using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using System.IO;
using System.Diagnostics;

namespace TileProjectTaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        public void createImage(int idx, string title, string subtitle)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                var logo = new BitmapImage(new Uri("tileEsc.png", UriKind.Relative));
                logo.CreateOptions = BitmapCreateOptions.None;

                logo.ImageOpened += (sender, e) =>
                {

                    var bmp = new WriteableBitmap(173, 173);

                    var img = new Image { Source = logo };

                    var bl = new TextBlock();

                    bl.Foreground = new SolidColorBrush(Colors.White);
                    bl.FontSize = 20.0;
                    bl.Text = title;

                    var bl2 = new TextBlock();
                    bl2.Foreground = new SolidColorBrush(Colors.White);
                    bl2.FontSize = 60.0;
                    bl2.Text = "#" + idx.ToString();

                    var bl3 = new TextBlock();
                    bl3.Foreground = new SolidColorBrush(Colors.White);
                    bl3.FontSize = 20.0;
                    bl3.Text = subtitle;

                    var transf = new TranslateTransform();
                    transf.X = 12;
                    transf.Y = 120;

                    var transf2 = new TranslateTransform();
                    transf2.X = 12;
                    transf2.Y = 62;

                    var transf3 = new TranslateTransform();
                    transf3.X = 12;
                    transf3.Y = 142;

                    var tt = new TranslateTransform();
                    tt.X = 173 - logo.PixelWidth;
                    tt.Y = 173 - logo.PixelHeight;


                    bmp.Render(img, tt);

                    bmp.Render(bl, transf);
                    bmp.Render(bl2, transf2);
                    bmp.Render(bl3, transf3);

                    bmp.Invalidate();

                    using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        var filename = "/Shared/ShellContent/testTile.jpg";
                        using (var st = new IsolatedStorageFileStream(filename, FileMode.Create, FileAccess.Write, store))
                        {
                            bmp.SaveJpeg(st, 173, 173, 0, 100);
                        }
                    }
                };



            });
        }

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
          
        }

        protected override void OnInvoke(ScheduledTask task)
        {
            Debug.WriteLine("invoke!!");

            if (task is PeriodicTask)
            {
                Debug.WriteLine("Achou a task");
                createImage(1, "Titulo", "Subtitulo");
                ShellTile TileToFind = ShellTile.ActiveTiles.First();

                if (TileToFind != null)
                {
                    StandardTileData NewTileData = new StandardTileData
                    {
                        BackgroundImage = new Uri("Background.png", UriKind.Relative),
                        Count = 0,
                        BackBackgroundImage = new Uri("isostore:/Shared/ShellContent/testTile.jpg", UriKind.Absolute)
                    };
                    TileToFind.Update(NewTileData);
                }
                else
                {
                    //Debug.WriteLine("The tile isn't pinned.");
                }

            }

            NotifyComplete();
        }
    }
}