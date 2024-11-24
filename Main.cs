using DDSS_NewYorkElevator.Patches;
using Il2CppProps.Elevator;
using MelonLoader;
using System;
using UnityEngine;

namespace DDSS_NewYorkElevator
{
    internal class MelonMain : MelonMod
    {
        internal const bool DEBUG = true;

        internal static MelonLogger.Instance _logger;

        public override void OnInitializeMelon()
        {
            _logger = LoggerInstance;

            ApplyPatch<Patch_ElevatorController>();

            _logger.Msg("Initialized");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            // Check for New York office Scene
            if (sceneName != "Level_0")
                return;

            // Enable Movers
            ElevatorMover[] elevatorMoverArr = Resources.FindObjectsOfTypeAll<ElevatorMover>();
            foreach (ElevatorMover mover in elevatorMoverArr)
            {
                mover.enabled = true;
                mover.gameObject.SetActive(true);
            }

            // Enable Controllers
            ElevatorController[] elevatorControllerArr = Resources.FindObjectsOfTypeAll<ElevatorController>();
            foreach (ElevatorController elevator in elevatorControllerArr)
            {
                // Enable the Elevator
                elevator.enabled = true;
                elevator.gameObject.SetActive(true);

                // Enable the Doors
                elevator.lowerDoor.enabled = true;
                elevator.upperDoor.enabled = true;

                // Fix Indicators
                foreach (var indicator in elevator.minLevelIndicator)
                    indicator.SetRequested(false);
                foreach (var indicator in elevator.maxLevelIndicator)
                    indicator.SetRequested(true);

                // Remove Out of Service Signs
                Transform signObjLower = elevator.lowerDoor.transform.Find("Plane/Service");
                if (signObjLower != null)
                    UnityEngine.Object.Destroy(signObjLower.parent.gameObject);
                Transform signObjUpper = elevator.upperDoor.transform.Find("Plane/Service");
                if (signObjUpper != null)
                    UnityEngine.Object.Destroy(signObjUpper.parent.gameObject);
            }

        }

        private void ApplyPatch<T>()
        {
            Type type = typeof(T);
            try
            {
                HarmonyInstance.PatchAll(type);
            }
            catch (Exception e)
            {
                LoggerInstance.Error($"Exception while attempting to apply {type.Name}: {e}");
            }
        }
    }   
}
