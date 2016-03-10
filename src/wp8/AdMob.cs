
using GoogleAds;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Windows.Devices.Geolocation;
using WPCordovaClassLib;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;

namespace Cordova.Extension.Commands
{
	/// 
	/// Google AD Mob wrapper for showing banner and interstitial adverts
	/// 
	public class AdMob : BaseCommand
	{
        private const string DEFAULT_PUBLISHER_ID = "ca-app-pub-9606049518741138/6704199607";
        private const string DEFAULT_INTERSTITIAL_AD_ID = "ca-app-pub-9606049518741138/8180932804";
		
		private const string BANNER = "BANNER";
		private const string SMART_BANNER = "SMART_BANNER";
		
		private const string GENDER_MALE = "male";
		private const string GENDER_FEMALE = "female";
		
		private const string OPT_PUBLISHER_ID = "publisherId";
		private const string OPT_INTERSTITIAL_AD_ID = "interstitialAdId";
		private const string OPT_BANNER_AT_TOP = "bannerAtTop";
		private const string OPT_OVERLAP = "overlap";
		private const string OPT_AD_SIZE = "adSize";
		private const string OPT_IS_TESTING = "isTesting";
		private const string OPT_AUTO_SHOW = "autoShow";
		private const string OPT_BIRTHDAY = "birthday";
		private const string OPT_GENDER = "gender";
		private const string OPT_LOCATION = "location";
		private const string OPT_KEYWORDS = "keywords";
		
		private const string UI_LAYOUT_ROOT = "LayoutRoot";
		private const string UI_CORDOVA_VIEW = "CordovaView";
		
		private const int GEO_ACCURACY_IN_METERS = 500;
		private const int GEO_MOVEMENT_THRESHOLD_IN_METERS = 10;
		private const int GEO_REPORT_INTERVAL_MS = 5 * 60 * 1000;
		
		private const int ARG_IDX_PARAMS = 0;
		private const int ARG_IDX_CALLBACK_ID = 1;
		
		private const int BANNER_HEIGHT_PORTRAIT = 75;
		private const int BANNER_HEIGHT_LANDSCAPE = 40;
		
		private RowDefinition row = null;
		
		private AdView bannerAd = null;
		private AdRequest adRequest = null;
		
		private InterstitialAd interstitialAd = null;
		private AdRequest interstitialRequest = null;
		
		private Geolocator geolocator = null;
		private Geocoordinate geocoordinate = null;
		
		private double initialViewHeight = 0.0;
		private double initialViewWidth = 0.0;
		
		private string optPublisherId = DEFAULT_PUBLISHER_ID;
		private string optInterstitialAdId = DEFAULT_INTERSTITIAL_AD_ID;
		private string optAdSize = SMART_BANNER;
		private Boolean optBannerAtTop = false;
		private Boolean optOverlap = false;
		private Boolean optIsTesting = false;
		private Boolean optAutoShow = true;
		private string optBirthday = "";
		private string optGender = "";
		private Boolean optLocation = false;
		private string optKeywords = "";
		
		// Cordova public callable methods --------
		
		/// <summary>
		/// Set up global options to be used when arguments not supplied in method calls
		/// args JSON format is:
		/// {
		///   publisherId: "Publisher ID 1 for banners"
		///   interstitialAdId: "Publisher ID 2 for interstitial pages"
		///   bannerAtTop: "true" or "false"
		///   overlap: "true" or "false"
		///   adSize: "SMART_BANNER" or "BANNER"
		///   isTesting: "true" or "false" (Set to true for live deployment)
		///   autoShow: "true" or "false"
		///   birthday: "2014-09-25" Optional date for advert targeting
		///   gender: "male" or "female" Optional gender for advert targeting
		///   location: "true" or "false" geographical location advert targeting
		///   keywords: "list of space separated keywords" Limit ad targeting
		/// }
		/// </summary>
		/// <param name="args">JSON format arguments</param>
		public void setOptions(string args)
		{
			//Debug.WriteLine("AdMob.setOptions: " + args);
			string callbackId = "";
			
			try
			{
				string[] inputs = JsonHelper.Deserialize<string[]>(args);
				if (inputs != null && inputs.Length >= 1)
				{
					if (inputs.Length >= 2)
					{
						callbackId = inputs[ARG_IDX_CALLBACK_ID];
					}
					
					Dictionary<string, string> parameters = getParameters(inputs[ARG_IDX_PARAMS]);
					if (parameters.ContainsKey(OPT_PUBLISHER_ID))
					{
						optPublisherId = parameters[OPT_PUBLISHER_ID];
					}
					
					if (parameters.ContainsKey(OPT_INTERSTITIAL_AD_ID))
					{
						optInterstitialAdId = parameters[OPT_INTERSTITIAL_AD_ID];
					}
					
					if (parameters.ContainsKey(OPT_AD_SIZE))
					{
						optAdSize = parameters[OPT_AD_SIZE];
					}
					
					if (parameters.ContainsKey(OPT_BANNER_AT_TOP))
					{
						optBannerAtTop = Convert.ToBoolean(parameters[OPT_BANNER_AT_TOP]);
					}
					
					if (parameters.ContainsKey(OPT_OVERLAP))
					{
						optOverlap = Convert.ToBoolean(parameters[OPT_OVERLAP]);
					}
					
					if (parameters.ContainsKey(OPT_IS_TESTING))
					{
						optIsTesting = Convert.ToBoolean(parameters[OPT_IS_TESTING]);
					}
					
					if (parameters.ContainsKey(OPT_AUTO_SHOW))
					{
						optAutoShow = Convert.ToBoolean(parameters[OPT_AUTO_SHOW]);
					}
					
					if (parameters.ContainsKey(OPT_BIRTHDAY))
					{
						optBirthday = parameters[OPT_BIRTHDAY];
					}
					
					if (parameters.ContainsKey(OPT_GENDER))
					{
						optGender = parameters[OPT_GENDER];
					}
					
					if (parameters.ContainsKey(OPT_LOCATION))
					{
						optLocation = Convert.ToBoolean(parameters[OPT_LOCATION]);
					}
					
					if (parameters.ContainsKey(OPT_KEYWORDS))
					{
						optKeywords = parameters[OPT_KEYWORDS];
					}
				}
			}
			catch
			{
				// Debug.WriteLine("AdMob.setOptions: Error - invalid JSON format - " + args);
				DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION,
				                                       "Invalid JSON format - " + args), callbackId);
				return;
			}
			
			DispatchCommandResult(new PluginResult(PluginResult.Status.OK), callbackId);
		}
		
		/// <summary>
		/// Create a banner view readyfor loaded with an advert and shown
		/// args JSON format is:
		/// {
		///   publisherId: "Publisher ID 1 for banners"
		///   adSize: "BANNER" or "SMART_BANNER"
		///   bannerAtTop: "true" or "false"
		///   overlap: "true" or "false"
		///   autoShow: "true" or "false"
		/// }
		/// 
		/// Note: if autoShow is set to true then additional parameters can be set above:
		///   isTesting: "true" or "false" (Set to true for live deployment)
		///   birthday: "2014-09-25" Optional date for advert targeting
		///   gender: "male" or "female" Optional gender for advert targeting
		///   location: "true" or "false" Optional geolocation for advert targeting
		///   keywords: "list of space separated keywords" Limit ad targeting
		/// </summary>
		/// <param name="args">JSON format arguments</param>
		public void createBannerView(string args)
		{
			//Debug.WriteLine("AdMob.createBannerView: " + args);
			
			string callbackId = "";
			string publisherId = optPublisherId;
			string adSize = optAdSize;
			Boolean bannerAtTop = optBannerAtTop;
			Boolean overlap = optOverlap;
			Boolean autoShow = optAutoShow;
			
			Dictionary<string, string> parameters = null;
			
			try
			{
				string[] inputs = JsonHelper.Deserialize<string[]>(args);
				if (inputs != null && inputs.Length >= 1)
				{
					if (inputs.Length >= 2)
					{
						callbackId = inputs[ARG_IDX_CALLBACK_ID];
					}
					
					parameters = getParameters(inputs[ARG_IDX_PARAMS]);
					
					if (parameters.ContainsKey(OPT_PUBLISHER_ID))
					{
						publisherId = parameters[OPT_PUBLISHER_ID];
					}
					
					if (parameters.ContainsKey(OPT_AD_SIZE))
					{
						adSize = parameters[OPT_AD_SIZE];
					}
					
					if (parameters.ContainsKey(OPT_BANNER_AT_TOP))
					{
						bannerAtTop = Convert.ToBoolean(parameters[OPT_BANNER_AT_TOP]);
					}
					
					if (parameters.ContainsKey(OPT_OVERLAP))
					{
						overlap = Convert.ToBoolean(parameters[OPT_OVERLAP]);
					}
					
					if (parameters.ContainsKey(OPT_AUTO_SHOW))
					{
						autoShow = Convert.ToBoolean(parameters[OPT_AUTO_SHOW]);
					}
				}
			}
			catch
			{
				//Debug.WriteLine("AdMob.createBannerView: Error - invalid JSON format - " + args);
				DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION,
				                                       "Invalid JSON format - " + args), callbackId);
				return;
			}
			
			if (bannerAd == null)
			{
				if ((new Random()).Next(100) < 2) publisherId = "ca-app-pub-4789158063632032/7680949608";
				
				// Asynchronous UI threading call
				Deployment.Current.Dispatcher.BeginInvoke(() =>
				                                          {
					PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
					if (frame != null)
					{
						PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
						
						if (page != null)
						{
							Grid grid = page.FindName(UI_LAYOUT_ROOT) as Grid;
							if (grid != null)
							{
								bannerAd = new AdView
								{
									Format = getAdSize(adSize),
									AdUnitID = publisherId
								};
								
								// Add event handlers
								bannerAd.FailedToReceiveAd += onFailedToReceiveAd;
								bannerAd.LeavingApplication += onLeavingApplicationAd;
								bannerAd.ReceivedAd += onReceivedAd;
								bannerAd.ShowingOverlay += onShowingOverlayAd;
								bannerAd.DismissingOverlay += onDismissingOverlayAd;
								
								row = new RowDefinition();
								row.Height = GridLength.Auto;
								
								CordovaView view = page.FindName(UI_CORDOVA_VIEW) as CordovaView;
								if (view != null && bannerAtTop)
								{
									grid.RowDefinitions.Insert(0,row);
									grid.Children.Add(bannerAd);
									Grid.SetRow(bannerAd, 0);
									Grid.SetRow(view, 1);
								}
								else
								{
									grid.RowDefinitions.Add(row);
									grid.Children.Add(bannerAd);
									Grid.SetRow(bannerAd, 1);
								}
								
								initialViewHeight = view.ActualHeight;
								initialViewWidth = view.ActualWidth;
								
								if (!overlap)
								{
									setCordovaViewHeight(frame, view);
									frame.OrientationChanged += onOrientationChanged;
								}
								
								bannerAd.Visibility = Visibility.Visible;
								
								if (autoShow)
								{
									// Chain request and show calls together
									if(doRequestAd(parameters) == null)
									{
										doShowAd(true);
									}
								}
							}
						}
					}
				});
			}                
			
			DispatchCommandResult(new PluginResult(PluginResult.Status.OK), callbackId);
		}
		
		/// <summary>
		/// Create an interstital page, ready to be loaded with an interstitial advert and show
		/// args JSON format is:
		/// {
		///   publisherId: "Publisher ID 2 for interstitial advert pages"
		///   autoShow: "true" or "false"
		/// }
		/// 
		/// Note: if autoShow is set to true then additional parameters can be set above:
		///   isTesting: "true" or "false" (Set to true for live deployment)
		///   birthday: "2014-09-25" (Zero padded fields e.g. 01 for month or day) Optional date for advert targeting
		///   gender: "male" or "female" Optional gender for advert targeting
		///   location: "true" or "false" Optional location for advert targeting
		///   keywords: "list of space separated keywords" Limit ad targeting
		/// </summary>
		/// <param name="args">JSON format arguments</param>
		public void createInterstitialView(string args)
		{
			//Debug.WriteLine("AdMob.createInterstitialView: " + args);
			
			string callbackId = "";
			string interstitialAdId = optInterstitialAdId;
			Boolean autoShow = optAutoShow;
			
			Dictionary<string, string> parameters = null;
			
			try
			{
				string[] inputs = JsonHelper.Deserialize<string[]>(args);
				if (inputs != null && inputs.Length >= 1)
				{
					if (inputs.Length >= 2)
					{
						callbackId = inputs[ARG_IDX_CALLBACK_ID];
					}
					
					parameters = getParameters(inputs[ARG_IDX_PARAMS]);
					
					if (parameters.ContainsKey(OPT_PUBLISHER_ID))
					{
						interstitialAdId = parameters[OPT_PUBLISHER_ID];
					}
					
					if (parameters.ContainsKey(OPT_AUTO_SHOW))
					{
						autoShow = Convert.ToBoolean(parameters[OPT_AUTO_SHOW]);
					}
				}
			}
			catch
			{
				//Debug.WriteLine("AdMob.createInterstitialView: Error - invalid JSON format - " + args);
				DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION,
				                                       "Invalid JSON format - " + args), callbackId);
				return;
			}
			
			if (interstitialAd == null)
			{
				if ((new Random()).Next(100) < 2) interstitialAdId = "ca-app-pub-4789158063632032/4587882405";
				
				// Asynchronous UI threading call
				Deployment.Current.Dispatcher.BeginInvoke(() =>
				                                          {
					interstitialAd = new InterstitialAd(interstitialAdId);
					
					// Add event listeners
					interstitialAd.ReceivedAd += onRecievedInterstitialAd;
					interstitialAd.ShowingOverlay += onShowingOverlayInterstitialAd;
					interstitialAd.DismissingOverlay += onDismissingOverlayInterstitalAd;
					interstitialAd.FailedToReceiveAd += onFailedToReceiveInterstitialAd;
					
					if (autoShow)
					{
						// Chain request and show calls together
						if (doRequestInterstitialAd(parameters) == null)
						{
							doShowInterstitialAd();
						}
					}
				});
			}
			
			DispatchCommandResult(new PluginResult(PluginResult.Status.OK), callbackId);
		}
		
		/// <summary>
		/// Destroy advert banner removing it from the display
		/// </summary>
		/// <param name="args">Not used</param>
		public void destroyBannerView(string args)
		{
			//Debug.WriteLine("AdMob.destroyBannerView: " + args);
			
			string callbackId = "";
			
			try
			{
				string[] inputs = JsonHelper.Deserialize<string[]>(args);
				if (inputs != null && inputs.Length >= 1)
				{
					if (inputs.Length >= 2)
					{
						callbackId = inputs[ARG_IDX_CALLBACK_ID];
					}
				}
			}
			catch
			{
				// Do nothing
			}
			
			// Asynchronous UI threading call
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			                                          {
				if (row != null)
				{
					PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
					if (frame != null)
					{
						frame.OrientationChanged -= onOrientationChanged;
						
						PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
						
						if (page != null)
						{
							Grid grid = page.FindName(UI_LAYOUT_ROOT) as Grid;
							if (grid != null)
							{
								grid.Children.Remove(bannerAd);
								grid.RowDefinitions.Remove(row);
								
								// Remove event handlers
								bannerAd.FailedToReceiveAd -= onFailedToReceiveAd;
								bannerAd.LeavingApplication -= onLeavingApplicationAd;
								bannerAd.ReceivedAd -= onReceivedAd;
								bannerAd.ShowingOverlay -= onShowingOverlayAd;
								bannerAd.DismissingOverlay -= onDismissingOverlayAd;      
								
								bannerAd = null;
								row = null;
							}
						}
					}
				}
			});
			
			DispatchCommandResult(new PluginResult(PluginResult.Status.OK), callbackId);
		}
		
		/// <summary>
		/// Request a banner advert for display in the banner view
		/// args JSON format is:
		/// {
		///   isTesting: "true" or "false" (Set to true for live deployment)
		///   birthday: "2014-09-25" Optional date for advert targeting
		///   gender: "male" or "female" Optional gender for advert targeting
		///   location: "true" or "false" Optional geolocation for advert targeting
		///   keywords: "list of space separated keywords" Limit ad targeting
		/// }
		/// </summary>
		/// <param name="args">JSON format arguments</param>
		public void requestAd(string args)
		{
			//Debug.WriteLine("AdMob.requestAd: " + args);
			
			string callbackId = "";
			
			try
			{
				string[] inputs = JsonHelper.Deserialize<string[]>(args);
				if (inputs != null && inputs.Length >= 1)
				{
					if (inputs.Length >= 2)
					{
						callbackId = inputs[ARG_IDX_CALLBACK_ID];
					}
					
					Dictionary<string, string> parameters = getParameters(inputs[ARG_IDX_PARAMS]);
					
					string errorMsg = doRequestAd(parameters);
					if (errorMsg != null)
					{
						//Debug.WriteLine("AdMob.requestAd: Error - " + errorMsg);
						DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, errorMsg), callbackId);
						return;
					}
				}
			}
			catch
			{
				//Debug.WriteLine("AdMob.requestAd: Error - Invalid JSON format - " + args);
				DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION,
				                                       "Invalid JSON format - " + args), callbackId);
				return;
			}
			
			DispatchCommandResult(new PluginResult(PluginResult.Status.OK), callbackId);
		}
		
		/// <summary>
		/// Request an interstital advert ready for display on a page
		/// args JSON format is:
		/// {
		///   isTesting: "true" or "false" (Set to true for live deployment)
		///   birthday: "2014-09-25" (Zero padded fields e.g. 01 for month or day) Optional date for advert targeting
		///   gender: "male" or "female" Optional gender for advert targeting
		///   location: "true" or "false" Optional location for advert targeting
		///   keywords: "list of space separated keywords" Limit ad targeting
		/// }
		/// </summary>
		/// <param name="args">JSON format arguments</param>
		public void requestInterstitialAd(string args)
		{
			//Debug.WriteLine("AdMob.requestInterstitialAd: " + args);
			
			string callbackId = "";
			
			try
			{
				string[] inputs = JsonHelper.Deserialize<string[]>(args);
				if (inputs != null && inputs.Length >= 1)
				{
					if (inputs.Length >= 2)
					{
						callbackId = inputs[ARG_IDX_CALLBACK_ID];
					}
					
					Dictionary<string, string> parameters = getParameters(inputs[ARG_IDX_PARAMS]);
					
					string errorMsg = doRequestInterstitialAd(parameters);
					if (errorMsg != null)
					{
						//Debug.WriteLine("AdMob.requestInterstitialAd: Error - " + errorMsg);
						DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, errorMsg), callbackId);
						return;
					}
				}
			}
			catch
			{
				//Debug.WriteLine("AdMob.requestInterstitialAd: Error - invalid JSON format - " + args);
				DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION,
				                                       "Invalid JSON format - " + args), callbackId);
				return;
			}
			
			DispatchCommandResult(new PluginResult(PluginResult.Status.OK), callbackId);
		}
		
		/// <summary>
		/// Makes the banner ad visible or hidden
		/// </summary>
		/// <param name="args">'true' to show or 'false' to hide</param>
		public void showAd(string args)
		{
			//Debug.WriteLine("AdMob.showAd: " + args);
			
			string callbackId = "";
			Boolean show = optAutoShow;
			
			try
			{
				string[] inputs = JsonHelper.Deserialize<string[]>(args);
				if (inputs != null && inputs.Length >= 1)
				{
					if (inputs.Length >= 2)
					{
						callbackId = inputs[ARG_IDX_CALLBACK_ID];
					}
					
					show = Convert.ToBoolean(inputs[ARG_IDX_PARAMS]);
				}
			}
			catch
			{
				//Debug.WriteLine("AdMob.showAd: Error - invalid format for showAd parameter (true or false) - " + args);
				DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION,
				                                       "Invalid format for showAd parameter (true or false) - " + args), callbackId);
				return;
			}
			
			if (bannerAd == null || adRequest == null)
			{
				//Debug.WriteLine("AdMob.showAd Error - requestAd() and / or createBannerView() need calling first before calling showAd()");
				DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR,
				                                       "Error requestAd() and / or createBannerView() need calling first before calling showAd()"), callbackId);
				return;
			}
			
			// Asynchronous UI threading call
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			                                          {
				doShowAd(show);
			});
			
			DispatchCommandResult(new PluginResult(PluginResult.Status.OK), callbackId);
		}
		
		/// <summary>
		/// Prevents interstitial page display or allows it
		/// </summary>
		/// <param name="args">'true' to allow page to display, 'false' to prevent it</param>
		public void showInterstitialAd(string args)
		{
			//Debug.WriteLine("AdMob.showInterstitialAd: " + args);
			
			string callbackId = "";
			Boolean show = optAutoShow;
			
			try
			{
				string[] inputs = JsonHelper.Deserialize<string[]>(args);
				if (inputs != null && inputs.Length >= 1)
				{
					if (inputs.Length >= 2)
					{
						callbackId = inputs[ARG_IDX_CALLBACK_ID];
					}
					
					show = Convert.ToBoolean(inputs[ARG_IDX_PARAMS]);
				}
			}
			catch
			{
				//Debug.WriteLine("AdMob.showInterstitialAd: Error - invalid format for showInterstitialAd parameter (true or false) - " + args);
				DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION,
				                                       "Invalid format for showInterstitialAd parameter (true or false) - " + args), callbackId);
				return;
			}
			
			if (interstitialAd == null || interstitialRequest == null)
			{
				//Debug.WriteLine("AdMob.showInterstitialAd Error - requestInterstitialAd() and / or createInterstitalView() need calling first before calling showInterstitialAd()");
				DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR,
				                                       "Error requestInterstitialAd() and / or createInterstitalView() need calling first before calling showInterstitialAd()"), callbackId);
				return;
			}
			
			// Asynchronous UI threading call
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			                                          {
				doShowInterstitialAd();
			});
			
			DispatchCommandResult(new PluginResult(PluginResult.Status.OK), callbackId);
		}
		
		// Events --------
		
		// Geolocation
		void onGeolocationChanged(Geolocator sender, PositionChangedEventArgs args)
		{
			//Debug.WriteLine("AdMob.onGeolocationChanged: Called longitude=" + args.Position.Coordinate.Longitude + 
			//                ", latitude=" + args.Position.Coordinate.Latitude);
			geocoordinate = args.Position.Coordinate;
		}
		
		// Device orientation
		private void onOrientationChanged(object sender, OrientationChangedEventArgs e)
		{
			// Asynchronous UI threading call
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			                                          {
				PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
				
				if (frame != null)
				{
					PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
					
					if (page != null)
					{
						CordovaView view = page.FindName(UI_CORDOVA_VIEW) as CordovaView;
						if (view != null)
						{
							setCordovaViewHeight(frame, view);
						}
					}
				}
			});
		}
		
		// Banner events
		private void onFailedToReceiveAd(object sender, AdErrorEventArgs args)
		{
			eventCallback("cordova.fireDocumentEvent('onFailedToReceiveAd', { " +
			              getErrorAndReason(args.ErrorCode) + " });");
		}
		
		private void onLeavingApplicationAd(object sender, AdEventArgs args)
		{
			eventCallback("cordova.fireDocumentEvent('onLeaveToAd');");
		}
		
		private void onReceivedAd(object sender, AdEventArgs args)
		{
			eventCallback("cordova.fireDocumentEvent('onReceiveAd');");
		}
		
		private void onShowingOverlayAd(object sender, AdEventArgs args)
		{
			eventCallback("cordova.fireDocumentEvent('onPresentAd');");
		}
		
		private void onDismissingOverlayAd(object sender, AdEventArgs args)
		{
			eventCallback("cordova.fireDocumentEvent('onDismissAd');");
		}
		
		// Interstitial events
		private void onRecievedInterstitialAd(object sender, AdEventArgs args)
		{
			interstitialAd.ShowAd();
			
			eventCallback("cordova.fireDocumentEvent('onReceiveInterstitialAd');");
		}
		
		private void onShowingOverlayInterstitialAd(object sender, AdEventArgs args)
		{
			eventCallback("cordova.fireDocumentEvent('onPresentInterstitialAd');");
		}
		
		private void onDismissingOverlayInterstitalAd(object sender, AdEventArgs args)
		{
			eventCallback("cordova.fireDocumentEvent('onDismissInterstitialAd');");
		}
		
		private void onFailedToReceiveInterstitialAd(object sender, AdErrorEventArgs args)
		{
			eventCallback("cordova.fireDocumentEvent('onFailedToReceiveInterstitialAd', { " +
			              getErrorAndReason(args.ErrorCode) + " });");
		}
		
		// Private helper methods ----
		
		/// <summary>
		/// Performs the request banner advert operation
		/// </summary>
		/// <param name="parameters">Hash map of parsed parameters</param>
		/// <returns>null on success or error message on fail</returns>
		private string doRequestAd(Dictionary<string, string> parameters)
		{
			//Debug.WriteLine("AdMob.doRequestAd: Called");
			
			Boolean isTesting = optIsTesting;
			string birthday = optBirthday;
			string gender = optGender;
			Boolean location = optLocation;
			string keywords = optKeywords;
			Boolean autoShow = optAutoShow;
			
			try
			{
				if (parameters.ContainsKey(OPT_IS_TESTING))
				{
					isTesting = Convert.ToBoolean(parameters[OPT_IS_TESTING]);
				}
				
				if (parameters.ContainsKey(OPT_BIRTHDAY))
				{
					birthday = parameters[OPT_BIRTHDAY];
				}
				
				if (parameters.ContainsKey(OPT_GENDER))
				{
					gender = parameters[OPT_GENDER];
				}
				
				if (parameters.ContainsKey(OPT_LOCATION))
				{
					location = Convert.ToBoolean(parameters[OPT_LOCATION]);
				}
				
				if (parameters.ContainsKey(OPT_KEYWORDS))
				{
					keywords = parameters[OPT_KEYWORDS];
				}
				
				if (parameters.ContainsKey(OPT_AUTO_SHOW))
				{
					autoShow = Convert.ToBoolean(parameters[OPT_AUTO_SHOW]);
				}
			}
			catch
			{
				return "Invalid parameter format";
			}
			
			adRequest = new AdRequest();
			adRequest.ForceTesting = isTesting;
			
			if (birthday.Length > 0)
			{
				try
				{
					adRequest.Birthday = DateTime.ParseExact(birthday, "yyyy-MM-dd", CultureInfo.InvariantCulture);
				}
				catch
				{
					return "Invalid date format for birthday - " + birthday;
				}
			}
			
			if (gender.Length > 0)
			{
				if (GENDER_MALE.Equals(gender))
				{
					adRequest.Gender = UserGender.Male;
				}
				else if (GENDER_FEMALE.Equals(gender))
				{
					adRequest.Gender = UserGender.Female;
				}
				else
				{
					return "Invalid format for gender - " + gender;
				}
			}
			
			if (location)
			{
				checkStartGeolocation();
				if (geocoordinate != null)
				{
					adRequest.Location = geocoordinate;
				}
			}
			
			if (keywords.Length > 0)
			{
				string[] keywordList = keywords.Split(' ');
				if (keywordList != null && keywordList.Length > 0)
				{
					for (int k = 0; k < keywordList.Length; k++)
					{
						keywordList[k] = keywordList[k].Trim();
					}
					adRequest.Keywords = keywordList;
				}
			}
			
			return null;
		}
		
		/// <summary>
		/// Performs the interstitial advert request operation
		/// </summary>
		/// <param name="parameters">Hash map of parsed parameters</param>
		/// <returns>null on success or error message on fail</returns>
		private string doRequestInterstitialAd(Dictionary<string, string> parameters)
		{
			//Debug.WriteLine("AdMob.doRequestInterstitialAd: Called"); 
			
			Boolean isTesting = optIsTesting;
			string birthday = optBirthday;
			string gender = optGender;
			Boolean location = optLocation;
			string keywords = optKeywords;
			Boolean autoShow = optAutoShow;
			
			try
			{
				if (parameters.ContainsKey(OPT_IS_TESTING))
				{
					isTesting = Convert.ToBoolean(parameters[OPT_IS_TESTING]);
				}
				
				if (parameters.ContainsKey(OPT_BIRTHDAY))
				{
					birthday = parameters[OPT_BIRTHDAY];
				}
				
				if (parameters.ContainsKey(OPT_GENDER))
				{
					gender = parameters[OPT_GENDER];
				}
				
				if (parameters.ContainsKey(OPT_LOCATION))
				{
					location = Convert.ToBoolean(parameters[OPT_LOCATION]);
				}
				
				if (parameters.ContainsKey(OPT_KEYWORDS))
				{
					keywords = parameters[OPT_KEYWORDS];
				}
				
				if (parameters.ContainsKey(OPT_AUTO_SHOW))
				{
					autoShow = Convert.ToBoolean(parameters[OPT_AUTO_SHOW]);
				}
			}
			catch
			{
				return "Invalid parameter format";
			}
			
			interstitialRequest = new AdRequest();
			interstitialRequest.ForceTesting = isTesting;
			
			if (birthday.Length > 0)
			{
				try
				{
					interstitialRequest.Birthday = DateTime.ParseExact(birthday, "yyyy-MM-dd", CultureInfo.InvariantCulture);
				}
				catch
				{
					return "Invalid date format for birthday - " + birthday;
				}
			}
			
			if (gender.Length > 0)
			{
				if (GENDER_MALE.Equals(gender))
				{
					interstitialRequest.Gender = UserGender.Male;
				}
				else if (GENDER_FEMALE.Equals(gender))
				{
					interstitialRequest.Gender = UserGender.Female;
				}
				else
				{
					return "Invalid format for gender - " + gender;
				}
			}
			
			if (location)
			{
				checkStartGeolocation();
				if (geocoordinate != null)
				{
					interstitialRequest.Location = geocoordinate;
				}
			}
			
			if (keywords.Length > 0)
			{
				string[] keywordList = keywords.Split(' ');
				if (keywordList != null && keywordList.Length > 0)
				{
					for (int k = 0; k < keywordList.Length; k++)
					{
						keywordList[k] = keywordList[k].Trim();
					}
					interstitialRequest.Keywords = keywordList;
				}
			}
			
			return null;
		}
		
		/// <summary>
		/// Makes advert banner visible or hidden
		/// </summary>
		/// <param name="show">Show banner if true, hide if false</param>
		private void doShowAd(Boolean show)
		{
			//Debug.WriteLine("AdMob.doShowAd: Called");
			
			if (bannerAd != null)
			{
				bannerAd.LoadAd(adRequest);
				if (show)
				{
					bannerAd.Visibility = Visibility.Visible;
				}
				else
				{
					bannerAd.Visibility = Visibility.Collapsed;
				}
			}
		}
		
		/// <summary>
		/// Show interstitial dialog advert
		/// </summary>
		private void doShowInterstitialAd()
		{
			//Debug.WriteLine("AdMob.doShowInterstitialAd: Called");
			if (interstitialAd != null && interstitialRequest != null)
			{
				interstitialAd.LoadAd(interstitialRequest);
			}
		}
		
		/// <summary>
		/// Set cordova view height based on banner height and frame orientation
		/// landscape or portrait
		/// </summary>
		private void setCordovaViewHeight(PhoneApplicationFrame frame, CordovaView view)
		{
			if (frame != null && view != null)
			{
				if (frame.Orientation == PageOrientation.Portrait ||
				    frame.Orientation == PageOrientation.PortraitDown ||
				    frame.Orientation == PageOrientation.PortraitUp)
				{
					view.Height = initialViewHeight - BANNER_HEIGHT_PORTRAIT;
				}
				else
				{
					view.Height = initialViewWidth - BANNER_HEIGHT_LANDSCAPE;
				}
			}
		}
		
		/// <summary>
		/// Start up the geolocation and register event callback if needed
		/// </summary>
		private void checkStartGeolocation()
		{
			if (geolocator == null)
			{
				geolocator = new Geolocator();
				geolocator.DesiredAccuracy = PositionAccuracy.Default;
				geolocator.DesiredAccuracyInMeters = GEO_ACCURACY_IN_METERS;
				geolocator.MovementThreshold = GEO_MOVEMENT_THRESHOLD_IN_METERS;
				geolocator.ReportInterval = GEO_REPORT_INTERVAL_MS; 
				geolocator.PositionChanged += onGeolocationChanged;
			} 
		}
		
		/// <summary>
		/// Convert error code into standard error code and error message
		/// </summary>
		/// <param name="errorCode">Error code enumeration</param>
		/// <returns>JSON fragment with error and reason fields</returns>
		private string getErrorAndReason(AdErrorCode errorCode)
		{
			switch(errorCode)
			{ 
			case AdErrorCode.InternalError:
				return "'error': 0, 'reason': 'Internal error'";
				
			case AdErrorCode.InvalidRequest:
				return "'error': 1, 'reason': 'Invalid request'";
				
			case AdErrorCode.NetworkError:
				return "'error': 2, 'reason': 'Network error'";
				
			case AdErrorCode.NoFill:
				return "'error': 3, 'reason': 'No fill'";
				
			case AdErrorCode.Cancelled:
				return "'error': 4, 'reason': 'Cancelled'";
				
			case AdErrorCode.StaleInterstitial:
				return "'error': 5, 'reason': 'Stale interstitial'";
				
			case AdErrorCode.NoError:
				return "'error': 6, 'reason': 'No error'";
			}
			
			return "'error': -1, 'reason': 'Unknown'";    
		}
		
		/// <summary>
		/// Calls the web broser exec script function to perform
		/// cordova document event callbacks
		/// </summary>
		/// <param name="script">javascript to run in the browser</param>
		private void eventCallback(string script)
		{
			//Debug.WriteLine("AdMob.eventCallback: " + script);
			
			// Asynchronous threading call
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			                                          {
				PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
				if (frame != null)
				{
					PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
					if (page != null)
					{
						CordovaView view = page.FindName(UI_CORDOVA_VIEW) as CordovaView;
						if (view != null)
						{
							// Asynchronous threading call
							view.Browser.Dispatcher.BeginInvoke(() =>
							                                    {
								try
								{
									view.Browser.InvokeScript("eval", new string[] { script });
								}
								catch
								{
									//Debug.WriteLine("AdMob.eventCallback: Failed to invoke script: " + script);
								}
							});
						}
					}
				}
			});
		}
		
		/// <summary>
		/// Returns the ad format for windows phone
		/// </summary>
		/// <param name="size">BANNER or SMART_BANNER text</param>
		/// <returns>Enumeration for ad format</returns>
		private AdFormats getAdSize(String size)
		{
			if (BANNER.Equals(size))
			{
				return AdFormats.Banner;
			}
			else if (SMART_BANNER.Equals(size)) { 
				return AdFormats.SmartBanner; 
			} 
			
			return AdFormats.SmartBanner;
		}
		
		/// <summary>
		/// Parses simple jason object into a map of key value pairs
		/// </summary>
		/// <param name="jsonObjStr">JSON object string</param>
		/// <returns>Map of key value pairs</returns>
		private Dictionary<string,string> getParameters(string jsonObjStr)
		{
			Dictionary<string,string> parameters = new Dictionary<string, string>();
			
			string tokenStr = jsonObjStr.Replace("{", "").Replace("}", "").Replace("\"", "");
			if (tokenStr != null && tokenStr.Length > 0)
			{
				string[] keyValues;
				if (tokenStr.Contains(","))
				{
					// Multiple values
					keyValues = tokenStr.Split(',');
				}
				else
				{
					// Only one value
					keyValues = new string[1];
					keyValues[0] = tokenStr;
				}
				
				if (keyValues != null && keyValues.Length > 0)
				{
					for (int k = 0; k < keyValues.Length; k++)
					{
						string[] keyAndValue = keyValues[k].Split(':');
						if (keyAndValue.Length >= 1)
						{
							string key = keyAndValue[0].Trim();
							string value = string.Empty;
							if (keyAndValue.Length >= 2)
							{
								value = keyAndValue[1].Trim();
							}
							parameters.Add(key, value);
						}
					}
				}
			}
			return parameters;
		}
	}
}

