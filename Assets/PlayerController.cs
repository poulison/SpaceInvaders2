using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject missilePrefab;
    public float fireRate = 0.5f;
    private float nextFire = 0f;
    private float minX = -8f;
    private float maxX = 8f;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector3 pos = transform.position;
        pos.x += moveX * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(missilePrefab, transform.position, Quaternion.identity);
        }
    }
}
