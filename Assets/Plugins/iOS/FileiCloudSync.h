//
//  FileiCloudSync.h
//  Unity-iPhone
//
//  Created by Administrator on 18/03/16.
//
//

#import <Foundation/Foundation.h>

static NSMetadataQuery *query;

@interface FileiCloudSync : NSObject

void NotifyUnity(void *notificationMessage);
+ (void) setup;
+ (NSURL *) getFilePath:(NSString *)file;
+ (BOOL) getiCloudStatus;
+ (void) copyEngineDataFromCloud;
+ (void) copyEngineDataToCloud;
+ (void) registerCloudNotification;
+ (void) performCloudFileOperations:(NSString *)file;

@end
