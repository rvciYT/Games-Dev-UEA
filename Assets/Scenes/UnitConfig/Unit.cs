using UnityEngine;

public class Units : MonoBehaviour
{

    void Start()
    {
       UnitSelections.Instance.unitList.Add(this.gameObject);
    }

    
    void OnDestroy()
    {
        UnitSelections.Instance.unitList.Remove(this.gameObject);
    }

   
}
