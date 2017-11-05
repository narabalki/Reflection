//
//  AppsFlyerWarpper.m
//
//
//  Created by AppsFlyer 2013
//
//

#import "IOSUtilities.h"
#include "FreeSpeech.h" //for cStringCopy
#include "Mixpanel/Mixpanel.h"
#include "MKiCloudSync.h"
#include "FileiCloudSync.h"

@implementation ViewController : UIViewController

-(void) shareOnlyTextMethod: (const char *) shareMessage
{
    
    NSString *message   = [NSString stringWithUTF8String:shareMessage];
    NSArray *postItems  = @[message];
    
    UIActivityViewController *activityVc = [[UIActivityViewController alloc] initWithActivityItems:postItems applicationActivities:nil];
    activityVc.completionWithItemsHandler = ^void(NSString * __nullable activityType, BOOL completed, NSArray * returnedItems, NSError * activityError){
        if(completed && !activityError){
            NSLog(@"Shared to : %@",activityType);
            NSDictionary *mixpanelProps = @{@"activityType":activityType};
            [[Mixpanel sharedInstance] track:@"Shared Text with Social Media" properties:mixpanelProps];
        }
        else{
            NSLog(@"Share Text Cancelled/Error");
            [[Mixpanel sharedInstance] track:@"Shared Text with Social Media Cancelled/Error"];
        }
    };
    
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad &&  [activityVc respondsToSelector:@selector(popoverPresentationController)] ) {
        
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activityVc];
        
        [popup presentPopoverFromRect:CGRectMake(self.view.frame.size.width/2, self.view.frame.size.height/4, 0, 0)
                               inView:[UIApplication sharedApplication].keyWindow.rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
    else
        [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:activityVc animated:YES completion:nil];
}

-(void) shareTextAndImageMethod: (const char *) shareMessage image:(const char*) pngPath
{
    
    NSString *messageURL   = [NSString stringWithUTF8String:shareMessage];
    NSString *message = messageURL;
    NSString *imagePath = [NSString stringWithUTF8String:pngPath];
    UIImage *screenshot = [UIImage imageWithContentsOfFile:imagePath];
    NSArray *postItems  = @[message,screenshot];
    
    UIActivityViewController *activityVc = [[UIActivityViewController alloc] initWithActivityItems:postItems applicationActivities:nil];
    activityVc.completionWithItemsHandler = ^void(NSString * __nullable activityType, BOOL completed, NSArray * returnedItems, NSError * activityError){
        if(completed && !activityError){
            NSLog(@"Shared to : %@",activityType);
            NSDictionary *mixpanelProps = @{@"activityType":activityType};
            [[Mixpanel sharedInstance] track:@"Shared with Social Media" properties:mixpanelProps];
        }
        else{
            NSLog(@"Share Cancelled/Error");
            [[Mixpanel sharedInstance] track:@"Shared with Social Media Cancelled/Error"];
        }
    };
    
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad &&  [activityVc respondsToSelector:@selector(popoverPresentationController)] ) {
        
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activityVc];
        
        [popup presentPopoverFromRect:CGRectMake(self.view.frame.size.width/2, self.view.frame.size.height/4, 0, 0)
                               inView:[UIApplication sharedApplication].keyWindow.rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
    else
        [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:activityVc animated:YES completion:nil];
}

@end


@interface IOSUtilities () {
    
}

@end



@implementation IOSUtilities


+(void) RestoreiCloudSettingsFromCustomFolder{
    NSString *dest =  [NSHomeDirectory() stringByAppendingPathComponent:@"Library/Preferences/com.freespeechapp.en.plist"];
    NSString *source = [[[[NSFileManager defaultManager]URLForUbiquityContainerIdentifier:nil] path] stringByAppendingPathComponent:@"/Documents/Custom/com.freespeechapp.en.plist"];
    NSError *err;
    if([[NSFileManager defaultManager] fileExistsAtPath:source] && [[NSFileManager defaultManager]ubiquityIdentityToken]){
        [[NSFileManager defaultManager]copyItemAtPath:source toPath:dest error:&err];
        if(err){
            NSLog(@"PlayerPrefs iCloud Copy Error %@",err);
        }
    }
}

extern "C" {
    
    const char *mGetClipBoardString () {
        NSString *clipbrd = [UIPasteboard generalPasteboard].string;
        return cStringCopy([clipbrd UTF8String]);
    }
    
    const void mSetClipBoardString(const char *appleAppID){
        [UIPasteboard generalPasteboard].string = [NSString stringWithUTF8String:cStringCopy(appleAppID)];
    }
    
    void _ShareSimpleText(const char * message){
        ViewController *vc = [[ViewController alloc] init];
        [vc shareOnlyTextMethod: message];
    }
    
    void _ShareTextWithImage(const char * message,const char * imagePath){
        ViewController *vc = [[ViewController alloc] init];
        [vc shareTextAndImageMethod:message image:imagePath];
    }
    
    void mSetLanguageString(const char * language){
        NSString *languageString = [NSString stringWithUTF8String:cStringCopy(language)];
        NSLog(@"Language is : %@",languageString);
        
        [[NSUserDefaults standardUserDefaults] setObject:[NSArray arrayWithObjects:languageString,@"en", nil] forKey:@"AppleLanguages"];
        [[NSUserDefaults standardUserDefaults] setObject:languageString forKey:@"language"];
        [[NSUserDefaults standardUserDefaults] synchronize];
    }
    
    const char *mGetDeviceLanguage(){
        NSString * langCode = [[NSLocale preferredLanguages] objectAtIndex:0];
        NSLog(@"Device Language is : %@",langCode);
        return cStringCopy([langCode UTF8String]);
    }
    
    const bool mGetiCloudStatus(){
        NSLog(@"BOOL value is : %@",[NSNumber numberWithBool:[FileiCloudSync getiCloudStatus]]);
        return [FileiCloudSync getiCloudStatus];
    }
    
    const char *mGetiCloudPath(const char *persistent_path){
        
        NSString *path;
        
        if([[NSFileManager defaultManager]ubiquityIdentityToken]){   //For iCloud use :
            path = [[[[NSFileManager defaultManager]URLForUbiquityContainerIdentifier:nil] path] stringByAppendingPathComponent:@"/Documents"];
            NSLog(@"iCloud Path : %@",path);
        }
        else{         //For reading from Documents :
            path = [NSString stringWithUTF8String:persistent_path];
        }
        
        return cStringCopy([path UTF8String]);
    }
    
    void mStartiCloud()
    {
        [IOSUtilities RestoreiCloudSettingsFromCustomFolder];
    }
    
    void mRestoreSettingsFromCustomFolder(){
        [IOSUtilities RestoreiCloudSettingsFromCustomFolder];
    }
    
    void mSaveSettingsToCustomFolder()
    {
        NSString *source =  [NSHomeDirectory() stringByAppendingPathComponent:@"Library/Preferences/com.freespeechapp.en.plist"];
        NSString *dest = [[[[NSFileManager defaultManager]URLForUbiquityContainerIdentifier:nil] path] stringByAppendingPathComponent:@"/Documents/Custom/com.freespeechapp.en.plist"];
        NSError *err;
        if([[NSFileManager defaultManager] fileExistsAtPath:source] && [[NSFileManager defaultManager]ubiquityIdentityToken]){
            [[NSFileManager defaultManager]copyItemAtPath:source toPath:dest error:&err];
            if(err){
                NSLog(@"PlayerPrefs iCloud Copy Error %@",err);
            }
        }
    }
    
}

@end