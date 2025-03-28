using UnityEngine;

public class Interact_Object : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed = 5f;
    [SerializeField] float pickItemDistance = 1.5f;
    [SerializeField] float ttl = 10f;

    public ItemData_Oficial item;
    public int count = 1;
    private void Awake()
    {
        player = GameManager.instance.player.transform;
    }
    public void Set(ItemData_Oficial item, int count)
    {
        this.item = item;
        this.count = count;
    }
    private void Update()
    {
        ttl -= Time.deltaTime;
        if (ttl < 0)
            Destroy(gameObject);
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < pickItemDistance)
            return;
        transform.position = Vector3.MoveTowards(transform.position,player.position,speed*Time.deltaTime);
        if(distance < .1f)
        {
           Destroy(gameObject);
        }
    }
}
