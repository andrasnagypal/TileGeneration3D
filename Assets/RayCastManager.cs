using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nagand
{
    public class RayCastManager : MonoBehaviour
    {
        public Camera MainCamera;

        RaycastHit hit;
        Ray ray;
        int BeginPath=-1, EndPath=-1;
        bool IsPathLaid = false;

        private void Update()
        {
            StartCoroutine(ClickOnItemForPathing());
            
        }

        private IEnumerator ClickOnItemForPathing()
        {
            yield return null;
            if (Input.GetButtonDown("Fire1") && !IsPathLaid)
            {
                Debug.Log("Valami");

                ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log("Valami2");

                    ShowPath onclicked = hit.collider.gameObject.GetComponent<ShowPath>();
                    if (onclicked != null)
                    {
                        Debug.Log("Valami3");
                        BeginPath = onclicked.BeginAndEndPath();

                    }

                }
            }
            yield return null;
            if (Input.GetButtonDown("Fire2") && !IsPathLaid)
            {
                Debug.Log("Valami");

                ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log("Valami2");

                    ShowPath onclicked = hit.collider.gameObject.GetComponent<ShowPath>();
                    if (onclicked != null)
                    {
                        Debug.Log("Valami3");
                        EndPath = onclicked.BeginAndEndPath();

                    }

                }
            }
            yield return null;
            if (BeginPath > -1 && EndPath > -1 && !IsPathLaid)
            {
                IsPathLaid = !IsPathLaid;
                GetComponent<AStarCalculator>().AStarCalc(BeginPath, EndPath);
                BeginPath = -1;
                EndPath = -1;
                StartCoroutine(PathLaidDelay());
            }
        }

        IEnumerator PathLaidDelay()
        {
            yield return new WaitForSeconds(.5f);
            IsPathLaid = !IsPathLaid;
        }
    }
}
