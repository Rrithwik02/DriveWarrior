using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [Header("Checkpoints")]
    public GameObject start;
    public GameObject end;
    public GameObject[] checkpoints;
    public GameObject CurrentCheckpoint;

    [Header("Settings")]
    public float laps = 1;

    [Header("Information")]
    private float currentCheckpoint;
    private float currentLap;
    private bool started;
    private bool finished;

    private float currentLapTime;
    private float bestLapTime;
    private float bestLap;

    public GameObject MainMenu;

    private void Start()
    {
        currentCheckpoint = 0;
        currentLap = 1;

        started = false;
        finished = false;

        currentLapTime = 0;
        bestLapTime = 0;
        bestLap = 0;

    }
    private void Update()
    {
        if (started && !finished)
        {
            currentLapTime += Time.deltaTime;

            if (bestLap == 0)
            {
                bestLap = 1;
            }
        }
        if (started)
        {
            if (bestLap == currentLap)
            {
                bestLapTime = currentLapTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CurrentCheckpoint != null)
            {
                transform.position = CurrentCheckpoint.transform.position;
                transform.rotation = Quaternion.LookRotation(CurrentCheckpoint.transform.right, Vector3.up);

            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            GameObject thisCheckpoint = other.gameObject;
            CurrentCheckpoint = thisCheckpoint;
            if (thisCheckpoint == start && !started)
            {

                started = true;
                print("Started");
            }
            else if (thisCheckpoint == end && started)
            {
                if (currentLap == laps)
                {
                    if (currentCheckpoint == checkpoints.Length)
                    {
                        if (currentLapTime < bestLapTime)
                        {
                            bestLap = currentLap;
                        }


                        finished = true;
                        print("Finished");
                    }
                    if (finished)
                    {
                        MainMenu.SetActive(true);
                        GetComponent<CarSript>().enabled = false;
                    }
                    else
                    {
                        print("Did not Pass through all the Checkpoints");
                    }
                }
                else if (currentLap < laps)
                {
                    if (currentCheckpoint == checkpoints.Length)
                    {
                        if (currentLapTime < bestLapTime)
                        {
                            bestLap = currentLap;
                            bestLapTime = currentLapTime;
                        }

                        currentLap++;
                        currentCheckpoint = 0;
                        currentLapTime = 0;
                        print($"Started Lap {currentLap} - {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                    }
                }
                else
                {
                    print("Did not pass through all the checkpoints");
                }
            }
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (finished)
                    return;

                if (thisCheckpoint == checkpoints[i] && i == currentCheckpoint)
                {
                    print($"Correct Checkpoint - {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                    currentCheckpoint++;
                }
                else if (thisCheckpoint == checkpoints[i] && i != currentCheckpoint)
                {
                    print("Incorrect Checkpoint");
                }
            }

        }
    }
    private void OnGUI()
    {
        string formattedCurrentTime = $"Current: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}-(Lap{currentLapTime})";
        GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentTime);

        string formattedBestTime = $"Best: {Mathf.FloorToInt(bestLapTime / 60)}:{bestLapTime % 60:00.000} - (Lap{bestLap})";
        GUI.Label(new Rect(250, 10, 250, 100), formattedBestTime);

    }

}
