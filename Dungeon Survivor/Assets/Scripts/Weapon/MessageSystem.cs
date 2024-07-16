using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    public static MessageSystem instance;

    private void Awake()
    {
        instance = this;
    }

    int objectCount = 10;
    int count;

    private void Start()
    {
        messagePool = new List<TMPro.TextMeshPro> ();
        for(int i = 0;i< objectCount; i++)
        {
            Populate();
        }
    }

    List<TMPro.TextMeshPro> messagePool;

    public void Populate()
    {
        GameObject go = Instantiate(damageMessage, transform);
        messagePool.Add(go.GetComponent<TMPro.TextMeshPro>());
        go.SetActive(false);
    }

    [SerializeField] GameObject damageMessage;

    public void PostMessage(string text, Vector3 worldPosition)
    {
        messagePool[count].gameObject.SetActive(true);
        messagePool[count].transform.position = worldPosition;
        messagePool[count].text = text;
        count += 1;

        if(count >= objectCount)
        {
            count = 0;
        }
    }
}
