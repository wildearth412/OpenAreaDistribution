using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSetting : MonoBehaviour
{
    public int boundPixelOffset = 10;
    public LineRenderer lineNorth;
    public LineRenderer lineSouth;
    public LineRenderer lineWest;
    public LineRenderer lineEast;

    public GameObject prefab;
    public GameObject prefab2;
    public int minPointsNum = 20;
    public int maxPointsNum = 30;

    private Vector3 pos1, pos2, pos3, pos4;
    private List<GameObject> points = new List<GameObject>();

	void Start ()
    {
        var pos0 = Camera.main.WorldToScreenPoint(Vector3.zero);
        pos1 = Camera.main.ScreenToWorldPoint(new Vector3(boundPixelOffset,Camera.main.pixelHeight - boundPixelOffset, pos0.z));
        pos2 = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - boundPixelOffset, Camera.main.pixelHeight - boundPixelOffset, pos0.z));
        pos3 = Camera.main.ScreenToWorldPoint(new Vector3(boundPixelOffset, boundPixelOffset, pos0.z));
        pos4 = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - boundPixelOffset, boundPixelOffset, pos0.z));
        lineNorth.SetPositions(new Vector3[] { pos1, pos2});
        lineSouth.SetPositions(new Vector3[] { pos3, pos4});
        lineWest.SetPositions(new Vector3[] { pos1, pos3 });
        lineEast.SetPositions(new Vector3[] { pos2, pos4 });

        var num = Random.Range(minPointsNum,maxPointsNum);
        for(int i = 0; i < num; i++)
        {
            CreatePoint();
        }
	}
	
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            int n = points.Count - 1;
            for (int i = n; i >= 0; i--)
            {
                Destroy(points[i]);
            }
            points.Clear();
            var num = Random.Range(minPointsNum, maxPointsNum);
            for (int i = 0; i < num; i++)
            {
                CreatePoint();
            }
        }
	}

    void CreatePoint()
    {
        var posX = Random.Range(pos1.x, pos2.x);
        var posY = Random.Range(pos3.y, pos1.y);
        var obj = Instantiate(prefab,new Vector3(posX,posY,0),Quaternion.identity);
        points.Add(obj);
    }

    
}
