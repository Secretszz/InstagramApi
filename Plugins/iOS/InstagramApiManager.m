//
//  InstagramApiManager.m
//  UnityFramework
//
//  Created by 晴天网络 on 2023/3/10.
//

#import <Foundation/Foundation.h>
#import "InstagramApiManager.h"
#import "UnityAppController.h"
#import "UnityAppController.h"
#import "CommonApi.h"

@implementation InstagramApiManager

static InstagramApiManager* _instance;

+ (InstagramApiManager *) instance {
    static dispatch_once_t token;
    dispatch_once(&token, ^{
        if(_instance == nil)
        {
            _instance = [[self alloc] init];
        }
    });
    return _instance;
}

/**
 是否安装了Instagram
 */
-(BOOL) isInstalledInstagram{
    NSURL *instagramURL = [NSURL URLWithString:@"instagram://app"];
    return [[UIApplication sharedApplication] canOpenURL:instagramURL];
}

/**
 universal link 启动Instagram App
 */
-(BOOL) openInstagramApp{
    NSURL *instagramURL = [NSURL URLWithString:@"https://www.instagram.com"];
    return [self openInstagram:instagramURL options:@{} completionHandler:nil];
}

/**
 universal link 启动Instagram App并打开相机，如果没有相机则打开照片库
 */
-(BOOL) openInstagramAndCamera{
    NSURL *instagramURL = [NSURL URLWithString:@"https://www.instagram.com/create/story"];
    return [self openInstagram:instagramURL options:@{} completionHandler:nil];
}

/**
 universal link 启动Instagram App并加载指定编号的帖子
 @param mediaId 帖子ID
 */
-(BOOL) openInstagramAndMedia:(int)mediaId{
    NSString* url = [NSString stringWithFormat:@"https://www.instagram.com/p/%d", mediaId];
    NSURL* instagramURL = [NSURL URLWithString:url];
    return [self openInstagram:instagramURL options:@{} completionHandler:nil];
}

/**
 universal link 启动Instagram App并加载指定账号的用户
 @param username 用户账号
 */
-(BOOL) openInstagramAndUser:(NSString*)username{
    NSString* url = [NSString stringWithFormat:@"https://www.instagram.com/%@", username];
    NSURL* instagramURL = [NSURL URLWithString:url];
    return [self openInstagram:instagramURL options:@{} completionHandler:nil];
}

/**
 universal link 启动Instagram App并加载指定编号的位置动态
 @param locationId 位置动态ID
 */
-(BOOL) openInstagramAndLocations:(int)locationId{
    NSString* url = [NSString stringWithFormat:@"https://www.instagram.com/explore/locations/%d", locationId];
    NSURL* instagramURL = [NSURL URLWithString:url];
    return [self openInstagram:instagramURL options:@{} completionHandler:nil];
}

/**
 universal link 启动Instagram App并加载指定编号的话题标签界面
 @param tagName 话题名称
 */
-(BOOL) openInstagramAndTags:(NSString*)tagName{
    NSString* url = [NSString stringWithFormat:@"https://www.instagram.com/explore/tags/%@", tagName];
    NSURL* instagramURL = [NSURL URLWithString:url];
    return [self openInstagram:instagramURL options:@{} completionHandler:nil];
}

/**
 universal link 启动Instagram App并通过苹果的文档交互API分享图片
 @param imageData 图片数据
 */
-(void) openInstagramWithImage:(UIImage*) imageData{
    //check for App is install or not
    /*
    if([self isInstalledInstagram])
    {
        UIImage *imageToUse = imageData;
        NSString *documentDirectory = [NSHomeDirectory() stringByAppendingPathComponent:@"Documents"];
        NSString* fileName = [NSString stringWithFormat:@"Slash_Girl_%@.ig",[self getCurrentDateString]];
        NSString *saveImagePath = [documentDirectory stringByAppendingPathComponent:fileName];
        NSData *imageData = UIImagePNGRepresentation(imageToUse);
        [imageData writeToFile:saveImagePath atomically:YES];
        NSURL *imageURL = [NSURL fileURLWithPath:saveImagePath];
        self.documentController = [UIDocumentInteractionController interactionControllerWithURL:imageURL];
        self.documentController.delegate = self;
        self.documentController.annotation = [NSDictionary dictionaryWithObjectsAndKeys:[NSString stringWithFormat:@"ShareImage"], @"InstagramCaption", nil];
        self.documentController.UTI = @"com.instagram.photo";
        UIViewController *vc = GetAppController().rootViewController;
        [self.documentController presentOpenInMenuFromRect:CGRectMake(1, 1, 1, 1) inView:vc.view animated:YES];
        return YES;
    }else {
        NSLog(@"== Instagram not installed ==");
        return NO;
    }
     */

    NSMutableArray* sharingItems = [NSMutableArray new];
    
    [sharingItems addObject:imageData];
    
    UIActivityViewController *activityViewController =[[UIActivityViewController alloc] initWithActivityItems:sharingItems applicationActivities:nil];
    activityViewController.popoverPresentationController.sourceView = UnityGetGLViewController().view;
    activityViewController.popoverPresentationController.sourceRect = CGRectMake(UnityGetGLViewController().view.frame.size.width/2, UnityGetGLViewController().view.frame.size.height/4, 0, 0);
    activityViewController.excludedActivityTypes = @[ UIActivityTypePostToFacebook, // 脸书
                                                      UIActivityTypePostToTwitter, // 推特
                                                      UIActivityTypePostToWeibo, // 微博
                                                      UIActivityTypeMessage, // 信息
                                                      UIActivityTypeMail, // 邮件
                                                      UIActivityTypePrint, // 打印
                                                      UIActivityTypeCopyToPasteboard,
                                                      UIActivityTypeAssignToContact,
                                                      UIActivityTypeSaveToCameraRoll,
                                                      UIActivityTypeAddToReadingList,
                                                      UIActivityTypePostToFlickr,
                                                      UIActivityTypePostToVimeo,
                                                      UIActivityTypePostToTencentWeibo,
                                                      UIActivityTypeAirDrop,
                                                      UIActivityTypeOpenInIBooks,
                                                      @"com.tencent.xin.sharetimeline"
    ];
    
    UIActivityViewControllerCompletionHandler myblock = ^(UIActivityType _Nullable activitytype, BOOL completed){
        
        NSLog(@"分享渠道%@", activitytype);
        if (completed) {
            NSLog(@"分享成功");
            if (self.onSuccess != nil) {
                self.onSuccess([activitytype UTF8String]);
            }
        }else{
            NSLog(@"分享失败");
            if (self.onError != nil) {
                self.onError(-1, [activitytype UTF8String]);
            }
        }
    };
    activityViewController.completionHandler = myblock;

    [UnityGetGLViewController() presentViewController:activityViewController animated:YES completion:nil];
}

-(BOOL) openInstagram:(NSURL*)url
              options:(NSDictionary<UIApplicationOpenExternalURLOptionsKey, id> *)options
    completionHandler:(void (^ __nullable)(BOOL success))completion{
    if ([[UIApplication sharedApplication] canOpenURL:url]) {
        if (@available(iOS 10.0, *)){
            [[UIApplication sharedApplication] openURL:url options:@{} completionHandler:nil];
        }else{
            [[UIApplication sharedApplication] openURL:url];
        }
        return YES;
    }
    return NO;
}

-(NSString*) getCurrentDateString{
    NSDateFormatter* format = [[NSDateFormatter alloc] init];
    [format setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
    return [format stringFromDate:[NSDate date]];
}

#pragma mark - UIDocumentInteractionControllerDelegate
- (void)documentInteractionControllerWillPresentOptionsMenu:(UIDocumentInteractionController *)controller{
    NSLog(@"documentInteractionControllerWillPresentOptionsMenu");
}
- (void)documentInteractionControllerDidDismissOptionsMenu:(UIDocumentInteractionController *)controller{
    NSLog(@"documentInteractionControllerDidDismissOptionsMenu");
}


#pragma mark - Unity FacebookSDKManager 脚本中的接口

#if __cplusplus
extern "C" {
#endif
    bool ins_isIntalled(){
        return [InstagramApiManager.instance isInstalledInstagram];
    }
    
    bool ins_openApp(){
        return [InstagramApiManager.instance openInstagramApp];
    }
    
    bool ins_openAppAndCamara(){
        return [InstagramApiManager.instance openInstagramAndCamera];
    }
    
    bool ins_openAppAndMedia(int mediaId){
        return [InstagramApiManager.instance openInstagramAndMedia:mediaId];
    }
    
    bool ins_openAppAndUser(const char* username){
        NSString* nsUsername = [NSString stringWithUTF8String:username];
        return [InstagramApiManager.instance openInstagramAndUser:nsUsername];
    }
    
    bool ins_openAppAndLocations(int locationId){
        return [InstagramApiManager.instance openInstagramAndLocations:locationId];
    }
    
    bool ins_openAppAndTags(const char* tagName){
        NSString* nsTagName = [NSString stringWithUTF8String:tagName];
        return [InstagramApiManager.instance openInstagramAndTags:nsTagName];
    }
    
    void ins_openDocumentSharebyData(Byte* datas, int length, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError){
        if(![InstagramApiManager.instance isInstalledInstagram]){
            onError(-1, "Instagram not installed");
        }else{
            NSData* imageData = [NSData dataWithBytes:datas length:length];
            UIImage* image = [UIImage imageWithData:imageData];
            InstagramApiManager.instance.onSuccess = onSuccess;
            InstagramApiManager.instance.onCancel = onCancel;
            InstagramApiManager.instance.onError = onError;
            [InstagramApiManager.instance openInstagramWithImage:image];
        }
    }
    
    void ins_openDocumentShare(const char* imagePath, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError){
        if(![InstagramApiManager.instance isInstalledInstagram]){
            onError(-1, "Instagram not installed");
        }else{
            NSString* path = [NSString stringWithUTF8String:imagePath];
            UIImage* image = [UIImage imageWithContentsOfFile:path];
            InstagramApiManager.instance.onSuccess = onSuccess;
            InstagramApiManager.instance.onCancel = onCancel;
            InstagramApiManager.instance.onError = onError;
            [InstagramApiManager.instance openInstagramWithImage:image];
        }
    }
#if __cplusplus
}
#endif

@end
