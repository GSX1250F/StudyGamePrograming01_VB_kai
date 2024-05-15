Imports System.Numerics
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Game
    'Public
    'ゲームウィンドウの大きさ
    Public mWindowW As Integer
    Public mWindowH As Integer
    'TicksCountの一時保持用。
    Public mTicksCountPre As Long

    'Private
    'ファイル名とテクスチャとのひもづけ配列
    Private mTextures As New Dictionary(Of String, Image)
    'すべてのアクター
    Private mActors As New List(Of Actor)
    'すべての待ちアクター
    Private mPendingActors As New List(Of Actor)
    'すべての描画されるスプライトコンポーネント
    Private mSprites As New List(Of SpriteComponent)
    'Private mWindow As SDL_Window   C++のmWindowに相当するのはForm,PictureBox
    'Private mRenderer As SDL_Renderer    C++のRendererに相当するのはCanvas,Graphics
    Private mWindow As Bitmap      'PictureBoxに表示するためのBitmapオブジェクト作成
    Private mRenderer As Graphics      'ImageオブジェクトのGraphicsオブジェクトを作成する
    'Private mTicksCount As Unit32       Stopwatchに相当？
    Private mTicksCount As New System.Diagnostics.Stopwatch()
    Private mIsRunning As Boolean
    Private mIsUpdatingActors As Boolean
    Private mKeyInputs As New List(Of System.Windows.Forms.KeyEventArgs)    'キー入力の配列

    'game specific
    Public mPaddle As Paddle
    Public numBalls As Integer = 1
    Public mBall(numBalls) As Ball
    Public mBalls As New List(Of Ball)

    'コンストラクタ
    Public Sub Game_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mIsRunning = True
        mIsUpdatingActors = False
        mWindowW = 1024
        mWindowH = 768
        Dim success = Initialize()

        'ここまででFormとPictureBoxが作成される。
        'この後は、イベントハンドラでInput、TimerでUpdateとOutputが実行される。
    End Sub

    Public Function Initialize() As Boolean
        'フォームを表示させ、ストップウォッチを開始
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = New Point(50, 50)
        mWindow = New Bitmap(mWindowW, mWindowH)      'PictureBoxと同じ大きさの画像を作る
        mRenderer = Graphics.FromImage(mWindow)           '画像のGraphicsクラスを生成


        LoadData()

        mTicksCount.Start()
        Timer1.Interval = 33.33       'FPS = 1000/interval
        Timer1.Enabled = True
        mTicksCountPre = mTicksCount.ElapsedMilliseconds

        Return True

    End Function

    Public Sub AddActor(ByRef actor As Actor)
        If mIsUpdatingActors Then
            mPendingActors.Add(actor)
        Else
            mActors.Add(actor)
        End If
    End Sub

    Public Sub RemoveActor(actor As Actor)
        Dim iter As Integer = mPendingActors.IndexOf(actor)
        '見つからなかったら-1が返される。
        If iter >= 0 Then
            mPendingActors.RemoveAt(iter)
        End If
        iter = mActors.IndexOf(actor)
        If iter >= 0 Then
            mActors.RemoveAt(iter)
        End If
    End Sub

    Public Sub AddSprite(sprite As SpriteComponent)
        Dim myDrawOrder As Integer = sprite.mDrawOrder
        Dim cnt As Integer = mSprites.Count     '配列の要素数
        Dim i As Integer = 0
        If cnt > 0 Then
            For i = 0 To mSprites.Count - 1
                If myDrawOrder < mSprites(i).mDrawOrder Then
                    Exit For
                End If
            Next
        End If
        mSprites.Insert(i, sprite)
    End Sub
    Public Sub RemoveSprite(sprite As SpriteComponent)
        Dim iter As Integer = mSprites.IndexOf(sprite)
        '見つからなかったら-1が返される。
        iter = mSprites.IndexOf(sprite)
        If iter >= 0 Then
            mSprites.RemoveAt(iter)
        End If
    End Sub
    Public Sub SetRunning(isrunning As Boolean)
        mIsRunning = isrunning
    End Sub
    Public Function GetTexture(ByRef filename As String) As Image
        Dim tex As System.Drawing.Image = Nothing
        Dim b As Boolean = mTextures.ContainsKey(filename)
        If b = True Then
            'すでに読み込み済み
            tex = mTextures(filename)
        Else
            '画像ファイルを読み込んで、Imageオブジェクトを作成し、ファイル名と紐づけする
            tex = Image.FromFile(Application.StartupPath & filename)
            mTextures.Add(filename, tex)
        End If
        Return tex
    End Function

    Private Sub KeyState(sender As Object, keyState As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        'Keyイベントハンドラ
        'キーコードを配列に入れる。
        mKeyInputs.Add(keyState)
    End Sub

    Private Sub ProcessInput()
        'キーイベントが無かったときでも、Nothingを引数にして
        'ActorとComponentのProcessInputを実行する
        'そのとき、Nothingの処理を必ず行う。
        If mKeyInputs.Count = 0 Then
            mKeyInputs.Add(Nothing)
        End If
        For i As Integer = 0 To mKeyInputs.Count - 1
            If Not mKeyInputs(i) Is Nothing Then
                'ESCキーでゲーム終了
                If mKeyInputs(i).KeyCode = Keys.Escape Then
                    mIsRunning = False
                End If
            End If

            mIsUpdatingActors = True
            For Each actor In mActors
                actor.ProcessInput(mKeyInputs(i))
            Next
        Next
        mIsUpdatingActors = False
        mKeyInputs.Clear()
    End Sub

    Private Sub UpdateGame()
        'デルタタイムの計算
        Dim deltaTime As Single = (mTicksCount.ElapsedMilliseconds - mTicksCountPre) / 1000
        'If deltatime > 0.05 Then deltatime = 0.05       'deltatime=0.05 ～　20fps
        'すべてのアクターを更新
        mIsUpdatingActors = True
        For Each actor In mActors
            actor.Update(deltaTime)
        Next
        mIsUpdatingActors = False
        '待ちアクターをmActorsに移動
        For Each pending In mPendingActors
            mActors.Add(pending)
        Next
        mPendingActors.Clear()
        '死んだアクターを一時配列に追加
        Dim deadActors As New List(Of Actor)
        For Each actor In mActors
            If actor.mState = Actor.State.EDead Then
                deadActors.Add(actor)
            End If
        Next
        '死んだアクターを削除
        For Each actor In deadActors
            actor.Dispose()
        Next
    End Sub

    Private Sub GenerateOutput()
        '画面のクリア
        mRenderer.Clear(Color.Black)

        'すべてのスプライトコンポーネントを描画
        For Each sprite In mSprites
            sprite.Draw(mRenderer)
        Next

        'バッファの交換・・・不要　PictureBoxはダブルバッファがデフォルトでオン。canvas→pictureboxでよい。
        'PictureBoxに表示する
        PictureBox.Image = mWindow
    End Sub

    Private Sub LoadData()
        'プレイヤーのパドルを作成
        mPaddle = New Paddle(Me)

        'ボールを複数生成
        For i As Integer = 0 To numBalls - 1
            mBall(i) = New Ball(Me)
        Next

    End Sub

    Private Sub UnloadData()
        While mActors.Count > 0
            mActors.Remove(mActors.Last)
        End While

        mTextures.Clear()
    End Sub

    Private Sub Shutdown()
        mTicksCount.Stop()
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If mIsRunning Then
            ProcessInput()
            UpdateGame()
            GenerateOutput()
            mTicksCountPre = mTicksCount.ElapsedMilliseconds
        Else
            Shutdown()
        End If
    End Sub

    'Game Specific
    Public Sub AddBall(ByRef ball As Ball)
        mBalls.Add(ball)
    End Sub
    Public Sub RemoveBall(ByRef ball As Ball)
        Dim iter As Integer = mBalls.IndexOf(ball)       'Listの中になかったら-1が返される
        If iter >= 0 Then
            mBalls.RemoveAt(iter)
        End If
    End Sub
End Class


