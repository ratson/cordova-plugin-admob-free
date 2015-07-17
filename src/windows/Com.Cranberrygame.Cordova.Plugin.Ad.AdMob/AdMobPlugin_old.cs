//Copyright (c) 2014 Sang Ki Kwon (Cranberrygame)
//Email: cranberrygame@yahoo.com
//Homepage: http://cranberrygame.github.io
//License: MIT (http://opensource.org/licenses/MIT)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics; //Debug.WriteLine
using Windows.UI.Core;//CoreDispatcher.RunAsync
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
//


namespace Com.Cranberrygame.Cordova.Plugin.Ad.AdMob
{
    public sealed class AdMobPlugin
    {

        //
        private static string appKey;
        private static string appSecret;
		//desktopad
        private static Banner bannerView;
		
        public static string echo(string str)
        {
            return str+"echo";
        }

		//////////////////////
		
        public static void setLicenseKey(string email, string licenseKey)
		{
            Debug.WriteLine("email: " + email);
            Debug.WriteLine("licenseKey: " + licenseKey);
			
			// Your UI update code goes here!
            //http://stackoverflow.com/questions/19341591/the-application-called-an-interface-that-was-marshalled-for-a-different-thread
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {                
                _setLicenseKey(email, licenseKey);
            });			
        }

        public static void setUp(string appKey, string appSecret, bool isOverlap, bool isTest)
        {
            //Debug.WriteLine("appKey: " + appKey);
            //Debug.WriteLine("appSecret: " + appSecret);
            //Debug.WriteLine("isOverlap: " + isOverlap);
            //Debug.WriteLine("isTest: " + isTest);
			Debug.WriteLine("appKey: " + appKey);
            Debug.WriteLine("appSecret: " + appSecret);
            Debug.WriteLine("isOverlap: " + isOverlap);
            Debug.WriteLine("isTest: " + isTest);	
			
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                _setUp(appKey, appSecret, isOverlap, isTest);
            });			
        }
		
		//
        public static void preloadBannerAd()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                _preloadBannerAd();
            });
        }

		public static void showBannerAd(string position, int size)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                _showBannerAd(position, size);
            });            
        }
		
        public static void reloadBannerAd()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
				_reloadBannerAd();
            });	
		}

        public static void hideBannerAd()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
				_hideBannerAd();
            });	
		}
		
        public static void preloadPopupAd()
        {			
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
				_preloadPopupAd();
            });			
        }
		
		public static void showPopupAd()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                _showPopupAd();
            });
        }
		
		//
		public static void _setLicenseKey(string email, string licenseKey)
		{
            	
        }
		
		public static void _setUp(string appKey, string appSecret, bool isOverlap, bool isTest)
		{
            AdMobPlugin.appKey = appKey;
            AdMobPlugin.appSecret = appSecret;
		}
		
        public static void _preloadBannerAd()
        {
            loadBannerAd();
        }

		
        private static void loadBannerAd()
        {		
/*
//MainPage.xaml
<Page
    x:Class="App1.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:App2"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d">

    <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">

    </Grid>
</Page>
//MainPage.xaml.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace App1
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
//MainPage.xaml
<Page
	x:Class="App1.MainPage"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:local="using:App1"
	xmlns:DesktopAd="using:DesktopAd"
	xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
	xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
	mc:Ignorable="d">

	<Grid Background="#FF835D5D" >
		<Button Content="Set myad as Transverse ad" HorizontalAlignment="Left" Margin="75,73,0,0" VerticalAlignment="Top" Click="Button_Transverse_Click" Height="40" Width="316"/>
		<Button Content="Set myad as Vertical ad" HorizontalAlignment="Left" Margin="75,122,0,0" VerticalAlignment="Top" Height="40" Width="316" Click="Button_Vertical_Click"/>
		<Button Content="Set myad as HalfTransverse ad" HorizontalAlignment="Left" Margin="75,172,0,0" VerticalAlignment="Top" Height="40" Width="316" Click="Button_HalfTransverse_Click"/>

		<Button Content="Open a Popup ad" HorizontalAlignment="Left" Margin="75,223,0,0" VerticalAlignment="Top" Click="Button_OpenPopup_Click" Width="316"/>

		<DesktopAd:Banner Name="myad" Margin="200,200,0,0" AppKey="Your appkey" AppSecret="Your appsecret" AdStyle="Transverse" ></DesktopAd:Banner>
	</Grid>
</Page>
*/

            Debug.WriteLine("appKey: " + appKey);
            Debug.WriteLine("appSecret: " + appSecret);

            if (bannerView == null) {
				//
				bannerView = new Banner();
				//bannerView.Margin="200,200,0,0";//
                bannerView.AppKey = appKey;
                bannerView.AppSecret = appSecret;				
				bannerView.AdStyle = BannerStyle.Transverse;
				//bannerView.AdStyle = BannerStyle.MinTransverse;
				//bannerView.AdStyle = BannerStyle.Vertical;
				bannerView.BannerError += bannerView_BannerError;
			}				
			
			bannerView.Load();
		}

        public static void _showBannerAd(string position, int size)
        {
            if (position.Equals("top-left"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Top;
                bannerView.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (position.Equals("top-center"))	{
                bannerView.VerticalAlignment = VerticalAlignment.Top;
                bannerView.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (position.Equals("top-right")) {
                bannerView.VerticalAlignment = VerticalAlignment.Top;
                bannerView.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else if (position.Equals("left")) {
                bannerView.VerticalAlignment = VerticalAlignment.Center;
                bannerView.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (position.Equals("center")) { 
                bannerView.VerticalAlignment = VerticalAlignment.Center;
                bannerView.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (position.Equals("right")) {
                bannerView.VerticalAlignment = VerticalAlignment.Center;
                bannerView.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else if (position.Equals("bottom-left")) {
                bannerView.VerticalAlignment = VerticalAlignment.Bottom;
                bannerView.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (position.Equals("bottom-center")) {
                bannerView.VerticalAlignment = VerticalAlignment.Bottom;
                bannerView.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (position.Equals("bottom-right")) {
                bannerView.VerticalAlignment = VerticalAlignment.Bottom;
                bannerView.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else {
                bannerView.VerticalAlignment = VerticalAlignment.Top;
                bannerView.HorizontalAlignment = HorizontalAlignment.Center;
            }

            Frame rootFrame = Window.Current.Content as Frame;
            Page rootPage = rootFrame.Content as Page;
            Grid rootGrid = rootPage.Content as Grid;
            rootGrid.Children.Add(bannerView);
        }

        public static void _reloadBannerAd()
        {
		
		}

        public static void _hideBannerAd()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            Page rootPage = rootFrame.Content as Page;
            Grid rootGrid = rootPage.Content as Grid;
            if (rootGrid.Children.Contains(bannerView))
            {
                rootGrid.Children.Remove(bannerView);
            }
		}
		
        private static void bannerView_BannerError(object sender, ErrorTypeEnum e)
        {
            string errorstr = "";
            switch (e)
            {
                case ErrorTypeEnum.NetworkErrorOrServerError:
                    errorstr = "Network Error";
                    break;
                case ErrorTypeEnum.AppKeyOrAppSecretError:
                    errorstr = "Appkey or Appsecret Error";
                    break;
                case ErrorTypeEnum.ResponseError:
                    errorstr = "Service Error";
                    break;
                case ErrorTypeEnum.Unknown:
                    break;
                default:
                    break;
            }
        }
		
		//
        public static void _preloadPopupAd()
        {
            loadPopupAd();
        }

        private static void loadPopupAd()
        {

		}

        private static void _showPopupAd()
        {
            Popup popup = new Popup();
            //BannerPopup.Create(popup, appKey, appSecret);
			BannerPopup.Create(popup, "Your appkey", "Your appscret");
        }	
    }

}

////////////////////////

/*
        private void _preloadBannerAd() {
			bannerAdPreload = true;	
			
			_hideBannerAd();
			
			loadBannerAd();
        }
		
        private void loadBannerAd() {
		    if (bannerView == null) {
				if(size == null) {
					size = "SMART_BANNER";
				}			
				//
				AdFormats format = AdFormats.Banner;
				//https://developers.google.com/mobile-ads-sdk/docs/admob/wp/banner		
				if (size.Equals("BANNER")) {
					format = AdFormats.Banner;//Banner (320x50, Phones and Tablets)
				}
				else if (size.Equals("SMART_BANNER")) {
					format = AdFormats.SmartBanner;//Smart banner (Auto size, Phones and Tablets) //https://developers.google.com/mobile-ads-sdk/docs/admob/android/banner#smart
				} 				
				else {
					format = AdFormats.SmartBanner;
				}
				//
				bannerView = new AdView
				{
					//Format = AdFormats.Banner,
					//Format = AdFormats.SmartBanner,
					Format = format,
					AdUnitID = this.bannerAdUnit
				};
				bannerView.ReceivedAd += OnBannerViewReceivedAd;
				bannerView.FailedToReceiveAd += OnBannerViewFailedToReceiveAd;
				bannerView.ShowingOverlay += OnBannerViewShowingOverlay;
				bannerView.DismissingOverlay += OnBannerViewDismissingOverlay;				
			}
	 
			AdRequest adRequest = new AdRequest();
			if(isTest) {
				adRequest.ForceTesting = true;
			}					
			bannerView.LoadAd(adRequest);
        }		
		
        private void _showBannerAd(string position, string size) {
			
			if (bannerIsShowing() && position.Equals(this.previousBannerPosition) && size.Equals(this.previousBannerSize)) {				
				return;
			}
				
			this.previousBannerPosition = position;
			this.previousBannerSize = size;
			
			if(bannerAdPreload) {
				bannerAdPreload = false;
			}
			else{
				_hideBannerAd();
				
				loadBannerAd();
			}			
*/
			
/*
//D:\share\cordova_test\testapp\platforms\wp8\MainPage.xaml
<?xml version='1.0' encoding='utf-8'?>
<phone:PhoneApplicationPage Background="Black" FontFamily="{StaticResource PhoneFontFamilyNormal}" FontSize="{StaticResource PhoneFontSizeNormal}" Foreground="{StaticResource PhoneForegroundBrush}" Orientation="portrait" SupportedOrientations="portrait" d:DesignHeight="768" d:DesignWidth="480" mc:Ignorable="d" shell:SystemTray.IsVisible="True" x:Class="com.cranberrygame.adrotatortest.MainPage" xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:d="http://schemas.microsoft.com/expression/blend/2008" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" xmlns:my="clr-namespace:WPCordovaClassLib" xmlns:phone="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone" xmlns:shell="clr-namespace:Microsoft.Phone.Shell;assembly=Microsoft.Phone" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Grid Background="Transparent" HorizontalAlignment="Stretch" x:Name="LayoutRoot">
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <my:CordovaView HorizontalAlignment="Stretch" Margin="0,0,0,0" VerticalAlignment="Stretch" x:Name="CordovaView" />
    </Grid>
</phone:PhoneApplicationPage>

//D:\share\cordova_test\testapp\platforms\wp8\MainPage.xaml.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace com.cranberrygame.adrotatortest
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.CordovaView.Loaded += CordovaView_Loaded;
        }

        private void CordovaView_Loaded(object sender, RoutedEventArgs e)
        {
            this.CordovaView.Loaded -= CordovaView_Loaded;
        }
    }
}

//D:\share\cordova_test\testapp\platforms\wp8\MainPage.xaml
<?xml version='1.0' encoding='utf-8'?>
<phone:PhoneApplicationPage Background="Black" FontFamily="{StaticResource PhoneFontFamilyNormal}" FontSize="{StaticResource PhoneFontSizeNormal}" Foreground="{StaticResource PhoneForegroundBrush}" Orientation="portrait" SupportedOrientations="portrait" d:DesignHeight="768" d:DesignWidth="480" mc:Ignorable="d" shell:SystemTray.IsVisible="True" x:Class="com.cranberrygame.adrotatortest.MainPage" xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:d="http://schemas.microsoft.com/expression/blend/2008" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" xmlns:my="clr-namespace:WPCordovaClassLib" xmlns:phone="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone" xmlns:shell="clr-namespace:Microsoft.Phone.Shell;assembly=Microsoft.Phone" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Grid Background="Transparent" HorizontalAlignment="Stretch" x:Name="LayoutRoot">
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <my:CordovaView HorizontalAlignment="Stretch" Margin="0,0,0,0" VerticalAlignment="Stretch" x:Name="CordovaView" />
		<!-- cranberrygame start -->
		<GoogleAds:AdView AdUnitID="YOUR_AD_UNIT_ID" HorizontalAlignment="Left" VerticalAlignment="Top" Width="480" Height="80" Margin="-10,527,-14,0" />
		<!-- cranberrygame end -->
    </Grid>
</phone:PhoneApplicationPage>
*/
	
/*	
			if (isOverlap) {
				PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
				if (frame != null) {
					PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
					if (page != null) {
						Grid grid = page.FindName("LayoutRoot") as Grid;
						if (grid != null) {
							if (position.Equals("top-left")) {
								bannerView.VerticalAlignment = VerticalAlignment.Top;
								bannerView.HorizontalAlignment = HorizontalAlignment.Left;
							}
							else if (position.Equals("top-center"))	{
								bannerView.VerticalAlignment = VerticalAlignment.Top;
								bannerView.HorizontalAlignment = HorizontalAlignment.Center;
							}
							else if (position.Equals("top-right")) {
								bannerView.VerticalAlignment = VerticalAlignment.Top;
								bannerView.HorizontalAlignment = HorizontalAlignment.Right;
							}
							else if (position.Equals("left")) {
								bannerView.VerticalAlignment = VerticalAlignment.Center;
								bannerView.HorizontalAlignment = HorizontalAlignment.Left;
							}
							else if (position.Equals("center")) { 
								bannerView.VerticalAlignment = VerticalAlignment.Center;
								bannerView.HorizontalAlignment = HorizontalAlignment.Center;
							}
							else if (position.Equals("right")) {
								bannerView.VerticalAlignment = VerticalAlignment.Center;
								bannerView.HorizontalAlignment = HorizontalAlignment.Right;
							}
							else if (position.Equals("bottom-left")) {
								bannerView.VerticalAlignment = VerticalAlignment.Bottom;
								bannerView.HorizontalAlignment = HorizontalAlignment.Left;
							}
							else if (position.Equals("bottom-center")) {
								bannerView.VerticalAlignment = VerticalAlignment.Bottom;
								bannerView.HorizontalAlignment = HorizontalAlignment.Center;
							}
							else if (position.Equals("bottom-right")) {
								bannerView.VerticalAlignment = VerticalAlignment.Bottom;
								bannerView.HorizontalAlignment = HorizontalAlignment.Right;
							}
							else {
								bannerView.VerticalAlignment = VerticalAlignment.Top;
								bannerView.HorizontalAlignment = HorizontalAlignment.Center;
							}

							grid.Children.Add(bannerView);
						}
					}
				}				
			}
			else {
				
			}
			
			PluginResult pr = new PluginResult(PluginResult.Status.OK, "onBannerAdShown");
			pr.KeepCallback = true;
			DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
			//DispatchCommandResult(pr);			
        }
      	
		protected boolean bannerIsShowingOverlap() {
			bool bannerIsShowing = false;
			//if banner is showing
			if (bannerView != null)	{
				PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
				if (frame != null) {
					PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
					if (page != null) {
						Grid grid = page.FindName("LayoutRoot") as Grid;							
						if (grid != null) {
							if (grid.Children.Contains(bannerView))	{
								bannerIsShowing = true;
							}									
						}
					}
				}
			}
			
			return bannerIsShowing;
		}
		
        private void _reloadBannerAd() {
			loadBannerAd();
        }
		
        private void _hideBannerAd() {
            removeBannerViewOverlap();
			
			PluginResult pr = new PluginResult(PluginResult.Status.OK, "onBannerAdHidden");
			pr.KeepCallback = true;
			DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
			//DispatchCommandResult(pr);			
        }
 	
        private void removeBannerViewOverlap() {
            if (bannerView == null)
				return
			
			PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (frame != null) {
				PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
				if (page != null) {
					Grid grid = page.FindName("LayoutRoot") as Grid;
					if (grid != null) {
						if (grid.Children.Contains(bannerView))	{
							grid.Children.Remove(bannerView);
						}
					}
				}
			}				
        }
*/		
