using UnityEngine;
using System.Collections;

public class CarSwitch : MonoBehaviour
{

    public Transform[] Cars;
    public Transform MyCamera;


    public void CurrentCarActive(int current)
    {

        int amount = 0;

        foreach (Transform Car in Cars)
        {
            if (current == amount)
            {
                MyCamera.GetComponent<VehicleCamera>().target = Car;

                MyCamera.GetComponent<VehicleCamera>().Switch = 0;
                MyCamera.GetComponent<VehicleCamera>().cameraSwitchView = Car.GetComponent<BuggyControl>().carSetting.cameraSwitchView;
                Car.GetComponent<BuggyControl>().activeControl = true;
            }
            else
            {
                Car.GetComponent<BuggyControl>().activeControl = false;
            }

            amount++;
        }
    }




}
