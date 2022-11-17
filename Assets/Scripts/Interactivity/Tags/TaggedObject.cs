
using UnityEngine;

namespace FQuest_Alt1
{
    internal class TaggedObject : MonoBehaviour
    {
        [field: Header("Tag - Properties")]
        [field: SerializeField] public  string[] Tags { get; set; }
    }
}