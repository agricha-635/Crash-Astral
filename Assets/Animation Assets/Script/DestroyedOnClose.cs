using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedOnClose : MonoBehaviour
{
    public void OnClose()
    {
        Destroy(gameObject);  // Destroys the game object when the close button is clicked.
    }
}
