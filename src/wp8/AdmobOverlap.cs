//Copyright (c) 2014 Sang Ki Kwon (Cranberrygame)
//Email: cranberrygame@yahoo.com
//Homepage: http://cranberrygame.github.io
//License: MIT (http://opensource.org/licenses/MIT)
using System;
using System.Windows;
using System.Runtime.Serialization;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;
using System.Diagnostics; //Debug.WriteLine
//
using GoogleAds;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace Cordova.Extension.Commands {
    public class AdmobOverlap : BaseCommand {
		protected Plugin plugin;	
		//
		private string adUnitBanner;
		private string adUnitFullScreen;
		private bool isOverlap;
		private bool isTest;
		//
		private string previousBannerPosition;
		private string previousBannerSize;
		private int lastOrientation;
		//
		public bool bannerAdPreload;	
		public bool fullScreenAdPreload;
		//admob
        private AdView bannerView;
        private InterstitialAd interstitialView;
			
		public AdmobOverlap(Plugin plugin_) {
			plugin = plugin_;
		}
	
        private void _setLicenseKey(string email, string licenseKey) {
        }
		
        private void _setUp(string adUnitBanner, string adUnitFullScreen, bool isOverlap, bool isTest) {
			this.adUnitBanner = adUnitBanner;
			this.adUnitFullScreen = adUnitFullScreen;
			this.isOverlap = isOverlap;
			this.isTest = isTest;
        }
		
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
					AdUnitID = this.adUnitBanner
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
		
        private void _preloadFullScreenAd() {
			fullScreenAdPreload = true;			
						
			loadFullScreenAd();
        }
		
		private void loadFullScreenAd() {
            if (interstitialView == null) {
                //interstitialView = new InterstitialAd("ca-app-pub-4906074177432504/4879304879");//x cf) wp8
                //interstitialView = new InterstitialAd("ca-app-pub-4906074177432504/5150650074");//o cf) android
                interstitialView = new InterstitialAd(this.adUnitFullScreen);
				//http://forums.xamarin.com/discussion/849/binding-library-for-inneractive-sdk
                interstitialView.ReceivedAd += OnInterstitialViewReceivedAd;
                interstitialView.FailedToReceiveAd += OnInterstitialViewFailedToReceiveAd;
                interstitialView.ShowingOverlay += OnInterstitialViewShowingOverlay;
				interstitialView.DismissingOverlay += OnInterstitialViewDismissingOverlay;
            }
						
			AdRequest adRequest = new AdRequest();
			if(isTest) {
				adRequest.ForceTesting = true;
			}				
			interstitialView.LoadAd(adRequest);
		}
		
        private void _showFullScreenAd() {
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
		
		//bannerView.ReceivedAd		
        private void OnBannerViewReceivedAd(object sender, AdEventArgs e) {
            Debug.WriteLine("OnBannerViewReceivedAd");

            PluginResult pr;
			if (bannerAdPreload) {
				pr = new PluginResult(PluginResult.Status.OK, "onBannerAdPreloaded");
				pr.KeepCallback = true;
				DispatchCommandResult(pr);
				//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
				//pr.KeepCallback = true;
				//DispatchCommandResult(pr);
			}
			
			pr = new PluginResult(PluginResult.Status.OK, "onBannerAdLoaded");
			pr.KeepCallback = true;
			DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
			//DispatchCommandResult(pr);
        }
		
		//bannerView.FailedToReceiveAd
        private void OnBannerViewFailedToReceiveAd(object sender, AdErrorEventArgs errorCode) {
            Debug.WriteLine("OnBannerViewFailedToReceiveAd " + errorCode.ErrorCode);
        }
		
		//bannerView.ShowingOverlay
        private void OnBannerViewShowingOverlay(object sender, AdEventArgs e) {
            Debug.WriteLine("OnBannerViewShowingOverlay");//click and ad opened //onBannerAdShown x
        }
		
		//bannerView.DismissingOverlay
        private void OnBannerViewDismissingOverlay(object sender, AdEventArgs e) {
            Debug.WriteLine("OnBannerViewDismissingOverlay");//onBannerAdHidden x
        }		
		
		//interstitialView.ReceivedAd
        private void OnInterstitialViewReceivedAd(object sender, AdEventArgs e) {
            Debug.WriteLine("OnInterstitialViewReceivedAd");

            PluginResult pr;
			if(fullScreenAdPreload) {
				pr = new PluginResult(PluginResult.Status.OK, "onFullScreenAdPreloaded");
				pr.KeepCallback = true;
				DispatchCommandResult(pr);
				//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
				//pr.KeepCallback = true;
				//DispatchCommandResult(pr);
			}
			
			pr = new PluginResult(PluginResult.Status.OK, "onFullScreenAdLoaded");
			pr.KeepCallback = true;
			DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
			//DispatchCommandResult(pr);
				
			if(!fullScreenAdPreload) {
				interstitialView.ShowAd();
			}
        }
		
		//interstitialView.FailedToReceiveAd
        private void OnInterstitialViewFailedToReceiveAd(object sender, AdErrorEventArgs errorCode) {
            Debug.WriteLine("OnInterstitialViewFailedToReceiveAd " + errorCode.ErrorCode);
        }
		
        //interstitialView.ShowingOverlay
        private void OnInterstitialViewShowingOverlay(object sender, AdEventArgs e) {
            Debug.WriteLine("OnInterstitialViewPresentScreen");
			
			PluginResult pr = new PluginResult(PluginResult.Status.OK, "onFullScreenAdShown");
			pr.KeepCallback = true;
			DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
			//DispatchCommandResult(pr);			
        }
		
        //interstitialView.DismissingOverlay
        private void OnInterstitialViewDismissingOverlay(object sender, AdEventArgs e) {
            Debug.WriteLine("OnInterstitialViewDismissScreen");	
			
			PluginResult pr = new PluginResult(PluginResult.Status.OK, "onFullScreenAdHidden");
			pr.KeepCallback = true;
			DispatchCommandResult(pr);
			//PluginResult pr = new PluginResult(PluginResult.Status.ERROR);
			//pr.KeepCallback = true;
			//DispatchCommandResult(pr);			
        }
	}
}