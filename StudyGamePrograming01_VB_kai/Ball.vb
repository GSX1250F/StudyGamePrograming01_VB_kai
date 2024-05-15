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

	Public Overrides Sub UpdateActor(deltaTime As Single)
		'ボールが画面外にいったとき
		If (mPosition.X < 0) Then
			'パドルとボールを一時的に消す。
			mState = State.EPaused
			mGame.mPaddle.mState = State.EPaused
		End If


		'ボールの壁での跳ね返り
		If (mPosition.X + mMove.mVelocity.X * deltaTime > 1024) _
		   Then
			mMove.mVelocity.X *= -1
		End If
		If (mPosition.Y + mMove.mVelocity.Y * deltaTime < 0) _
		   Or (mPosition.Y + mMove.mVelocity.Y * deltaTime > 768) _
		   Then
			mMove.mVelocity.Y *= -1
		End If

		'ボールのパドルでの跳ね返り
		If (mPosition.Y > mGame.mPaddle.mPosition.Y - mGame.mPaddle.mSprite.mTexHeight / 2) _
		   And (mPosition.Y < mGame.mPaddle.mPosition.Y + mGame.mPaddle.mSprite.mTexHeight / 2) _
		   And (Math.Abs(mPosition.X + mMove.mVelocity.X * deltaTime - mGame.mPaddle.mPosition.X) <= mGame.mPaddle.mSprite.mTexWidth) _
		   Then
			mMove.mVelocity.X *= -1.2      '横方向ボールスピードup
		End If
		If (mPosition.X > mGame.mPaddle.mPosition.X - mGame.mPaddle.mSprite.mTexWidth / 2) _
		   And (mPosition.X < mGame.mPaddle.mPosition.X + mGame.mPaddle.mSprite.mTexWidth / 2) _
		   And (Math.Abs(mPosition.Y + mMove.mVelocity.Y * deltaTime - mGame.mPaddle.mPosition.Y) <= mGame.mPaddle.mSprite.mTexHeight / 2) _
		   Then
			mMove.mVelocity.Y *= -1
		End If

	End Sub

	Public Sub Init()
		mState = State.EActive
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
