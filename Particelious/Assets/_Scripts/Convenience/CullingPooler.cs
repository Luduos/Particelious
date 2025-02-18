﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Cullable : MonoBehaviour
{
    public int Index { get; set; }
    public abstract void Reset();
}

[System.Serializable]
public struct CullingPoolerInfo
{
    public int InitialSize;
    public int MaxSize;
    
    public CullingPoolerInfo(int initialSize, int maxSize)
    {
        this.InitialSize = initialSize;
        this.MaxSize = maxSize;
    }
}

public class CullingPooler {

    protected Stack<Cullable> m_FreeInstances;
    protected Cullable[] m_AllInstances;
    protected BoundingSphere[] m_BoundingSpheres;
    protected CullingGroup m_CullingGroup;
    protected int m_ActualInstanceCount = 0;
    protected int m_NumberOfActiveCullables = 0;

    protected Cullable m_Original;

    private static readonly Vector4 s_UndangerousBoundingSpherePosition = new Vector4(float.MaxValue, float.MaxValue, float.MaxValue, 1.0f);

    public CullingPooler(Cullable original, Camera targetCamera, CullingPoolerInfo poolerInfo)
    {
        m_Original = original;
        poolerInfo.MaxSize = poolerInfo.InitialSize > poolerInfo.MaxSize ? poolerInfo.InitialSize : poolerInfo.MaxSize; // take maximum between the two sizes

        m_ActualInstanceCount = poolerInfo.InitialSize;
        m_FreeInstances = new Stack<Cullable>(poolerInfo.InitialSize);
        m_AllInstances = new Cullable[poolerInfo.MaxSize];
        m_BoundingSpheres = new BoundingSphere[poolerInfo.MaxSize];

        for (int i = 0; i < poolerInfo.InitialSize; ++i)
        {
            Cullable obj = Object.Instantiate(original);
            obj.Index = i;
            obj.gameObject.SetActive(false);
            m_FreeInstances.Push(obj);
            m_AllInstances[i] = obj;
            m_BoundingSpheres[i] = new BoundingSphere(s_UndangerousBoundingSpherePosition);
        }
        m_CullingGroup = new CullingGroup();
        m_CullingGroup.SetBoundingSpheres(m_BoundingSpheres);
        m_CullingGroup.SetBoundingSphereCount(m_ActualInstanceCount);
        m_CullingGroup.onStateChanged = OnStateChanged;
        m_CullingGroup.targetCamera = targetCamera;
    }

    private void OnStateChanged(CullingGroupEvent evt)
    {
        if (evt.hasBecomeInvisible)
        {
            Free(m_AllInstances[evt.index]);
        }
    }

    public GameObject Get()
    {
        return Get(new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, 1.0f);
    }

    public GameObject Get(Vector3 pos, Quaternion rot, float maxSize)
    {
        Cullable ret = null;
        if (m_FreeInstances.Count > 0)
        {
            ret = m_FreeInstances.Pop();
        }else
        {
            if (m_ActualInstanceCount >= m_AllInstances.Length - 2)
            {
                DoubleArrayLengths();
            }

            ret = Object.Instantiate(m_Original);
            ret.Index = m_ActualInstanceCount;
            m_AllInstances[m_ActualInstanceCount] = ret;
            m_ActualInstanceCount++;
            m_CullingGroup.SetBoundingSphereCount(m_ActualInstanceCount);
        }
        ret.gameObject.SetActive(true);
        ret.Reset();
        ret.transform.position = pos;
        ret.transform.rotation = rot;
        m_BoundingSpheres[ret.Index] = new BoundingSphere(pos, maxSize);
        m_CullingGroup.SetBoundingSpheres(m_BoundingSpheres);
        return ret.gameObject;
    }

    private void DoubleArrayLengths()
    {
        Cullable[] LengthenedCullableArray = new Cullable[m_ActualInstanceCount * 2];
        m_AllInstances.CopyTo(LengthenedCullableArray, 0);
        m_AllInstances = LengthenedCullableArray;

        BoundingSphere[] LengthenedBSArray = new BoundingSphere[m_ActualInstanceCount * 2];
        m_BoundingSpheres.CopyTo(LengthenedBSArray, 0);
        m_BoundingSpheres = LengthenedBSArray;
    }

    private void Free(Cullable obj)
    {
        if(null != obj)
        {
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(false);
            m_BoundingSpheres[obj.Index] = new BoundingSphere(s_UndangerousBoundingSpherePosition);
            m_FreeInstances.Push(obj);
        }
    }

    public void Dispose()
    {
        m_CullingGroup.Dispose();
    }
}
