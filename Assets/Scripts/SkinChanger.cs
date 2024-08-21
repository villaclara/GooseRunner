using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    public SkinAnimator[] skins;
    private int _skinIndex;
    public GameObject character;
    // Start is called before the first frame update
    SpriteRenderer characterSpriteRenderer;
    private AnimatorOverrideController overrideController;
    void Awake()
    {
        _skinIndex = PlayerPrefs.GetInt("SkinIndex");
        characterSpriteRenderer = character.GetComponent<SpriteRenderer>();
        characterSpriteRenderer.sprite = skins[_skinIndex].sprite;
        Animator characterAnimator = character.GetComponent<Animator>();
        for(int i = 0; i< skins.Length; i++)
        {
            characterAnimator.SetBool(skins[i].triggerName, false);
        }
        characterAnimator.SetBool(skins[_skinIndex].triggerName, true);

    }
}

[System.Serializable]
public class SkinAnimator
{
    public Sprite sprite;
    public AnimationClip animationClip;
    public string triggerName;
}
