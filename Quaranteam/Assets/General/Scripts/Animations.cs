using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public AnimationClip[] animations;
    Animation animation;
    void Start()
    {
        animation = new Animation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play(string animationClipName)
    {
        foreach(AnimationClip anim in animations)
        {
            if (anim.name == animationClipName)
            {
                animation.Play(animationClipName);
            }
        }
    }

    public AnimationClip getAnimation(string name)
    {
        for(int i=0; i<animations.Length; i++)
        {
            if(animations[i].name == name)
            {
                return animations[i];
            }
        }
        return null;
    }

    public float getAnimationClipTime(string name)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i].name == name)
            {
                return animations[i].length;
            }
        }
        return 0;
    }

}
