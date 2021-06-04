using UnityEngine;

public class HandBehaviour : MonoBehaviour
{
    public static HandBehaviour instance;
    private Vector3 _targetPosition;
    public float range;
    void Awake()
    {
        instance = this;
        InvokeRepeating("ChangeTarget",1f,.5f);
    }

    void ChangeTarget(){
        _targetPosition = new Vector3(Random.Range(-range,range),0,Random.Range(-range,range));
    }


    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, _targetPosition, Time.deltaTime*5f);

    }
}
