using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Resources.Scripts { 
public class SelectorRay : MonoBehaviour
{
    public string TargetTag;
    public Transform MyCamera;
    public GameObject _buttons;
    Ray _ray;
    private ClickButton _button;
    public Camera Cam;
    RaycastHit _hit;
    private Button[] _clickbuttons;
        // Update is called once per frame
        void Update()
        {
            if (_buttons != null)
            {
                _clickbuttons = _buttons.GetComponentsInChildren<Button>();
                foreach (Button button in _clickbuttons)
                {
                    button.GetComponent<ClickButton>().IsHovering = false;
                }
                _ray = new Ray(MyCamera.position, MyCamera.TransformDirection(Vector3.forward));

                if (Physics.Raycast(_ray, out _hit) && _hit.collider.tag.Equals(TargetTag))
                {
                    
                    
                    _button = _hit.collider.gameObject.GetComponent<ClickButton>();
                    _button.IsHovering = true;
                }
              
            }
        }
	}
}
