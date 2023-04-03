using UnityEngine;
using System.Collections.Generic;

public class CompressedBooleanList {
    private int length;
    public int Length
    {
        get { return length; }
        set { length = value; }
    }
    
    private List<uint> data;

    public CompressedBooleanList(int length)
    {
        Length = length;
        this.data = new List<uint>();
        var numBatches = Mathf.CeilToInt(length / 32);
        for (int i = 0; i < numBatches; i++)
        {
            this.data.Add(0);
        }
    }

    public bool this[int index]{
        get{ return this.Get(index); }
        set{ this.Set(index, value); }
    }    

    public bool Get(int index){
        int batch = (int)(index / 32);
        int batchPos = index % 32;
        
        return (data[batch] & (1 << batchPos)) > 0;
    }
    public void Set(int index, bool value){
        int batchIndex = (int)(index / 32);
        int batchPos = index % 32;
        
        uint batch = data[batchIndex];
        if(!value)
            data[batchIndex] = (uint)(data[batchIndex] & ~(1 << batchPos));
        else
            data[batchIndex] = (uint)(data[batchIndex] | (1 << batchPos));        
    }
}