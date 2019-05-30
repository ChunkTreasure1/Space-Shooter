using Microsoft.Xna.Framework;
using System;

public class Camera2D
{
    private float m_Zoom;
    private Vector2 m_Position;
    private float m_Rotation;
    private Vector2 m_Origin;
    
    private int m_Height = 0;
    private int m_Width = 0;
    private Matrix m_Mat;

    //Getting
    public float GetZoom() { return m_Zoom; }
    public float GetRotation() { return m_Rotation; }
    public Vector2 GetPosition() { return m_Position; }
    public Vector2 GetOrigin() { return m_Origin; }
    public Matrix GetTransform()
    {
        Matrix transform = Matrix.CreateTranslation(new Vector3(-m_Position.X, -m_Position.Y, 0)) *
            Matrix.CreateRotationZ(m_Rotation) *
            Matrix.CreateScale(new Vector3(m_Zoom, m_Zoom, 1)) *
            Matrix.CreateTranslation(new Vector3(m_Width * 0.5f, m_Height * 0.5f, 0));
        return transform;
    }

    //Setting
    public void SetPosition(Vector2 pos) { m_Position = pos; }
    public void SetRotation(float rot) { m_Rotation = rot; }

    //Constructor sets all the star values
    public Camera2D(int width, int height)
    {
        m_Zoom = 1;
        m_Position = Vector2.Zero;
        m_Rotation = 0;
        m_Origin = Vector2.Zero;
        m_Position = Vector2.Zero;

        m_Width = width;
        m_Height = height;
        m_Mat = GetTransform();
    }

    //Moves the camera in desired direction
    public void Move(Vector2 direction)
    {
        //Move the camera using the direction and update the matrix
        m_Position += direction;
        m_Mat = GetTransform();
    }

    //Returns the full transform matrix of the camera
}