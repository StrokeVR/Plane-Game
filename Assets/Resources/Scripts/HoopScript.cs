using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class HoopScript : MonoBehaviour
    {
        private bool _willFloat;
        private bool _willOscillate;
        private int _ageInFrames;
        private GameObject _spawnPoint;
        private Color _hoopColor;
        private GameObject _player;

        private Vector3 _pos1;
        private Vector3 _pos2;
        private float speed = 0.5f;
        void Start()
        {
            _ageInFrames = 0;
            _player = GameObject.Find("OVRPlayerController");
            _spawnPoint = GameObject.Find("hoopspawnpoint");
            Spawn spawn = _spawnPoint.GetComponent<Spawn>();
            _willOscillate = spawn.WillOscillate;
            _willFloat = spawn.WillFloat;
            _hoopColor = Random.ColorHSV(0f, 1f, 80f, 100f, 0f, 1f);
            this.gameObject.GetComponent<Renderer>().material.color = _hoopColor;
            transform.LookAt(_player.transform);
            _pos1 = transform.position + new Vector3(0f, 0f, 1);
            _pos2 = transform.position + new Vector3(0f, 0f, -1);
        }

        void Update()
        {
            if (_willFloat)
            {
                if (_ageInFrames < 150)
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0f, .5f, 0f), Time.deltaTime);
                }
                if (transform.position.y < -10)
                {
                    _spawnPoint.GetComponent<Spawn>().DecrementPrefab();
                    Destroy(gameObject);
                }
                _ageInFrames++;
            }
            else if (_willOscillate)
            {
                transform.position = Vector3.Lerp(_pos1, _pos2, Mathf.PingPong(Time.time * speed, 1.0f));
            }
        }
    }
}
