﻿using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public GameObject[] Shapes;
    public Transform[] SpawnPoints;
    public float startTime = 2.0f;
    public float spawnTime;
    public RaycastManager raycastManager;
    public Material cubeNormal, cubeOver, ballNormal, ballOver;
    public GameObject explosion;
    public ScoreManager scoreManager;

    private AudioSource _explosion_sound;
    private GameObject _currentFoundObject;
    private GameObject _previousFoundObject;


    private void Awake()
    {
        _explosion_sound = GetComponent<AudioSource>();
    }


    //  Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnShapes", spawnTime, spawnTime);
    }


    //  Spawns random shape at random spawnpoint
    //  void -> void
    public void SpawnShapes()
    {
        int spawnPointIndex = Random.Range(0, SpawnPoints.Length);
        int shapeIndex = Random.Range(0, Shapes.Length);

        GameObject shape = Shapes[shapeIndex];
        Instantiate(shape, SpawnPoints[spawnPointIndex].position, SpawnPoints[spawnPointIndex].rotation);
    }


    //  Spawns explosion and destroys shape object
    //  void -> void
    public void DestroyShape()
    {
        if (_currentFoundObject != null && _currentFoundObject.tag == "Shootable")
        {
            Instantiate(explosion, _currentFoundObject.transform.position, _currentFoundObject.transform.rotation);
            _explosion_sound.Play();
            Destroy(_currentFoundObject);
            scoreManager.AddToScore();
        }        
    }


    // Checks found object for "Shootable" tag
    // void -> void
    public void CheckForShootable()
    {
        GameObject newObject, oldObject;

        if (raycastManager.GetCurrentFoundObject())
        {
            newObject = raycastManager.GetCurrentFoundObject();

            if (_currentFoundObject != newObject)
            {
                _currentFoundObject = newObject;

                if (_currentFoundObject.tag == "Shootable")
                {
                    ChangeShapeColor(_currentFoundObject);
                }
            }            
        }

        if (raycastManager.GetPreviousFoundObject())
        {
            oldObject = raycastManager.GetPreviousFoundObject();

            if (oldObject.tag == "Shootable")
            {
                _previousFoundObject = oldObject;
                RevertShapeColor(_previousFoundObject);
            }
        }       
    }


    // Changes current found object material to over color material
    // gameObject -> void
    public void ChangeShapeColor(GameObject go)
    {
        if (go.name == "Ball(Clone)")
        {
            go.GetComponent<Renderer>().material = ballOver;
        }

        if (go.name == "Cube(Clone)")
        {
            go.GetComponent<Renderer>().material = cubeOver;
        }
    }


    // Reverts previously found object material to normal color material
    // gameObject -> void
    public void RevertShapeColor(GameObject go)
    {
        if (go.name == "Ball(Clone)")
        {
            go.GetComponent<Renderer>().material = ballNormal;
        }

        if (go.name == "Cube(Clone)")
        {
            go.GetComponent<Renderer>().material = cubeNormal;
        }
    }


    private void OnEnable()
    {
        RaycastManager.OnNewObjectFound += CheckForShootable;
    }

    private void OnDisable()
    {
        RaycastManager.OnNewObjectFound -= CheckForShootable;
    }    
}

