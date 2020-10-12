using System;
using System.Linq;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToastNotificationException
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string ToastCollectionId = "toastCollection";
        private ToastNotifier _toastNotifierManager;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ShowNotification_OnClick(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("ms-appx:///Assets/StoreSolo.png");
            var toastCollection = new ToastCollection(ToastCollectionId, "ContosoInc", "Args", uri);

            await ToastNotificationManager.GetDefault().GetToastCollectionManager().SaveToastCollectionAsync(toastCollection);

            _toastNotifierManager = await ToastNotificationManager.GetDefault().GetToastNotifierForToastCollectionIdAsync(ToastCollectionId);

            var toastsHistory = ToastNotificationManager.History.GetHistory().ToList();

        }
    }
}
