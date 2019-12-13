using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager current;

    public float movementSpeed = 7.0f;
    public PlatformSegment[] platformSegments;
    public float platformSpawnHeight = 0.0f;
    public float platformHorizBound = 0.0f;

    public BoxCollider2D screenLeftBoundCol;
    //public BoxCollider2D screenRightBoundCol;

    private Vector3 screenRightBound = new Vector3(10.0f, 0.0f, 0.0f);
    private PlatformSegment startingSegment = null;
    private Vector3 startingSegmentPos;
    private Vector3 availableSegmentPos;
    private Vector3 segmentMovementDelta;

    private bool canNewSegmentSpawn = true;

    private List<PlatformSegment> availableSegments;
    private List<PlatformSegment> activeSegments;

    private void Awake()
    {
        // Create a single accessible instance of the class (singleton pattern)
        current = this;

        availableSegments = new List<PlatformSegment>();
        activeSegments = new List<PlatformSegment>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startingSegmentPos = new Vector3(-2.0f, platformSpawnHeight, 0.0f);
        availableSegmentPos = new Vector3(screenRightBound.x + 10.0f, platformSpawnHeight, 0.0f);
        segmentMovementDelta = Vector3.right * movementSpeed;

        // If there are platform segment 'blueprints' to use
        if (platformSegments.Length > 0)
        {
            // Create an instance of each segment and store it ready for use
            foreach (PlatformSegment Segment in platformSegments)
            {
                PlatformSegment SpawnedSegment = Instantiate(Segment) as PlatformSegment;
                SpawnedSegment.transform.position = availableSegmentPos;
                availableSegments.Add(SpawnedSegment);
            }

            // Always make sure that the simplest segment is the one first encountered by the player
            startingSegment = availableSegments[0];
            startingSegment.transform.position = startingSegmentPos;
            activeSegments.Add(startingSegment);
            availableSegments.Remove(startingSegment);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activeSegments.Count > 0)
        {

            PlatformSegment newSegment = null;
            int newSegmentIndex = 0;

            foreach (PlatformSegment segment in activeSegments)
            {
                // If the segment has just been deactivated, don't bother processing it
                if (!segment.isActiveAndEnabled)
                {
                    break;
                }

                segment.transform.position -= segmentMovementDelta * Time.deltaTime;
            }

            // If the end of a segment has gone off the left side of screen
            if (activeSegments[0].segmentEnd.position.x < -screenRightBound.x)
            {
                // Deactivate the segment and move it into the pile of available segments on the right side of screen
                PlatformSegment removedSegment = activeSegments[0];
                removedSegment.gameObject.SetActive(false);
                removedSegment.transform.position = availableSegmentPos;

                availableSegments.Add(removedSegment);
                activeSegments.Remove(removedSegment);
            }

            // If the end of the segment is just about to travel past the right side of the screen and a new segment can be spawned
            if ((activeSegments[activeSegments.Count - 1].segmentStart.position.x < screenRightBound.x))
            {
                if (availableSegments.Count > 0)
                {
                    print("Add one in");

                    // Pick an available segment at random and place it immediately behind the segment above, ready for moving across the screen
                    newSegmentIndex = Random.Range(0, availableSegments.Count);
                    newSegment = availableSegments[newSegmentIndex];

                    newSegment.transform.position = new Vector3(activeSegments[activeSegments.Count - 1].segmentEnd.position.x + 1.0f, newSegment.transform.position.y, 0.0f);
                    newSegment.GenerateCoins();
                    newSegment.gameObject.SetActive(true);

                    activeSegments.Add(newSegment);
                    availableSegments.Remove(newSegment);
                }
            }
        }
    }

    public void DisablePlatforms()
    {
        foreach (PlatformSegment segment in activeSegments)
        {
            segment.gameObject.SetActive(false);
            segment.transform.position = availableSegmentPos;
            availableSegments.Add(segment);
        }

        activeSegments.Clear();
    }

    public void EnablePlatforms()
    {
        startingSegment.transform.position = startingSegmentPos;
        startingSegment.gameObject.SetActive(true);
        activeSegments.Add(startingSegment);
        availableSegments.Remove(startingSegment);
    }
}
