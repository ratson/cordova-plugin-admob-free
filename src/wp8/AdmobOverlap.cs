//Copyright (c) 2014 Sang Ki Kwon (Cranberrygame)
//Email: cranberrygame@yahoo.com
//Homepage: http://cranberrygame.github.io
//License: MIT (http://opensource.org/licenses/MIT)
using System;
using System.Windows;
using System.Runtime.Serialization;
using WPCordovaClassLib;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Diagnostics; //Debug.WriteLine
//
using GoogleAds;

namespace Test {

    public class AdmobOverlap : PluginDelegate
    {
		Plugin plugin;	
		//
        protected string bannerAdUnit;
        protected string fullScreenAdUnit;
        protected bool isOverlap;
        protected bool isTest;
		//
        protected String bannerPreviousPosition;
        protected String bannerPreviousSize;
        protected int lastOrientation;
		//
        protected bool bannerAdPreload;
        protected bool fullScreenAdPreload;
		//admob
        protected AdView bannerView;
        protected InterstitialAd interstitialView;

        public AdmobOverlap(Plugin plugin_)
        {
			plugin = plugin_;
		}
	
        public void _setLicenseKey(string email, string licenseKey) {
        }

        public void _setUp(string bannerAdUnit, string fullScreenAdUnit, bool isOverlap, bool isTest)
        {
			this.bannerAdUnit = bannerAdUnit;
			this.fullScreenAdUnit = fullScreenAdUnit;
			this.isOverlap = isOverlap;
			this.isTest = isTest;
        }

        public void _preloadBannerAd()
        {
			bannerAdPreload = true;	
			
			_hideBannerAd();
			
			loadBannerAd();
        }
		
        private void loadBannerAd() {
		    if (bannerView == null) {
                if (bannerPreviousSize == null)
                {
                    //bannerPreviousSize = "SMART_BANNER";
                    bannerPreviousSize = "BANNER";
                }			
				//
				AdFormats format = AdFormats.Banner;
				//https://developers.google.com/mobile-ads-sdk/docs/admob/wp/banner		
                if (bannerPreviousSize.Equals("BANNER"))
                {
					format = AdFormats.Banner;//Banner (320x50, Phones and Tablets)
				}
                else if (bannerPreviousSize.Equals("SMART_BANNER"))
                {
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
				bannerView.ReceivedAd += bannerView_ReceivedAd;
				bannerView.FailedToReceiveAd += bannerView_FailedToReceiveAd;
				bannerView.ShowingOverlay += bannerView_ShowingOverlay;
				bannerView.LeavingApplication += bannerView_LeavingApplicationAd;
				bannerView.DismissingOverlay += bannerView_DismissingOverlay;				
			}
	 
			AdRequest adRequest = new AdRequest();
			if(isTest) {
				adRequest.ForceTesting = true;
			}					
			bannerView.LoadAd(adRequest);
        }

        public void _showBannerAd(string position, string size)
        {
            if (bannerIsShowingOverlap() && position.Equals(this.bannerPreviousPosition) && size.Equals(this.bannerPreviousSize))
            {				
				return;
			}

            this.bannerPreviousPosition = position;
            this.bannerPreviousSize = size;
			
			if(bannerAdPreload) {
				bannerAdPreload = false;
			}
			else{
				_hideBannerAd();
				
				loadBannerAd();
			}			

            addBannerViewOverlap(position, size);
			
			PluginResult pr = new PluginResult(PluginResult.Status.OK, "onBannerAdShown");
			pr.KeepCallback = true;
			plugin.DispatchCommandResult(pr);
            //PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
            //plugin.DispatchCommandResult(pr);
        }

        protected virtual bool bannerIsShowingOverlap()
        {
			bool bannerIsShowing = false;

            if (bannerView != null)	
            {
                PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                PhoneApplicationPage rootPage = rootFrame.Content as PhoneApplicationPage;
                Grid rootGrid = rootPage.FindName("LayoutRoot") as Grid;
                //rootGrid.ShowGridLines = true;
                CordovaView rootView = rootPage.FindName("CordovaView") as CordovaView;

                if (rootGrid.Children.Contains(bannerView))
                    bannerIsShowing = true;
            }
			
			return bannerIsShowing;
		}

        protected virtual void addBannerViewOverlap(String position, String size)
        {
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

            if (position.Equals("top-left"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Top;
                bannerView.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (position.Equals("top-center"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Top;
                bannerView.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (position.Equals("top-right"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Top;
                bannerView.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else if (position.Equals("left"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Center;
                bannerView.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (position.Equals("center"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Center;
                bannerView.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (position.Equals("right"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Center;
                bannerView.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else if (position.Equals("bottom-left"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Bottom;
                bannerView.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (position.Equals("bottom-center"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Bottom;
                bannerView.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (position.Equals("bottom-right"))
            {
                bannerView.VerticalAlignment = VerticalAlignment.Bottom;
                bannerView.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                bannerView.VerticalAlignment = VerticalAlignment.Top;
                bannerView.HorizontalAlignment = HorizontalAlignment.Center;
            }

            PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            PhoneApplicationPage rootPage = rootFrame.Content as PhoneApplicationPage;
            Grid rootGrid = rootPage.FindName("LayoutRoot") as Grid;
			//rootGrid.ShowGridLines = true;
            CordovaView rootView = rootPage.FindName("CordovaView") as CordovaView;
			
            rootGrid.Children.Add(bannerView);
        }

        public void _reloadBannerAd()
        {
			loadBannerAd();
        }

        public void _hideBannerAd()        
        {
            removeBannerViewOverlap();
			
			PluginResult pr = new PluginResult(PluginResult.Status.OK, "onBannerAdHidden");
			pr.KeepCallback = true;
            plugin.DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
            //plugin.DispatchCommandResult(pr);			
        }

        protected virtual void removeBannerViewOverlap()
        {
            if (bannerView == null)
                return;

            PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            PhoneApplicationPage rootPage = rootFrame.Content as PhoneApplicationPage;
            Grid rootGrid = rootPage.FindName("LayoutRoot") as Grid;
            //rootGrid.ShowGridLines = true;
            CordovaView rootView = rootPage.FindName("CordovaView") as CordovaView;

            if (rootGrid.Children.Contains(bannerView))
            {
                rootGrid.Children.Remove(bannerView);
            }
        }

        public void _preloadFullScreenAd()
        {
			fullScreenAdPreload = true;			
						
			loadFullScreenAd();
        }
		
		private void loadFullScreenAd() {
            if (interstitialView == null) {
                //interstitialView = new InterstitialAd("ca-app-pub-4906074177432504/4879304879");//x cf) wp8
                //interstitialView = new InterstitialAd("ca-app-pub-4906074177432504/5150650074");//o cf) android
                interstitialView = new InterstitialAd(this.fullScreenAdUnit);
				//http://forums.xamarin.com/discussion/849/binding-library-for-inneractive-sdk
                interstitialView.ReceivedAd += interstitialView_ReceivedAd;
                interstitialView.FailedToReceiveAd += interstitialView_FailedToReceiveAd;
                interstitialView.ShowingOverlay += interstitialView_ShowingOverlay;
				interstitialView.DismissingOverlay += interstitialView_DismissingOverlay;
            }
						
			AdRequest adRequest = new AdRequest();
			if(isTest) {
				adRequest.ForceTesting = true;
			}				
			interstitialView.LoadAd(adRequest);
		}

        public void _showFullScreenAd()
        {
			if (fullScreenAdPreload) {
				fullScreenAdPreload = false;

				//An exception of type 'System.NullReferenceException' occurred in GoogleAds.DLL and wasn't handled before a managed/native boundary
                try {
                    interstitialView.ShowAd();
                }
                catch (Exception ex) {
                }
			}
			else {
				loadFullScreenAd();
			}
        }
		
        private void bannerView_ReceivedAd(object sender, AdEventArgs e) {
            Debug.WriteLine("BannerView_ReceivedAd");

            PluginResult pr;
			if (bannerAdPreload) {
				pr = new PluginResult(PluginResult.Status.OK, "onBannerAdPreloaded");
				pr.KeepCallback = true;
                plugin.DispatchCommandResult(pr);
				//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
				//pr.KeepCallback = true;
                //plugin.DispatchCommandResult(pr);
			}
			
			pr = new PluginResult(PluginResult.Status.OK, "onBannerAdLoaded");
			pr.KeepCallback = true;
            plugin.DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
            //plugin.DispatchCommandResult(pr);
        }
		
        private void bannerView_FailedToReceiveAd(object sender, AdErrorEventArgs errorCode) {
            Debug.WriteLine("BannerView_FailedToReceiveAd " + errorCode.ErrorCode);
        }
		
        private void bannerView_ShowingOverlay(object sender, AdEventArgs e) {
            Debug.WriteLine("BannerView_ShowingOverlay");//click and ad opened //onBannerAdShown x
        }
		
		private void bannerView_LeavingApplicationAd(object sender, AdEventArgs args)
		{
			Debug.WriteLine("bannerView_LeavingApplicationAd");
		}
		
        private void bannerView_DismissingOverlay(object sender, AdEventArgs e) {
            Debug.WriteLine("BannerView_DismissingOverlay");//onBannerAdHidden x
        }		
		
        private void interstitialView_ReceivedAd(object sender, AdEventArgs e) {
            Debug.WriteLine("interstitialView_ReceivedAd");

            PluginResult pr;
			if(fullScreenAdPreload) {
				pr = new PluginResult(PluginResult.Status.OK, "onFullScreenAdPreloaded");
				pr.KeepCallback = true;
                plugin.DispatchCommandResult(pr);
				//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
				//pr.KeepCallback = true;
				//plugin.DispatchCommandResult(pr);
			}
			
			pr = new PluginResult(PluginResult.Status.OK, "onFullScreenAdLoaded");
			pr.KeepCallback = true;
			plugin.DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
			//plugin.DispatchCommandResult(pr);
				
			if(!fullScreenAdPreload) {
				interstitialView.ShowAd();
			}
        }
		
        private void interstitialView_FailedToReceiveAd(object sender, AdErrorEventArgs errorCode) {
            Debug.WriteLine("interstitialView_FailedToReceiveAd " + errorCode.ErrorCode);
        }
		
        private void interstitialView_ShowingOverlay(object sender, AdEventArgs e) {
            Debug.WriteLine("OnInterstitialViewPresentScreen");
			
			PluginResult pr = new PluginResult(PluginResult.Status.OK, "onFullScreenAdShown");
			pr.KeepCallback = true;
            plugin.DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
            //plugin.DispatchCommandResult(pr);			
        }
		
        private void interstitialView_DismissingOverlay(object sender, AdEventArgs e) {
            Debug.WriteLine("interstitialView_DismissingOverlay");	
			
			PluginResult pr = new PluginResult(PluginResult.Status.OK, "onFullScreenAdHidden");
			pr.KeepCallback = true;
            plugin.DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
            //plugin.DispatchCommandResult(pr);			
        }
	}
}