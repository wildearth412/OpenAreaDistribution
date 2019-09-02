using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSetting : MonoBehaviour
{
    public LineRenderer lineNorth;
    public LineRenderer lineSouth;
    public LineRenderer lineWest;
    public LineRenderer lineEast;
    public int boundPixelOffset = 10;
    public GameObject prefab;
    public GameObject prefab2;
    public int minPointsNum = 20;
    public int maxPointsNum = 30;
    public int maxFindingCount = 2;
    public float gridSize = 0.5f;
    public float distanceThresholdPoint= 3.7f;
    public float distanceThresholdBound = 1.3f;
    public List<Vector3> positions = new List<Vector3>();
    public List<gridPosAttr> gridPositions = new List<gridPosAttr>();
    public List<Vector3> selectedPositions = new List<Vector3>();

    [System.Serializable]
    public class gridPosAttr
    {
        public Vector3 pos;
        public float dis;       // Min distance from slef position to other position.
    }

    private Vector3 pos1, pos2, pos3, pos4;
    private List<GameObject> points = new List<GameObject>();
    private List<GameObject> points2 = new List<GameObject>();

    void Awake ()
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

        var hl = Mathf.Abs(pos1.x - pos2.x);
        var hn = hl / gridSize;
        hn = Mathf.CeilToInt(hn);
        var hc = hl / hn;
        hn -= 1;

        var vl = Mathf.Abs(pos1.y - pos3.y);
        var vn = vl / gridSize;
        vn = Mathf.CeilToInt(vn) - 1;
        var vc = vl / vn;
        vn -= 1;

        for (int i = 1; i <= hn; i++)
        {
            for (int j = 1; j <= vn; j++)
            {
                Vector3 p = new Vector3(pos1.x + i * hc, pos1.y - j * vc, 0);
                gridPosAttr g = new gridPosAttr();
                g.pos = p;
                gridPositions.Add(g);
            }
        }

        var num = Random.Range(minPointsNum,maxPointsNum);
        for(int i = 0; i < num; i++)
        {
            CreatePoint();
        }

        FindOpenArea();
        ShowSelectedPoints();
    }
	
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();

            int n = points.Count - 1;
            for (int i = n; i >= 0; i--)
            {
                Destroy(points[i]);
            }
            points.Clear();
            int n2 = points2.Count - 1;
            for (int i = n2; i >= 0; i--)
            {
                Destroy(points2[i]);
            }
            points2.Clear();
            positions.Clear();
            selectedPositions.Clear();
            var num = Random.Range(minPointsNum, maxPointsNum);
            for (int i = 0; i < num; i++)
            {
                CreatePoint();
            }

            FindOpenArea();
            ShowSelectedPoints();
        }
	}

    void CreatePoint()
    {
        var posX = Random.Range(pos1.x, pos2.x);
        var posY = Random.Range(pos3.y, pos1.y);
        Vector3 p = new Vector3(posX, posY, 0);
        positions.Add(p);
        var obj = Instantiate(prefab,p,Quaternion.identity);
        points.Add(obj);
    }

    void FindOpenArea()
    {
        for (int i = 0; i < gridPositions.Count; i++)
        {
            float dis = 99999f;
            for (int j = 0; j < positions.Count; j++)
            {
                float d = Vector3.Distance(gridPositions[i].pos, positions[j]);
                if(d < dis) { dis = d; }
            }
            gridPositions[i].dis = dis;
        }

        gridPositions.Sort((a, b) => b.dis.CompareTo(a.dis));

        int max = Mathf.Min(maxFindingCount,gridPositions.Count);
        int count = 0;
        List<Vector3> ptmps = new List<Vector3>();
        ptmps.Add(new Vector3(9999f, 9999f, 0));
        for (int k = 0; k < gridPositions.Count; k++)
        {
            var p = gridPositions[k].pos;
            if (p.x >= pos1.x + distanceThresholdBound && p.x <= pos2.x - distanceThresholdBound && p.y <= pos1.y - distanceThresholdBound && p.y >= pos3.y + distanceThresholdBound)
            {
                var dtmpm = 9999f;
                for(int l = 0; l < ptmps.Count; l++)
                {
                    var dtmp = Vector3.Distance(p, ptmps[l]);
                    if(dtmp < dtmpm) { dtmpm = dtmp; }
                }
                if (dtmpm > distanceThresholdPoint)
                {
                    ptmps.Add(p);
                    selectedPositions.Add(p);

                    count++;
                    if(count >= max)
                    {
                        return;
                    }
                }
            }
        }
    }

    void ShowSelectedPoints()
    {
        for(int i = 0; i < selectedPositions.Count; i++)
        {
            StartCoroutine(ShowWithDelay(i+1,selectedPositions[i]));
        }
    }

    IEnumerator ShowWithDelay(int n, Vector3 p)
    {
        yield return new WaitForSeconds(0.5f * n);
        var obj = Instantiate(prefab2, p, Quaternion.identity);
        points2.Add(obj);
    }
}
