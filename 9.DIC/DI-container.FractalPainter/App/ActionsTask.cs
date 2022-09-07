using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;

namespace FractalPainting.App
{
    public class ImageSettingsAction : IUiAction
    {
        private readonly IImageHolder _holder;
        private readonly ImageSettings _imageSettings;

        public ImageSettingsAction(IImageHolder holder, ImageSettings imageSettings)
        {
            _holder = holder;
            _imageSettings = imageSettings;
        }
        
        public MenuCategory Category => MenuCategory.Settings;
        public string Name => "Изображение...";
        public string Description => "Размеры изображения";

        public void Perform()
        {
            SettingsForm.For(_imageSettings).ShowDialog();
            _holder.RecreateImage(_imageSettings);
            // var imageSettings = Services.GetImageSettings();
            // SettingsForm.For(imageSettings).ShowDialog();
            // Services.GetImageHolder().RecreateImage(imageSettings);
        }
    }

    public class SaveImageAction : IUiAction
    {
        private readonly AppSettings _appSettings;
        private readonly IImageHolder _holder;

        public SaveImageAction(AppSettings appSettings, IImageHolder holder)
        {
            _appSettings = appSettings;
            _holder = holder;
        }

        public MenuCategory Category => MenuCategory.File;
        public string Name => "Сохранить...";
        public string Description => "Сохранить изображение в файл";

        public void Perform()
        {
            var dialog = new SaveFileDialog
            {
                CheckFileExists = false,
                InitialDirectory = Path.GetFullPath(_appSettings.ImagesDirectory),
                DefaultExt = "bmp",
                FileName = "image.bmp",
                Filter = "Изображения (*.bmp)|*.bmp"
            };
            var res = dialog.ShowDialog();
            if (res == DialogResult.OK)
                _holder.SaveImage(dialog.FileName);
            // Services.GetImageHolder().SaveImage(dialog.FileName);
        }
    }

    public class PaletteSettingsAction : IUiAction
    {
        private readonly Palette _palette;
        public PaletteSettingsAction(Palette palette)
        {
            _palette = palette;
        }

        public MenuCategory Category => MenuCategory.Settings;
        public string Name => "Палитра...";
        public string Description => "Цвета для рисования фракталов";

        public void Perform()
        {
            SettingsForm.For(_palette).ShowDialog();
            // SettingsForm.For(Services.GetPalette()).ShowDialog();
        }
    }


    public class MainForm : Form
    {
        // public MainForm()
        //     : this(
        //         new IUiAction[]
        //         {
        //             new SaveImageAction(Services.GetAppSettings(), Services.GetImageHolder()),
        //             new DragonFractalAction(),
        //             new KochFractalAction(),
        //             new ImageSettingsAction(Services.GetImageHolder(), Services.GetImageSettings()),
        //             new PaletteSettingsAction(Services.GetPalette())
        //         }, Services.GetPictureBoxImageHolder())
        // { }

        public MainForm(IUiAction[] actions, PictureBoxImageHolder pictureBox)
        {
            var imageSettings = CreateSettingsManager().Load().ImageSettings;
            ClientSize = new Size(imageSettings.Width, imageSettings.Height);

            pictureBox.RecreateImage(imageSettings);
            pictureBox.Dock = DockStyle.Fill;
            Controls.Add(pictureBox);

            var mainMenu = new MenuStrip();
            mainMenu.Items.AddRange(actions.ToMenuItems());
            mainMenu.Dock = DockStyle.Top;
            Controls.Add(mainMenu);
        }

        private static SettingsManager CreateSettingsManager()
        {
            return new SettingsManager(new XmlObjectSerializer(), new FileBlobStorage());
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Text = "Fractal Painter";
        }
    }
}
