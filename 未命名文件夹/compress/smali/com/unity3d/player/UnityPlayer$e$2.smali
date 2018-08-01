.class final Lcom/unity3d/player/UnityPlayer$e$2;
.super Ljava/lang/Object;

# interfaces
.implements Landroid/os/MessageQueue$IdleHandler;


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

    iput-object p1, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    return-void
.end method


# virtual methods
.method public final queueIdle()Z
    .locals 4

    const/4 v3, 0x1

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer$e;->f:Lcom/unity3d/player/UnityPlayer;

    invoke-virtual {v0}, Lcom/unity3d/player/UnityPlayer;->executeGLThreadJobs()V

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-boolean v0, v0, Lcom/unity3d/player/UnityPlayer$e;->b:Z

    if-nez v0, :cond_1

    :cond_0
    :goto_0
    return v3

    :cond_1
    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-boolean v0, v0, Lcom/unity3d/player/UnityPlayer$e;->c:Z

    if-eqz v0, :cond_0

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget v0, v0, Lcom/unity3d/player/UnityPlayer$e;->e:I

    if-ltz v0, :cond_3

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget v0, v0, Lcom/unity3d/player/UnityPlayer$e;->e:I

    if-nez v0, :cond_2

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer$e;->f:Lcom/unity3d/player/UnityPlayer;

    invoke-static {v0}, Lcom/unity3d/player/UnityPlayer;->a(Lcom/unity3d/player/UnityPlayer;)Z

    move-result v0

    if-eqz v0, :cond_2

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer$e;->f:Lcom/unity3d/player/UnityPlayer;

    invoke-static {v0}, Lcom/unity3d/player/UnityPlayer;->b(Lcom/unity3d/player/UnityPlayer;)V

    :cond_2
    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget v1, v0, Lcom/unity3d/player/UnityPlayer$e;->e:I

    add-int/lit8 v1, v1, -0x1

    iput v1, v0, Lcom/unity3d/player/UnityPlayer$e;->e:I

    :cond_3
    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer$e;->f:Lcom/unity3d/player/UnityPlayer;

    invoke-virtual {v0}, Lcom/unity3d/player/UnityPlayer;->isFinishing()Z

    move-result v0

    if-nez v0, :cond_4

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer$e;->f:Lcom/unity3d/player/UnityPlayer;

    invoke-static {v0}, Lcom/unity3d/player/UnityPlayer;->c(Lcom/unity3d/player/UnityPlayer;)Z

    move-result v0

    if-nez v0, :cond_4

    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer$e;->f:Lcom/unity3d/player/UnityPlayer;

    invoke-static {v0}, Lcom/unity3d/player/UnityPlayer;->d(Lcom/unity3d/player/UnityPlayer;)V

    :cond_4
    iget-object v0, p0, Lcom/unity3d/player/UnityPlayer$e$2;->a:Lcom/unity3d/player/UnityPlayer$e;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer$e;->a:Landroid/os/Handler;

    const/16 v1, 0x8dd

    sget-object v2, Lcom/unity3d/player/UnityPlayer$d;->h:Lcom/unity3d/player/UnityPlayer$d;

    invoke-static {v0, v1, v2}, Landroid/os/Message;->obtain(Landroid/os/Handler;ILjava/lang/Object;)Landroid/os/Message;

    move-result-object v0

    invoke-virtual {v0}, Landroid/os/Message;->sendToTarget()V

    goto :goto_0
.end method
