using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using OpenTK.Mathematics;


class TriangleProgram
{
    private static WindowHandle _window = null!;
    private static OpenGLContextHandle _context = null!;
    private static int _vao;
    private static float _rotationAngle;
    
    public static void Main()
    {
        Toolkit.Init(new ToolkitOptions());

        var hints = new OpenGLGraphicsApiHints
        {
            Version = new Version(3, 3),
            Profile = OpenGLProfile.Core,
            ForwardCompatibleFlag = true
        };

        _window = Toolkit.Window.Create(hints);
        _context = Toolkit.OpenGL.CreateFromWindow(_window);
        Toolkit.OpenGL.SetCurrentContext(_context);
        OpenTK.Graphics.GLLoader.LoadBindings(Toolkit.OpenGL.GetBindingsContext(_context));

        Toolkit.Window.SetTitle(_window, "OpenTK Triangle");
        Toolkit.Window.SetSize(_window, 800, 600);
        Toolkit.Window.SetMode(_window, WindowMode.Normal);

        EventQueue.EventRaised += (PalHandle? handle, PlatformEventType type, EventArgs e) =>
        {
            if (e is CloseEventArgs)
                Toolkit.Window.Destroy(_window);
        };

        // --- Setup OpenGL ---
        GL.ClearColor(0.2f, 0.3f, 0.4f, 1.0f);
        GL.Viewport(0, 0, 800, 600);
        // Create shader program
        Shader shader = new Shader();
        shader.Setup();

        Vector3[] verticies = [(-0.5f, -0.5f, 0.0f), (0.5f, -0.5f, 0.0f), (-0.5f, 0.5f, 0.0f),
                                (0.5f, -0.5f, 0.0f), (0.5f, 0.5f, 0.0f), (-0.5f, 0.5f, 0.0f)];


        Matrix4 rotation = Matrix4.CreateRotationZ(0.5f);

        // Create and bind VAO (required for OpenGL 3.3 core)
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, verticies.Length * sizeof(float) * 3, verticies, BufferUsage.StaticDraw);


        uint position = (uint)GL.GetAttribLocation(shader.ProgramHandle, "position");
        GL.EnableVertexAttribArray(position);
        GL.VertexAttribPointer(position, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);


        int uniform = GL.GetUniformLocation(shader.ProgramHandle, "rotation");
        Console.WriteLine(uniform);

        GL.UseProgram(shader.ProgramHandle);
        GL.BindVertexArray(_vao);



        while (!Toolkit.Window.IsWindowDestroyed(_window))
        {
            Toolkit.Window.ProcessEvents(false);

            if (true)
            {
                _rotationAngle += 0.001f;
                rotation = Matrix4.CreateRotationZ(_rotationAngle);
            }

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(shader.ProgramHandle);
            GL.BindVertexArray(_vao);

            // Pass matrix data to shader
            GL.UniformMatrix4f(uniform, 1, false, in rotation);

            GL.DrawArrays(PrimitiveType.Triangles, 0, verticies.Length);

            Toolkit.OpenGL.SwapBuffers(_context);
            
            // Small delay to control frame rate
            //System.Threading.Thread.Sleep(16); // ~60 FPS
        }

        Cleanup(shader.ProgramHandle);
    }

    private static void Cleanup(int shaderProgram)
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteProgram(shaderProgram);
    }
}
