
using ExtEvents;
using UnityEngine;
using UnityEngine.Playables;

namespace FunkyQuest
{
    internal abstract class Activatable : MonoBehaviour
    {
        [Header("ReadOnly")]
        [SerializeField][ReadOnly]  protected   bool HasActivated;

        [field: Header("Activatable")]
        [field: SerializeField]     public      bool                CanActivate { get; set; }
        [SerializeField]            protected   PlayableDirector    OptionalCutscene;
        [SerializeField]            protected   ExtEvent            PreCutscene;
        [SerializeField]            protected   ExtEvent            PostCutscene;

        protected void Activate()
        {
            HasActivated = true;
            PreCutscene?.Invoke();

            if (OptionalCutscene != null)
            {
                void OnDirectorStopped(PlayableDirector director)
                {
                    OptionalCutscene.stopped -= OnDirectorStopped;
                    PostCutscene?.Invoke();
                    HasActivated = false;
                }

                OptionalCutscene.stopped += OnDirectorStopped;
                OptionalCutscene.Play();
            }
            else
            {
                PostCutscene?.Invoke();
                HasActivated = false;
            }
        }
    }
}