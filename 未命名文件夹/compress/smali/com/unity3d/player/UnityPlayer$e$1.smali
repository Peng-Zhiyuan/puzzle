.class final Lcom/unity3d/player/UnityPlayer$e$1;
.super Ljava/lang/Object;

# interfaces
.implements Landroid/os/Handler$Callback;


# annotations
.annotation system Ldalvik/annotation/EnclosingMethod;
    value = Lcom/unity3d/player/UnityPlayer$e;->run()V
.end annotation

.annotation system Ldalvik/annotation/InnerClass;
    accessFlags = 0x0
    name = null
.end annotation


# instance fields
.field final synthetic a:Lcom/unity3d/player/UnityPlayer$e;


# direct methods
.method constructor <init>(Lcom/unity3d/player/UnityPlayer$e;)V
    .locals 0

    iput-object p1, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    return-void
.end method

.method private a()V
    .locals 2

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget v0, v0, Lcom/unity3d/player/UnityPlayer$e;->d:I

    sget v1, Lcom/unity3d/player/UnityPlayer$b;->c:I

    if-ne v0, v1, :cond_0

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-boolean v0, v0, Lcom/unity3d/player/UnityPlayer$e;->c:Z

    if-eqz v0, :cond_0

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer$e;->f:Lcom/unity3d/player/UnityPlayer;

    const/4 v1, 0x1

    invoke-static {v0, v1}, Lcom/unity3d/player/UnityPlayer;->a(Lcom/unity3d/player/UnityPlayer;Z)V

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    sget v1, Lcom/unity3d/player/UnityPlayer$b;->a:I

    iput v1, v0, Lcom/unity3d/player/UnityPlayer$e;->d:I

    :cond_0
    return-void
.end method


# virtual methods
.method public final handleMessage(Landroid/os/Message;)Z
    .locals 4

    const/4 v2, 0x1

    const/4 v1, 0x0

    iget v0, p1, Landroid/os/Message;->what:I

    const/16 v3, 0x8dd

    if-eq v0, v3, :cond_0

    move v0, v1

    :goto_0
    return v0

    :cond_0
    iget-object v0, p1, Landroid/os/Message;->obj:Ljava/lang/Object;

    check-cast v0, Lcom/unity3d/player/UnityPlayer$d;

    sget-object v3, Lcom/unity3d/player/UnityPlayer$d;->h:Lcom/unity3d/player/UnityPlayer$d;

    if-ne v0, v3, :cond_1

    move v0, v2

    goto :goto_0

    :cond_1
    sget-object v3, Lcom/unity3d/player/UnityPlayer$d;->c:Lcom/unity3d/player/UnityPlayer$d;

    if-ne v0, v3, :cond_3

    invoke-static {}, Landroid/os/Looper;->myLooper()Landroid/os/Looper;

    move-result-object v0

    invoke-virtual {v0}, Landroid/os/Looper;->quit()V

    :cond_2
    :goto_1
    move v0, v2

    goto :goto_0

    :cond_3
    sget-object v3, Lcom/unity3d/player/UnityPlayer$d;->b:Lcom/unity3d/player/UnityPlayer$d;

    if-ne v0, v3, :cond_4

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    iput-boolean v2, v0, Lcom/unity3d/player/UnityPlayer$e;->b:Z

    goto :goto_1

    :cond_4
    sget-object v3, Lcom/unity3d/player/UnityPlayer$d;->a:Lcom/unity3d/player/UnityPlayer$d;

    if-ne v0, v3, :cond_5

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    iput-boolean v1, v0, Lcom/unity3d/player/UnityPlayer$e;->b:Z

    goto :goto_1

    :cond_5
    sget-object v3, Lcom/unity3d/player/UnityPlayer$d;->d:Lcom/unity3d/player/UnityPlayer$d;

    if-ne v0, v3, :cond_6

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    iput-boolean v1, v0, Lcom/unity3d/player/UnityPlayer$e;->c:Z

    goto :goto_1

    :cond_6
    sget-object v3, Lcom/unity3d/player/UnityPlayer$d;->e:Lcom/unity3d/player/UnityPlayer$d;

    if-ne v0, v3, :cond_7

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    iput-boolean v2, v0, Lcom/unity3d/player/UnityPlayer$e;->c:Z

    invoke-direct {p0}, Lcom/unity3d/player/UnityPlayer$e$1;->a()V

    goto :goto_1

    :cond_7
    sget-object v3, Lcom/unity3d/player/UnityPlayer$d;->f:Lcom/unity3d/player/UnityPlayer$d;

    if-ne v0, v3, :cond_9

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget v0, v0, Lcom/unity3d/player/UnityPlayer$e;->d:I

    sget v3, Lcom/unity3d/player/UnityPlayer$b;->a:I

    if-ne v0, v3, :cond_8

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer$e;->f:Lcom/unity3d/player/UnityPlayer;

    invoke-static {v0, v1}, Lcom/unity3d/player/UnityPlayer;->a(Lcom/unity3d/player/UnityPlayer;Z)V

    :cond_8
    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    sget v1, Lcom/unity3d/player/UnityPlayer$b;->b:I

    iput v1, v0, Lcom/unity3d/player/UnityPlayer$e;->d:I

    goto :goto_1

    :cond_9
    sget-object v1, Lcom/unity3d/player/UnityPlayer$d;->g:Lcom/unity3d/player/UnityPlayer$d;

    if-ne v0, v1, :cond_2

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$1;->a:Lcom/unity3d/player/UnityPlayer$e;

    sget v1, Lcom/unity3d/player/UnityPlayer$b;->c:I

    iput v1, v0, Lcom/unity3d/player/UnityPlayer$e;->d:I

    invoke-direct {p0}, Lcom/unity3d/player/UnityPlayer$e$1;->a()V

    goto :goto_1
.end method
