using LinqToTwitter;
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

namespace Quack.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Threading.AutoResetEvent autoResetEvent;
        private static TwitterContext context = null;
        static TextBlock tb = null;

        String pin;

        public MainWindow()
        {
            InitializeComponent();

            autoResetEvent = new System.Threading.AutoResetEvent(false);

            tb = (TextBlock)this.FindName("tweet");

            var auth = QTwitter.GetPinAuthorizer();
            auth.GoToTwitterAuthorization = pageLink =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    WebBrowser webBrowser = (WebBrowser)this.FindName("webBrowser");
                    webBrowser.Source = new Uri(pageLink);
                }));
            };

            auth.BeginAuthorizeAsync().ContinueWith(antecendent =>
            {
                autoResetEvent.WaitOne();
                
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TextBox textBox = (TextBox)this.FindName("textBox");
                    pin = textBox.Text;

                    auth.CompleteAuthorizeAsync(pin).ContinueWith(antecendent1 =>
                    {
                        context = new LinqToTwitter.TwitterContext(auth);

                        Task task = Query(context);
                        task.Wait();
                    });
                }));
            });
        }

        private async Task Query(TwitterContext context)
        {
            var tweets = await
                   (from tweet in context.Status
                    where tweet.Type == StatusType.Home
                    select tweet)
                   .ToListAsync();

            await Task.Run(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var t = tweets.First();
                    tb.Text = "@" + t.ScreenName + " " + t.Text;
                }));
            });
        }

        void button_Click(object sender, EventArgs e)
        {
            autoResetEvent.Set();
        }
    }
}
