using Microsoft.Xna.Framework;

public class Camera2D
{
    public Camera2D(int width, int height)
    {
        Zoom = 1;
        Position = Vector2.Zero;
        Rotation = 0;
        Origin = Vector2.Zero;
        Position = Vector2.Zero;

        m_Width = width;
        m_Height = height;
        m_Mat = GetTransform();
    }

    public float Zoom { get; set; }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 Origin { get; set; }
    
    private int m_Height = 0;
    private int m_Width = 0;
    private Matrix m_Mat;

    public void Move(Vector2 direction)
    {
        Position += direction;
        m_Mat = GetTransform();
    }

    public Matrix GetTransform()
    {
        var translationMatrix = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0));
        var rotationMatrix = Matrix.CreateRotationZ(Rotation);
        var scaleMatrix = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
        var originMatrix = Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0));

        return translationMatrix * rotationMatrix * scaleMatrix * originMatrix;
    }

    public Vector2 ScreenToWorldCoords(Vector2 screenCords)
    {
        //Invert the Y value
        screenCords.Y = m_Height - screenCords.Y;

        //Change origo pos anfd fix scaling
        screenCords -= new Vector2(m_Width / 2, m_Height / 2);
        screenCords /= Zoom;

        //Translate with the camera
        screenCords += Position;

        return screenCords;
    }

    public Vector2 WorldToScreenCoords(ref Vector2 worldCoords)
    {
        Vector2 vec;
        Vector2.Transform(ref worldCoords, ref m_Mat, out vec);

        return vec;
    }
}