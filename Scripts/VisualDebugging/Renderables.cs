using System;
using UnityEngine;

namespace VisualDebugging.Renderables
{
    public abstract class Renderable
    {
        public Color color = Color.green;
        public Vector3 size = Vector3.one * 0.33f;
        public abstract void Render();

        public virtual Renderable WithColor(Color color){
            this.color = color;
            return this;
        }
    }

    public class DebugPoint : Renderable
    {        
        public Vector3 point {get; private set;}

        public DebugPoint(Vector3 point)
        {
            this.point = point;
        }

        public override void Render()
        {
            Gizmos.color = this.color;
            Gizmos.DrawWireCube(this.point, this.size);
        }
    }

    public class DebugSegment : Renderable
    {
        public override void Render()
        {

        }
    }

    public class DebugPlane : Renderable
    {
        public override void Render()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DebugPath : Renderable
    {
        public override void Render()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DebugMesh : Renderable
    {
        private Mesh mesh;

        public DebugMesh(Mesh mesh)
        {
            this.mesh = mesh;
        }

        public override void Render()
        {            
            Gizmos.color = this.color;
            Gizmos.DrawWireMesh(mesh);
        }
    }

}