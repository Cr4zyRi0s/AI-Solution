using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace VisualDebugging.Renderables
{
    // public abstract class RenderableSource<T>
    // {        
    //     protected dynamic source; 
    //     protected Func<dynamic, IEnumerable<T>> selector;

    //     public RenderableSource(dynamic source, Func<dynamic, IEnumerable<T>> selector)
    //     {
    //         this.source = source;
    //         this.selector = selector;
    //     }

    //     public abstract IEnumerable<Renderable> GetRenderables();
    // }

    // public class PointSource : RenderableSource<Vector3>
    // {
    //     private Func<Vector3, Color> colorSelector; 
    //     public PointSource(dynamic source, Func<dynamic,IEnumerable<Vector3>> selector) : base(source, selector)
    //     {}
    //     public PointSource(dynamic source, Func<dynamic,
    //     IEnumerable<Vector3>> selector,
    //     Func<Vector3, Color> colorSelector) : base(source, selector)
    //     {
    //         this.colorSelector = colorSelector;            
    //     }

    //     public override IEnumerable<Renderable> GetRenderables()
    //     {
    //         var positions = this.selector(source);
    //         return positions.Select((pos) => new VisualPoint(pos));
    //     }        
    // }
}