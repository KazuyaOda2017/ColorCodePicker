using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ColorCodePicker.UserControls
{
    /// <summary>
    /// ColorCodeImage.xaml の相互作用ロジック
    /// </summary>
    public partial class ColorCodeImage : UserControl
    {

        #region プロパティ
        public static readonly DependencyProperty ColorCodeProperty =
            DependencyProperty.Register("ColorCode", typeof(string), typeof(ColorCodeImage),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback((sender, e) => {
                (sender as ColorCodeImage).OnColorCodePropertyChanged(sender, e);
            })));

        private void OnColorCodePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var code = e.NewValue.ToString();

                if (string.IsNullOrEmpty(code)) return;

                //カラーコードからSolidBrushを作成
                var obj = System.Windows.Media.ColorConverter.ConvertFromString(code);

                var solidColor = new SolidColorBrush((System.Windows.Media.Color)obj);

                //円のバックグラウンドを書き換え
                this.curcle.Fill = solidColor;

            }
            catch (Exception ex)
            {
            }
        }

        public string ColorCode
        {
            get { return GetValue(ColorCodeProperty).ToString(); }
            set { SetValue(ColorCodeProperty, value); }
        }
        #endregion

        #region コンストラクタ
        public ColorCodeImage()
        {
            InitializeComponent();
        }
        #endregion


        #region コピー処理
        private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Clipboard.SetText(this.ColorCode);

                
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
