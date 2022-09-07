using System;
using System.Drawing;
using System.Linq;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Ninject;
using Ninject.Extensions.Conventions;

namespace FractalPainting.App
{
    public static class DIContainerTask
    {
        public static MainForm CreateMainForm()
        {
            // Example: ConfigureContainer()...
            // return new MainForm();
            return ConfigureContainer().Get<MainForm>();
        }

        public static StandardKernel ConfigureContainer()
        {
            // Example
            // container.Bind<TService>().To<TImplementation>();
            
            var container = new StandardKernel();
            container.Bind<PictureBoxImageHolder>().ToConstant(Services.GetPictureBoxImageHolder());
            // container.Bind(s => s.FromThisAssembly().SelectAllClasses().BindAllInterfaces());
            
            container.Bind<IUiAction>().To<SaveImageAction>();
            container.Bind<IUiAction>().To<DragonFractalAction>();
            container.Bind<IUiAction>().To<KochFractalAction>();
            container.Bind<IUiAction>().To<ImageSettingsAction>();
            container.Bind<IUiAction>().To<PaletteSettingsAction>();
            
            container.Bind<AppSettings>().ToConstant(Services.GetAppSettings());
            container.Bind<IImageHolder>().ToConstant(Services.GetImageHolder());
            container.Bind<ImageSettings>().ToConstant(Services.GetImageSettings());
            container.Bind<Palette>().ToConstant(Services.GetPalette());

            return container;
        }
    }

    public static class Services
    {
        private static readonly SettingsManager SettingsManager;
        private static readonly PictureBoxImageHolder PictureBoxImageHolder;
        private static readonly Palette Palette;
        private static readonly AppSettings AppSettings;
        private static readonly IImageSettingsProvider ImageSettingsProvider;
        private static readonly IImageDirectoryProvider ImageDirectoryProvider;

        static Services()
        {
            Palette = new Palette();
            PictureBoxImageHolder = new PictureBoxImageHolder();
            SettingsManager = new SettingsManager(new XmlObjectSerializer(), new FileBlobStorage());
            AppSettings = SettingsManager.Load();
            ImageSettingsProvider = AppSettings;
            ImageDirectoryProvider = AppSettings;
        }

        public static IObjectSerializer CreateObjectSerializer()
        {
            return new XmlObjectSerializer();
        }

        public static IBlobStorage CreateIBlobStorage()
        {
            return new FileBlobStorage();
        }

        public static SettingsManager GetSettingsManager()
        {
            return SettingsManager;
        }

        public static PictureBoxImageHolder GetPictureBoxImageHolder()
        {
            return PictureBoxImageHolder;
        }

        public static IImageHolder GetImageHolder()
        {
            return PictureBoxImageHolder;
        }

        public static Palette GetPalette()
        {
            return Palette;
        }

        public static ImageSettings GetImageSettings()
        {
            return AppSettings.ImageSettings;
        }

        public static IImageSettingsProvider GetImageSettingsProvider()
        {
            return ImageSettingsProvider;
        }

        public static AppSettings GetAppSettings()
        {
            return AppSettings;
        }
    }

    public class DragonFractalAction : IUiAction
    {
        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Дракон";
        public string Description => "Дракон Хартера-Хейтуэя";

        public void Perform()
        {
            var dragonSettings = CreateRandomSettings();
            // редактируем настройки:
            SettingsForm.For(dragonSettings).ShowDialog();
            // создаём painter с такими настройками
            var painter = new DragonPainter(Services.GetImageHolder(), dragonSettings);
            painter.Paint();
        }

        private static DragonSettings CreateRandomSettings()
        {
            return new DragonSettingsGenerator(new Random()).Generate();
        }
    }

    public class KochFractalAction : IUiAction
    {
        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Кривая Коха";
        public string Description => "Кривая Коха";

        public void Perform()
        {
            var painter = new KochPainter(Services.GetImageHolder(), Services.GetPalette());
            painter.Paint();
        }
    }

    public class DragonPainter
    {
        private readonly IImageHolder imageHolder;
        private readonly DragonSettings settings;
        private readonly float size;
        private Size imageSize;

        public DragonPainter(IImageHolder imageHolder, DragonSettings settings)
        {
            this.imageHolder = imageHolder;
            this.settings = settings;
            imageSize = imageHolder.GetImageSize();
            size = Math.Min(imageSize.Width, imageSize.Height) / 2.1f;
        }

        public void Paint()
        {
            using (var graphics = imageHolder.StartDrawing())
            {
                graphics.FillRectangle(Brushes.Black, 0, 0, imageSize.Width, imageSize.Height);
                var r = new Random();
                var cosa = (float)Math.Cos(settings.Angle1);
                var sina = (float)Math.Sin(settings.Angle1);
                var cosb = (float)Math.Cos(settings.Angle2);
                var sinb = (float)Math.Sin(settings.Angle2);
                var shiftX = settings.ShiftX * size * 0.8f;
                var shiftY = settings.ShiftY * size * 0.8f;
                var scale = settings.Scale;
                var p = new PointF(0, 0);
                foreach (var i in Enumerable.Range(0, settings.IterationsCount))
                {
                    graphics.FillRectangle(Brushes.Yellow, imageSize.Width / 3f + p.X, imageSize.Height / 2f + p.Y, 1, 1);
                    if (r.Next(0, 2) == 0)
                        p = new PointF(scale * (p.X * cosa - p.Y * sina), scale * (p.X * sina + p.Y * cosa));
                    else
                        p = new PointF(scale * (p.X * cosb - p.Y * sinb) + shiftX, scale * (p.X * sinb + p.Y * cosb) + shiftY);
                    if (i % 100 == 0) imageHolder.UpdateUi();
                }
            }
            
            imageHolder.UpdateUi();
        }
    }
}