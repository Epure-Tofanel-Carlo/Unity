using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMove : MonoBehaviour
{
    [SerializeField] Transform[] Points;
    [SerializeField] private float moveSpeed;
    private int pointsIndex;
    [SerializeField] private GameObject npc;

    void Start()
    {
        npc.transform.position = Points[pointsIndex].transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if(pointsIndex <= Points.Length - 1)
        {
            print(pointsIndex);
            npc.transform.position = Vector2.MoveTowards(transform.position, Points[pointsIndex].transform.position, moveSpeed * Time.deltaTime);
           
            if (npc.transform.position.x == Points[pointsIndex].transform.position.x 
                && npc.transform.position.y == Points[pointsIndex].transform.position.y)
            {
                pointsIndex++;
            }
            if(pointsIndex == Points.Length)
            {
                pointsIndex = 0;
            }
        }
    }
}
