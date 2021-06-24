using UnityEngine;
using PathCreation;
using System.Collections.Generic;


public abstract class PersonCreator : MonoBehaviour
{
    [Header("-PATH-")]
    protected PathCreator _pathCreator;     

    [Header("-PERSONS-")]
    protected int personCount = 1;
    [SerializeField] protected PersonBase personPrefab;
    [SerializeField] [Range(.01f, 5)] protected float space=.4f;

    private void Awake()
    {
        _pathCreator = GameObject.FindGameObjectWithTag("MainPath").GetComponent<PathCreator>();       
        space *= Random.Range(.95f,1.05f);
    }

    public void CreatePersons()
    {
        if (personPrefab != null)
        {
            int x = 0;
            int y = 0;
            int added = 0;
            float spaceX = personPrefab.transform.localScale.x * space;
            float spaceZ = personPrefab.transform.localScale.z * space;
            for (int i = 0; i < personCount; i++)
            {
                GameObject g = Instantiate(personPrefab.gameObject, this.transform);
                PersonBase personBase = g.GetComponent<PersonBase>();
                g.name = typeof(PersonBase)+ "-"+i;

                x = i % 4;
                if (added < 4)
                {                   
                    added++;
                }
                else
                {
                    added = 0;
                    y++;
                }

                Vector3 pos = personPrefab.transform.position;
                pos.x = x * spaceX- ((i - 1) * spaceX/4);
                pos.z = y * spaceZ;

                personBase.SetPath(_pathCreator);
                personBase.SetStartPos(pos);
                GameManager.Instance.AddPerson(personBase);
            }

            transform.localPosition = _pathCreator.path.GetPointAtDistance(0);
            transform.localRotation = _pathCreator.path.GetRotationAtDistance(0);
        }
    }

   

}
