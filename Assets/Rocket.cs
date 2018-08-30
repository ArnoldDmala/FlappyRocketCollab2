using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

    bool collisionsDisabled = false;

   [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;
    

    // Use this for initialization
    void Start () 
	{
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        // todo somewhere
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
            
        }

        if(Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }

        //todo only if debug on
        RespondToDebugKeys();
        
	}

    private void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey((KeyCode.A)))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey((KeyCode.D)))
        {
           
            transform.Rotate(-Vector3.forward * rotationThisFrame );
        }

        rigidBody.freezeRotation = false; //resume physics of rotation

        
    }

    void OnCollisionEnter(Collision collision) 
    {
        if(state != State.Alive || collisionsDisabled)
        {
            return; //ignore collisions when dead
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    
                    break;
                }
            case "Finish":
                {
                    StartSuccessSequence();

                    break;
                }
            default:
                print("Hit something deadly");
                StartDeathSequence();
               


                //kill player

                break;


        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;

        //my changes
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();


        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;

        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();


        Invoke("LoadNextLevel", levelLoadDelay); //parameterise time
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        print(currentSceneIndex);
        if (nextSceneIndex % nextSceneIndex == 0)
        {
            SceneManager.LoadScene(0);
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void LoadFirstLevel() //Death
    {
        

        SceneManager.LoadScene(0); //First level is actually one since I've changed the menu
    }

    private void RespondToThrustInput()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrustd();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
     }

    private void ApplyThrustd()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime); //in respect to time change BS

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}

