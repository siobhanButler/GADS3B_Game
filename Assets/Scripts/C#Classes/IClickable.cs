using UnityEngine;

public interface IClickable
{
    //Paste into class implementing this interface:
    /*
    public Animation clickAnimation;
    public AudioSource clickAudio;
    public Animation anim => clickAnimation;
    public AudioSource audioSource => clickAudio;

    public void CustomClick() {}
    */

    //public Animation anim { get; }
    //public AudioSource audioSource { get; }

    /// Called when this object is clicked. Plays animation and sound, then calls CustomClick.
    public void OnClick(ClickDetector clicker)
    {
        //anim.Play();
        //audioSource.Play();

        CustomClick(clicker);
    }

    public void CustomClick(ClickDetector clicker);
}
