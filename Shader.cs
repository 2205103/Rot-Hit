using OpenTK.Graphics.OpenGL;
class Shader
{
    private readonly string vertex = @"
    #version 330 core
    in vec3 position;

    void main()
    {
        gl_Position = vec4(position, 1.0);
    }";

    private readonly string fragment = @"
    #version 330 core
    out vec4 fragColor;
    void main()
    {
        fragColor = vec4(1.0);
    }";

    public int ProgramHandle { get; private set; }

    public void Setup()
    {
        int vertexHandle = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexHandle, vertex);
        GL.CompileShader(vertexHandle);

        int fragmentHandle = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentHandle, fragment);
        GL.CompileShader(fragmentHandle);

        ProgramHandle = GL.CreateProgram();
        GL.AttachShader(ProgramHandle, vertexHandle);
        GL.AttachShader(ProgramHandle, fragmentHandle);
        GL.LinkProgram(ProgramHandle);

        GL.DetachShader(ProgramHandle, vertexHandle);
        GL.DetachShader(ProgramHandle, fragmentHandle);
        GL.DeleteShader(vertexHandle);
        GL.DeleteShader(fragmentHandle);

        GL.UseProgram(ProgramHandle);
    }
}