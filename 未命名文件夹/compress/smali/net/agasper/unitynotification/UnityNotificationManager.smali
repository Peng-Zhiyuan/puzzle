.class public Lnet/agasper/unitynotification/UnityNotificationManager;
.super Landroid/content/BroadcastReceiver;
.source "UnityNotificationManager.java"


# static fields
.field private static channels:Ljava/util/Set;
    .annotation system Ldalvik/annotation/Signature;
        value = {
            "Ljava/util/Set",
            "<",
            "Ljava/lang/String;",
            ">;"
        }
    .end annotation
.end field


# direct methods
.method static constructor <clinit>()V
    .locals 1

    .prologue
    .line 34
    new-instance v0, Ljava/util/HashSet;

    invoke-direct {v0}, Ljava/util/HashSet;-><init>()V

    sput-object v0, Lnet/agasper/unitynotification/UnityNotificationManager;->channels:Ljava/util/Set;

    return-void
.end method

.method public constructor <init>()V
    .locals 0

    .prologue
    .line 32
    invoke-direct {p0}, Landroid/content/BroadcastReceiver;-><init>()V

    return-void
.end method

.method public static CancelPendingNotification(I)V
    .locals 5
    .param p0, "id"    # I

    .prologue
    .line 230
    sget-object v1, Lcom/unity3d/player/UnityPlayer;->currentActivity:Landroid/app/Activity;

    .line 231
    .local v1, "currentActivity":Landroid/app/Activity;
    const-string v4, "alarm"

    invoke-virtual {v1, v4}, Landroid/app/Activity;->getSystemService(Ljava/lang/String;)Ljava/lang/Object;

    move-result-object v0

    check-cast v0, Landroid/app/AlarmManager;

    .line 232
    .local v0, "am":Landroid/app/AlarmManager;
    new-instance v2, Landroid/content/Intent;

    const-class v4, Lnet/agasper/unitynotification/UnityNotificationManager;

    invoke-direct {v2, v1, v4}, Landroid/content/Intent;-><init>(Landroid/content/Context;Ljava/lang/Class;)V

    .line 233
    .local v2, "intent":Landroid/content/Intent;
    const/high16 v4, 0x8000000

    invoke-static {v1, p0, v2, v4}, Landroid/app/PendingIntent;->getBroadcast(Landroid/content/Context;ILandroid/content/Intent;I)Landroid/app/PendingIntent;

    move-result-object v3

    .line 234
    .local v3, "pendingIntent":Landroid/app/PendingIntent;
    invoke-virtual {v0, v3}, Landroid/app/AlarmManager;->cancel(Landroid/app/PendingIntent;)V

    .line 235
    return-void
.end method

.method public static ClearShowingNotifications()V
    .locals 3

    .prologue
    .line 239
    sget-object v0, Lcom/unity3d/player/UnityPlayer;->currentActivity:Landroid/app/Activity;

    .line 240
    .local v0, "currentActivity":Landroid/app/Activity;
    const-string v2, "notification"

    invoke-virtual {v0, v2}, Landroid/app/Activity;->getSystemService(Ljava/lang/String;)Ljava/lang/Object;

    move-result-object v1

    check-cast v1, Landroid/app/NotificationManager;

    .line 241
    .local v1, "nm":Landroid/app/NotificationManager;
    invoke-virtual {v1}, Landroid/app/NotificationManager;->cancelAll()V

    .line 242
    return-void
.end method

.method public static CreateChannel(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;ILjava/lang/String;III[JLjava/lang/String;)V
    .locals 9
    .param p0, "identifier"    # Ljava/lang/String;
    .param p1, "name"    # Ljava/lang/String;
    .param p2, "description"    # Ljava/lang/String;
    .param p3, "importance"    # I
    .param p4, "soundName"    # Ljava/lang/String;
    .param p5, "enableLights"    # I
    .param p6, "lightColor"    # I
    .param p7, "enableVibration"    # I
    .param p8, "vibrationPattern"    # [J
    .param p9, "bundle"    # Ljava/lang/String;

    .prologue
    .line 37
    sget v6, Landroid/os/Build$VERSION;->SDK_INT:I

    const/16 v7, 0x1a

    if-ge v6, v7, :cond_0

    .line 58
    :goto_0
    return-void

    .line 40
    :cond_0
    sget-object v6, Lnet/agasper/unitynotification/UnityNotificationManager;->channels:Ljava/util/Set;

    invoke-interface {v6, p0}, Ljava/util/Set;->add(Ljava/lang/Object;)Z

    .line 42
    sget-object v6, Lcom/unity3d/player/UnityPlayer;->currentActivity:Landroid/app/Activity;

    const-string v7, "notification"

    invoke-virtual {v6, v7}, Landroid/app/Activity;->getSystemService(Ljava/lang/String;)Ljava/lang/Object;

    move-result-object v4

    check-cast v4, Landroid/app/NotificationManager;

    .line 43
    .local v4, "nm":Landroid/app/NotificationManager;
    new-instance v2, Landroid/app/NotificationChannel;

    invoke-direct {v2, p0, p1, p3}, Landroid/app/NotificationChannel;-><init>(Ljava/lang/String;Ljava/lang/CharSequence;I)V

    .line 44
    .local v2, "channel":Landroid/app/NotificationChannel;
    invoke-virtual {v2, p2}, Landroid/app/NotificationChannel;->setDescription(Ljava/lang/String;)V

    .line 45
    if-eqz p4, :cond_1

    .line 46
    sget-object v6, Lcom/unity3d/player/UnityPlayer;->currentActivity:Landroid/app/Activity;

    invoke-virtual {v6}, Landroid/app/Activity;->getResources()Landroid/content/res/Resources;

    move-result-object v5

    .line 47
    .local v5, "res":Landroid/content/res/Resources;
    new-instance v6, Ljava/lang/StringBuilder;

    invoke-direct {v6}, Ljava/lang/StringBuilder;-><init>()V

    const-string v7, "raw/"

    invoke-virtual {v6, v7}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v6

    invoke-virtual {v6, p4}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v6

    invoke-virtual {v6}, Ljava/lang/StringBuilder;->toString()Ljava/lang/String;

    move-result-object v6

    const/4 v7, 0x0

    sget-object v8, Lcom/unity3d/player/UnityPlayer;->currentActivity:Landroid/app/Activity;

    invoke-virtual {v8}, Landroid/app/Activity;->getPackageName()Ljava/lang/String;

    move-result-object v8

    invoke-virtual {v5, v6, v7, v8}, Landroid/content/res/Resources;->getIdentifier(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)I

    move-result v3

    .line 48
    .local v3, "id":I
    new-instance v6, Landroid/media/AudioAttributes$Builder;

    invoke-direct {v6}, Landroid/media/AudioAttributes$Builder;-><init>()V

    const/4 v7, 0x5

    invoke-virtual {v6, v7}, Landroid/media/AudioAttributes$Builder;->setUsage(I)Landroid/media/AudioAttributes$Builder;

    move-result-object v6

    const/4 v7, 0x4

    invoke-virtual {v6, v7}, Landroid/media/AudioAttributes$Builder;->setContentType(I)Landroid/media/AudioAttributes$Builder;

    move-result-object v6

    invoke-virtual {v6}, Landroid/media/AudioAttributes$Builder;->build()Landroid/media/AudioAttributes;

    move-result-object v1

    .line 49
    .local v1, "audioAttributes":Landroid/media/AudioAttributes;
    new-instance v6, Ljava/lang/StringBuilder;

    invoke-direct {v6}, Ljava/lang/StringBuilder;-><init>()V

    const-string v7, "android.resource://"

    invoke-virtual {v6, v7}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v6

    move-object/from16 v0, p9

    invoke-virtual {v6, v0}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v6

    const-string v7, "/"

    invoke-virtual {v6, v7}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v6

    invoke-virtual {v6, v3}, Ljava/lang/StringBuilder;->append(I)Ljava/lang/StringBuilder;

    move-result-object v6

    invoke-virtual {v6}, Ljava/lang/StringBuilder;->toString()Ljava/lang/String;

    move-result-object v6

    invoke-static {v6}, Landroid/net/Uri;->parse(Ljava/lang/String;)Landroid/net/Uri;

    move-result-object v6

    invoke-virtual {v2, v6, v1}, Landroid/app/NotificationChannel;->setSound(Landroid/net/Uri;Landroid/media/AudioAttributes;)V

    .line 51
    .end local v1    # "audioAttributes":Landroid/media/AudioAttributes;
    .end local v3    # "id":I
    .end local v5    # "res":Landroid/content/res/Resources;
    :cond_1
    const/4 v6, 0x1

    if-ne p5, v6, :cond_3

    const/4 v6, 0x1

    :goto_1
    invoke-virtual {v2, v6}, Landroid/app/NotificationChannel;->enableLights(Z)V

    .line 52
    invoke-virtual {v2, p6}, Landroid/app/NotificationChannel;->setLightColor(I)V

    .line 53
    const/4 v6, 0x1

    move/from16 v0, p7

    if-ne v0, v6, :cond_4

    const/4 v6, 0x1

    :goto_2
    invoke-virtual {v2, v6}, Landroid/app/NotificationChannel;->enableVibration(Z)V

    .line 54
    if-nez p8, :cond_2

    .line 55
    const/4 v6, 0x2

    new-array v0, v6, [J

    move-object/from16 p8, v0

    .end local p8    # "vibrationPattern":[J
    fill-array-data p8, :array_0

    .line 56
    .restart local p8    # "vibrationPattern":[J
    :cond_2
    move-object/from16 v0, p8

    invoke-virtual {v2, v0}, Landroid/app/NotificationChannel;->setVibrationPattern([J)V

    .line 57
    invoke-virtual {v4, v2}, Landroid/app/NotificationManager;->createNotificationChannel(Landroid/app/NotificationChannel;)V

    goto/16 :goto_0

    .line 51
    :cond_3
    const/4 v6, 0x0

    goto :goto_1

    .line 53
    :cond_4
    const/4 v6, 0x0

    goto :goto_2

    .line 55
    :array_0
    .array-data 8
        0x3e8
        0x3e8
    .end array-data
.end method

.method public static SetNotification(IJLjava/lang/String;Ljava/lang/String;Ljava/lang/String;ILjava/lang/String;IILjava/lang/String;Ljava/lang/String;ILjava/lang/String;Ljava/lang/String;Ljava/util/ArrayList;)V
    .locals 13
    .param p0, "id"    # I
    .param p1, "delayMs"    # J
    .param p3, "title"    # Ljava/lang/String;
    .param p4, "message"    # Ljava/lang/String;
    .param p5, "ticker"    # Ljava/lang/String;
    .param p6, "sound"    # I
    .param p7, "soundName"    # Ljava/lang/String;
    .param p8, "vibrate"    # I
    .param p9, "lights"    # I
    .param p10, "largeIconResource"    # Ljava/lang/String;
    .param p11, "smallIconResource"    # Ljava/lang/String;
    .param p12, "bgColor"    # I
    .param p13, "bundle"    # Ljava/lang/String;
    .param p14, "channel"    # Ljava/lang/String;
    .annotation system Ldalvik/annotation/Signature;
        value = {
            "(IJ",
            "Ljava/lang/String;",
            "Ljava/lang/String;",
            "Ljava/lang/String;",
            "I",
            "Ljava/lang/String;",
            "II",
            "Ljava/lang/String;",
            "Ljava/lang/String;",
            "I",
            "Ljava/lang/String;",
            "Ljava/lang/String;",
            "Ljava/util/ArrayList",
            "<",
            "Lnet/agasper/unitynotification/NotificationAction;",
            ">;)V"
        }
    .end annotation

    .prologue
    .line 73
    .local p15, "actions":Ljava/util/ArrayList;, "Ljava/util/ArrayList<Lnet/agasper/unitynotification/NotificationAction;>;"
    sget v2, Landroid/os/Build$VERSION;->SDK_INT:I

    const/16 v3, 0x1a

    if-lt v2, v3, :cond_1

    .line 74
    if-nez p14, :cond_0

    .line 75
    const-string p14, "default"

    .line 76
    :cond_0
    const/4 v2, 0x1

    move/from16 v0, p9

    if-ne v0, v2, :cond_2

    const/4 v5, 0x1

    :goto_0
    const/4 v2, 0x1

    move/from16 v0, p8

    if-ne v0, v2, :cond_3

    const/4 v6, 0x1

    :goto_1
    move-object/from16 v2, p14

    move-object/from16 v3, p3

    move-object/from16 v4, p7

    move-object/from16 v7, p13

    invoke-static/range {v2 .. v7}, Lnet/agasper/unitynotification/UnityNotificationManager;->createChannelIfNeeded(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;ZZLjava/lang/String;)V

    .line 79
    :cond_1
    sget-object v10, Lcom/unity3d/player/UnityPlayer;->currentActivity:Landroid/app/Activity;

    .line 80
    .local v10, "currentActivity":Landroid/app/Activity;
    const-string v2, "alarm"

    invoke-virtual {v10, v2}, Landroid/app/Activity;->getSystemService(Ljava/lang/String;)Ljava/lang/Object;

    move-result-object v8

    check-cast v8, Landroid/app/AlarmManager;

    .line 81
    .local v8, "am":Landroid/app/AlarmManager;
    new-instance v11, Landroid/content/Intent;

    const-class v2, Lnet/agasper/unitynotification/UnityNotificationManager;

    invoke-direct {v11, v10, v2}, Landroid/content/Intent;-><init>(Landroid/content/Context;Ljava/lang/Class;)V

    .line 82
    .local v11, "intent":Landroid/content/Intent;
    const-string v2, "ticker"

    move-object/from16 v0, p5

    invoke-virtual {v11, v2, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 83
    const-string v2, "title"

    move-object/from16 v0, p3

    invoke-virtual {v11, v2, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 84
    const-string v2, "message"

    move-object/from16 v0, p4

    invoke-virtual {v11, v2, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 85
    const-string v2, "id"

    invoke-virtual {v11, v2, p0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;I)Landroid/content/Intent;

    .line 86
    const-string v2, "color"

    move/from16 v0, p12

    invoke-virtual {v11, v2, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;I)Landroid/content/Intent;

    .line 87
    const-string v3, "sound"

    const/4 v2, 0x1

    move/from16 v0, p6

    if-ne v0, v2, :cond_4

    const/4 v2, 0x1

    :goto_2
    invoke-virtual {v11, v3, v2}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Z)Landroid/content/Intent;

    .line 88
    const-string v2, "soundName"

    move-object/from16 v0, p7

    invoke-virtual {v11, v2, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 89
    const-string v3, "vibrate"

    const/4 v2, 0x1

    move/from16 v0, p8

    if-ne v0, v2, :cond_5

    const/4 v2, 0x1

    :goto_3
    invoke-virtual {v11, v3, v2}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Z)Landroid/content/Intent;

    .line 90
    const-string v3, "lights"

    const/4 v2, 0x1

    move/from16 v0, p9

    if-ne v0, v2, :cond_6

    const/4 v2, 0x1

    :goto_4
    invoke-virtual {v11, v3, v2}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Z)Landroid/content/Intent;

    .line 91
    const-string v2, "l_icon"

    move-object/from16 v0, p10

    invoke-virtual {v11, v2, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 92
    const-string v2, "s_icon"

    move-object/from16 v0, p11

    invoke-virtual {v11, v2, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 93
    const-string v2, "bundle"

    move-object/from16 v0, p13

    invoke-virtual {v11, v2, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 94
    const-string v2, "channel"

    move-object/from16 v0, p14

    invoke-virtual {v11, v2, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 95
    new-instance v9, Landroid/os/Bundle;

    invoke-direct {v9}, Landroid/os/Bundle;-><init>()V

    .line 96
    .local v9, "b":Landroid/os/Bundle;
    const-string v2, "actions"

    move-object/from16 v0, p15

    invoke-virtual {v9, v2, v0}, Landroid/os/Bundle;->putParcelableArrayList(Ljava/lang/String;Ljava/util/ArrayList;)V

    .line 97
    const-string v2, "actionsBundle"

    invoke-virtual {v11, v2, v9}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Landroid/os/Bundle;)Landroid/content/Intent;

    .line 98
    sget v2, Landroid/os/Build$VERSION;->SDK_INT:I

    const/16 v3, 0x17

    if-lt v2, v3, :cond_7

    .line 99
    const/4 v2, 0x0

    invoke-static {}, Ljava/lang/System;->currentTimeMillis()J

    move-result-wide v4

    add-long/2addr v4, p1

    const/high16 v3, 0x8000000

    invoke-static {v10, p0, v11, v3}, Landroid/app/PendingIntent;->getBroadcast(Landroid/content/Context;ILandroid/content/Intent;I)Landroid/app/PendingIntent;

    move-result-object v3

    invoke-virtual {v8, v2, v4, v5, v3}, Landroid/app/AlarmManager;->setExact(IJLandroid/app/PendingIntent;)V

    .line 102
    :goto_5
    return-void

    .line 76
    .end local v8    # "am":Landroid/app/AlarmManager;
    .end local v9    # "b":Landroid/os/Bundle;
    .end local v10    # "currentActivity":Landroid/app/Activity;
    .end local v11    # "intent":Landroid/content/Intent;
    :cond_2
    const/4 v5, 0x0

    goto/16 :goto_0

    :cond_3
    const/4 v6, 0x0

    goto/16 :goto_1

    .line 87
    .restart local v8    # "am":Landroid/app/AlarmManager;
    .restart local v10    # "currentActivity":Landroid/app/Activity;
    .restart local v11    # "intent":Landroid/content/Intent;
    :cond_4
    const/4 v2, 0x0

    goto :goto_2

    .line 89
    :cond_5
    const/4 v2, 0x0

    goto :goto_3

    .line 90
    :cond_6
    const/4 v2, 0x0

    goto :goto_4

    .line 101
    .restart local v9    # "b":Landroid/os/Bundle;
    :cond_7
    const/4 v2, 0x0

    invoke-static {}, Ljava/lang/System;->currentTimeMillis()J

    move-result-wide v4

    add-long/2addr v4, p1

    const/high16 v3, 0x8000000

    invoke-static {v10, p0, v11, v3}, Landroid/app/PendingIntent;->getBroadcast(Landroid/content/Context;ILandroid/content/Intent;I)Landroid/app/PendingIntent;

    move-result-object v3

    invoke-virtual {v8, v2, v4, v5, v3}, Landroid/app/AlarmManager;->set(IJLandroid/app/PendingIntent;)V

    goto :goto_5
.end method

.method public static SetRepeatingNotification(IJLjava/lang/String;Ljava/lang/String;Ljava/lang/String;JILjava/lang/String;IILjava/lang/String;Ljava/lang/String;ILjava/lang/String;Ljava/lang/String;Ljava/util/ArrayList;)V
    .locals 12
    .param p0, "id"    # I
    .param p1, "delayMs"    # J
    .param p3, "title"    # Ljava/lang/String;
    .param p4, "message"    # Ljava/lang/String;
    .param p5, "ticker"    # Ljava/lang/String;
    .param p6, "rep"    # J
    .param p8, "sound"    # I
    .param p9, "soundName"    # Ljava/lang/String;
    .param p10, "vibrate"    # I
    .param p11, "lights"    # I
    .param p12, "largeIconResource"    # Ljava/lang/String;
    .param p13, "smallIconResource"    # Ljava/lang/String;
    .param p14, "bgColor"    # I
    .param p15, "bundle"    # Ljava/lang/String;
    .param p16, "channel"    # Ljava/lang/String;
    .annotation system Ldalvik/annotation/Signature;
        value = {
            "(IJ",
            "Ljava/lang/String;",
            "Ljava/lang/String;",
            "Ljava/lang/String;",
            "JI",
            "Ljava/lang/String;",
            "II",
            "Ljava/lang/String;",
            "Ljava/lang/String;",
            "I",
            "Ljava/lang/String;",
            "Ljava/lang/String;",
            "Ljava/util/ArrayList",
            "<",
            "Lnet/agasper/unitynotification/NotificationAction;",
            ">;)V"
        }
    .end annotation

    .prologue
    .line 107
    .local p17, "actions":Ljava/util/ArrayList;, "Ljava/util/ArrayList<Lnet/agasper/unitynotification/NotificationAction;>;"
    sget v3, Landroid/os/Build$VERSION;->SDK_INT:I

    const/16 v4, 0x1a

    if-lt v3, v4, :cond_1

    .line 108
    if-nez p16, :cond_0

    .line 109
    const-string p16, "default"

    .line 110
    :cond_0
    const/4 v3, 0x1

    move/from16 v0, p11

    if-ne v0, v3, :cond_2

    const/4 v5, 0x1

    :goto_0
    const/4 v3, 0x1

    move/from16 v0, p10

    if-ne v0, v3, :cond_3

    const/4 v6, 0x1

    :goto_1
    move-object/from16 v2, p16

    move-object v3, p3

    move-object/from16 v4, p9

    move-object/from16 v7, p15

    invoke-static/range {v2 .. v7}, Lnet/agasper/unitynotification/UnityNotificationManager;->createChannelIfNeeded(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;ZZLjava/lang/String;)V

    .line 113
    :cond_1
    sget-object v10, Lcom/unity3d/player/UnityPlayer;->currentActivity:Landroid/app/Activity;

    .line 114
    .local v10, "currentActivity":Landroid/app/Activity;
    const-string v3, "alarm"

    invoke-virtual {v10, v3}, Landroid/app/Activity;->getSystemService(Ljava/lang/String;)Ljava/lang/Object;

    move-result-object v2

    check-cast v2, Landroid/app/AlarmManager;

    .line 115
    .local v2, "am":Landroid/app/AlarmManager;
    new-instance v11, Landroid/content/Intent;

    const-class v3, Lnet/agasper/unitynotification/UnityNotificationManager;

    invoke-direct {v11, v10, v3}, Landroid/content/Intent;-><init>(Landroid/content/Context;Ljava/lang/Class;)V

    .line 116
    .local v11, "intent":Landroid/content/Intent;
    const-string v3, "ticker"

    move-object/from16 v0, p5

    invoke-virtual {v11, v3, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 117
    const-string v3, "title"

    invoke-virtual {v11, v3, p3}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 118
    const-string v3, "message"

    move-object/from16 v0, p4

    invoke-virtual {v11, v3, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 119
    const-string v3, "id"

    invoke-virtual {v11, v3, p0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;I)Landroid/content/Intent;

    .line 120
    const-string v3, "color"

    move/from16 v0, p14

    invoke-virtual {v11, v3, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;I)Landroid/content/Intent;

    .line 121
    const-string v4, "sound"

    const/4 v3, 0x1

    move/from16 v0, p8

    if-ne v0, v3, :cond_4

    const/4 v3, 0x1

    :goto_2
    invoke-virtual {v11, v4, v3}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Z)Landroid/content/Intent;

    .line 122
    const-string v3, "soundName"

    move-object/from16 v0, p9

    invoke-virtual {v11, v3, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 123
    const-string v4, "vibrate"

    const/4 v3, 0x1

    move/from16 v0, p10

    if-ne v0, v3, :cond_5

    const/4 v3, 0x1

    :goto_3
    invoke-virtual {v11, v4, v3}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Z)Landroid/content/Intent;

    .line 124
    const-string v4, "lights"

    const/4 v3, 0x1

    move/from16 v0, p11

    if-ne v0, v3, :cond_6

    const/4 v3, 0x1

    :goto_4
    invoke-virtual {v11, v4, v3}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Z)Landroid/content/Intent;

    .line 125
    const-string v3, "l_icon"

    move-object/from16 v0, p12

    invoke-virtual {v11, v3, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 126
    const-string v3, "s_icon"

    move-object/from16 v0, p13

    invoke-virtual {v11, v3, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 127
    const-string v3, "bundle"

    move-object/from16 v0, p15

    invoke-virtual {v11, v3, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 128
    const-string v3, "channel"

    move-object/from16 v0, p16

    invoke-virtual {v11, v3, v0}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 129
    new-instance v9, Landroid/os/Bundle;

    invoke-direct {v9}, Landroid/os/Bundle;-><init>()V

    .line 130
    .local v9, "b":Landroid/os/Bundle;
    const-string v3, "actions"

    move-object/from16 v0, p17

    invoke-virtual {v9, v3, v0}, Landroid/os/Bundle;->putParcelableArrayList(Ljava/lang/String;Ljava/util/ArrayList;)V

    .line 131
    const-string v3, "actionsBundle"

    invoke-virtual {v11, v3, v9}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Landroid/os/Bundle;)Landroid/content/Intent;

    .line 132
    const/4 v3, 0x0

    invoke-static {}, Ljava/lang/System;->currentTimeMillis()J

    move-result-wide v4

    add-long/2addr v4, p1

    const/4 v6, 0x0

    invoke-static {v10, p0, v11, v6}, Landroid/app/PendingIntent;->getBroadcast(Landroid/content/Context;ILandroid/content/Intent;I)Landroid/app/PendingIntent;

    move-result-object v8

    move-wide/from16 v6, p6

    invoke-virtual/range {v2 .. v8}, Landroid/app/AlarmManager;->setRepeating(IJJLandroid/app/PendingIntent;)V

    .line 133
    return-void

    .line 110
    .end local v2    # "am":Landroid/app/AlarmManager;
    .end local v9    # "b":Landroid/os/Bundle;
    .end local v10    # "currentActivity":Landroid/app/Activity;
    .end local v11    # "intent":Landroid/content/Intent;
    :cond_2
    const/4 v5, 0x0

    goto/16 :goto_0

    :cond_3
    const/4 v6, 0x0

    goto/16 :goto_1

    .line 121
    .restart local v2    # "am":Landroid/app/AlarmManager;
    .restart local v10    # "currentActivity":Landroid/app/Activity;
    .restart local v11    # "intent":Landroid/content/Intent;
    :cond_4
    const/4 v3, 0x0

    goto :goto_2

    .line 123
    :cond_5
    const/4 v3, 0x0

    goto :goto_3

    .line 124
    :cond_6
    const/4 v3, 0x0

    goto :goto_4
.end method

.method private static buildActionIntent(Lnet/agasper/unitynotification/NotificationAction;I)Landroid/app/PendingIntent;
    .locals 4
    .param p0, "action"    # Lnet/agasper/unitynotification/NotificationAction;
    .param p1, "id"    # I

    .prologue
    .line 218
    sget-object v0, Lcom/unity3d/player/UnityPlayer;->currentActivity:Landroid/app/Activity;

    .line 219
    .local v0, "currentActivity":Landroid/app/Activity;
    new-instance v1, Landroid/content/Intent;

    const-class v2, Lnet/agasper/unitynotification/UnityNotificationActionHandler;

    invoke-direct {v1, v0, v2}, Landroid/content/Intent;-><init>(Landroid/content/Context;Ljava/lang/Class;)V

    .line 220
    .local v1, "intent":Landroid/content/Intent;
    const-string v2, "id"

    invoke-virtual {v1, v2, p1}, Landroid/content/Intent;->putExtra(Ljava/lang/String;I)Landroid/content/Intent;

    .line 221
    const-string v2, "gameObject"

    invoke-virtual {p0}, Lnet/agasper/unitynotification/NotificationAction;->getGameObject()Ljava/lang/String;

    move-result-object v3

    invoke-virtual {v1, v2, v3}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 222
    const-string v2, "handlerMethod"

    invoke-virtual {p0}, Lnet/agasper/unitynotification/NotificationAction;->getHandlerMethod()Ljava/lang/String;

    move-result-object v3

    invoke-virtual {v1, v2, v3}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 223
    const-string v2, "actionId"

    invoke-virtual {p0}, Lnet/agasper/unitynotification/NotificationAction;->getIdentifier()Ljava/lang/String;

    move-result-object v3

    invoke-virtual {v1, v2, v3}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Ljava/lang/String;)Landroid/content/Intent;

    .line 224
    const-string v2, "foreground"

    invoke-virtual {p0}, Lnet/agasper/unitynotification/NotificationAction;->isForeground()Z

    move-result v3

    invoke-virtual {v1, v2, v3}, Landroid/content/Intent;->putExtra(Ljava/lang/String;Z)Landroid/content/Intent;

    .line 225
    const/high16 v2, 0x8000000

    invoke-static {v0, p1, v1, v2}, Landroid/app/PendingIntent;->getBroadcast(Landroid/content/Context;ILandroid/content/Intent;I)Landroid/app/PendingIntent;

    move-result-object v2

    return-object v2
.end method

.method private static createChannelIfNeeded(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;ZZLjava/lang/String;)V
    .locals 10
    .param p0, "identifier"    # Ljava/lang/String;
    .param p1, "name"    # Ljava/lang/String;
    .param p2, "soundName"    # Ljava/lang/String;
    .param p3, "enableLights"    # Z
    .param p4, "enableVibration"    # Z
    .param p5, "bundle"    # Ljava/lang/String;
    .annotation build Landroid/annotation/TargetApi;
        value = 0x18
    .end annotation

    .prologue
    const/4 v7, 0x1

    const/4 v0, 0x0

    .line 62
    sget-object v1, Lnet/agasper/unitynotification/UnityNotificationManager;->channels:Ljava/util/Set;

    invoke-interface {v1, p0}, Ljava/util/Set;->contains(Ljava/lang/Object;)Z

    move-result v1

    if-eqz v1, :cond_0

    .line 67
    :goto_0
    return-void

    .line 64
    :cond_0
    sget-object v1, Lnet/agasper/unitynotification/UnityNotificationManager;->channels:Ljava/util/Set;

    invoke-interface {v1, p0}, Ljava/util/Set;->add(Ljava/lang/Object;)Z

    .line 66
    new-instance v1, Ljava/lang/StringBuilder;

    invoke-direct {v1}, Ljava/lang/StringBuilder;-><init>()V

    invoke-virtual {v1, p0}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v1

    const-string v2, " notifications"

    invoke-virtual {v1, v2}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v1

    invoke-virtual {v1}, Ljava/lang/StringBuilder;->toString()Ljava/lang/String;

    move-result-object v2

    const/4 v3, 0x3

    if-eqz p3, :cond_1

    move v5, v7

    :goto_1
    const v6, -0xff0100

    if-eqz p4, :cond_2

    :goto_2
    const/4 v8, 0x0

    move-object v0, p0

    move-object v1, p1

    move-object v4, p2

    move-object v9, p5

    invoke-static/range {v0 .. v9}, Lnet/agasper/unitynotification/UnityNotificationManager;->CreateChannel(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;ILjava/lang/String;III[JLjava/lang/String;)V

    goto :goto_0

    :cond_1
    move v5, v0

    goto :goto_1

    :cond_2
    move v7, v0

    goto :goto_2
.end method


# virtual methods
.method public onReceive(Landroid/content/Context;Landroid/content/Intent;)V
    .locals 33
    .param p1, "context"    # Landroid/content/Context;
    .param p2, "intent"    # Landroid/content/Intent;

    .prologue
    .line 137
    const-string v30, "notification"

    move-object/from16 v0, p1

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Context;->getSystemService(Ljava/lang/String;)Ljava/lang/Object;

    move-result-object v20

    check-cast v20, Landroid/app/NotificationManager;

    .line 139
    .local v20, "notificationManager":Landroid/app/NotificationManager;
    const-string v30, "ticker"

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Intent;->getStringExtra(Ljava/lang/String;)Ljava/lang/String;

    move-result-object v27

    .line 140
    .local v27, "ticker":Ljava/lang/String;
    const-string v30, "title"

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Intent;->getStringExtra(Ljava/lang/String;)Ljava/lang/String;

    move-result-object v28

    .line 141
    .local v28, "title":Ljava/lang/String;
    const-string v30, "message"

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Intent;->getStringExtra(Ljava/lang/String;)Ljava/lang/String;

    move-result-object v17

    .line 142
    .local v17, "message":Ljava/lang/String;
    const-string v30, "s_icon"

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Intent;->getStringExtra(Ljava/lang/String;)Ljava/lang/String;

    move-result-object v23

    .line 143
    .local v23, "s_icon":Ljava/lang/String;
    const-string v30, "l_icon"

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Intent;->getStringExtra(Ljava/lang/String;)Ljava/lang/String;

    move-result-object v15

    .line 144
    .local v15, "l_icon":Ljava/lang/String;
    const-string v30, "color"

    const/16 v31, 0x0

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    move/from16 v2, v31

    invoke-virtual {v0, v1, v2}, Landroid/content/Intent;->getIntExtra(Ljava/lang/String;I)I

    move-result v10

    .line 145
    .local v10, "color":I
    const-string v30, "bundle"

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Intent;->getStringExtra(Ljava/lang/String;)Ljava/lang/String;

    move-result-object v8

    .line 146
    .local v8, "bundle":Ljava/lang/String;
    const-string v30, "sound"

    const/16 v31, 0x0

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    move/from16 v2, v31

    invoke-virtual {v0, v1, v2}, Landroid/content/Intent;->getBooleanExtra(Ljava/lang/String;Z)Z

    move-result v30

    invoke-static/range {v30 .. v30}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object v24

    .line 147
    .local v24, "sound":Ljava/lang/Boolean;
    const-string v30, "soundName"

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Intent;->getStringExtra(Ljava/lang/String;)Ljava/lang/String;

    move-result-object v25

    .line 148
    .local v25, "soundName":Ljava/lang/String;
    const-string v30, "vibrate"

    const/16 v31, 0x0

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    move/from16 v2, v31

    invoke-virtual {v0, v1, v2}, Landroid/content/Intent;->getBooleanExtra(Ljava/lang/String;Z)Z

    move-result v30

    invoke-static/range {v30 .. v30}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object v29

    .line 149
    .local v29, "vibrate":Ljava/lang/Boolean;
    const-string v30, "lights"

    const/16 v31, 0x0

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    move/from16 v2, v31

    invoke-virtual {v0, v1, v2}, Landroid/content/Intent;->getBooleanExtra(Ljava/lang/String;Z)Z

    move-result v30

    invoke-static/range {v30 .. v30}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object v16

    .line 150
    .local v16, "lights":Ljava/lang/Boolean;
    const-string v30, "id"

    const/16 v31, 0x0

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    move/from16 v2, v31

    invoke-virtual {v0, v1, v2}, Landroid/content/Intent;->getIntExtra(Ljava/lang/String;I)I

    move-result v13

    .line 151
    .local v13, "id":I
    const-string v30, "channel"

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Intent;->getStringExtra(Ljava/lang/String;)Ljava/lang/String;

    move-result-object v9

    .line 152
    .local v9, "channel":Ljava/lang/String;
    const-string v30, "actionsBundle"

    move-object/from16 v0, p2

    move-object/from16 v1, v30

    invoke-virtual {v0, v1}, Landroid/content/Intent;->getBundleExtra(Ljava/lang/String;)Landroid/os/Bundle;

    move-result-object v6

    .line 153
    .local v6, "b":Landroid/os/Bundle;
    const-string v30, "actions"

    move-object/from16 v0, v30

    invoke-virtual {v6, v0}, Landroid/os/Bundle;->getParcelableArrayList(Ljava/lang/String;)Ljava/util/ArrayList;

    move-result-object v5

    .line 155
    .local v5, "actions":Ljava/util/ArrayList;, "Ljava/util/ArrayList<Lnet/agasper/unitynotification/NotificationAction;>;"
    invoke-virtual/range {p1 .. p1}, Landroid/content/Context;->getResources()Landroid/content/res/Resources;

    move-result-object v22

    .line 157
    .local v22, "res":Landroid/content/res/Resources;
    invoke-virtual/range {p1 .. p1}, Landroid/content/Context;->getPackageManager()Landroid/content/pm/PackageManager;

    move-result-object v30

    move-object/from16 v0, v30

    invoke-virtual {v0, v8}, Landroid/content/pm/PackageManager;->getLaunchIntentForPackage(Ljava/lang/String;)Landroid/content/Intent;

    move-result-object v19

    .line 159
    .local v19, "notificationIntent":Landroid/content/Intent;
    invoke-static/range {p1 .. p1}, Landroid/support/v4/app/TaskStackBuilder;->create(Landroid/content/Context;)Landroid/support/v4/app/TaskStackBuilder;

    move-result-object v26

    .line 160
    .local v26, "stackBuilder":Landroid/support/v4/app/TaskStackBuilder;
    move-object/from16 v0, v26

    move-object/from16 v1, v19

    invoke-virtual {v0, v1}, Landroid/support/v4/app/TaskStackBuilder;->addNextIntent(Landroid/content/Intent;)Landroid/support/v4/app/TaskStackBuilder;

    .line 162
    const/16 v30, 0x0

    const/high16 v31, 0x8000000

    move-object/from16 v0, p1

    move/from16 v1, v30

    move-object/from16 v2, v19

    move/from16 v3, v31

    invoke-static {v0, v1, v2, v3}, Landroid/app/PendingIntent;->getActivity(Landroid/content/Context;ILandroid/content/Intent;I)Landroid/app/PendingIntent;

    move-result-object v21

    .line 165
    .local v21, "pendingIntent":Landroid/app/PendingIntent;
    if-nez v9, :cond_0

    .line 166
    const-string v9, "default"

    .line 168
    :cond_0
    new-instance v7, Landroid/support/v4/app/NotificationCompat$Builder;

    move-object/from16 v0, p1

    invoke-direct {v7, v0, v9}, Landroid/support/v4/app/NotificationCompat$Builder;-><init>(Landroid/content/Context;Ljava/lang/String;)V

    .line 170
    .local v7, "builder":Landroid/support/v4/app/NotificationCompat$Builder;
    move-object/from16 v0, v21

    invoke-virtual {v7, v0}, Landroid/support/v4/app/NotificationCompat$Builder;->setContentIntent(Landroid/app/PendingIntent;)Landroid/support/v4/app/NotificationCompat$Builder;

    move-result-object v30

    const/16 v31, 0x1

    .line 171
    invoke-virtual/range {v30 .. v31}, Landroid/support/v4/app/NotificationCompat$Builder;->setAutoCancel(Z)Landroid/support/v4/app/NotificationCompat$Builder;

    move-result-object v30

    .line 172
    move-object/from16 v0, v30

    move-object/from16 v1, v28

    invoke-virtual {v0, v1}, Landroid/support/v4/app/NotificationCompat$Builder;->setContentTitle(Ljava/lang/CharSequence;)Landroid/support/v4/app/NotificationCompat$Builder;

    move-result-object v30

    .line 173
    move-object/from16 v0, v30

    move-object/from16 v1, v17

    invoke-virtual {v0, v1}, Landroid/support/v4/app/NotificationCompat$Builder;->setContentText(Ljava/lang/CharSequence;)Landroid/support/v4/app/NotificationCompat$Builder;

    .line 175
    sget v30, Landroid/os/Build$VERSION;->SDK_INT:I

    const/16 v31, 0x15

    move/from16 v0, v30

    move/from16 v1, v31

    if-lt v0, v1, :cond_1

    .line 176
    invoke-virtual {v7, v10}, Landroid/support/v4/app/NotificationCompat$Builder;->setColor(I)Landroid/support/v4/app/NotificationCompat$Builder;

    .line 178
    :cond_1
    if-eqz v27, :cond_2

    invoke-virtual/range {v27 .. v27}, Ljava/lang/String;->length()I

    move-result v30

    if-lez v30, :cond_2

    .line 179
    move-object/from16 v0, v27

    invoke-virtual {v7, v0}, Landroid/support/v4/app/NotificationCompat$Builder;->setTicker(Ljava/lang/CharSequence;)Landroid/support/v4/app/NotificationCompat$Builder;

    .line 181
    :cond_2
    if-eqz v23, :cond_3

    invoke-virtual/range {v23 .. v23}, Ljava/lang/String;->length()I

    move-result v30

    if-lez v30, :cond_3

    .line 182
    const-string v30, "drawable"

    invoke-virtual/range {p1 .. p1}, Landroid/content/Context;->getPackageName()Ljava/lang/String;

    move-result-object v31

    move-object/from16 v0, v22

    move-object/from16 v1, v23

    move-object/from16 v2, v30

    move-object/from16 v3, v31

    invoke-virtual {v0, v1, v2, v3}, Landroid/content/res/Resources;->getIdentifier(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)I

    move-result v30

    move/from16 v0, v30

    invoke-virtual {v7, v0}, Landroid/support/v4/app/NotificationCompat$Builder;->setSmallIcon(I)Landroid/support/v4/app/NotificationCompat$Builder;

    .line 184
    :cond_3
    if-eqz v15, :cond_4

    invoke-virtual {v15}, Ljava/lang/String;->length()I

    move-result v30

    if-lez v30, :cond_4

    .line 185
    const-string v30, "drawable"

    invoke-virtual/range {p1 .. p1}, Landroid/content/Context;->getPackageName()Ljava/lang/String;

    move-result-object v31

    move-object/from16 v0, v22

    move-object/from16 v1, v30

    move-object/from16 v2, v31

    invoke-virtual {v0, v15, v1, v2}, Landroid/content/res/Resources;->getIdentifier(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)I

    move-result v30

    move-object/from16 v0, v22

    move/from16 v1, v30

    invoke-static {v0, v1}, Landroid/graphics/BitmapFactory;->decodeResource(Landroid/content/res/Resources;I)Landroid/graphics/Bitmap;

    move-result-object v30

    move-object/from16 v0, v30

    invoke-virtual {v7, v0}, Landroid/support/v4/app/NotificationCompat$Builder;->setLargeIcon(Landroid/graphics/Bitmap;)Landroid/support/v4/app/NotificationCompat$Builder;

    .line 187
    :cond_4
    invoke-virtual/range {v24 .. v24}, Ljava/lang/Boolean;->booleanValue()Z

    move-result v30

    if-eqz v30, :cond_5

    .line 188
    if-eqz v25, :cond_9

    .line 189
    new-instance v30, Ljava/lang/StringBuilder;

    invoke-direct/range {v30 .. v30}, Ljava/lang/StringBuilder;-><init>()V

    const-string v31, "raw/"

    invoke-virtual/range {v30 .. v31}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v30

    move-object/from16 v0, v30

    move-object/from16 v1, v25

    invoke-virtual {v0, v1}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v30

    invoke-virtual/range {v30 .. v30}, Ljava/lang/StringBuilder;->toString()Ljava/lang/String;

    move-result-object v30

    const/16 v31, 0x0

    invoke-virtual/range {p1 .. p1}, Landroid/content/Context;->getPackageName()Ljava/lang/String;

    move-result-object v32

    move-object/from16 v0, v22

    move-object/from16 v1, v30

    move-object/from16 v2, v31

    move-object/from16 v3, v32

    invoke-virtual {v0, v1, v2, v3}, Landroid/content/res/Resources;->getIdentifier(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)I

    move-result v14

    .line 190
    .local v14, "identifier":I
    new-instance v30, Ljava/lang/StringBuilder;

    invoke-direct/range {v30 .. v30}, Ljava/lang/StringBuilder;-><init>()V

    const-string v31, "android.resource://"

    invoke-virtual/range {v30 .. v31}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v30

    move-object/from16 v0, v30

    invoke-virtual {v0, v8}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v30

    const-string v31, "/"

    invoke-virtual/range {v30 .. v31}, Ljava/lang/StringBuilder;->append(Ljava/lang/String;)Ljava/lang/StringBuilder;

    move-result-object v30

    move-object/from16 v0, v30

    invoke-virtual {v0, v14}, Ljava/lang/StringBuilder;->append(I)Ljava/lang/StringBuilder;

    move-result-object v30

    invoke-virtual/range {v30 .. v30}, Ljava/lang/StringBuilder;->toString()Ljava/lang/String;

    move-result-object v30

    invoke-static/range {v30 .. v30}, Landroid/net/Uri;->parse(Ljava/lang/String;)Landroid/net/Uri;

    move-result-object v30

    move-object/from16 v0, v30

    invoke-virtual {v7, v0}, Landroid/support/v4/app/NotificationCompat$Builder;->setSound(Landroid/net/Uri;)Landroid/support/v4/app/NotificationCompat$Builder;

    .line 195
    .end local v14    # "identifier":I
    :cond_5
    :goto_0
    invoke-virtual/range {v29 .. v29}, Ljava/lang/Boolean;->booleanValue()Z

    move-result v30

    if-eqz v30, :cond_6

    .line 196
    const/16 v30, 0x2

    move/from16 v0, v30

    new-array v0, v0, [J

    move-object/from16 v30, v0

    fill-array-data v30, :array_0

    move-object/from16 v0, v30

    invoke-virtual {v7, v0}, Landroid/support/v4/app/NotificationCompat$Builder;->setVibrate([J)Landroid/support/v4/app/NotificationCompat$Builder;

    .line 200
    :cond_6
    invoke-virtual/range {v16 .. v16}, Ljava/lang/Boolean;->booleanValue()Z

    move-result v30

    if-eqz v30, :cond_7

    .line 201
    const v30, -0xff0100

    const/16 v31, 0xbb8

    const/16 v32, 0xbb8

    move/from16 v0, v30

    move/from16 v1, v31

    move/from16 v2, v32

    invoke-virtual {v7, v0, v1, v2}, Landroid/support/v4/app/NotificationCompat$Builder;->setLights(III)Landroid/support/v4/app/NotificationCompat$Builder;

    .line 203
    :cond_7
    if-eqz v5, :cond_a

    .line 204
    const/4 v11, 0x0

    .local v11, "i":I
    :goto_1
    invoke-virtual {v5}, Ljava/util/ArrayList;->size()I

    move-result v30

    move/from16 v0, v30

    if-ge v11, v0, :cond_a

    .line 205
    invoke-virtual {v5, v11}, Ljava/util/ArrayList;->get(I)Ljava/lang/Object;

    move-result-object v4

    check-cast v4, Lnet/agasper/unitynotification/NotificationAction;

    .line 206
    .local v4, "action":Lnet/agasper/unitynotification/NotificationAction;
    const/4 v12, 0x0

    .line 207
    .local v12, "icon":I
    invoke-virtual {v4}, Lnet/agasper/unitynotification/NotificationAction;->getIcon()Ljava/lang/String;

    move-result-object v30

    if-eqz v30, :cond_8

    invoke-virtual {v4}, Lnet/agasper/unitynotification/NotificationAction;->getIcon()Ljava/lang/String;

    move-result-object v30

    invoke-virtual/range {v30 .. v30}, Ljava/lang/String;->length()I

    move-result v30

    if-lez v30, :cond_8

    .line 208
    invoke-virtual {v4}, Lnet/agasper/unitynotification/NotificationAction;->getIcon()Ljava/lang/String;

    move-result-object v30

    const-string v31, "drawable"

    invoke-virtual/range {p1 .. p1}, Landroid/content/Context;->getPackageName()Ljava/lang/String;

    move-result-object v32

    move-object/from16 v0, v22

    move-object/from16 v1, v30

    move-object/from16 v2, v31

    move-object/from16 v3, v32

    invoke-virtual {v0, v1, v2, v3}, Landroid/content/res/Resources;->getIdentifier(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)I

    move-result v12

    .line 209
    :cond_8
    invoke-virtual {v4}, Lnet/agasper/unitynotification/NotificationAction;->getTitle()Ljava/lang/String;

    move-result-object v30

    invoke-static {v4, v11}, Lnet/agasper/unitynotification/UnityNotificationManager;->buildActionIntent(Lnet/agasper/unitynotification/NotificationAction;I)Landroid/app/PendingIntent;

    move-result-object v31

    move-object/from16 v0, v30

    move-object/from16 v1, v31

    invoke-virtual {v7, v12, v0, v1}, Landroid/support/v4/app/NotificationCompat$Builder;->addAction(ILjava/lang/CharSequence;Landroid/app/PendingIntent;)Landroid/support/v4/app/NotificationCompat$Builder;

    .line 204
    add-int/lit8 v11, v11, 0x1

    goto :goto_1

    .line 192
    .end local v4    # "action":Lnet/agasper/unitynotification/NotificationAction;
    .end local v11    # "i":I
    .end local v12    # "icon":I
    :cond_9
    const/16 v30, 0x2

    invoke-static/range {v30 .. v30}, Landroid/media/RingtoneManager;->getDefaultUri(I)Landroid/net/Uri;

    move-result-object v30

    move-object/from16 v0, v30

    invoke-virtual {v7, v0}, Landroid/support/v4/app/NotificationCompat$Builder;->setSound(Landroid/net/Uri;)Landroid/support/v4/app/NotificationCompat$Builder;

    goto/16 :goto_0

    .line 213
    :cond_a
    invoke-virtual {v7}, Landroid/support/v4/app/NotificationCompat$Builder;->build()Landroid/app/Notification;

    move-result-object v18

    .line 214
    .local v18, "notification":Landroid/app/Notification;
    move-object/from16 v0, v20

    move-object/from16 v1, v18

    invoke-virtual {v0, v13, v1}, Landroid/app/NotificationManager;->notify(ILandroid/app/Notification;)V

    .line 215
    return-void

    .line 196
    nop

    :array_0
    .array-data 8
        0x3e8
        0x3e8
    .end array-data
.end method
