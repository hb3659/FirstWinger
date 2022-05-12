using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoCachableEffect : MonoBehaviour
{
    public string FilePath
    {
        get; set;
    }

    public IEnumerator effectCoroutine;

    public void OnEnable()
    {
        if (effectCoroutine == null)
            effectCoroutine = CheckAlive();

        StartCoroutine(effectCoroutine);
    }

    public IEnumerator CheckAlive()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);

            if (!GetComponent<ParticleSystem>().IsAlive(true))
            {
                //SystemManager.Instance.EffectMng.RemoveEffect(this);
                SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectMng.RemoveEffect(this);
                break;
            }
        }
    }
}
