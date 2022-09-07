using System;
using System.Drawing;
using System.Linq;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Ninject;
using Ninject.Extensions.Factory;

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
            var container = new StandardKernel();
            
            container.Bind<IUiAction>().To<SaveImageAction>();
            container.Bind<IUiAction>().To<DragonFractalAction>();
            container.Bind<IUiAction>().To<KochFractalAction>();
            container.Bind<IUiAction>().To<ImageSettingsAction>();
            container.Bind<IUiAction>().To<PaletteSettingsAction>();

            container.Bind<IObjectSerializer>().To<XmlObjectSerializer>();
            container.Bind<IBlobStorage>().To<FileBlobStorage>();

            container.Bind<AppSettings>().ToMethod(context => context.Kernel.Get<SettingsManager>().Load())
                .InSingletonScope();
            container.Bind<IDragonPainterFactory>().ToFactory();
            container.Bind<ImageSettings>().ToMethod(context => context.Kernel.Get<AppSettings>().ImageSettings)
                .InSingletonScope();
            container.Bind<Palette>().To<Palette>().InSingletonScope();
            container.Bind<IImageHolder, PictureBoxImageHolder>().To<PictureBoxImageHolder>().InSingletonScope();

            return container;
        }
    }

    public interface IDragonPainterFactory
    {
        DragonPainter CreatePainter(IImageHolder imageHolder, DragonSettings settings);
    }

    public class DragonFractalAction : IUiAction
    {
        private readonly IImageHolder _holder;
        private readonly IDragonPainterFactory _dragonPainterFactory;

        public DragonFractalAction(IDragonPainterFactory dragonPainterFactory, IImageHolder holder)
        {
            _dragonPainterFactory = dragonPainterFactory;
            _holder = holder;
        }

        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Дракон";
        public string Description => "Дракон Хартера-Хейтуэя";

        public void Perform()
        {
            var dragonSettings = CreateRandomSettings();
            // редактируем настройки:
            SettingsForm.For(dragonSettings).ShowDialog();
            // создаём painter с такими настройками
            // var painter = new DragonPainter(Services.GetImageHolder(), dragonSettings);
            var painter = _dragonPainterFactory.CreatePainter(_holder, dragonSettings);
            painter.Paint();
        }

        private static DragonSettings CreateRandomSettings()
        {
            return new DragonSettingsGenerator(new Random()).Generate();
        }
    }

    public class KochFractalAction : IUiAction
    {
        private readonly Lazy<IImageHolder> _holder;
        private readonly Palette _palette;

        public KochFractalAction(Lazy<IImageHolder> holder, Palette palette)
        {
            _holder = holder;
            _palette = palette;
        }

        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Кривая Коха";
        public string Description => "Кривая Коха";

        public void Perform()
        {
            // var painter = new KochPainter(Services.GetImageHolder(), Services.GetPalette());
            var painter = new KochPainter(_holder.Value, _palette);
            painter.Paint();
        }
    }

    public class DragonPainter
    {
        private readonly IImageHolder _imageHolder;
        private readonly DragonSettings _settings;
        private readonly float _size;
        private Size _imageSize;
        private readonly Palette _palette;

        public DragonPainter(IImageHolder imageHolder, DragonSettings settings, Palette palette)
        {
            _imageHolder = imageHolder;
            _settings = settings;
            _palette = palette;
            _imageSize = imageHolder.GetImageSize();
            _size = Math.Min(_imageSize.Width, _imageSize.Height) / 2.1f;
        }

        public void Paint()
        {
            using (var graphics = _imageHolder.StartDrawing())
            {
                graphics.FillRectangle(new SolidBrush(_palette.BackgroundColor), 0, 0, _imageSize.Width, _imageSize.Height);
                var r = new Random();
                var cosa = (float)Math.Cos(_settings.Angle1);
                var sina = (float)Math.Sin(_settings.Angle1);
                var cosb = (float)Math.Cos(_settings.Angle2);
                var sinb = (float)Math.Sin(_settings.Angle2);
                var shiftX = _settings.ShiftX * _size * 0.8f;
                var shiftY = _settings.ShiftY * _size * 0.8f;
                var scale = _settings.Scale;
                var p = new PointF(0, 0);
                foreach (var i in Enumerable.Range(0, _settings.IterationsCount))
                {
                    graphics.FillRectangle(new SolidBrush(_palette.PrimaryColor), _imageSize.Width / 3f + p.X, _imageSize.Height / 2f + p.Y, 1, 1);
                    if (r.Next(0, 2) == 0)
                        p = new PointF(scale * (p.X * cosa - p.Y * sina), scale * (p.X * sina + p.Y * cosa));
                    else
                        p = new PointF(scale * (p.X * cosb - p.Y * sinb) + shiftX, scale * (p.X * sinb + p.Y * cosb) + shiftY);
                    if (i % 100 == 0) _imageHolder.UpdateUi();
                }
            }
            
            _imageHolder.UpdateUi();
        }
    }
}
