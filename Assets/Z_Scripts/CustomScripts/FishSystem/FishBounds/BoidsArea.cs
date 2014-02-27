using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;

class BoidsArea : MonoBehaviour
{
    public bool DrawGizmos = false;
    public bool DrawEmptyGizmos = false;
    public bool DrawFullGizmos = false;

    public GameObject fishPrefab1;
    public int spawnCount1 = 30;

	// Use this for initialization
    public static List<BoundsOctree> boundsCollection;
    public static bool fastObstacle = true;
    private BoundsOctree bounds;
    public int maxLevels = 4;
    public float boundsSize = 10.0f;
    public string BoundsName = "fishBounds1";
    public Color BoundsColor = Color.magenta;

    public LayerMask ObstacleMask;
    private bool isStarted;

    public Vector3 startingPoint;
    public bool RandomStart = true;

    public bool GenerateBounds = false;
    public bool LoadBoundsFromFile = false;
    public bool CleanUpBounds = false;

	void Start () 
    {
        isStarted = true;

	    if (GenerateBounds)
	    {

            DebugOutput.Shout("####GENERATING BOUNDS AND SAVING#####");
            CreateBounds();

            SaveBounds();
            DebugOutput.Shout("####GENERATING BOUNDS AND SAVING#####");

	    }

	    if (LoadBoundsFromFile)
	    {
            DebugOutput.Shout("####LOADING BOUNDS FROM FILE#####");

            LoadBounds();
            DebugOutput.Shout("####LOADING BOUNDS FROM FILE#####");
	        
	    }
        if (boundsCollection == null)
        {
            boundsCollection = new List<BoundsOctree>();
        }
        boundsCollection.Add(bounds); // iffy quick solution, will add proper management to created bounds later when i need it, at least it is a start.
		GL_OctreeRenderer.OctreeRenderer.boundsOctree = bounds;
        PopulateBounds();      
      
    }

    static public BoundsOctree getBoundsOctree(Vector3 pos)
    {
        if (boundsCollection != null)
        {
            BoundsOctree bo;

            foreach (var bounds in boundsCollection)
            {
                bo = bounds.GetBound(pos);
                if (bo != null)
                {
                    return bo;
                }
            }
        }
        return null;
    }
    static public void ToggleLazyGhost()
    {
        fastObstacle = !fastObstacle;
    }
    public void PopulateBounds()
    {
        List<BoundsContainment> population = new List<BoundsContainment>();
        if (fishPrefab1)
        {
            for (int i = 0; i < spawnCount1; i++)
            {
                GameObject g = (GameObject)Instantiate(fishPrefab1);
                g.transform.parent = transform;
                g.transform.localPosition = startingPoint;
                BoundsContainment BC = g.GetComponent<BoundsContainment>();
                if (BC != null)
                {
                    population.Add(BC);

                  
                }
            }
        }


        int randomSpawnCount = 0;
        int randomSpawnOctree = 0;
        foreach (var pop in population)
        {
            pop.baseOctree = bounds;
            if (RandomStart)
            {
                if (randomSpawnCount <= 0)
                {
                    randomSpawnCount = UnityEngine.Random.Range(1, 15);
                    randomSpawnOctree = UnityEngine.Random.Range(0, bounds.relatedOctrees.Count - 1);
                }
                randomSpawnOctree--;
                pop.boundsOctree = bounds.relatedOctrees[randomSpawnOctree];
            }
            else
            {
                pop.boundsOctree = bounds.relatedOctrees[0];
            }
            pop.transform.position = pop.boundsOctree.position;
        }
        

    }
    public void CreateBounds()
    {

        DebugOutput.Shout("Starting BoundsOctrees test");
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        Stopwatch stopWatch2 = new Stopwatch();
        stopWatch2.Start();

        bounds = new BoundsOctree(null, 0, BoundsName, boundsSize, transform.position, maxLevels, 0, null);
        bounds.CheckBounds(ObstacleMask);

        stopWatch2.Stop();
        DebugOutput.Shout("created bounds in " + (stopWatch2.ElapsedMilliseconds) + ", a total of " + bounds.relatedOctrees.Count + " octrees were created");

        Stopwatch stopWatch3 = new Stopwatch();
        stopWatch3.Start();
        foreach (var relatedOctree in bounds.relatedOctrees)
        {
            relatedOctree.RefreshNeighbors();
        }
        int oldCnt = bounds.relatedOctrees.Count;
        stopWatch3.Stop();

        DebugOutput.Shout("established neighbors in " + (stopWatch3.ElapsedMilliseconds));

        Stopwatch stopWatch4 = new Stopwatch();
        stopWatch4.Start();


        Stopwatch stopWatch41 = new Stopwatch();
        stopWatch41.Start();
        BoundsOctree origoBound = bounds.GetBound(true,transform.position + startingPoint);
        if (origoBound != null)
        {
            origoBound.EstablishRelations();
        }
        else
        {
            DebugOutput.Shout("Origopoint is unreachable.");
        }
        stopWatch41.Stop();

        DebugOutput.Shout("Established Relations in" + (stopWatch41.ElapsedMilliseconds));

        if (CleanUpBounds)
        {
            Stopwatch stopWatch42 = new Stopwatch();
            stopWatch42.Start();
            bounds.CleanOutUnrelated();
            foreach (var relatedOctree in bounds.relatedOctrees)
            {
                relatedOctree.RefreshNeighbors();
            }
            stopWatch42.Stop();
            DebugOutput.Shout("cleanedoutUnrelated in" + (stopWatch42.ElapsedMilliseconds));

        }
        
        stopWatch4.Stop();
        DebugOutput.Shout("cleaned up octrees in" + (stopWatch4.ElapsedMilliseconds)
            + ", a total of " + (oldCnt - bounds.relatedOctrees.Count) + "were eliminated");

        int cnt = 0;
        foreach (var relatedBounds in bounds.relatedOctrees)
        {
            if (relatedBounds.Empty && relatedBounds.Related)
            {
                cnt++;
            }
        }
        stopWatch.Stop();
        DebugOutput.Shout("finished bounds in " + (stopWatch.ElapsedMilliseconds) 
            + ", a total of " + bounds.relatedOctrees.Count + " octrees remains, of which "
            + cnt + " is traversable");
               
    }
	public void SaveBounds()
	{
        Debug.Log("Testing SaveBounds");
        string fileName = "Assets/Resources/OctreeData/"+BoundsName+".bytes";

        if (File.Exists(fileName))
        {
            //Console.WriteLine("{0} already exists!", FILE_NAME);
            File.Delete(fileName);
            //return;
        }
        using (FileStream fs = new FileStream(fileName, FileMode.CreateNew))
        {
            using (BinaryWriter w = new BinaryWriter(fs))
            {
                DebugOutput.Shout("bounds were created: " + DateTime.Now.ToString());

                w.Write(new DateTime().ToString());
                w.Write(maxLevels);       
                //w.Write("TestingTesting");
                //w.Write();
                bounds.SaveOctreeToStream(w);

            }
        }
	}
    public void LoadBounds()
    {
        DebugOutput.Shout("Starting BoundsOctrees test");
        //myCitizen.myOctree.RefreshNeighbors();
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        DebugOutput.Shout("Testing LoadingBounds");
        string fileName = "OctreeData/" + BoundsName;

        TextAsset ta = (TextAsset)Resources.Load(fileName);
        //DebugOutput.Shout(ta.text);
        BinaryReader br = new BinaryReader(new MemoryStream(ta.bytes));
        DebugOutput.Shout("bounds were created: " +br.ReadString());
        bounds = new BoundsOctree(null, 0, BoundsName, boundsSize, transform.position, br.ReadInt32(), 0, null);

        bounds.CreateFromStream(br);

        foreach (var relatedOctree in bounds.relatedOctrees)
        {
            relatedOctree.RefreshNeighbors();
        }
        BoundsOctree origoBound = bounds.GetBound(true, transform.position + startingPoint);
        if (origoBound != null)
        {
            origoBound.EstablishRelations();
        }
        else
        {
            DebugOutput.Shout("Origopoint is unreachable.");
        }
        stopWatch.Stop();
        DebugOutput.Shout("Finished Loading bounds in "+stopWatch.ElapsedMilliseconds +" milliseconds.");
        int cnt = 0;
        foreach (var relatedBounds in bounds.relatedOctrees)
        {
            if (relatedBounds.Empty && relatedBounds.Related)
            {
                cnt++;
            }
        }
        DebugOutput.Shout("finished bounds in " + (stopWatch.ElapsedMilliseconds) + ", a total of " + bounds.relatedOctrees.Count + " octrees remains, of which " + cnt + " is traversable");

    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position + startingPoint, 0.3f);
            Gizmos.DrawWireCube(transform.position, Vector3.one * boundsSize * 2.0f);
            Gizmos.DrawWireSphere(transform.position, boundsSize * 1.732f);
            if (isStarted)
            {
                //Gizmos.color = BoundsColor;
                if (DrawEmptyGizmos)
                {

                    bounds.DrawEmptyGizmos();
                }
                if (DrawFullGizmos)
                {

                    bounds.DrawFullGizmos();
                }

            }
        }
        
    }
#endif
}
