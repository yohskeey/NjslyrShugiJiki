using MahApps.Metro.Controls;
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
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;

namespace ShugiJikiGame
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private Ikusa IkusaData { get; set; }
        private bool IsStart { get; set; }

        private List<Polygon> Outers { get; set; }
        private List<Polygon> Shugi { get; set; }

        public string DispMessage { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.Outers = new List<Polygon> {
                Outer0, Outer1, Outer2, Outer3, Outer4, Outer5, Outer6, Outer7, Outer8, Outer9,
                Outer10, Outer11, Outer12, Outer13, Outer14, Outer15, Outer16, Outer17, Outer18, Outer19 };
            this.Shugi = new List<Polygon> { Shugi0, Shugi1, Shugi2, Shugi3 };
            this.IsStart = false;
            this.AmbushEasy.IsChecked = true;
        }
        

        private async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsStart == false)
            {
                this.IkusaData = new Ikusa(AmbushHard.IsChecked.HasValue ? AmbushHard.IsChecked.Value : false);
                this.ButtonStart.Content = "賽を振る";
                this.IsStart = true;
                this.textBlockDice.Text = "0";
                this.DispMessage = "<ゲームの開始な>";
                this.textBox.Text = this.DispMessage;
                ViewRefreshPosition();
                return;
            }

            this.ButtonStart.IsEnabled = false;

            var move = Ikusa.StartDice();
            this.textBlockDice.Text = move.ToString();
            DispMessage = "<" + move.ToString() + "マス進むドスエ>\n" + DispMessage;

            this.IkusaData.MovePlayer(move).ForEach(v => { DispMessage = v + "\n" + DispMessage;  });

            this.textBox.Text = this.DispMessage;
            ViewRefreshPosition();

            var res = await Task.Run(async () =>
            {
                await Task.Delay(500);
                return "";
            });

            DispMessage = "<" + move.ToString() + "ニンジャスレイヤーの行動ドスエ>\n" + DispMessage;
            this.IkusaData.MoveNinjaSlayer().ForEach(v => { DispMessage = v + "\n" + DispMessage; });

            this.textBox.Text = this.DispMessage;
            ViewRefreshPosition();

            this.ButtonStart.IsEnabled = true;

            JudgeGame();

        }

        private async void JudgeGame()
        {
            if (this.IkusaData.MyPlayer.IsDead)
            {
                var settings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "ツイートする",
                    NegativeButtonText = "ツイートしない"
                };
                var res = await this.ShowMessageAsync("ゲームオーバーな", this.textBlockLap.Text + "\n" + this.textBlockAmbush.Text, MessageDialogStyle.AffirmativeAndNegative, settings);
                this.IsStart = false;
                if(res == MessageDialogResult.Affirmative)
                {
                    System.Diagnostics.Process.Start(
                        "https://twitter.com/intent/tweet?text=ニンジャスレイヤーから" + this.IkusaData.MyPlayer.LapCount + "周逃げ続け、" +
                        this.IkusaData.MyPlayer.AmbushCount + "回のアンブッシュを決めたものの哀れ爆発四散してしまった…&hashtags=シュギ・ジキと夜の獣");
                }
            }
        }

        private void ViewRefreshPosition()
        {
            var defaultSolidColorBrush = new SolidColorBrush() { Color = Colors.Beige, Opacity = 0.4 };

            var playerSplidColorBrush = new SolidColorBrush() { Color = Colors.SpringGreen, Opacity = 0.4 };
            var njslyrSplidColorBrush = new SolidColorBrush() { Color = Colors.Crimson, Opacity = 0.4 };

            this.Outers.ForEach((v) =>
            {
                v.Fill = defaultSolidColorBrush;
            });
            this.Shugi.ForEach((v) =>
            {
                v.Fill = defaultSolidColorBrush;
            });

            if (this.IkusaData.NinjaSlayer.IsShugi)
            {
                this.Shugi[this.IkusaData.NinjaSlayer.PositionShugi].Fill = njslyrSplidColorBrush;
            }
            else
            {
                this.Outers[this.IkusaData.NinjaSlayer.PositionOuter].Fill = njslyrSplidColorBrush;
            }

            if (this.IkusaData.MyPlayer.IsShugi)
            {
                this.Shugi[this.IkusaData.MyPlayer.PositionShugi].Fill = playerSplidColorBrush;
            }
            else
            {
                this.Outers[this.IkusaData.MyPlayer.PositionOuter].Fill = playerSplidColorBrush;
            }

            this.textBlockLap.Text = this.IkusaData.MyPlayer.LapCount + "周";
            this.textBlockAmbush.Text = this.IkusaData.MyPlayer.AmbushCount + "回アンブッシュ成功";
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            this.flyout.IsOpen = true;
        }
        private void hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // リンククリック
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void AmbushEasy_Checked(object sender, RoutedEventArgs e)
        {
            if (this.AmbushEasy.IsChecked.HasValue)
            {
                this.AmbushHard.IsChecked = !this.AmbushEasy.IsChecked.Value;
            }
        }

        private void AmbushHard_Checked(object sender, RoutedEventArgs e)
        {
            if (this.AmbushHard.IsChecked.HasValue)
            {
                this.AmbushEasy.IsChecked = !this.AmbushHard.IsChecked.Value;
            }
        }
    }
}
