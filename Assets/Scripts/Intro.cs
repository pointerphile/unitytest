using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    private Slider m_LoadScrollCtl = null;

    void Awake()
    {
        PP.PPPrefabManager.Load("Prefabs", null);
    }
    // Use this for initialization
    void Start()
    {
        m_LoadScrollCtl = gameObject.GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        m_LoadScrollCtl.value = Mathf.MoveTowards(m_LoadScrollCtl.value, 1.0f, 0.008f);
        if (m_LoadScrollCtl.value >= 1.0f)
        {
            PP.PPScreenFader.LoadLevel(1, 1.0f, 1.0f, Color.black);
        }
    }
}
