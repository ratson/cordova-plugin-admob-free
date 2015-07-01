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

namespace Test {

    public class AdmobSplit : AdmobOverlap {
        private double rootViewHeight = 0.0;
        private double rootViewWidth = 0.0;
        private const int BANNER_HEIGHT_PORTRAIT = 50;
        private const int BANNER_HEIGHT_LANDSCAPE = 32;
        private RowDefinition rowDefinition = new RowDefinition { Height = GridLength.Auto };

        public AdmobSplit(Plugin plugin_) : base(plugin_)
        {
            PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            PhoneApplicationPage rootPage = rootFrame.Content as PhoneApplicationPage;
            Grid rootGrid = rootPage.FindName("LayoutRoot") as Grid;
            //rootGrid.ShowGridLines = true;
            CordovaView rootView = rootPage.FindName("CordovaView") as CordovaView;

            rootViewHeight = rootView.ActualHeight;
            rootViewWidth = rootView.ActualWidth;
		}

        protected override bool bannerIsShowingOverlap()
        {
            return base.bannerIsShowingOverlap();
		}

        protected override void addBannerViewOverlap(string position, string size)
        {
            //http://www.1java2c.com/Code/CSharp/Windows-Presentation-Foundation/WindowwithGrid.htm
            //http://www.1java2c.com/Code/CSharp/Windows-Presentation-Foundation/SetGridRowandColumnforaButton.htm
            //http://www.1java2c.com/Code/CSharp/Windows-Presentation-Foundation/ArrangeUIElementsinaGrid.htm
            //http://www.1java2c.com/Code/CSharp/Windows-Presentation-Foundation/SimpleGridxaml.htm
            //http://www.1java2c.com/Code/CSharp/Windows-Presentation-Foundation/EmptyRowDefinitionandColumnDefinition.htm
            //
            //http://www.1java2c.com/Code/CSharp/Windows-Presentation-Foundation/ClearAllRows.htm

            PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            PhoneApplicationPage rootPage = rootFrame.Content as PhoneApplicationPage;
            Grid rootGrid = rootPage.FindName("LayoutRoot") as Grid;
            //rootGrid.ShowGridLines = true;
            CordovaView rootView = rootPage.FindName("CordovaView") as CordovaView;

            if (position.Equals("top-left") || position.Equals("top-center") || position.Equals("top-right"))
            //if (true)
            {
                //rootGrid.RowDefinitions.Insert(0, new RowDefinition { Height = new GridLength(500) });
                //rootGrid.RowDefinitions.Insert(0, new RowDefinition { Height = GridLength.Auto });
                rootGrid.RowDefinitions.Insert(0, rowDefinition);

                rootGrid.Children.Add(bannerView);

                Grid.SetRow(bannerView, 0);
                Grid.SetRow(rootView, 1);
            }
            //else if (position.Equals("left") || position.Equals("center") || position.Equals("right") || position.Equals("bottom-left") || position.Equals("bottom-center") || position.Equals("bottom-right"))
            else
            {
                //rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(500) });
                //rootGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                rootGrid.RowDefinitions.Add(rowDefinition);

                rootGrid.Children.Add(bannerView);

                Grid.SetRow(rootView, 0);
                Grid.SetRow(bannerView, 1);
            }

            //
            if (rootFrame.Orientation == PageOrientation.Portrait ||
                rootFrame.Orientation == PageOrientation.PortraitDown ||
                rootFrame.Orientation == PageOrientation.PortraitUp)
            {
                rootView.Height = rootViewHeight - BANNER_HEIGHT_PORTRAIT;
            }
            else
            {
                rootView.Height = rootViewWidth - BANNER_HEIGHT_LANDSCAPE;
            }

            rootFrame.OrientationChanged += rootFrame_OrientationChanged;
        }

        protected override void removeBannerViewOverlap()
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
                rootGrid.RowDefinitions.Remove(rowDefinition);
                
                rootGrid.Children.Remove(bannerView);

                if (rootFrame.Orientation == PageOrientation.Portrait ||
                    rootFrame.Orientation == PageOrientation.PortraitDown ||
                    rootFrame.Orientation == PageOrientation.PortraitUp)
                {
                    rootView.Height = rootViewHeight;
                }
                else
                {
                    rootView.Height = rootViewWidth;
                }
            }

            rootFrame.OrientationChanged -= rootFrame_OrientationChanged;
        }

        private void rootFrame_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if (!bannerIsShowingOverlap())
                return;

            PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            PhoneApplicationPage rootPage = rootFrame.Content as PhoneApplicationPage;
            Grid rootGrid = rootPage.FindName("LayoutRoot") as Grid;
            //rootGrid.ShowGridLines = true;
            CordovaView rootView = rootPage.FindName("CordovaView") as CordovaView;

            //
            if (rootFrame.Orientation == PageOrientation.Portrait ||
                rootFrame.Orientation == PageOrientation.PortraitDown ||
                rootFrame.Orientation == PageOrientation.PortraitUp)
            {
                rootView.Height = rootViewHeight - BANNER_HEIGHT_PORTRAIT;
            }
            else
            {
                rootView.Height = rootViewWidth - BANNER_HEIGHT_LANDSCAPE;
            } 
        }
	}	
}