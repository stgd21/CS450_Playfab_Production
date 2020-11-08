using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;

    private float startTime;
    private float timeTaken;

    private int collectablesPicked = 0;
    public int maxCollectables = 10;

    private int obstaclesHit = 0;

    private bool isPlaying;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;
    public Vector3 startPos;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isPlaying)
            return;
        rig.AddForce(0, 0, speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.W))
            rig.AddForce(-40 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        if (Input.GetKey(KeyCode.S))
            rig.AddForce(40 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        //float x = Input.GetAxis("Horizontal") * speed;
        //float z = Input.GetAxis("Vertical") * speed;

        //rig.velocity = new Vector3(x, rig.velocity.y, z);
        curTimeText.text = (Time.time - startTime).ToString("F2");
        if (transform.position.y < -2f)
        {
            rig.velocity = Vector3.zero;
            transform.position = startPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            Destroy(other.gameObject);

            //if (collectablesPicked == maxCollectables)
            // End();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            obstaclesHit++;
            Debug.Log("hit an obstacle");
        }
            
    }

    public void Begin()
    {
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);
    }

    public void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        playButton.SetActive(true);

        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        CollectiblesLeaderboard.instance.SetLeaderboardEntry(collectablesPicked);
        ObstaclesLeaderboard.instance.SetLeaderboardEntry(obstaclesHit);
    }
}
