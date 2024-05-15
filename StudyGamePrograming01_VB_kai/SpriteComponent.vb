Imports Windows.Win32.UI.Input

Public Class SpriteComponent
    Inherits Component
    Sub New(ByRef owner As Actor, ByVal drawOrder As Integer)
        MyBase.New(owner, drawOrder)
        'mTexture = Nothing
        mDrawOrder = drawOrder
        mTexWidth = 0
        mTexHeight = 0
        mOwner.mGame.AddSprite(Me)
    End Sub
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then
            If disposing Then
                ' Insert code to free managed resources.
            End If
            ' Insert code to free unmanaged resources.
            mOwner.mGame.RemoveSprite(Me)
        End If
        MyBase.Dispose(disposing)
    End Sub

    Public Overridable Sub Draw(ByRef mRenderer As Object)
        If mOwner.mState <> Actor.State.EPaused Then
            Dim w = CInt(mTexWidth * mOwner.mScale)
            Dim h = CInt(mTexHeight * mOwner.mScale)
            'Dim x = CInt(mOwner.mPosition.X - w / 2)
            'Dim y = CInt(mOwner.mPosition.Y - h / 2)
            Dim img As New Bitmap(w, h)
            img = mTexture

            '新しい座標位置を計算する
            Dim x(3) As Single
            Dim y(3) As Single
            Dim x2(3) As Single
            Dim y2(3) As Single
            x(0) = mOwner.mPosition.X - w / 2
            y(0) = mOwner.mPosition.Y - h / 2
            x(1) = mOwner.mPosition.X + w / 2
            y(1) = mOwner.mPosition.Y - h / 2
            x(2) = mOwner.mPosition.X - w / 2
            y(2) = mOwner.mPosition.Y + h / 2
            For i = 0 To 2
                x2(i) = (x(i) - mOwner.mPosition.X) * Math.Cos(mOwner.mRotation) + (y(i) - mOwner.mPosition.Y) * Math.Sin(mOwner.mRotation) + mOwner.mPosition.X
                y2(i) = -(x(i) - mOwner.mPosition.X) * Math.Sin(mOwner.mRotation) + (y(i) - mOwner.mPosition.Y) * Math.Cos(mOwner.mRotation) + mOwner.mPosition.Y
            Next

            'Point配列を作成
            Dim destinationPoints() As Point = {
                New Point(CInt(x2(0)), CInt(y2(0))),
                New Point(CInt(x2(1)), CInt(y2(1))),
                New Point(CInt(x2(2)), CInt(y2(2)))}

            mRenderer.DrawImage(img, destinationPoints)
        End If

    End Sub
    Public Sub SetTexture(tex As Image)
        mTexture = tex
        mTexWidth = tex.Width
        mTexHeight = tex.Height
        ' 高さと幅の平均をActorの直径とする。
        mOwner.mRadius = (mTexWidth + mTexHeight) / 4
    End Sub
    Public mTexture As Image
    Public mDrawOrder As Integer
    Public mTexWidth As Integer
    Public mTexHeight As Integer

End Class
