using UnityEngine;
using System.Collections.Generic;
using VisualDebugging.Renderables;
using System;
using System.Linq;

namespace VisualDebugging
{
    public static class VisualDebugger
    {
        private static Dictionary<MonoBehaviour, VisualDebuggerInstance> instances;      

        private static VisualDebuggerInstance GetInstance(MonoBehaviour mono){
            if(instances == null){
                instances = new Dictionary<MonoBehaviour, VisualDebuggerInstance>();
            }

            VisualDebuggerInstance inst = null;
            instances.TryGetValue(mono, out inst);
            if(inst == null){
                inst = new VisualDebuggerInstance();
                instances[mono] = inst; 
            }
            return inst;
        }
        public static void ClearPoints(MonoBehaviour caller){
            GetInstance(caller).ClearPoints();
        }        
        
        public static void ClearMeshes(MonoBehaviour caller){
            GetInstance(caller).ClearMeshes();
        }        

        public static void ClearAll(MonoBehaviour caller){
            var instance = GetInstance(caller);
            instance.ClearPoints();
            instance.ClearMeshes();
            instance.ClearSegments();
        }

        public static DebugPoint AddPoint(MonoBehaviour caller, Vector3 point, bool onSelected = true){
            var instance = GetInstance(caller);
            var debugPoint = new DebugPoint(point);
            
            instance.AddPoint(debugPoint);            
            return debugPoint;
        }

        public static IEnumerable<DebugPoint> AddPoints(MonoBehaviour caller, IEnumerable<Vector3> points){
            var instance = GetInstance(caller);
            var dPoints = new List<DebugPoint>();
            foreach (var point in points)
            {
                var dPoint = new DebugPoint(point);
                dPoints.Add(dPoint);
                instance.AddPoint(dPoint);
            }
            return dPoints;
        }

        public static DebugSegment AddSegment(MonoBehaviour caller, Vector3 start, Vector3 end)
        {
            var instance = GetInstance(caller);
            var segment = new DebugSegment();

            instance.AddSegment(segment);

            return segment;
        }

        public static void RemoveSegment(MonoBehaviour caller, DebugSegment segment)
        {
            GetInstance(caller).RemoveSegment(segment);
        }

        public static void ClearSegments(MonoBehaviour caller) {
            GetInstance(caller).ClearSegments();
        }

        public static DebugMesh AddMesh(MonoBehaviour caller, Vector3[] vertices, int[] triangles){
            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return AddMesh(caller, mesh);
        }
        public static DebugMesh AddMesh(MonoBehaviour caller, Mesh mesh){
            var instance = GetInstance(caller);
            var debugMesh = new DebugMesh(mesh);
            instance.AddMesh(debugMesh);
            return debugMesh;
        }



        public static void Render(MonoBehaviour caller){
            GetInstance(caller).Render();
        }
    }

    class VisualDebuggerInstance
    {
        private HashSet<DebugPoint> points;
        private HashSet<DebugSegment> segments;
        private HashSet<DebugMesh> meshes;
        private HashSet<Renderable> renderables;

        public VisualDebuggerInstance()
        {
            this.points = new HashSet<DebugPoint>();
            this.segments = new HashSet<DebugSegment>();
            this.meshes = new HashSet<DebugMesh>();

            this.renderables = new HashSet<Renderable>();
        }

        public void AddPoint(DebugPoint point){
            points.Add(point);
            renderables.Add(point);
        }
        public void AddMesh(DebugMesh mesh){
            meshes.Add(mesh);
            renderables.Add(mesh);
        }
        internal void AddSegment(DebugSegment segment)
        {
            segments.Add(segment);
            renderables.Add(segment);
        }

        internal void RemoveSegment(DebugSegment segment)
        {
            segments.Remove(segment);
            renderables.Remove(segment);
        }

        public void Render(){
            foreach (var renderable in renderables)
            {
                renderable.Render();
            }            
        }

        internal void ClearPoints()
        {
            renderables.RemoveWhere(rend => points.Contains(rend));
            points.Clear();                       
        }

        internal void ClearMeshes()
        {
            renderables.RemoveWhere(rend => meshes.Contains(rend));
            meshes.Clear();                       
        }

        internal void ClearSegments()
        {
            renderables.RemoveWhere(rend => segments.Contains(rend));
            segments.Clear();
        }
    }
}
