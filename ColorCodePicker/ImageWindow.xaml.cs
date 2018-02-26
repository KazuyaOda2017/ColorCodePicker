using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ColorCodePicker
{
    /// <summary>
    /// ImageWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageWindow : Window
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        #region イベント
        public delegate void ColorCodeCallBackEventHandler(System.Windows.Media.Color color);
        /// <summary>
        /// カラーコードのコールバック
        /// </summary>
        public event ColorCodeCallBackEventHandler ColorCodeCallBack = delegate { return; };
        #endregion

        #region プロパティ
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(BitmapImage), typeof(ImageWindow),
            new PropertyMetadata(null, new PropertyChangedCallback((sender, e) => {
                (sender as ImageWindow).OnImageSourcePropertyChanged(sender, e);
            })));

        private void OnImageSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var bmp = (BitmapImage)e.NewValue;

                if (bmp == null) return;

                this.image.Source = bmp;

                //画像サイズとピクセルサイズを合わせる
                this.image.Height = bmp.PixelHeight;
                this.image.Width = bmp.PixelWidth;
            }
            catch (Exception ex)
            {
            }
        }

        public BitmapImage ImageSource
        {
            get { return (BitmapImage)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        #endregion

        #region ImageBitmapSourceProperty
        public static readonly DependencyProperty ImageBitmapSourceProperty =
            DependencyProperty.Register("ImageBitmapSource", typeof(BitmapSource), typeof(ImageWindow),
            new PropertyMetadata(null, new PropertyChangedCallback((sender, e) => {
                (sender as ImageWindow).OnImageBitmapSourcePropertyChanged(sender, e);
            })));

        private void OnImageBitmapSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var bmpsource = (BitmapSource)e.NewValue;

                if (bmpsource == null) return;

                this.image.Source = bmpsource;

                //画像サイズとピクセルサイズを合わせる
                this.image.Height = bmpsource.PixelHeight;
                this.image.Width = bmpsource.PixelWidth;
            }
            catch (Exception ex)
            {
            }
        }

        public BitmapSource ImageBitmapSource
        {
            get { return (BitmapSource)GetValue(ImageBitmapSourceProperty); }
            set { SetValue(ImageBitmapSourceProperty, value); }
        }
        #endregion

        #region コンストラクタ
        public ImageWindow()
        {
            InitializeComponent();
        }

        #endregion


        #region クリック処理
        /// <summary>
        /// クリック処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //クリック位置取得
                System.Windows.Point point = e.GetPosition(image);

                int pointX = (int)point.X;
                int pointY = (int)point.Y;

                BitmapSource bmp = image.Source as BitmapSource;

               

                int wb = (int)bmp.PixelWidth;
                int hb = (int)bmp.PixelHeight;

                if (pointX >= wb) pointX = wb - 1;
                if (pointY >= hb) pointY = hb - 1;


                System.Windows.Media.Color color = GetPixelColor(pointX, pointY, bmp);

                if (color == null) return;

                //カラーコードをコールバック
                ColorCodeCallBack(color);

            }
            catch (Exception ex)
            {
            }
        }

       
        #endregion

        #region カラーコード取得
        private System.Windows.Media.Color GetPixelColor(int pointX, int pointY, BitmapSource bmp)
        {
            System.Windows.Media.Color c = new System.Windows.Media.Color();
            try
            {
                var cb = new CroppedBitmap(bmp, new Int32Rect(pointX, pointY, 1, 1));
                var fcb = new FormatConvertedBitmap(cb, PixelFormats.Bgra32, BitmapPalettes.Halftone256Transparent, 0);

                Byte[] pixels = new byte[4];

                fcb.CopyPixels(pixels, 4, 0);

               
                c = System.Windows.Media.Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);

                
            }
            catch (Exception e)
            {
                throw e;
            }
            return c;
        }
        #endregion

        public BitmapPalette Nothing { get; set; }
    }
}
