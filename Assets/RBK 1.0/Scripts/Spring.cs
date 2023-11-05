using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]

public class Spring : MonoBehaviour
{


    public LookObjectsClass[] SpringsObject;
    [System.Serializable]
    public class LookObjectsClass
    {
        public Transform SpringUp;
        public Transform SpringDown;
    }

	void Update () {

        for (int x = 0; x < SpringsObject.Length; x++)
        {
            SpringsObject[x].SpringUp.LookAt(SpringsObject[x].SpringDown.position,transform.up);
            SpringsObject[x].SpringDown.LookAt(SpringsObject[x].SpringUp.position, transform.up);

        }
	}




}
