using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClipName
{
    /// <summary>
    /// 野猪攻击音效.
    /// </summary>
    BoarAttack,
    /// <summary>
    /// 野猪死亡音效.
    /// </summary>
    BoarDeath,
    /// <summary>
    /// 野猪受伤音效.
    /// </summary>
    BoarInjured,
    /// <summary>
    /// 丧尸攻击音效.
    /// </summary>
    ZombieAttack,
    /// <summary>
    /// 丧尸死亡音效.
    /// </summary>
    ZombieDeath,
    /// <summary>
    /// 丧尸受伤音效.
    /// </summary>
    ZombieInjured,
    /// <summary>
    /// 丧尸尖叫音效.
    /// </summary>
    ZombieScream,
    /// <summary>
    /// 子弹命中地面音效.
    /// </summary>
    BulletImpactDirt,
    /// <summary>
    /// 子弹命中身体音效.
    /// </summary>
    BulletImpactFlesh,
    /// <summary>
    /// 子弹命中金属音效.
    /// </summary>
    BulletImpactMetal,
    /// <summary>
    /// 子弹命中石头音效.
    /// </summary>
    BulletImpactStone,
    /// <summary>
    /// 子弹命中木材音效.
    /// </summary>
    BulletImpactWood,
    /// <summary>
    /// 玩家角色急促呼吸声.
    /// </summary>
    PlayerBreathingHeavy,
    /// <summary>
    /// 玩家角色受伤音效.
    /// </summary>
    PlayerHurt,
    /// <summary>
    /// 玩家角色死亡音效.
    /// </summary>
    PlayerDeath,
    /// <summary>
    /// 身体命中音效.
    /// </summary>
    BodyHit
}


public class AudioManager : MonoBehaviour 
{
    public static AudioManager Instance;

    public AudioClip[] audioClips;
    public Dictionary<string, AudioClip> audioDic;

    private void Awake()
    {
        Instance = this;

        audioDic = new Dictionary<string, AudioClip>();
        audioClips = Resources.LoadAll<AudioClip>("Audios/All/");
        for(int i = 0; i < audioClips.Length; i++)
        {
            audioDic.Add(audioClips[i].name, audioClips[i]);
        }
    }


    /// <summary>
    ///  通过名称获取音频资源
    /// </summary>
    public AudioClip GetAudioClipByName(ClipName name)
    {
        AudioClip res = null;
        audioDic.TryGetValue(name.ToString(), out res);
        return res;
    }


    /// <summary>
    ///  通过名称和位置在指定位置播放音频资源
    /// </summary>
    public void PlayAudioClipByName(ClipName name,Vector3 pos)
    {
        AudioClip res = GetAudioClipByName(name);
        AudioSource.PlayClipAtPoint(res, pos);
    }


    /// <summary>
    ///  给某一个物体添加组件并进行操作
    /// </summary>
    public AudioSource AddAudioClipComponentToGameObject(GameObject obj, ClipName name, 
        bool isLoop = true,bool playOnAwake = true)
    {
        
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClipByName(name);

        audioSource.loop = isLoop;
        if(playOnAwake)
        {
            audioSource.playOnAwake = playOnAwake;
            audioSource.Play();
        }
        return audioSource;
    }


}
