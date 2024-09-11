//
//  InstagramApiManager.h
//  UnityFramework
//
//  Created by 晴天网络 on 2023/3/10.
//

#import "CommonApi.h"

@interface InstagramApiManager : NSObject<UIDocumentInteractionControllerDelegate>

/**
 分享回调
 */
@property (nonatomic, assign) U3DBridgeCallback_Success onSuccess;
@property (nonatomic, assign) U3DBridgeCallback_Cancel onCancel;
@property (nonatomic, assign) U3DBridgeCallback_Error onError;

/**
 文档交互
 */
@property (nonatomic,strong) UIDocumentInteractionController * documentController;

+ (InstagramApiManager *) instance;

/**
 是否安装了Instagram
 */
-(BOOL) isInstalledInstagram;

/**
 universal link 启动Instagram App
 */
-(BOOL) openInstagramApp;

/**
 universal link 启动Instagram App并打开相机，如果没有相机则打开照片库
 */
-(BOOL) openInstagramAndCamera;

/**
 universal link 启动Instagram App并加载指定编号的帖子
 @param mediaId 帖子ID
 */
-(BOOL) openInstagramAndMedia:(int)mediaId;

/**
 universal link 启动Instagram App并加载指定账号的用户
 @param username 用户账号
 */
-(BOOL) openInstagramAndUser:(NSString*)username;

/**
 universal link 启动Instagram App并加载指定编号的位置动态
 @param locationId 位置动态ID
 */
-(BOOL) openInstagramAndLocations:(int)locationId;

/**
 universal link 启动Instagram App并加载指定编号的话题标签界面
 @param tagName 话题名称
 */
-(BOOL) openInstagramAndTags:(NSString*)tagName;

/**
 universal link 启动Instagram App并通过苹果的文档交互API分享图片
 @param imageData 图片数据
 */
-(void) openInstagramWithImage:(UIImage*) imageData;
@end
