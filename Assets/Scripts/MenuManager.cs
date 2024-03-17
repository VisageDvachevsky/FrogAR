using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool isMenuOpen = true;
    public Animator menuAnimator;

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuAnimator.SetBool("isMenuOpen", isMenuOpen);
    }
}
