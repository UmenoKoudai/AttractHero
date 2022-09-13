using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] ParticleSystem _particle;
    public void AnimaPlay(string AnimName)
    {
        _anim.Play(AnimName);
    }
    public void ParticlePlay()
    {
        _particle.Play();
    }
}
