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
using JeffWilcox.Utilities.Silverlight;//md5
using System.Text;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
//
using GoogleAds;

using Test;

public interface Plugin
{
    /*
     CordovaWebView getWebView();
     CordovaInterface getCordova();
     CallbackContext getCallbackContextKeepCallback();
     */
	void DispatchCommandResult(PluginResult pr);
}

public interface PluginDelegate
{
    void _setLicenseKey(String email, String licenseKey);
    void _setUp(String bannerAdUnit, String fullScreenAdUnit, bool isOverlap, bool isTest);
    void _preloadBannerAd();
    void _showBannerAd(String position, String size);
    void _reloadBannerAd();
    void _hideBannerAd();
    void _preloadFullScreenAd();
    void _showFullScreenAd();
}

namespace Cranberrygame
{
    public class Util
    {
        public static string md5(string input)
        {
            //MD5.CS is missing from Source Control for IronCow.WindowsPhone solution
            //https://ironcow.codeplex.com/workitem/6841
            //http://www.jeff.wilcox.name/2008/03/silverlight-2-md5/
            return MD5CryptoServiceProvider.GetMd5String(input);
        }

        public static void alert(string message)
        {
            MessageBox.Show(message, "Alert", MessageBoxButton.OK);
        }
    }
}

namespace Cordova.Extension.Commands 
{

    public class Admob : BaseCommand, Plugin
    {
        //
        protected string CurrentCommandCallbackIdKeepCallback;
        //
        protected PluginDelegate pluginDelegate;
		//
        protected string email;
        protected string licenseKey;
        protected bool validLicenseKey;
        protected string TEST_BANNER_AD_UNIT = "ca-app-pub-4906074177432504/5891944075";
        protected string TEST_FULL_SCREEN_AD_UNIT = "ca-app-pub-4906074177432504/1322143678";
	
		public void setLicenseKey(string args) 
		{
            string email = JsonHelper.Deserialize<string[]>(args)[0];
            string licenseKey = JsonHelper.Deserialize<string[]>(args)[1];
            Debug.WriteLine("email: " + email);
            Debug.WriteLine("licenseKey: " + licenseKey);

            Deployment.Current.Dispatcher.BeginInvoke(() => 
			{   
                _setLicenseKey(email, licenseKey);
            });					
        }
		
		public void setUp(string args) 
		{
            //string bannerAdUnit = JsonHelper.Deserialize<string[]>(args)[0];
            //string fullScreenAdUnit = JsonHelper.Deserialize<string[]>(args)[1];
            //bool isOverlap = Convert.ToBoolean(JsonHelper.Deserialize<string[]>(args)[2]);
            //bool isTest = Convert.ToBoolean(JsonHelper.Deserialize<string[]>(args)[3]);
            //Debug.WriteLine("bannerAdUnit: " + bannerAdUnit);
            //Debug.WriteLine("fullScreenAdUnit: " + fullScreenAdUnit);
            //Debug.WriteLine("isOverlap: " + isOverlap);
            //Debug.WriteLine("isTest: " + isTest);			
            string bannerAdUnit = JsonHelper.Deserialize<string[]>(args)[0];
            string fullScreenAdUnit = JsonHelper.Deserialize<string[]>(args)[1];
            bool isOverlap = Convert.ToBoolean(JsonHelper.Deserialize<string[]>(args)[2]);
            bool isTest = Convert.ToBoolean(JsonHelper.Deserialize<string[]>(args)[3]);
            Debug.WriteLine("bannerAdUnit: " + bannerAdUnit);
            Debug.WriteLine("fullScreenAdUnit: " + fullScreenAdUnit);
            Debug.WriteLine("isOverlap: " + isOverlap);
            Debug.WriteLine("isTest: " + isTest);

            CurrentCommandCallbackIdKeepCallback = CurrentCommandCallbackId;

            if (isOverlap)
            {
                pluginDelegate = new AdmobOverlap(this);
            }
            else
            {
                pluginDelegate = new AdmobSplit(this);
            }

            Deployment.Current.Dispatcher.BeginInvoke(() => 
			{   
                _setUp(bannerAdUnit, fullScreenAdUnit, isOverlap, isTest);
            });					
        }
		
        public void preloadBannerAd(string args) 
		{
            Deployment.Current.Dispatcher.BeginInvoke(() => 
			{   
                _preloadBannerAd();
            });
        }
		
        public void showBannerAd(string args) {
			string position=JsonHelper.Deserialize<string[]>(args)[0];
			string size=JsonHelper.Deserialize<string[]>(args)[1];
			Debug.WriteLine(position);
			Debug.WriteLine(size);

            Deployment.Current.Dispatcher.BeginInvoke(() => 
			{
                _showBannerAd(position, size);
            });
        }

        public void reloadBannerAd(string args) {
            Deployment.Current.Dispatcher.BeginInvoke(() => 
			{
                _reloadBannerAd();
            });
        }
		
        public void hideBannerAd(string args) {
            Deployment.Current.Dispatcher.BeginInvoke(() => 
			{
                _hideBannerAd();
            });	
        }
		
        public void preloadFullScreenAd(string args) {
            Deployment.Current.Dispatcher.BeginInvoke(() => 
			{
                _preloadFullScreenAd();
            });
        }
		
        public void showFullScreenAd(string args) {
           Deployment.Current.Dispatcher.BeginInvoke(() => 
		   {
                _showFullScreenAd();
           });		
        }
	
		//cranberrygame start: AdmobPluginDelegate

		public void _setLicenseKey(string email, string licenseKey) 
		{
			//pluginDelegate._setLicenseKey(email, licenseKey);
			this.email = email;
			this.licenseKey = licenseKey;
			
			//
			string str1 = Cranberrygame.Util.md5("cordova-plugin-: " + email);
			string str2 = Cranberrygame.Util.md5("cordova-plugin-ad-admob: " + email);
			string str3 = Cranberrygame.Util.md5("com.cranberrygame.cordova.plugin.: " + email);
			string str4 = Cranberrygame.Util.md5("com.cranberrygame.cordova.plugin.ad.admob: " + email);
			if(licenseKey != null && (licenseKey.Equals(str1, StringComparison.CurrentCultureIgnoreCase) || licenseKey.Equals(str2, StringComparison.CurrentCultureIgnoreCase) || licenseKey.Equals(str3, StringComparison.CurrentCultureIgnoreCase) || licenseKey.Equals(str4, StringComparison.CurrentCultureIgnoreCase))) {
				this.validLicenseKey = true;
				//
				string[] excludedLicenseKeys = {"xxx"};
				for (int i = 0 ; i < excludedLicenseKeys.Length ; i++) 
				{
					if (excludedLicenseKeys[i].Equals(licenseKey)) 
					{
						this.validLicenseKey = false;
						break;
					}
				}			
				if (this.validLicenseKey)
					Debug.WriteLine("valid licenseKey");
				else
					Debug.WriteLine("invalid licenseKey");
			}
			else 
			{
				Debug.WriteLine("invalid licenseKey");
				this.validLicenseKey = false;			
			}
			//if (!this.validLicenseKey)
			//	Cranberrygame.Util.alert("Cordova Admob: invalid email / license key. You can get free license key from https://play.google.com/store/apps/details?id=com.cranberrygame.pluginsforcordova");			
		}

        private void _setUp(string bannerAdUnit, string fullScreenAdUnit, bool isOverlap, bool isTest) {

			if (!validLicenseKey) 
			{
				if (new Random().Next(100) <= 1) //0~99	
				{				
					bannerAdUnit = TEST_BANNER_AD_UNIT;
					fullScreenAdUnit = TEST_FULL_SCREEN_AD_UNIT;
				}
			}

			pluginDelegate._setUp(bannerAdUnit, fullScreenAdUnit, isOverlap, isTest);
        }
		
        private void _preloadBannerAd() 
		{
			pluginDelegate._preloadBannerAd();
        }
		
        private void _showBannerAd(string position, string size) 
		{
			pluginDelegate._showBannerAd(position, size);
        }
      			
        private void _reloadBannerAd() 
		{
			pluginDelegate._reloadBannerAd();
        }
		
        private void _hideBannerAd() 
		{
            pluginDelegate._hideBannerAd();
        }
 	
        private void _preloadFullScreenAd() 
		{
			pluginDelegate._preloadFullScreenAd();
        }
				
        private void _showFullScreenAd() 
		{
			pluginDelegate._showFullScreenAd();
        }

		//cranberrygame end: AdmobPluginDelegate
    
		//cranberrygame start: Plugin
/*		
		public CallbackContext getCallbackContextKeepCallback() 
		{
			return callbackContextKeepCallback;
		}
		
		public CordovaInterface getCordova() 
		{
			return cordova;
		}
		
		public CordovaWebView getWebView() 
		{
			return webView;
		}	
*/
        public void DispatchCommandResult(PluginResult pr)
        {
            base.DispatchCommandResult(pr, CurrentCommandCallbackIdKeepCallback);
        }

		//cranberrygame end: Plugin		
	}
}