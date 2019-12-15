using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    // Fields
    public static PlatformManager current; // A reference to the single accessible instance of the class

    public float movementSpeed = 7.0f; // How fast each platform segment moves across the screen
    public PlatformSegment[] platformSegments; // The platform segment 'blueprints' to spawn instances of
    public float platformSpawnHeight = 0.0f; // The Y position to spawn each segment at
    public float gapBetweenPlatforms = 2.0f; // How far apart each platform segment is spawned

    private Vector3 screenRightBound = new Vector3(10.0f, 0.0f, 0.0f); // The location of the right side of the screen
    private PlatformSegment startingSegment = null; // A reference to the segment always displayed first
    private Vector3 startingSegmentPos; // The location where the starting segment is spawned at the beginning of play
    private Vector3 availableSegmentPos; // The location where segments are stored when not in use
    private List<PlatformSegment> availableSegments; // Stores segment instances that are being stored when not in use
    private List<PlatformSegment> activeSegments; // Stores segment instances that are in use and being moved across the screen

    /// <summary>
    /// Processes gameplay logic immediately after objects are initialized
    /// </summary>
    private void Awake()
    {
        // Create a single accessible instance of the class (singleton pattern)
        current = this;

        availableSegments = new List<PlatformSegment>();
        activeSegments = new List<PlatformSegment>();
    }

    /// <summary>
    /// Processes gameplay logic prior to the first frame being displayed
    /// </summary>
    void Start()
    {
        // Store the location
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

    /// <summary>
    /// Updates gameplay logic any time a new frame is displayed to the screen
    /// </summary>
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

    /// <summary>
    /// Transfers each active segment into the available segments list off screen and make them inactive
    /// </summary>
    public void DisablePlatforms()
    {
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

    /// <summary>
    /// Transfers the starting segment into the active segments list, which kickstarts the segment movement process
    /// </summary>
    public void EnablePlatforms()
    {
        startingSegment.transform.position = startingSegmentPos;
        startingSegment.GenerateCoins();
        startingSegment.GenerateObstacles();
        startingSegment.gameObject.SetActive(true);

        TransferSegmentToActive(startingSegment);
    }

    /// <summary>
    /// Transfers a segment instance from available to active
    /// </summary>
    /// <param name="_segment">A reference to the platform segment to transfer</param>
    public void TransferSegmentToActive(PlatformSegment _segment)
    {
        activeSegments.Add(_segment);
        availableSegments.Remove(_segment);
    }

    /// <summary>
    /// Transfers a segment instance from active to available
    /// </summary>
    /// <param name="_segment">A reference to the platform segment to transfer</param>
    public void TransferSegmentToAvailable(PlatformSegment _segment)
    {
        availableSegments.Add(_segment);
        activeSegments.Remove(_segment);
    }
}
