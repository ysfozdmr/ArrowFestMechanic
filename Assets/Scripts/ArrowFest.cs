using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.EditorCoroutines.Editor;

public class ArrowFest : MonoBehaviour
{
    public List<GameObject> arrows = new List<GameObject>();
    public GameObject arrowObject;
    public Transform parent;
    public float minX, maxX;
    public LayerMask layerMask;
    public float mesafe;

    private bool isDecrase = false;

    [Range(0, 300)] public int arrowCount;
    void Start()
    {

    }


    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            GetRay();
        }
    }
    private void OnValidate()
    {
        if (arrowCount > arrows.Count && !isDecrase)
        {
            CreateArrow();
        }
        else if (arrows.Count > arrowCount)
        {
            isDecrase = true;
            DestroyArrows();
        }
        else
        {
            Diz();
        }
    }
    void CreateArrow()
    {
        for(int i = arrows.Count; i < arrowCount; i++)
        {
            GameObject g = Instantiate(arrowObject, parent);
            arrows.Add(g);
            g.transform.localPosition = Vector3.zero;
        }
        Diz();
    }
    IEnumerator DestroyObject(GameObject g)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(g);
    }
    void DestroyArrows()
    {
        for (int i = arrows.Count-1; i >= arrowCount; i--)
        {
            GameObject g = arrows[arrows.Count - 1];
            arrows.RemoveAt(arrows.Count - 1);
            EditorCoroutineUtility.StartCoroutine(DestroyObject(g),this);
        }
        isDecrase = false;
        Diz();
    }
    void MoveObjects(Transform objectTransform,float degree)
    {
        Vector3 pos = Vector3.zero;
        pos.x = Mathf.Cos(degree * Mathf.Deg2Rad);
        pos.y = Mathf.Sin(degree * Mathf.Deg2Rad);

        objectTransform.localPosition = pos * mesafe;
    }
    void Diz()
    {
        
        float angle = 1f;
        float arrowCounts = arrows.Count;

        angle = 360f / arrowCounts;

        for(int i = 0; i < arrowCounts; i++)
        {
            MoveObjects(arrows[i].transform,i*angle);
        }
    }
    void GetRay()
    {
        
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.z;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            Vector3 mouse = hit.point;
            mouse.x = Mathf.Clamp(mouse.x, minX, maxX);

            mesafe = mouse.x;
            Diz();
        }
    }
}
