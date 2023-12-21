using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Arrow { public GameObject head, body; }

public class ArrowGenerator : MonoBehaviour
{
    public static ArrowGenerator instance;

    private void Awake() => instance = this;

    public GameObject arrowHead;
    public GameObject arrowBody;

    private List<Arrow> availableArrowPool = new List<Arrow>();
    private List<Arrow> inUseArrowPool = new List<Arrow>();

    //Call DrawArrow() on every frame to draw arrows
    public void DrawArrow(Vector2 startPosition, Vector2 endPosition, Color color)
    {
        //create new arrow if none available
        if(availableArrowPool.Count <= 0)
        {
            Arrow arr = new Arrow
            {
                head = Instantiate(arrowHead),
                body = Instantiate(arrowBody)
            };
            availableArrowPool.Add(arr);
        }

        Vector2 direction = endPosition - startPosition;
        Arrow cog = availableArrowPool[0];
        cog.head.transform.right = direction;
        cog.body.transform.right = direction;

        cog.body.transform.position = startPosition;
        cog.head.transform.position = endPosition;
        cog.body.transform.localScale = new Vector3(Vector2.Distance(startPosition,endPosition), 1, 1);

        cog.head.GetComponent<SpriteRenderer>().color = color;
        cog.body.GetComponent<SpriteRenderer>().color = color;

        availableArrowPool.Remove(cog);
        inUseArrowPool.Add(cog);
    }

    private void Update()
    {
        if(availableArrowPool.Count > 0)
        {
            foreach(Arrow arrow in availableArrowPool.Except(inUseArrowPool).ToArray())
            {
                Destroy(arrow.body);
                Destroy(arrow.head);
            }
        }
        availableArrowPool = new List<Arrow>(inUseArrowPool);
        inUseArrowPool = new List<Arrow>();
    }
}
