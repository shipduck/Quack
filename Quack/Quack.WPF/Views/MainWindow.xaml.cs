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
    public partial class MainWindow : Window, Quack.IAuthWebView
    {
        private static TwitterContext context = null;
        private static IAuthorizer auth = null;

        String pin;

        public void SetWebViewLocation(Uri authWebPageLink)
        {
            webBrowser.Source = authWebPageLink;
        }

        public MainWindow()
        {
            InitializeComponent();

            auth = QTwitter.BeginAuthorize(this);
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
                    tweet.Text = "@" + t.ScreenName + " " + t.Text;
                }));
            });
        }

        void button_Click(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                context = QTwitter.CompleteAuthorize(auth, textBox.Text);
            }));

            Task task = Query(QTwitter.GetContext("temp"));
            task.Wait();
        }
    }
}
