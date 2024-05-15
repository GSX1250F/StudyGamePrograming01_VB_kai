Imports System.Buffers
Imports System.Numerics

Public Class Paddle
    Inherits Actor
    Sub New(ByRef game As Game)
        MyBase.New(game)
        mScale = 1.0

        'スプライトコンポーネント作成、テクスチャ設定
        mSprite = New SpriteComponent(Me, 30)
        mSprite.SetTexture(game.GetTexture("\Assets\paddle.png"))

        Init()
    End Sub

    Public Overrides Sub UpdateActor(deltaTime As Single)
        mPosition.Y += mPaddleDir * mPaddleSpeed * deltaTime
        If mPosition.Y - mSprite.mTexHeight / 2 < 0 Then
            mPosition.Y = mSprite.mTexHeight / 2
        ElseIf mPosition.Y + mSprite.mTexHeight / 2 > mGame.mWindowH Then
            mPosition.Y = mGame.mWindowH - mSprite.mTexHeight / 2
        End If
    End Sub

    Public Overrides Sub ActorInput(keyState As KeyEventArgs)
        MyBase.ActorInput(keyState)
        mPaddleDir = 0
        If Not keyState Is Nothing Then
            If keyState.KeyCode = Keys.Up Then
                mPaddleDir = -1
            ElseIf keyState.KeyCode = Keys.Down Then
                mPaddleDir = 1
            ElseIf keyState.KeyCode = Keys.R Then
                '再スタート
                Init()
                For i = 0 To mGame.numBalls - 1
                    mGame.mBalls(i).Init()
                Next
            End If
        End If

    End Sub

    Public Sub Init()
        mState = State.EActive
        mPosition.X = 50
        mPosition.Y = mGame.mWindowH / 2

        mPaddleSpeed = 400
        mPaddleDir = 0
    End Sub

    Public mSprite As SpriteComponent
    Public mPaddleSpeed As Single
    Public mPaddleDir As Integer
End Class
