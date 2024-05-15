Public Class Component

    Implements IDisposable

    Sub New(ByRef owner As Actor, ByVal updateOrder As Integer)
        'updateOrderが小さいコンポーネントほど早く更新される
        mOwner = owner
        mUpdateOrder = updateOrder
        mOwner.AddComponent(Me)
    End Sub

    Protected disposed = False
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not disposed Then
            If disposing Then
                '*** アンマネージリソースの開放

            End If
            '*** マネージドリソースの開放
            mOwner.RemoveComponent(Me)
        End If
        disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub


    ' 各コンポーネント更新（オーバーライド可能）
    Public Overridable Sub Update(deltaTime As Single)

    End Sub

    ' 各コンポーネント入力処理（オーバーライド可能）
    Public Overridable Sub ProcessInput(ByVal keyState As System.Windows.Forms.KeyEventArgs)

    End Sub

    Public mOwner As Actor      '所有アクター
    Public mUpdateOrder As Integer      'コンポーネントの更新順序

End Class
