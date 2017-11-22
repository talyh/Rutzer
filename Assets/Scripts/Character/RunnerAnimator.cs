using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RunnerAnimator : MonoBehaviour
{
    private enum AnimationParameters { Character_Rainbow };
    public enum AnimationLayers { Grey = 0, Red, Yellow, Green, Blue, Purple, Pink };

    [SerializeField]
    private Animator _animator;

    private void Start()
    {
        RunInitialChecks();
    }

    public void ChangeColor(AnimationLayers color)
    {
        int possibleColors = System.Enum.GetValues(typeof(AnimationLayers)).Length;

        // if getting an unkown color, choose one at random, excluding grey
        if ((int)color >= possibleColors)
        {
            color = (AnimationLayers)Random.Range(1, possibleColors);
            Supporting.Log("using random color: " + color.ToString(), 2);
        }

        // loop through animation layers activating the one that corresponded to the current powerUp and deactivating the rest
        // starting at 1 so we don't go back to grey
        for (int i = 1; i < possibleColors; i++)
        {
            if (i == (int)color)
            {
                _animator.SetLayerWeight(i, 1);
            }
            else
            {
                _animator.SetLayerWeight(i, 0);
            }
        }
    }

    public IEnumerator FlashRainbow(int times)
    {
        for (int i = 0; i < times; i++)
        {
            _animator.Play(AnimationParameters.Character_Rainbow.ToString());
            yield return null;
        }
    }

    private void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _animator, "Animator");
    }
}
