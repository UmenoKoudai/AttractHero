using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlay : MonoBehaviour
{
    [SerializeField] ParticleSystem _startEffect;

    public void ParticleStart()
    {
        _startEffect.Play();
    }
}
