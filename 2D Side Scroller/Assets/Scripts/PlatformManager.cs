using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    // Fields
    public static PlatformManager current;

    public float movementSpeed = 7.0f;
    public PlatformSegment[] platformSegments;
    public float platformSpawnHeight = 0.0f;
    public float platformHorizBound = 0.0f;
    public float gapBetweenPlatforms = 2.0f;

    private Vector3 screenRightBound = new Vector3(10.0f, 0.0f, 0.0f);
    private PlatformSegment startingSegment = null;
    private Vector3 startingSegmentPos;
    private Vector3 availableSegmentPos;
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
        startingSegmentPos = new Vector3(-screenRightBound.x, platformSpawnHeight, 0.0f);
        availableSegmentPos = new Vector3(screenRightBound.x + 10.0f, platformSpawnHeight, 0.0f);

        // If there are platform segment 'blueprints' to use
        if (platformSegments.Length > 0)
        {
            // Create an instance of each segment and store it
            foreach (PlatformSegment Segment in platformSegments)
            {
                PlatformSegment SpawnedSegment = Instantiate(Segment) as PlatformSegment;
                SpawnedSegment.transform.position = availableSegmentPos;
                availableSegments.Add(SpawnedSegment);
            }

            // Always make sure that the simplest segment is the one first encountered by the player
            startingSegment = availableSegments[0];
            startingSegment.transform.position = startingSegmentPos;
            TransferSegmentToActive(startingSegment);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activeSegments.Count > 0)
        {
            foreach (PlatformSegment segment in activeSegments)
            {
                // If the segment has just been deactivated, don't bother processing it
                if (!segment.isActiveAndEnabled)
                {
                    break;
                }

                // Move the segment towards the left side of screen to give the illusion of the player running over it
                segment.transform.position = Vector3.Lerp(segment.transform.position, segment.transform.position - (Vector3.right * movementSpeed), Time.smoothDeltaTime);
            }

            // If the end of a segment has gone off the left side of screen
            if (activeSegments[0].segmentEnd.position.x < -screenRightBound.x)
            {
                // Deactivate the segment and transfer it into the pile of available segments
                PlatformSegment removedSegment = activeSegments[0];
                removedSegment.gameObject.SetActive(false);
                TransferSegmentToAvailable(removedSegment);
            }

            // If the end of the segment is just about to travel past the right side of the screen and a new segment can be spawned
            if ((activeSegments[activeSegments.Count - 1].segmentStart.position.x < screenRightBound.x))
            {
                if (availableSegments.Count > 0)
                {
                    // Pick an available segment at random and place it immediately behind the segment above
                    int newSegmentIndex = Random.Range(0, availableSegments.Count);
                    PlatformSegment newSegment = availableSegments[newSegmentIndex];
                    newSegment.transform.position = new Vector3(activeSegments[activeSegments.Count - 1].segmentEnd.position.x + gapBetweenPlatforms, newSegment.transform.position.y, 0.0f);

                    // Enable a random assortment of coins and obstacles on the segment
                    newSegment.GenerateCoins();
                    newSegment.GenerateObstacles();

                    // Activate the segment
                    newSegment.gameObject.SetActive(true);

                    // Transfer the segment into the list of active segments
                    TransferSegmentToActive(newSegment);
                }
            }
        }
    }

    public void DisablePlatforms()
    {
        // Transfer each active segment into the available segments list off screen and make them inactive
        if (activeSegments.Count > 0)
        {
            foreach (PlatformSegment segment in activeSegments)
            {
                segment.gameObject.SetActive(false);
                availableSegments.Add(segment);
            }

            activeSegments.Clear();
        }
    }

    public void EnablePlatforms()
    {
        // Make the starting segment active, which will result in subsequent segments being added later on
        startingSegment.transform.position = startingSegmentPos;
        startingSegment.GenerateCoins();
        startingSegment.GenerateObstacles();
        startingSegment.gameObject.SetActive(true);

        // Transfer the starting segment into the active segments list
        TransferSegmentToActive(startingSegment);
    }

    public void TransferSegmentToActive(PlatformSegment _segment)
    {
        activeSegments.Add(_segment);
        availableSegments.Remove(_segment);
    }

    public void TransferSegmentToAvailable(PlatformSegment _segment)
    {
        availableSegments.Add(_segment);
        activeSegments.Remove(_segment);
    }
}
