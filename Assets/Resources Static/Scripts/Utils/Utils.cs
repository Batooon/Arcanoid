using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Utils Instance { get; private set; }
    
    [field: SerializeField] public Camera MainCamera { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
}
