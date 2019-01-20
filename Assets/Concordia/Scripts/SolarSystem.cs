using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour {

    public float spread = 0.1f;
    public int numNewPoints = 3;

    private void OnEnable()
    {
        //ConcordiaOSCReceiver.OnReceive += Receive;
        ConcordiaCSV.OnReceive += Receive;
        AllFeelsOSCReceiver.OnReceive += Receive;
    }

    private void OnDisable()
    {
        //ConcordiaOSCReceiver.OnReceive -= Receive;
        ConcordiaCSV.OnReceive -= Receive;
        AllFeelsOSCReceiver.OnReceive -= Receive;
    }
    
    List<Vector3[]> coordinates = new List<Vector3[]>();
    public List<LineRenderer> lines = new List<LineRenderer>();
    public int numLines = 100;
    public LineRenderer linePrefab;

    void Start()
    {
        lineParent.transform.parent = transform;
        //currentScale = scale;
        for (int i=0; i< numLines; i++)
        {
            LineRenderer line = Instantiate(linePrefab);
            line.name = i.ToString();
            line.transform.parent = lineParent.transform;
            lines.Add(line);
        }
    }

    int currentLineIndex = 0;
    public GameObject lineParent;

    void Update()
    {
        if(positionsQueue.Count > 0)
        {
            LineRenderer line = lines[currentLineIndex];
            currentLineIndex++;
            if(currentLineIndex >= lines.Count)
            {
                currentLineIndex = 0;
            }
            line.useWorldSpace = false;
            line.positionCount = numNewPoints + 2;
            Vector3[] positions = positionsQueue.Dequeue();

            float x1 = positions[0].x + Random.Range(-spread, spread);
            float y1 = positions[0].y + Random.Range(-spread, spread);
            float z1 = positions[0].z + Random.Range(-spread, spread);
            float x2 = positions[1].x + Random.Range(-spread, spread);
            float y2 = positions[1].y + Random.Range(-spread, spread);
            float z2 = positions[1].z + Random.Range(-spread, spread);
            Vector3 startPoint = new Vector3(x1, y1, z1);
            Vector3 endPoint = new Vector3(x2, y2, z2);

            line.SetPosition(0, startPoint); // positions[0]);
            for (int i=0; i<numNewPoints; i++) {
                float progress = (float) (i+1) / (float) numNewPoints;
                line.SetPosition(i+1, Vector3.Lerp(startPoint, endPoint, progress));
            }
            line.SetPosition(numNewPoints+1, endPoint);
            //line.GetComponent<AlignCapsule>().Align(positions[0], positions[1]);

        }
    }

    Queue<Vector3[]> positionsQueue = new Queue<Vector3[]>();

    void Receive(PlanetPacket p0, PlanetPacket p1)
    {
        positionsQueue.Enqueue(new Vector3[] { p0.GetCoordinates(), p1.GetCoordinates() });
    }
}
