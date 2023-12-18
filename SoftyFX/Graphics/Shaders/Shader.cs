using SoftyFX.Mathematics;

namespace SoftyFX.Graphics.Shaders
{
    public abstract class Shader
    {
        public abstract Vector3 Vertex(in Vector3 pos);
    }
}