Imports System.Buffers
Imports System.Numerics

Public Class Paddle
    Inherits Actor
    Sub New(ByRef game As Game)
        MyBase.New(game)
        mScale = 1.0

        'スプライトコンポーネント作成、テクスチャ設定
        Dim sc As New SpriteComponent(Me, 30)
        sc.SetTexture(game.GetTexture("\Assets\paddle.png"))

        'InputComponent作成
        mInput = New InputComponent(Me, 10)
        mInput.mForwardKey = Keys.Up
        mInput.mBackwardKey = Keys.Down
        mInput.mMaxForwardForce = 300.0
        mInput.mMoveResist = 30.0

        mRotation = Math.PI * 0.5

        Init()
    End Sub

    Public Overrides Sub UpdateActor(deltaTime As Single)


    End Sub

    Public Overrides Sub ActorInput(keyState As KeyEventArgs)
        MyBase.ActorInput(keyState)


    End Sub

    Public Sub Init()
        mPosition.X = 50
        mPosition.Y = mGame.mWindowH / 2

        mInput.mVelocity.X = 0.0
        mInput.mVelocity.Y = 0.0
    End Sub

    Public mInput As InputComponent

End Class
