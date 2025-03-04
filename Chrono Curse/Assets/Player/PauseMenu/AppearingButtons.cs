using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearingButtons : MonoBehaviour
{

    [SerializeField]
    public float fadeInTime;
    public GameObject buttonObj;
    public Button button;

    void OnEnable()
    {
        StartCoroutine(fadeButton(button, true, fadeInTime));
    }

    void OnDisable()
    {
        StopCoroutine(fadeButton(button, false, fadeInTime));
    }

    public void disableButton()
    {
        StartCoroutine(fadeButton(button, false, fadeInTime));
    }

    IEnumerator fadeButton(Button button, bool fadeIn, float duration)
    {
        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0;
            b = 1;
        }
        else
        {
            a = 1;
            b = 0;
        }

        Image buttonImage = button.GetComponent<Image>();

        //Enable both Button, Image and Text components
        if (!button.enabled)
            button.enabled = true;

        if (!buttonImage.enabled)
            buttonImage.enabled = true;

        //For Button None or ColorTint mode
        Color buttonColor = buttonImage.color;

        //For Button SpriteSwap mode
        ColorBlock colorBlock = button.colors;


        //Do the actual fading
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            if (button.transition == Selectable.Transition.None || button.transition == Selectable.Transition.ColorTint)
            {
                buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, alpha);//Fade Traget Image
            }
            else if (button.transition == Selectable.Transition.SpriteSwap)
            {
                ////Fade All Transition Images
                colorBlock.normalColor = new Color(colorBlock.normalColor.r, colorBlock.normalColor.g, colorBlock.normalColor.b, alpha);
                colorBlock.pressedColor = new Color(colorBlock.pressedColor.r, colorBlock.pressedColor.g, colorBlock.pressedColor.b, alpha);

                button.colors = colorBlock; //Assign the colors back to the Button
                buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, alpha);//Fade Traget Image
            }
            else
            {
                Debug.LogError("Button Transition Type not Supported");
            }

            yield return null;
        }

        if (!fadeIn)
        {
            //Disable both Button, Image and Text components
            buttonImage.enabled = false;
            button.enabled = false;
        }
    }
}
