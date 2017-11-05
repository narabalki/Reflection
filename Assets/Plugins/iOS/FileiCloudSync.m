//
//  FileiCloudSync.m
//  Unity-iPhone
//
//  Created by Administrator on 18/03/16.
//
//

#import "FileiCloudSync.h"

@implementation FileiCloudSync :NSObject
static void (*notificationCallback)();
static bool alreadyShowedNotification = false;
static bool iCloudPathActivated = false;


void mNotifyUnity(void (*notificationMessage))
{
    notificationCallback = notificationMessage;
}

- (id)init
{
    self = [super init];
    if (self) {
        // Initialization code here.
    }
    
    return self;
}

+(void) cleanUpContainer
{
    NSFileManager *fileManager = [[NSFileManager alloc] init];
    NSURL *ubiquitousContainerURL = [fileManager URLForUbiquityContainerIdentifier:nil];
    NSArray *subpaths = [fileManager subpathsAtPath:[ubiquitousContainerURL path]];
    NSError *error;
    
    for (NSString *spaths in subpaths) {
        NSString *fullPath = [[ubiquitousContainerURL path] stringByAppendingPathComponent:spaths];
        if([[NSFileManager defaultManager]ubiquityIdentityToken]){
            if([fileManager fileExistsAtPath:fullPath])
            {
                [fileManager removeItemAtPath:fullPath error:&error];
                
                if(error)
                    NSLog(@"Error is: %@",error);
            }
        }
    }
    
    NSLog(@"Contents : %@",[fileManager subpathsAtPath:[ubiquitousContainerURL path]]);
    NSLog(@"Ubiquitous container cleared");
}

+ (NSMetadataQuery *)query {
    if (!query) {
        query = [[NSMetadataQuery alloc]init];
        
        NSArray *scopes = @[NSMetadataQueryUbiquitousDocumentsScope,NSMetadataQueryUbiquitousDataScope];
        query.searchScopes = scopes;
        //        query.notificationBatchingInterval = 1.0;
        
        if (![query startQuery]) {
            NSLog(@"Query didn't start");
        }
    }
    return query;
}

+(void) performCloudFileOperations:(NSString *)file
{
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
        
        if([[NSFileManager defaultManager]ubiquityIdentityToken]){
            NSError *error = nil;
            //    NSURL *url;
            NSFileManager *fileManager = [NSFileManager defaultManager];
            NSURL *ubiquitousContainerURL = [fileManager URLForUbiquityContainerIdentifier:nil];
            
            NSString *urlString = [@"file://" stringByAppendingString:[NSHomeDirectory() stringByAppendingPathComponent:file]];
            NSURL *sourceURL = [NSURL URLWithString:urlString];
            NSURL *ubiquitousFileURL = [ubiquitousContainerURL URLByAppendingPathComponent:file];
            
            NSLog(@"Source file is : %@",sourceURL);
            NSLog(@"Ubiquitous file : %@",ubiquitousFileURL);

            [fileManager startDownloadingUbiquitousItemAtURL:ubiquitousFileURL error:&error];
            [self GetURLStatus:sourceURL];
            if(error)
            {
                NSLog(@"[Download error] %@ (%@) (%@)", error, sourceURL, ubiquitousFileURL);
            }
            
            UnitySendMessage("ScreenManager", "iCloudCallback", sourceURL.path.UTF8String);
            
            [FileiCloudSync showContents];
        }
    });
}

+ (void) registerCloudNotification
{
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(fileContentChanged:) name:NSMetadataQueryDidUpdateNotification object:[FileiCloudSync query]];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(fileContentChanged:) name:NSMetadataQueryDidStartGatheringNotification object:[FileiCloudSync query]];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(fileContentChanged:) name:NSMetadataQueryGatheringProgressNotification object:[FileiCloudSync query]];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(fileContentChanged:) name:NSMetadataQueryDidFinishGatheringNotification object:[FileiCloudSync query]];
}

+ (void) showContents
{
    NSFileManager *fileManager = [NSFileManager defaultManager];
    NSURL *ubiquitousContainerURL = [fileManager URLForUbiquityContainerIdentifier:nil];
    NSLog(@"Contents : %@",[fileManager subpathsAtPath:[ubiquitousContainerURL path]]);
}

+(void) fileContentChanged:(NSNotification *) notification
{
    NSLog(@"File contents altered : %@",notification);
    NSLog(@"Notification User info: %@",notification.userInfo);
    
    NSMutableArray *items = [notification.userInfo valueForKey:NSMetadataQueryUpdateAddedItemsKey];
    [items addObjectsFromArray:[notification.userInfo valueForKey:NSMetadataQueryUpdateChangedItemsKey]];
    [items addObjectsFromArray:[notification.userInfo valueForKey:NSMetadataQueryUpdateRemovedItemsKey]];
    
    for (NSMetadataItem *it in items) {
        NSURL *changedURL = [it valueForAttribute:NSMetadataItemURLKey];
        [self GetURLStatus:changedURL];
    }
}

+(void)GetURLStatus :(NSURL *)url
{
    NSError *err;
    NSNumber *val;
    NSArray *keys = @[NSURLIsUbiquitousItemKey,NSURLUbiquitousItemHasUnresolvedConflictsKey,NSURLUbiquitousItemIsDownloadingKey,NSURLUbiquitousItemIsUploadingKey];
    for (NSString *k in keys) {
        [url getResourceValue:&val forKey:k error:&err];
        NSLog(@"URL Status : %@ in %@ : %@",k,url,val);
        if(k == NSURLUbiquitousItemHasUnresolvedConflictsKey && val.boolValue == true)
        {
            NSLog(@"Conflict in file : %@",url);
        }
    }
}


+(BOOL)getiCloudStatus
{
    return iCloudPathActivated;
}

@end
