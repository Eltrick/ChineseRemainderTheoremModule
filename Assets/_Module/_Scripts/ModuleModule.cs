// using KModkit; // You must import this namespace to use KMBombInfoExtensions, among other things. See KModKit Docs below.
using KeepCoding;
using UnityEngine;

[RequireComponent(typeof(KMBombModule), typeof(KMSelectable))]
public partial class ModuleModule : ModuleScript
{
    // private KMBombInfo _bombInfo;
    // private KMAudio _audio;
    private KMBombModule _module;

    private static int s_moduleCount;
    private int _moduleId;

#pragma warning disable IDE0051
    // Called before anything else.
    private void Awake()
    {
        _moduleId = s_moduleCount++;

        _module = GetComponent<KMBombModule>();
        // _bombInfo = GetComponent<KMBombInfo>(); // (*)
        // _audio = GetComponent<KMAudio>();

        _module.OnActivate += Activate;
        // _bombInfo.OnBombExploded += OnBombExploded; // (**). Requires (*)
        // _bombInfo.OnBombSolved += OnBombSolved; // (***). Requires (*)

        // Declare other references here if needed.
    }

    // Called after Awake has been called on all components in the scene, but before anything else.
    // Things like querying edgework need to be done after Awake is called, eg. subscribing to OnInteract events.
    private void Start()
    {
        Log("foo");
    }

    // Called once the lights turn on.
    private void Activate()
    {

    }

    // Called every frame if the MonoBehaviour is enabled.
    // Time.deltaTime returns the time elapsed from the last Update call
    // https://docs.unity3d.com/2017.4/Documentation/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {

    }

    // Frame-rate independent, modify Time.fixedDeltaTime to change behaviour
    // https://docs.unity3d.com/2017.4/Documentation/ScriptReference/MonoBehaviour.FixedUpdate.html
    private void FixedUpdate()
    {

    }

    // Called when the module is removed from the game world.
    // Examples of when this happens include when the bomb explodes, or if the player quits to the office.
    private void OnDestroy()
    {

    }
#pragma warning restore IDE0051

    // private void OnBombExploded() { }
    // private void OnBombSolved() { }

    public void Strike(string message)
    {
        Log($"✕ {message}");
        _module.HandleStrike();
        // Add code that should execute on every strike (eg. a strike animation) here.
    }

    public void Solve()
    {
        Log("◯ Module solved!");
        _module.HandlePass();
        // Add code that should execute on solve (eg. a solve animation) here.
    }
}
