using UnityEngine;
using UnityEngine.UI;

public class VignetteController : MonoBehaviour
{
    RawImage vignetteImage;
    float vignetteFadeInTime = 10f;
    float vignetteFadeOutTime = 3f;
    float maxVignetteAlpha = 1f;
    bool vignetteFadingIn = false;
    bool vignetteFadingOut = false;

    void Awake()
    {
        vignetteImage = GetComponent<RawImage>();
    }

    void Start()
    {
        SetInvisibleVignette();
        RoombaController.attackedPlayer.AddListener(AttackedPlayer);
        RoombaController.startedChasingPlayer.AddListener(StartedChasingPlayer);
        RoombaController.stoppedChasingPlayer.AddListener(StoppedChasingPlayer);
    }

    void Update()
    {
        if (vignetteFadingIn)
        {
            VignetteFadeIn();
        }
        if (vignetteFadingOut)
        {
            VignetteFadeOut();
        }
    }

    public void AttackedPlayer()
    {
        SetRedVignette();
        StartVignetteFadeIn();
    }

    public void StartedChasingPlayer()
    {
        SetBlackVignette();
        StartVignetteFadeIn();
    }

    public void StoppedChasingPlayer()
    {
        StartVignetteFadeOut();
    }

    public void SetInvisibleVignette()
    {
        Color origColor = vignetteImage.color;
        origColor.a = 0;
        vignetteImage.color = origColor;
    }

    public void StartVignetteFadeIn()
    {
        vignetteFadingIn = true;
        vignetteFadingOut = false;
    }
    
    public void StartVignetteFadeOut()
    {
        vignetteFadingOut = true;
        vignetteFadingIn = false;
    }

    public void SetRedVignette()
    {
        vignetteImage.color = Color.white;
    }

    public void SetBlackVignette()
    {
        vignetteImage.color = Color.black;
    }

    void VignetteFadeIn()
    {
        Color origColor = vignetteImage.color;
        if (origColor.a >= maxVignetteAlpha)
        {
            vignetteFadingIn = false;
            return; 
        }
        origColor.a += 1 / vignetteFadeInTime * Time.deltaTime;
        vignetteImage.color = origColor;
    }
    
    void VignetteFadeOut()
    {
        Color origColor = vignetteImage.color;
        if (origColor.a <= 0)
        {
            vignetteFadingOut = false;
            return;
        }
        origColor.a -= 1 / vignetteFadeOutTime * Time.deltaTime;
        vignetteImage.color = origColor;
    }
}
