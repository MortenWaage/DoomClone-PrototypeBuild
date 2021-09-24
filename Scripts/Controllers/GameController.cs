using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public LayerMask LayerMaskGround;
    public LayerMask LayerMaskAttackable;
    public LayerMask TestLayer;

    [HideInInspector] public W_ImpactEffects w_effects;

    [SerializeField] [Range(0, 1f)] float volume = 0.5f;

    RNG rng;
    public RNG Rntable { get => rng; private set { } }
    public static GameController Instance { get; private set; }

    public GameObject DoomGuy;
    public RawImage weaponImage;
    public DebugInfo debug;
    public Inputs input;

    public InterfaceController Interface;


    public IScreenGlow screenGlowHurt;
    public IPortrait portrait;

    [SerializeField] GameObject player_spawn_1;

    public DemonScriptableObjectsList demons;

    public GameObject SpawnPoint { get; private set; }

    void Awake()
    {
        w_effects = GetComponent<W_ImpactEffects>();

        rng = new RNG();

        input = GetComponent<Inputs>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SpawnPoint = player_spawn_1;
        //SpawnPoint = player_spawn_1.transform.position;

        MeshRenderer player_spawn_1_rend = player_spawn_1.GetComponent<MeshRenderer>();
        player_spawn_1_rend.enabled = false;

        debug = GetComponentInChildren<DebugInfo>();
        demons = GetComponentInChildren<DemonScriptableObjectsList>();
        screenGlowHurt = GetComponentInChildren<IScreenGlow>();
        portrait = GetComponentInChildren<IPortrait>();

        Interface = GetComponentInChildren<InterfaceController>();

    }

    public void PlayScreenGlow(int damage)
    {
        screenGlowHurt.PlayScreenGlow(damage);
    }
    private void OnValidate()
    {
        AudioListener.volume = volume;
    }
}
