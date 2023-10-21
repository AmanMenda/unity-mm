using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    public GameObject ghostPrefab;

    public float delay = 0.01f;

    public float delta = 0;

    PlayerMovement player;
    SpriteRenderer sr;

    public float destroyTime = 0.1f;
    public Color color;

    public Material material = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delta > 0)
        {
            delta -= Time.deltaTime; // StartTime/CurrentTime - deltaTime
        }
        else
        {
            delta = delay;
            createGhost();
        }
    }

    void createGhost()
    {
        GameObject ghostObj = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghostObj.transform.localScale = player.transform.localScale;
        Destroy(ghostObj, destroyTime);

        sr = ghostObj.GetComponent<SpriteRenderer>();
        sr.sprite = player.sr.sprite;
        sr.color = color;

        if (material)
            sr.material = material;
    }
}
