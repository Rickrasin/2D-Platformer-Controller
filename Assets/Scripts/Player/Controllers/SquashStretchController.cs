using UnityEngine;

public class SquashStretchController : MonoBehaviour
{
    private Player player;
    
    private Animator AnimHeight;

    

    private void Start()
    {
        player = GetComponentInParent<Player>();
        AnimHeight = GetComponent<Animator>();

    }

   public void setJumpSquashStretch(bool value)
    {
        AnimHeight.SetBool("jump", value);
    }

    public void setLandSquashStretch(bool value)
    {
        AnimHeight.SetBool("land", value);
    }




    #region Animation Triggers

    public void StopJumpSquashStretch() => AnimHeight.SetBool("jump", false);

    public void StopLandSquashStretch() => AnimHeight.SetBool("land", false);
    #endregion


}
