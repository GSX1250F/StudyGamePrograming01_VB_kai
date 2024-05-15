Public Class InputComponent
	Inherits MoveComponent

	Sub New(ByRef owner As Actor, ByVal updateOrder As Integer)
		MyBase.New(owner, updateOrder)
		mForwardKey = 0
		mBackwardKey = 0
		mClockwiseKey = 0
		mCounterClockwiseKey = 0
	End Sub

	Public Overrides Sub ProcessInput(keyState As KeyEventArgs)
		Dim forwardforce As Single = 0.0
		Dim rotforce As Single = 0.0
		If Not keyState Is Nothing Then
			'古典物理学でMoveComponentのための計算
			'MoveComponentには前進か回転方向の力の最大値だけを渡す
			If keyState.KeyValue = mForwardKey Then
				forwardforce += mMaxForwardForce
			ElseIf keyState.KeyValue = mBackwardKey Then
				forwardforce -= mMaxForwardForce
			End If

			If keyState.KeyValue = mClockwiseKey Then
				rotforce -= mMaxRotForce        '角度の+方向はCCW
			ElseIf keyState.KeyValue = mCounterClockwiseKey Then
				rotforce += mMaxRotForce        '角度の+方向はCCW
			End If
		End If
		mMoveForce = forwardforce * mOwner.GetForward()
		mRotForce = rotforce
	End Sub



	' 前進・後退のためのキー
	Public mForwardKey As Integer
	Public mBackwardKey As Integer

	' 回転運動のキー
	Public mClockwiseKey As Integer
	Public mCounterClockwiseKey As Integer

	' 前進・回転方向の力の最大値
	Public mMaxForwardForce As Single
	Public mMaxRotForce As Single
	Private disposedValue As Boolean
End Class