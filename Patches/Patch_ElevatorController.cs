using HarmonyLib;
using Il2CppProps.Elevator;
using UnityEngine;
using DDSS_ModHelper.Utils;
using System.Collections;
using Il2Cpp;

namespace DDSS_NewYorkElevator.Patches
{
    internal class Patch_ElevatorController
    {
        private const int _lowerFloor = 0;
        private const int _upperFloor = 1;
        private const float _speed = 4f;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ElevatorController), nameof(ElevatorController.Start))]
        private static bool Start_Prefix(ElevatorController __instance)
        {
            // Start Movement Coroutine
            __instance.targetElevatorHeight = __instance.maxElevatorHeight;
            if (__instance.isServer)
                __instance.StartCoroutine(MovementCoroutine(__instance));

            // Prevent Original
            return false;
        }

        private static IEnumerator MovementCoroutine(ElevatorController controller)
        {
            int _currentFloor = _upperFloor;
            
            Vector3 currentPos = controller.transform.position;
            float upperFloorY = currentPos.y;
            float lowerFloorY = currentPos.y + controller.minElevatorHeight + -controller.maxElevatorHeight;

            // Infinite Loop
            while (true)
            {
                // Check for Floor Changes
                if (controller.requests.Count <= 0)
                {
                    yield return null;
                    continue;
                }

                // Check for Floor Changes
                int _requestedFloor = controller.requests[0];
                controller.requests.RemoveAt(0);
                if (_requestedFloor == _currentFloor)
                {
                    yield return null;
                    continue;
                }

                MelonMain._logger.Msg($"Requested Floor: {_requestedFloor}");
                MelonMain._logger.Msg("Moving...");

                // Apply Floor Indicators
                foreach (ElevatorLevelIndicator indicator in controller.minLevelIndicator)
                    indicator.SetRequested(_requestedFloor == _lowerFloor);
                foreach (ElevatorLevelIndicator indicator in controller.maxLevelIndicator)
                    indicator.SetRequested(_requestedFloor == _upperFloor);

                // Close Doors
                controller.upperDoor.NetworkcanOpenDoor = false;
                controller.lowerDoor.NetworkcanOpenDoor = false;
                controller.upperDoor.NetworkisDoorOpen = false;
                controller.lowerDoor.NetworkisDoorOpen = false;

                // Wait for Doors to Close
                yield return new WaitForSeconds(2f);
                controller.NetworkisMoving = true;

                // Get Target Pos
                float targetY = _requestedFloor == _upperFloor
                    ? upperFloorY
                    : lowerFloorY;
                Vector3 targetPos = controller.transform.position;
                targetPos.y = targetY;

                // Move Elevator Toward Floor
                float distance = 1f;
                while (distance >= 0.001f)
                {
                    Vector3 firstPos = controller.transform.position;
                    distance = Vector3.Distance(firstPos, targetPos);

                    controller.currentElevatorSpeed = Mathf.Lerp(
                        controller.currentElevatorSpeed, 
                        Mathf.Clamp(distance, -_speed, _speed),
                        _speed * Time.deltaTime);

                    controller.transform.position = 
                        Vector3.MoveTowards(firstPos, targetPos, (controller.currentElevatorSpeed * Time.deltaTime));

                    yield return null;
                }

                // Wait for Stop
                //yield return new WaitForSeconds(1f);
                controller.NetworkisMoving = false;
                MelonMain._logger.Msg("Opening Doors...");

                // Check Upper Floor Door
                if (_requestedFloor == _upperFloor)
                {
                    controller.upperDoor.NetworkcanOpenDoor = true;
                    controller.upperDoor.NetworkisDoorOpen = true;
                    controller.upperDoor.RequestOpenDoor();
                }

                // Check Lower Floor Door
                if (_requestedFloor == _lowerFloor)
                {
                    controller.lowerDoor.NetworkcanOpenDoor = true;
                    controller.lowerDoor.NetworkisDoorOpen = true;
                    controller.lowerDoor.RequestOpenDoor();
                }

                // Delay until Next Request
                yield return new WaitForSeconds(3f);

                // Apply Floor
                MelonMain._logger.Msg($"Moved to Floor: {_requestedFloor}");
                _currentFloor = _requestedFloor;
                yield return null;
            }

            // End Coroutine
            yield break;
        }
    }
}