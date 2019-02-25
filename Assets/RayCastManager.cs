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
        ShowPath Beginning, Ending;

        private void Update()
        {
            StartCoroutine(ClickOnItemForPathing());
            
        }

        private IEnumerator ClickOnItemForPathing()
        {
            yield return null;
            if (Input.GetButtonDown("Fire1") && !IsPathLaid)
            {
                

                ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                   

                    ShowPath onclicked = hit.collider.gameObject.GetComponent<ShowPath>();
                    if (onclicked != null)
                    {
                        
                        
                       
                        if (Beginning!=null)
                            Beginning.TurnBackToOriginalColor();
                            Beginning = onclicked;
                        BeginPath = onclicked.BeginAndEndPath();
                    }

                }
            }
            yield return null;
            if (Input.GetButtonDown("Fire2") && !IsPathLaid)
            {
                

                ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    

                    ShowPath onclicked = hit.collider.gameObject.GetComponent<ShowPath>();
                    if (onclicked != null)
                    {
                       
                                               
                            if (Ending!=null)
                            Ending.TurnBackToOriginalColor();
                            Ending = onclicked;
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
