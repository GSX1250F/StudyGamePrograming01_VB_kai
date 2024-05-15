Imports System.Numerics
Imports System.Security.Cryptography

Public Class Ball
	Inherits Actor

	Sub New(ByRef game As Game)
		MyBase.New(game)

		'スプライトコンポーネント作成、テクスチャ設定
		Dim sc As SpriteComponent = New SpriteComponent(Me, 40)
		sc.SetTexture(game.GetTexture("\Assets\ball.png"))

		'MoveComponent作成
		mMove = New MoveComponent(Me, 10)

		'CircleComponent作成
		mCircle = New CircleComponent(Me, 10)
		mGame.AddBall(Me)
		Init()
	End Sub
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If Not Me.disposed Then
			If disposing Then
				' Insert code to free managed resources.
			End If
			' Insert code to free unmanaged resources.
			mGame.RemoveBall(Me)
		End If
		MyBase.Dispose(disposing)
	End Sub

	Public Overrides Sub UpdateActor(detaTime As Single)
	End Sub

	Public Sub Init()
		mPosition.X = mGame.mWindowW / 2
		mPosition.Y = mGame.mWindowH / 2
		Dim angle As Integer = RandomNumberGenerator.GetInt32(15, 75)
		Dim pmx As Integer = 2 * RandomNumberGenerator.GetInt32(0, 2) - 1
		Dim pmy As Integer = 2 * RandomNumberGenerator.GetInt32(0, 2) - 1
		mMove.mVelocity.X = pmx * 300 * Math.Cos(angle / 180 * Math.PI)
		mMove.mVelocity.Y = pmy * 300 * Math.Sin(angle / 180 * Math.PI)
	End Sub
	Public mCircle As CircleComponent
	Public mMove As MoveComponent
End Class
