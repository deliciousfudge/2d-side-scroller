using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager current;

    public float movementSpeed = 5.0f;
    public PlatformSegment[] platformSegments;
    public float platformSpawnHeight = 0.0f;
    public float platformHorizBound = 0.0f;

    private Vector3 screenRightBound = new Vector3(10.0f, 0.0f, 0.0f);
    private PlatformSegment startingSegment = null;

    private List<PlatformSegment> availablePlatformSegments;
    private Queue<PlatformSegment> movingPlatformSegments;

    private void Awake()
    {
        current = this;

        availablePlatformSegments = new List<PlatformSegment>();
        movingPlatformSegments = new Queue<PlatformSegment>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (platformSegments.Length > 0)
        {
            foreach (PlatformSegment Segment in platformSegments)
            {
                PlatformSegment SpawnedSegment = Instantiate(Segment) as PlatformSegment;
                SpawnedSegment.transform.position = new Vector3(screenRightBound.x + 1.0f, platformSpawnHeight, 0.0f);
                availablePlatformSegments.Add(SpawnedSegment);
            }

            // Move the first 
            startingSegment = availablePlatformSegments[0];
            startingSegment.transform.position = new Vector3(-2.0f, platformSpawnHeight, 0.0f);
            movingPlatformSegments.Enqueue(startingSegment);
            availablePlatformSegments.Remove(startingSegment);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (movingPlatformSegments.Count > 0)
        {
            PlatformSegment removedSegment = null;
            PlatformSegment newSegment = null;
            int newSegmentIndex = 0;

            foreach (PlatformSegment segment in movingPlatformSegments)
            {
                if (segment.segmentEnd.position.x < -platformHorizBound)
                {
                    //print("Segment end reached the left bound");
                    segment.gameObject.SetActive(false);
                    segment.transform.position = new Vector3(screenRightBound.x + 1.0f, platformSpawnHeight, 0.0f);
                    removedSegment = segment;
                }

                if ((segment.segmentEnd.position.x < screenRightBound.x + 1.0f) && segment.segmentEnd.position.x > screenRightBound.x + 0.5f)
                {
                    print("New segment triggered when end pos x is: " + segment.segmentEnd.position.x);

                    if (availablePlatformSegments.Count > 0)
                    {
                        newSegmentIndex = Random.Range(0, availablePlatformSegments.Count);
                        print("Index is " + newSegmentIndex);
                        newSegment = availablePlatformSegments[newSegmentIndex];
                        newSegment.transform.position = new Vector3(segment.segmentEnd.position.x + 1.0f, newSegment.transform.position.y, 0.0f);
                        print("Set up new segment");
                    }
                }

                segment.transform.position -= Vector3.right * movementSpeed * Time.deltaTime;
            }

            if (newSegment != null)
            {
                newSegment.gameObject.SetActive(true);
                movingPlatformSegments.Enqueue(availablePlatformSegments[newSegmentIndex]);
                availablePlatformSegments.Remove(newSegment);
            }

            if (removedSegment != null)
            {
                movingPlatformSegments.Dequeue();

                availablePlatformSegments.Add(removedSegment);
            }
        }
    }

    public void ResetPlatforms()
    {
        foreach (PlatformSegment segment in movingPlatformSegments)
        {
            segment.gameObject.SetActive(false);
            segment.transform.position = new Vector3(screenRightBound.x + 1.0f, platformSpawnHeight, 0.0f);

            availablePlatformSegments.Add(segment);
        }

        movingPlatformSegments.Clear();
        startingSegment.transform.position = new Vector3(-2.0f, platformSpawnHeight, 0.0f);
        movingPlatformSegments.Enqueue(startingSegment);
    }
}
