//Copyright (c) 2014 Sang Ki Kwon (Cranberrygame)
//Email: cranberrygame@yahoo.com
//Homepage: http://cranberrygame.github.io
//License: MIT (http://opensource.org/licenses/MIT)
#import "AdmobSplit.h"

@implementation AdmobSplit

- (BOOL) bannerIsShowingOverlap {
	//
	BOOL bannerIsShowing = NO;
/*
	if (bannerView != nil) {
		//if banner is showing
		//if ([bannerView isDescendantOfView:[webView superview]]) {			
		UIView* webView = [bannerView superview];
		if (webView != nil) {
			bannerIsShowing = YES;
		}
	}
*/	
	return bannerIsShowing;
}

- (void) addBannerViewOverlap:(NSString*)position aSize:(NSString*)size {
/*
	if (webView != nil)
	{
		UIView* parentView = [webView superview];
		if (parentView != nil) {
			[parentView addSubview:bannerView];		
		}
	}		
*/
}

- (void)_removeBannerViewOverlap {

}

@end
