using UnityEngine;

public class VRCamClamp : MonoBehaviour { 

    [SerializeField]
    GameObject variable;
// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
