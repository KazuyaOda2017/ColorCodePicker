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
    /// ColorListView.xaml の相互作用ロジック
    /// </summary>
    public partial class ColorListView : UserControl
    {

        #region コンストラクタ
        public ColorListView()
        {
            InitializeComponent();
        }
        #endregion

        #region カラーコントロール追加
        public void AddColor(Color color)
        {
            try
            {
                //カラーコードを文字列に変換
                string colorCode = color.ToString();
                if (string.IsNullOrEmpty(colorCode))
                {
                    return;
                }

                //ColorImageコントロールを作成してWrapPanelに追加する
                var colorImage = new ColorCodeImage();
                colorImage.ColorCode = colorCode;

                //一番目に追加する
                main_wp.Children.Insert(0,colorImage);
            }
            catch (Exception e)
            {
            }
        
        }
        #endregion
    }
}
