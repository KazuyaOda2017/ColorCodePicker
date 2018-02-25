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
    /// TitleBar.xaml の相互作用ロジック
    /// </summary>
    public partial class TitleBar : UserControl
    {

        #region 定義
        public enum TitleStyleSelection
        {
            Nomal,
            CloseOnly,
            NoResize,
        }
        #endregion

        #region 変数
        private Window ParentWindow;
        #endregion

        #region プロパティ
        #region TitleProperty
        public static readonly DependencyProperty TitleProprety =
            DependencyProperty.Register("Title", typeof(string), typeof(TitleBar),
            new PropertyMetadata("Window"));
        public string Title
        {
            get { return GetValue(TitleProprety).ToString(); }
            set { SetValue(TitleProprety, value); }
        }
        #endregion

        #region TitleStyleProperty
        public static readonly DependencyProperty TitleStyleProperty =
            DependencyProperty.Register("TitleStyle",typeof(TitleStyleSelection),typeof(TitleBar),
            new PropertyMetadata(TitleStyleSelection.Nomal,new PropertyChangedCallback((sender,e)=>{
                (sender as TitleBar).OnTitleStylePropertyChanged(sender, e);
            })));

        private void OnTitleStylePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var type = (TitleStyleSelection)e.NewValue;

                switch (type)
                {
                    case TitleStyleSelection.Nomal:
                        //何もしない
                        break;
                    case TitleStyleSelection.CloseOnly:
                        this.minBtn.Visibility = System.Windows.Visibility.Collapsed;
                        this.sizeMaxBtn.Visibility = System.Windows.Visibility.Collapsed;
                        this.resizeBtn.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case TitleStyleSelection.NoResize:
                        this.sizeMaxBtn.Visibility = System.Windows.Visibility.Collapsed;
                        this.resizeBtn.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                }
            }
            catch (Exception ex)
            {
            }

        }

        public TitleStyleSelection TitleStyle
        {
            get { return (TitleStyleSelection)GetValue(TitleStyleProperty); }
            set { SetValue(TitleStyleProperty, value); }
        }
        #endregion
        #endregion

        #region コンストラクタ
        public TitleBar()
        {
            InitializeComponent();

           
        }
        #endregion


        #region WindowのStateChangeイベント
        void TitleBar_StateChanged(object sender, EventArgs e)
        {
            if (ParentWindow == null) return;

            //最大化、リサイズボタンの入れ替え
            if (ParentWindow.WindowState == WindowState.Maximized)
            {
                this.sizeMaxBtn.Visibility = System.Windows.Visibility.Collapsed;
                this.resizeBtn.Visibility = System.Windows.Visibility.Visible;
            }
            else if (ParentWindow.WindowState == WindowState.Normal)
            {
                this.sizeMaxBtn.Visibility = System.Windows.Visibility.Visible;
                this.resizeBtn.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        #endregion

        #region ボタンクリック処理
        #region Min
        private void minBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;

        }
        #endregion

        #region Max
        private void sizeMaxBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Maximized;
        }
        #endregion

        #region リサイズ
        private void resizeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Normal;
        }
        #endregion

        #region Close
        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
        #endregion
        #endregion

        #region ウィンドウのドラッグ
        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Window.GetWindow(this).DragMove();
        }
        #endregion

        #region ダブルクリック
        /// <summary>
        /// ダブルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (ParentWindow == null) return;

            if (ParentWindow.WindowState == WindowState.Normal)
            {
                //最大化する
                ParentWindow.WindowState = WindowState.Maximized;
            }
            else if (ParentWindow.WindowState == WindowState.Maximized)
            {
                //最小化する
                ParentWindow.WindowState = WindowState.Normal;
            }
        }
        #endregion

        #region ロードイベント
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ParentWindow = Window.GetWindow(this);

                //Windowのイベントを拾う
                Window.GetWindow(this).StateChanged += TitleBar_StateChanged;
            }
            catch (Exception ex)
            {
            }

        }
        #endregion

     
    }
}
