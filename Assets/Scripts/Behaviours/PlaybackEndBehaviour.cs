using UnityEngine;
using Ateneo;

public class PlaybackEndBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<IntroManager>().onPlaybackStateExit();
    }
}
