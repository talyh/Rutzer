using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RunnerAnimator : MonoBehaviour
{
    private enum AnimationParameters { Dead, Speed };
    private enum AnimationClips { Character_Rainbow };
    public enum AnimationLayers { Grey = 0, Red, Yellow, Green, Blue, Purple, Pink };

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private ParticleSystem _particles;
    [SerializeField]
    private Color32 _grey;
    [SerializeField]
    private Color32 _red;
    [SerializeField]
    private Color32 _yellow;
    [SerializeField]
    private Color32 _green;
    [SerializeField]
    private Color32 _blue;
    [SerializeField]
    private Color32 _purple;
    [SerializeField]
    private Color32 _pink;

    private void Start()
    {
        RunInitialChecks();
        AdjustSpeed();
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
        for (int i = 0; i < possibleColors; i++)
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

        ChangeParticlesColor(color);
    }

    private void ChangeParticlesColor(AnimationLayers color)
    {
        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = _particles.colorOverLifetime;
        colorOverLifetime.enabled = true;

        Color32 newColor;

        switch (color)
        {
            case AnimationLayers.Grey:
            default:
                {
                    newColor = _grey;
                    break;
                }
            case AnimationLayers.Red:
                {
                    newColor = _red;
                    break;
                }
            case AnimationLayers.Yellow:
                {
                    newColor = _yellow;
                    break;
                }
            case AnimationLayers.Green:
                {
                    newColor = _green;
                    break;
                }
            case AnimationLayers.Blue:
                {
                    newColor = _blue;
                    break;
                }
            case AnimationLayers.Purple:
                {
                    newColor = _purple;
                    break;
                }
            case AnimationLayers.Pink:
                {
                    newColor = _pink;
                    break;
                }
        }

        Gradient gradient = new Gradient();
        gradient.SetKeys(new GradientColorKey[] {
            new GradientColorKey(newColor, 0.5f),
            new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.5f),
                new GradientAlphaKey(1.0f, 1.0f) });

        colorOverLifetime.color = gradient;
    }

    public IEnumerator FlashRainbow(int times)
    {
        for (int i = 0; i < times; i++)
        {
            _animator.Play(AnimationClips.Character_Rainbow.ToString());
            yield return null;
        }
    }

    public void AdjustSpeed()
    {
        _animator.SetFloat(AnimationParameters.Speed.ToString(), GameController.instance.speed);
    }

    public void Die()
    {
        _animator.SetTrigger(AnimationParameters.Dead.ToString());
    }

    private void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _animator, "Animator");
        Supporting.CheckRequiredProperty(gameObject, _particles, "Particle System");
    }
}
