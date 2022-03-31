using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Pumpkin : MonoBehaviour
{
    public GameObject npcCharacter;

    public GameObject pumpkin;

    public GameObject plane;

    public TMP_Text ammoText;

    public TMP_Text hitStudentsText;

    public AudioSource[] sounds;
    public AudioSource soundOne;
    public AudioSource soundTwo;

    public int pumpkinAmmo = 100;

    public float xCoordinate;

    public float yCoordinate = 0.88f;

    public float zCoordinate;

    public bool fallingState = false;

    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponents<AudioSource>();
        soundOne = sounds[0];
        soundTwo = sounds[1];
        Score.hitStudents = 0;
        ammoText.text = pumpkinAmmo.ToString();
        hitStudentsText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                //transform.position = raycastHit.point;
                Debug.Log(raycastHit.point);

                xCoordinate = raycastHit.point[0];
                zCoordinate = raycastHit.point[2];
            }

            pumpkin.SetActive(true);

            // Set the position of the pumpkin on the top
            pumpkin.transform.position = new Vector3(xCoordinate, yCoordinate, zCoordinate);

            pumpkinAmmo = pumpkinAmmo - 1;
            ammoText.text = pumpkinAmmo.ToString();
            soundOne.Play();
        }

        if (pumpkin.activeSelf)
        {
            pumpkin.transform.Translate(Vector3.down * 1);

            // If pumpkin hits the ground, make it dissapear
            if (Vector3.Distance(pumpkin.transform.position, plane.transform.position) == 0)
            {
                //Debug.Log("NOT HIT");
                pumpkin.SetActive(false);
            }

            // If pumpkin hits the NPC character
            if (Vector3.Distance(pumpkin.transform.position, npcCharacter.transform.position) < 0.5)
            {
                if (States.npcRunningState == true)
                {
                    Score.hitStudents = Score.hitStudents + 3;
                }
                else
                {
                    Score.hitStudents = Score.hitStudents + 1;
                }
                soundTwo.Play();
                hitStudentsText.text = Score.hitStudents.ToString();
                pumpkin.SetActive(false);
                npcCharacter.SetActive(false);
                States.countDown = 0;
            }
        }

        // If players are out of ammo, change to scene in which they can see they score and whether they completed or failed level
        if (pumpkinAmmo == 0)
        {
            SceneManager.LoadScene("LevelResult");
        }

    }
}
