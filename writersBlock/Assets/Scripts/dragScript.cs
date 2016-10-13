using UnityEngine;
using System.Collections;

public class dragScript : MonoBehaviour {

    public void OnDrag()
    {
        transform.position = Input.mousePosition;
    }

}
