using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class Spawn : MonoBehaviour {
        public bool WillFloat;
        public bool WillOscillate = Data.willOscillate;
        public int CurrentInteractables;
        public GameObject Prefab;
        public Vector3 Randomvector;
        public Vector3 Offsetvector;

        [Range(0, 10)]
        public float R;

        public Spawn()
        {
            R = 1f;
            
        }
        
        
        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, R);
        }

        public virtual GameObject SpawnPrefab()
        {
            Vector3 difficultyOffset;
            if (Data.difficulty <= 1)
            {
                float invertedZ = 1 - Data.difficulty;
                 difficultyOffset = new Vector3(0, 0, -invertedZ * 2);
            }
            else
            {
                 difficultyOffset = new Vector3(0, 0, Data.difficulty / 1.5f);
            }
            
            Randomvector = new Vector3(Random.Range(-R, R), -.5f, Random.Range(0.3f, -0.3f));
            CurrentInteractables++;
            if (Prefab.gameObject.name.Equals("hoop"))
            {
                return Instantiate(Prefab, (transform.position + Randomvector + Offsetvector) + difficultyOffset, Quaternion.identity);

            }
            else
            {
                return Instantiate(Prefab, transform.position + Randomvector + Offsetvector, Quaternion.identity);

            }
        }
        public virtual void DecrementPrefab()
        {
            CurrentInteractables--;
        }
    }
}
