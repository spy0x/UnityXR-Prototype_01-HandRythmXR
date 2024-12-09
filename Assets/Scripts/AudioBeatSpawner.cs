using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

// public enum EHandPose
// {
//     None,
//     Paper,
//     Scissors,
//     // Rock,
//     // ThumbUp,
//     // ThumbDown,
// }

public enum EHandTurn
{
    None,
    Left,
    Right,
    Both
}
public class AudioBeatSpawner : MonoBehaviour
{
    [SerializeField] float bpm = 144;
    [SerializeField] Transform leftSpawnPoint;
    [SerializeField] Transform rightSpawnPoint;
    [SerializeField] GameObject[] leftPosePrefabs;
    [SerializeField] GameObject[] rightPosePrefabs;
    [SerializeField] PlayableDirector songTimeline;


    private EHandTurn[] handTurn = { EHandTurn.Right, EHandTurn.Left };
    private Coroutine currentCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (songTimeline) PlaySong();
    }

    public void StartCoroutineSpawnBeats()
    {
        currentCoroutine = StartCoroutine(SpawnBeatPoses());
    }
    public void StopCoroutineSpawnBeats()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
    }


    private IEnumerator SpawnBeatPoses()
    {
        if (leftPosePrefabs.Length == 0 || rightPosePrefabs.Length == 0) yield break;
        while (true)
        {
            float beatInterval = 60 / bpm;
            EHandTurn randomHandTurn = handTurn[Random.Range(0, handTurn.Length)];
            int randomLeftIndex = Random.Range(0, leftPosePrefabs.Length);
            int randomRightIndex = Random.Range(0, rightPosePrefabs.Length);
            switch (randomHandTurn)
            {
                case EHandTurn.Left:
                    Instantiate(leftPosePrefabs[randomLeftIndex], leftSpawnPoint.position, leftSpawnPoint.rotation);
                    break;
                case EHandTurn.Right:
                    Instantiate(rightPosePrefabs[randomRightIndex], rightSpawnPoint.position, rightSpawnPoint.rotation);
                    break;
                case EHandTurn.Both:
                    Instantiate(leftPosePrefabs[randomLeftIndex], leftSpawnPoint.position, leftSpawnPoint.rotation);
                    Instantiate(rightPosePrefabs[randomRightIndex], rightSpawnPoint.position, rightSpawnPoint.rotation);
                    break;
            }

            yield return new WaitForSeconds(beatInterval);
        }
    }

    private void PlaySong()
    {
        songTimeline.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pose"))
        {
            if (other.TryGetComponent(out ActiveStateSelector selector))
            {
                selector.WhenSelected += OnPoseSelected;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pose"))
        {
            if (other.TryGetComponent(out ActiveStateSelector selector))
            {
                selector.WhenSelected -= OnPoseSelected;
            }
        }
    }

    private void OnPoseSelected()
    {
        Debug.Log("Pose selected");
    }
    
    public void SetBpm(float newBpm)
    {
        bpm = newBpm;
    }
    public void SetHalfBpm()
    {
        bpm /= 2;
    }
    
    public void SetDoubleBpm()
    {
        bpm *= 2;
    }
}