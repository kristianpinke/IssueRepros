using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Notifications;

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
            Task.Run(async () =>
            {
                var uri = new Uri("ms-appx:///Assets/StoreSolo.png");
                var toastCollection = new ToastCollection(ToastCollectionId, "ContosoInc", "Args", uri);

                await ToastNotificationManager.GetDefault().GetToastCollectionManager().SaveToastCollectionAsync(toastCollection);

                _toastNotifierManager = await ToastNotificationManager.GetDefault().GetToastNotifierForToastCollectionIdAsync(ToastCollectionId);

                var toastsHistory = ToastNotificationManager.History.GetHistory().ToList();

                foreach (var toast in toastsHistory)
                {
                    lock (_toastNotifierManager)
                    {
                        _toastNotifierManager.Hide(toast);
                    }
                }

                await ShowDownloadNotification("status", 2, "tag");
            });
            
        }

        public async Task ShowDownloadNotification(string status, int numberOfParts, string tag)
        {
            if (!ApiInformation.IsTypePresent("Windows.ApplicationModel.Background.ToastNotificationActionTrigger"))
                return;

            var toast = BuildToast(status, tag);

            toast.Data.Values["mainTitle"] = "Title";
            toast.Data.Values["progressValue"] = "0";
            toast.Data.Values["progressValueOverride"] = string.Format("{0}{1}", numberOfParts, "parts");
            toast.Data.Values["progressStatus"] = "Status";
            toast.Data.SequenceNumber = 0;

            ToastNotificationManager.History.Remove(tag, "collectionid");

            lock (_toastNotifierManager)
            {
                _toastNotifierManager.Show(toast);
            }
        }

        private ToastNotification BuildToast(string status, string tag, bool isIndeterminate = false)
        {
            var progressBar = new AdaptiveProgressBar
            {
                Title = status,
                Value = new BindableProgressBarValue("progressValue"),
                ValueStringOverride = new BindableString("progressValueOverride"),
                Status = new BindableString("progressStatus")
            };

            var content = new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = new BindableString("mainTitle")
                            }
                        },
                        HeroImage = new ToastGenericHeroImage()
                        {
                            Source = "https://workheld.com/en/wp-content/uploads/sites/6/2020/04/header-2.jpg"
                        }
                    }
                }
            };

            if (isIndeterminate)
                progressBar.Value = AdaptiveProgressBarValue.Indeterminate;

            content.Visual.BindingGeneric.Children.Add(progressBar);

            var toast = new ToastNotification(content.GetXml())
            {
                Tag = tag,
                Group = "collectionid",

                Data = new NotificationData()
            };

            return toast;
        }
    }
}
