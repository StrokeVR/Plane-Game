using System.Collections;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class PlaneScript : MonoBehaviour
    {
        private GameObject _landing;
        private GameObject _hand;
        private int _grabbableLayer;
        public Level Currlevel;

        public void Start()
        {
            
            Currlevel = GameObject.Find("PlaneLevel").GetComponent<Level>();
            // setting Layer to Default, to be changed to grabbable once the plane is in its start position on the table
            gameObject.layer = 0;
            // random vibrant color for new plane
            Color randColor = Random.ColorHSV(0f,1f,80f,100f,0f,0.5f); //to change
            int planeType = PlaneColorSelection(randColor);
            if(planeType == 0)
            {
                _hand = GameObject.Find("Oculus_Hand_Right");
                _hand.GetComponent<OvrAvatarRightHand>().SetReleasedToFalse();
            }
            else if (planeType == 1)
            {
                _hand = GameObject.Find("Oculus_Hand_Left");
                _hand.GetComponent<OvrAvatarLeftHand>().SetReleasedToFalse();                  
            }
        }
        // setting plane color and its spawnpoint
        int PlaneColorSelection(Color randColor)
        {
            if (gameObject.name == "plane(Clone)")
            {
                // for plane clones, set the spawnpoint and layer to the right/rightgrabbable, and assign a random color to the wings
                // high saturation results in vibrant colors
                _landing = GameObject.Find("landing2");
                _grabbableLayer = 9;
                Renderer wings = gameObject.GetComponentInChildren<Renderer>();
                wings.material.SetColor("_Color", randColor);
                return 0;
            }
            else if(gameObject.name == "jet(Clone)")
            {
                // for jet plane clones, set the spawnpoint and layer to the left/left grabbable, and assign a 
                // random color to the wings and front cone (object347, object351, and wings001)
                // high saturation results in vibrant colors
                randColor = Random.ColorHSV(0f, 1f, 80f, 100f, 0f, 1f);
                _landing = GameObject.Find("landing1");
                _grabbableLayer = 8;
                Renderer[] wingsandFront = gameObject.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < wingsandFront.Length; i++)
                {
                    if (wingsandFront[i].tag == "Jet")
                    {
                        wingsandFront[i].material.color = randColor;
                    }
                }
                return 1;
            }
            return -1;
        }
        // to change, replace with animation (?)
        void Update()
        {
            // if the plane is within 3 seconds old, move to table position
            if (gameObject.layer == 0 && transform.position.y < _landing.transform.position.y - .01)
            {
                transform.position = Vector3.Lerp(transform.position, _landing.transform.position, Time.deltaTime);
            }
            else
            {
                gameObject.layer = _grabbableLayer;
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            // plane goes through hoop
            if (other.gameObject.CompareTag("Hoop"))
            {
                // change hoop layer to prevent another plane from tracking it
                other.gameObject.tag = "Untagged";
                // plane drops, to change to animation
                GetComponent<Rigidbody>().useGravity = true;
                // a pause before hoop drops
                StartCoroutine(HoopDrop(other));
                Currlevel.DecrementInteractables(gameObject);
                Currlevel.IncrementScore();
                
            }
            // plane goes out of bounds
            if (other.gameObject.CompareTag("Ground"))
            {
                
                Currlevel.DecrementInteractables(gameObject);
                _landing.GetComponent<Spawn>().DecrementPrefab();
                Destroy(gameObject);
            }
            if (other.gameObject.CompareTag("Collider"))
            {
                if (_hand.name == "hand_right")
                {
                    _hand.GetComponent<OvrAvatarRightHand>().SetExtendedToTrue();
                }
                else
                {
                    _hand.GetComponent<OvrAvatarLeftHand>().SetExtendedToTrue();
                }
            }
        }
        private IEnumerator HoopDrop(Collider other)
        {
            // wait 1.5 seconds before dropping hoop
            yield return new WaitForSeconds(.7F);
            other.GetComponent<HoopScript>()._willOscillate = false;
            other.GetComponentInParent<Rigidbody>().useGravity = true;
        }
    }
}
