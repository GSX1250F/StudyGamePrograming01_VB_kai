Imports System.Numerics

Public Class MoveComponent
    Inherits Component
    Sub New(ByRef owner As Actor, ByVal updateOrder As Integer)
        MyBase.New(owner, updateOrder)
        mVelocity.X = 0.0
        mVelocity.Y = 0.0
        mRotSpeed = 0.0
        mMass = 1.0
        mMoveForce.X = 0.0
        mMoveForce.Y = 0.0
        mMoveAccel.X = 0.0
        mMoveAccel.Y = 0.0
        mRotForce = 0.0
        mRotAccel = 0.0
        mTorque = 0.0
        mImoment = 0.0
        mMoveResist = 0.0
        mRotResist = 0.0
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Overrides Sub Update(deltaTime As Single)
        ' Actorの位置を更新 x=x0+vt
        mOwner.mPosition.X += mVelocity.X * deltaTime
        mOwner.mPosition.Y += mVelocity.Y * deltaTime
        ' Actorの方向を更新 θ=θ0+ωt
        mOwner.mRotation += mRotSpeed * deltaTime

        ' Actorの重心速度を更新
        If mMass <> 0 Then
            mMoveAccel = mMoveForce * (1 / mMass)    '重心加速度の計算　F=ma  a=F*(1/m)
            '抵抗力 = 速さ*抵抗係数    減速 = -速さ*抵抗係数/質量
            Dim movedecel As Vector2 = mVelocity * mMoveResist * 0.01 * (1 / mMass)
            mMoveAccel -= movedecel
        Else
            mMoveAccel.X = 0.0
            mMoveAccel.Y = 0.0
        End If

        ' 方向を更新
        ' 慣性モーメント計算　※※2次元においては、一様密度の円板とする。 I=0.5*質量*半径^2
        mImoment = 0.5 * mMass * mOwner.GetRadius() * mOwner.GetRadius()
        If (mImoment <> 0) Then
            'トルク計算　トルク=回転方向の力 * 半径
            mTorque = mRotForce * mOwner.GetRadius()
            '回転加速度の計算　回転加速度 = トルク / 慣性モーメント
            mRotAccel = mTorque / mImoment    '回転加速度の計算 Fr=Ia  a=Fr/I
            '抵抗力 = 速さ*抵抗係数    減速 = -速さ*抵抗係数*半径/慣性モーメント
            Dim rotDecel As Single = mRotSpeed * mOwner.GetRadius() * mRotResist / mImoment
            mRotAccel -= rotDecel
        Else
            mRotAccel = 0
        End If
        mVelocity += mMoveAccel * deltaTime     'v = v0 + at
        mRotSpeed += mRotAccel * deltaTime     'ω = ωo + bt
    End Sub

    '単純移動パラメータ
    '重心移動速度
    Public mVelocity As Vector2
    '回転速度
    Public mRotSpeed As Single

    '古典物理パラメータ
    Public mMass As Single
    '重心にかかる力
    Public mMoveForce As Vector2
    '回転方向の力F　 トルク=RotForce * Radius = Imoment * RotAccel
    Public mRotForce As Single
    'トルク=回転方向の力 * 半径 = 慣性モーメント * 回転加速度
    Public mTorque As Single
    '重心加速度
    Public mMoveAccel As Vector2
    '回転加速度
    Public mRotAccel As Single
    '慣性モーメント
    Public mImoment As Single
    '重心速度抵抗率(%)
    Public mMoveResist As Single
    '回転速度抵抗率(%)
    Public mRotResist As Single

End Class
