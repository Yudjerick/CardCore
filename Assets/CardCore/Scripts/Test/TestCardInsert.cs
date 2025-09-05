using CardCore;
using UnityEngine;

public class TestCardInsert : MonoBehaviour
{
    [SerializeField]
    private Transform cardToInsert;
    [SerializeField]
    private HandCardContainer handLayout;
    void Start()
    {
        Invoke("Test", 3f);
    }

    private void Test()
    {
        handLayout.InsertCard(0, cardToInsert.GetComponent<Card>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
