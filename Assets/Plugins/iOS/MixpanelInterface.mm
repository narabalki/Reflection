//
//  MixpanelInterface.mm
//  Unity-iPhone
//
//  Created by Malar Kannan on 1/28/16.
//
//

#include "MixpanelInterface.h"
#include "Mixpanel/Mixpanel.h"

#include "FreeSpeech.h" //for cStringCopy



@interface MixpanelInterface () {
}

@end



@implementation MixpanelInterface


extern "C" {

    void mixpanelSetToken (const char *mixToken) {
        NSString *mixTokenString= [NSString stringWithUTF8String:cStringCopy(mixToken)];
        [Mixpanel sharedInstanceWithToken:mixTokenString];
    }

    void mixpanelSetSuperProperties(const char *superProperties,const char *superValues){
        NSString *superPropertiesString= [NSString stringWithUTF8String:cStringCopy(superProperties)];
        NSString *superValuesString= [NSString stringWithUTF8String:cStringCopy(superValues)];
        NSDictionary *mixpanelsuperProps = [NSDictionary dictionaryWithObjects:[superValuesString componentsSeparatedByString:@"*##*"] forKeys:[superPropertiesString componentsSeparatedByString:@"*##*"]];
        [[Mixpanel sharedInstance] registerSuperProperties:mixpanelsuperProps];
    }

    void mixpanelTrack(const char* eventString){
        [[Mixpanel sharedInstance] track:[NSString stringWithUTF8String:cStringCopy(eventString)]];
    }


    void mixpanelTrackWithProps(const char* eventString,const char *properties,const char *values){
        NSString *propertiesString= [NSString stringWithUTF8String:cStringCopy(properties)];
        NSString *valuesString= [NSString stringWithUTF8String:cStringCopy(values)];
        NSArray *valArray = [valuesString componentsSeparatedByString:@"*##*"];
        NSArray *propArray = [propertiesString componentsSeparatedByString:@"*##*"];
        NSDictionary *mixpanelProps;
        if(valArray.count == propArray.count){
            mixpanelProps = [NSDictionary dictionaryWithObjects:valArray forKeys:propArray];
        }
        else{
            mixpanelProps = @{};
        }
        NSLog(@"mix properties : %@",propertiesString);
        NSLog(@"mix values : %@",valuesString);
        NSLog(@"mix propdict : %@",mixpanelProps);
        NSLog(@"mix instance : %@",[Mixpanel sharedInstance]);
        [[Mixpanel sharedInstance] track:[NSString stringWithUTF8String:cStringCopy(eventString)] properties:mixpanelProps];
    }
    
    void mixpanelUpdateProfile(const char* dictinct_id,const char *properties,const char *values){
        NSString *propertiesString= [NSString stringWithUTF8String:cStringCopy(properties)];
        NSString *valuesString= [NSString stringWithUTF8String:cStringCopy(values)];
        NSDictionary *mixpanelProps = [NSDictionary dictionaryWithObjects:[valuesString componentsSeparatedByString:@"*##*"] forKeys:[propertiesString componentsSeparatedByString:@"*##*"]];
        [[Mixpanel sharedInstance] identify:[NSString stringWithUTF8String:cStringCopy(dictinct_id)]];
        [[Mixpanel sharedInstance].people set:mixpanelProps];
    }

}

@end
