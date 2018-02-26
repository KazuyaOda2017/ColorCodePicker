using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorCodePicker
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 変数
        private ImageWindow imageWindow;
        #endregion

        #region コンストラクタ
        public MainWindow()
        {
            InitializeComponent();

            //titlebar.Title = Properties.Resources.windowTitle;
        }
        #endregion


        #region ドラッグ＆ドロップ
        private void window_image_Drop(object sender, DragEventArgs e)
        {
            //領域の塗りつぶし
            border_image.Background = new SolidColorBrush(Colors.White);

            try
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];

                //複数ファイルを選択している
                if (files.Count() > 1) return;

                //ファイルチェック
                if (!FileCheck(files[0])) return;

                //画像生成
                var bmp = CreatBitmap(files[0]);
                if (bmp == null) return;

                //画像をセット
                image.Source = bmp;

                ShowImageWindow(bmp);

            }
            catch (Exception ex)
            {

            }


        }


        private void window_image_PreviewDragOver(object sender, DragEventArgs e)
        {
            //領域の塗りつぶし
            border_image.Background = new SolidColorBrush(Colors.LightBlue);

            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void Window_PreviewDragLeave(object sender, DragEventArgs e)
        {
            //領域の塗りつぶし
            border_image.Background = new SolidColorBrush(Colors.White);
        }
        #endregion


        /// <summary>
        /// イメージウィンドウのクローズイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageWindow_Closed(object sender, EventArgs e)
        {

            this.imageWindow.ColorCodeCallBack -= imageWindow_ColorCodeCallBack;
            imageWindow.Closed -= imageWindow_Closed;
            this.imageWindow = null;
        }

       

        #region ファイルチェック
        private bool FileCheck(string path)
        {

            string[] extensions = {".png",".jpg",".bmp"};
            try
            {
                //ディレクトリはリターン
                if(Directory.Exists(path))
                {
                    return false;
                }

                //png,bmp,jpg以外はFalse
                //拡張子
                var extension = System.IO.Path.GetExtension(path).ToLower();

                if (!extensions.Any(a => a.Equals(extension)))
                {
                    return false;
                }


                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region 画像生成
        private BitmapImage CreatBitmap(string filepath)
        {

            BitmapImage bmp = new BitmapImage();
            try
            {
                if (string.IsNullOrEmpty(filepath) ||
                    !File.Exists(filepath))
                {
                    return null;
                }

                using (var fs = File.OpenRead(filepath))
                {
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.StreamSource = fs;
                    bmp.EndInit();

                }

                return bmp;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion

        #region カラーコード取得
        void imageWindow_ColorCodeCallBack(Color color)
        {
            try
            {
                //カラーリストにImageを追加する
                this.colorListView.AddColor(color);

            }
            catch (Exception e)
            {
            }
        }
        #endregion

        #region 画像クリック処理
        private void image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var source = (sender as Image).Source;
                
                if (source is BitmapImage)
                {
                    var bmp = source as BitmapImage;
                    if (bmp != null)
                    {
                        //画像ウィンドウを表示する
                        ShowImageWindow(bmp);
                    }
                }
                else if (source is BitmapSource)
                {
                    var bmp = source as BitmapSource;
                    if (bmp != null)
                    {
                        ShowImageWindow(bmp);
                    }
                }
               

            }
            catch (Exception ex) { }

        }
        #endregion

        #region 画像ウィンドウの表示
        private void ShowImageWindow(BitmapImage bmp)
        {
            try
            {
                //ImageWindow生成 インスタンスを使いまわす
                if (imageWindow == null)
                {
                    imageWindow = new ImageWindow();

                    //コールバックイベント登録
                    imageWindow.ColorCodeCallBack += imageWindow_ColorCodeCallBack;
                    //クローズイベントを登録
                    imageWindow.Closed += imageWindow_Closed;
                }

                //画像セット
                imageWindow.ImageSource = bmp;
                imageWindow.Owner = this;
                //モーダル表示する
                imageWindow.Show();
            }
            catch (Exception e)
            {
            }
        }

        private void ShowImageWindow(BitmapSource source)
        {
            try
            {
                //ImageWindow生成 インスタンスを使いまわす
                if (imageWindow == null)
                {
                    imageWindow = new ImageWindow();

                    //コールバックイベント登録
                    imageWindow.ColorCodeCallBack += imageWindow_ColorCodeCallBack;
                    //クローズイベントを登録
                    imageWindow.Closed += imageWindow_Closed;
                }

                //画像セット
                imageWindow.ImageBitmapSource = source;
                imageWindow.Owner = this;

                imageWindow.Show();
            }
            catch (Exception e)
            {
            }
        }
        #endregion

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        #region Windowのキーダウンイベント
        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.V)
                {
                    ModifierKeys modifierkey = Keyboard.Modifiers;
                    //Ctrl+Vを検知する
                    if ((modifierkey & ModifierKeys.Control) != ModifierKeys.None)
                    {

                        //クリップボードにあるデータを取得する
                        var clip = Clipboard.GetDataObject();

                        var c_bmp = clip.GetData(typeof(System.Drawing.Bitmap)) as System.Drawing.Bitmap;
                        //var c_bmp = clip.GetData(typeof(BitmapImage)) as BitmapImage;
                        
                        
                        var bitmapsource = Clipboard.GetImage();
                        if (bitmapsource != null)
                        {

                            var handle = c_bmp.GetHbitmap();

                            try
                            {
                                var imagesource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());


                                this.image.Source = imagesource;


                                this.ShowImageWindow(imagesource);
                                return;
                            }
                            catch (Exception ex)
                            {
                            }

                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion


    }
}
