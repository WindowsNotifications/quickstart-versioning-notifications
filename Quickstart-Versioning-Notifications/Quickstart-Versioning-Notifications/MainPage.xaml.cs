using NotificationsExtensions.Tiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Quickstart_Versioning_Notifications
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private static Version GetOSVersion()
        {
            // The DeviceFamilyVersion is a string, which is actually a ulong number representing the version
            // https://www.suchan.cz/2015/08/uwp-quick-tip-getting-device-os-and-app-info/

            ulong versionAsLong = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);

            ulong v1 = (versionAsLong & 0xFFFF000000000000L) >> 48;
            ulong v2 = (versionAsLong & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (versionAsLong & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (versionAsLong & 0x000000000000FFFFL);

            return new Version((int)v1, (int)v2, (int)v3, (int)v4);
        }

        private void ButtonSendTileNotification_Click(object sender, RoutedEventArgs e)
        {
            var version = GetOSVersion();
            var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;

            TileContent content;

            // If they're running 10586 or newer
            if (version > new Version(10, 0, 10586, 0))
            {
                content = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileMedium = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                                {
                                    new TileText()
                                    {
                                        Text = $"Build {version.Build}",
                                        Wrap = true
                                    },

                                    new TileText()
                                    {
                                        Text = $"Family: {deviceFamily}",
                                        Wrap = true
                                    }
                                },

                                // We include the peek image here
                                PeekImage = new TilePeekImage()
                                {
                                    Source = new TileImageSource("Assets/map.jpg"),
                                    Overlay = 20
                                }
                            }
                        }
                    }
                };
            }

            // Otherwise assume they're running 10240
            else
            {
                content = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileMedium = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                                {
                                    new TileText()
                                    {
                                        Text = $"Build {version.Build}",
                                        Wrap = true
                                    },

                                    new TileText()
                                    {
                                        Text = $"Family: {deviceFamily}",
                                        Wrap = true
                                    }
                                }

                                // No peek image
                            }
                        }
                    }
                };
            }

            TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(content.GetXml()));
        }
    }
}
