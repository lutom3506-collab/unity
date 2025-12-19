using UnityEngine;

public class Collectible : MonoBehaviour
{
    

    void Update()
    {
        transform.Rotate(new Vector3(0, 100, 0) * Time.deltaTime); //Drehung
    }

    private void OnTriggerEnter(Collider other) //wenn etwas in Collider trifft
    {
        if (other.CompareTag("Player"))   //Falls Objekt den Tag "Player" hat
        { 
            Collect();
        }
    }

    void Collect()    //<- besondere Mechanik für einsammeln einbauen!!!
    {
        Destroy(gameObject); //was beim einsammeln passiert
    }
}
