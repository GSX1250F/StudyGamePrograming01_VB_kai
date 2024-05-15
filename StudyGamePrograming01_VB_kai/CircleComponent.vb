Imports System.Numerics

Public Class CircleComponent
    Inherits Component

    Sub New(ByRef owner As Actor, ByVal updateOrder As Integer)
        MyBase.New(owner, updateOrder)

    End Sub
    Public Function GetRadius() As Single
        Return mOwner.mRadius
    End Function

    Public Function GetCenter() As Vector2
        Return mOwner.mPosition
    End Function

    Public mRadius As Single

    Public Function Intersect(ByRef a As CircleComponent, ByRef b As CircleComponent) As Boolean
        ' ２つのCircleComponentの中心間距離を計算
        Dim diff As Vector2 = a.GetCenter() - b.GetCenter()
        Dim distSq As Single = diff.Length() * diff.Length()

        ' ２つのCircleComponentの半径の和を計算 
        Dim sumRadiusSq As Single = (a.GetRadius() + b.GetRadius()) * (a.GetRadius() + b.GetRadius())

        ' 中心間距離 <= 半径の和 のとき、衝突したと判定
        If distSq <= sumRadiusSq Then
            Return True
        Else
            Return False
        End If
    End Function


End Class
