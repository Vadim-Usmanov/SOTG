using UnityEngine;

public class RandomOffsetAnimation : StateMachineBehaviour
{
    private bool _hasRandomized;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_hasRandomized)
        {
            animator.Play(stateInfo.fullPathHash, layerIndex, Random.Range(-0f, 3f));
            _hasRandomized = true;
        }
    }
}
