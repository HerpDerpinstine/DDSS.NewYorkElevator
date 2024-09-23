using Il2CppProps.Elevator;
using MelonLoader;
using UnityEngine;

namespace DDSS_NewYorkElevator
{
    internal class MelonMain : MelonMod
    {
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            // Check for New York office Scene
            if (sceneName != "Level_0")
                return;

            // Get All Elevator Controllers
            ElevatorController[] elevatorControllerArr = Resources.FindObjectsOfTypeAll<ElevatorController>();
            foreach (ElevatorController elevator in elevatorControllerArr)
            {
                // Enable the Elevator
                elevator.enabled = true;
                elevator.gameObject.SetActive(true);

                // Enable the Doors
                elevator.upperDoor.enabled = true;
                elevator.lowerDoor.enabled = true;

                // Fix Button Light
                foreach (var indicator in elevator.maxLevelIndicator)
                    indicator.SetRequested(true);
                foreach (var indicator in elevator.minLevelIndicator)
                    indicator.SetRequested(false);

                // Remove Out of Service Sign
                Transform signObjUpper = elevator.upperDoor.transform.Find("Plane/Service");
                if (signObjUpper != null)
                    GameObject.Destroy(signObjUpper.parent.gameObject);
                Transform signObjLower = elevator.lowerDoor.transform.Find("Plane/Service");
                if (signObjLower != null)
                    GameObject.Destroy(signObjLower.parent.gameObject);
            }
        }
    }   
}
