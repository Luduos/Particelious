using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Cullable : MonoBehaviour
{
    public int Index { get; set; }
    public abstract void Reset();
}

public class CullingPooler {

    protected Stack<Cullable> m_FreeInstances;
    protected Cullable[] m_AllInstances;
    protected BoundingSphere[] m_BoundingSpheres;
    protected CullingGroup m_CullingGroup;
    protected int m_ActualInstanceCount = 0;
    protected int m_NumberOfActiveCullables = 0;

    protected Cullable m_Original;

    public CullingPooler(Cullable original, Camera targetCamera, int initialSize, int maxSize)
    {
        m_Original = original;
        maxSize = initialSize > maxSize ? initialSize : maxSize; // take maximum between the two sizes

        m_ActualInstanceCount = initialSize;
        m_FreeInstances = new Stack<Cullable>(initialSize);
        m_AllInstances = new Cullable[maxSize];
        m_BoundingSpheres = new BoundingSphere[maxSize];

        for (int i = 0; i < initialSize; ++i)
        {
            Cullable obj = Object.Instantiate(original);
            obj.Index = i;
            obj.gameObject.SetActive(false);
            m_FreeInstances.Push(obj);
            m_AllInstances[i] = obj;
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
            if (m_ActualInstanceCount >= m_AllInstances.Length)
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
        m_BoundingSpheres[obj.Index] = new BoundingSphere();
        obj.transform.SetParent(null);
        obj.gameObject.SetActive(false);

        m_FreeInstances.Push(obj);
    }

    public void Dispose()
    {
        m_CullingGroup.Dispose();
    }
}
