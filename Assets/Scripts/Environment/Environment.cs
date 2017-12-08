using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField]
    private Sprite _dayBackground;
    [SerializeField]
    private Sprite _nightBackground;
    [SerializeField]
    private ParticleSystem _wind;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        RunInitialChecks();

        GameController.halfDay += SwapBackground;
    }

    private void Start()
    {
        BlowWind(false);
    }

    public void SwapBackground()
    {
        if (_renderer.sprite == _dayBackground)
        {
            _renderer.sprite = _nightBackground;
            BlowWind(true);
        }
        else if (_renderer.sprite == _nightBackground)
        {
            _renderer.sprite = _dayBackground;
            BlowWind(false);
        }
    }

    private void RunInitialChecks()
    {
        _renderer = GetComponent<SpriteRenderer>();

        Supporting.CheckRequiredProperty(gameObject, _renderer, "Spirte Renderer");
        Supporting.CheckRequiredProperty(gameObject, _wind, "Wind");
    }

    private void BlowWind(bool active)
    {
        _wind.gameObject.SetActive(active);
    }

    private void OnDisable()
    {
        GameController.halfDay -= SwapBackground;
    }
}
