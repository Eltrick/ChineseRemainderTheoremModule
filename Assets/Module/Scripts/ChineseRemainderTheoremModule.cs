using KeepCoding;
using System;
using UnityEngine;
using Rnd = UnityEngine.Random;

[RequireComponent(typeof(KMBombModule), typeof(KMSelectable))]
public partial class ChineseRemainderTheoremModule : ModuleScript
{
    private KMBombModule _module;

#pragma warning disable CS0649
    [SerializeField]
    private KMSelectable[] _buttons;

    [SerializeField]
    private KMSelectable _clearButton, _submitButton;

    [SerializeField]
    private TextMesh _inputText, _equationText, _indexText;
#pragma warning restore CS0649

    private CRTPuzzle _puzzle;
    private int _internalIndex;
    private ulong _submitted;

    private bool _isSeedSet, _hasModInitialised;
    private int _seed;
    private System.Random _rnd;

    private bool _isModuleSolved;

    public int GetRandom(int low, int high)
    {
        if (low > high)
            throw new ArgumentException("Low bound cannot exceed high bound.");

        return _rnd.Next(low, high);
    }

    public ulong GetRandom(ulong low, ulong high)
    {
        if (low > high)
            throw new ArgumentException("Low bound cannot exceed high bound.");

        ulong rand, urange = high - low;
        byte[] buf = new byte[8];
        do
        {
            _rnd.NextBytes(buf);

            rand = BitConverter.ToUInt64(buf, 0);
        } while (rand > ulong.MaxValue - ((ulong.MaxValue % urange) + 1) % urange);

        return rand % urange + low;
    }

#pragma warning disable IDE0051
    private new void Awake()
    {
        base.Awake();
        _module = GetComponent<KMBombModule>();
        _module.OnActivate += Activate;
        _puzzle = GetComponent<CRTPuzzle>();
    }

    private void Start()
    {
        if (!_isSeedSet)
        {
            _seed = Rnd.Range(int.MinValue, int.MaxValue);
            Log("The seed is: " + _seed.ToString());
            _isSeedSet = true;
        }

        _rnd = new System.Random(_seed);
        // SET SEED ABOVE IN CASE OF BUGS!!
        // _rnd = new System.Random(loggedSeed);

        _clearButton.Assign(onInteract: () => ClearInput());
        _submitButton.Assign(onInteract: () => SubmitPress());

        for (int i = 0; i < _buttons.Length; i++)
        {
            int x = i;
            _buttons[x].Assign(onInteract: () => TryAppendDigit(x));
        }
    }

    private void Activate()
    {
        _puzzle.GeneratePuzzle();
        _puzzle.Log();

        _hasModInitialised = true;

        ClearInput();
        SetDisplays(_internalIndex);
    }

    private void SetDisplays(int index)
    {
        _indexText.text = (index + 1).ToString();
        _equationText.text = _puzzle.FormatEquation(index);
    }

    private void TryAppendDigit(int d)
    {
        ButtonEffect(_buttons[d], .1f, KMSoundOverride.SoundEffect.ButtonPress);

        if (_isModuleSolved || _submitted.ToString().Length >= 16 || !_hasModInitialised)
            return;

        _submitted *= 10;
        _submitted += (ulong)d;

        _inputText.text = (_submitted % 100000000000).ToString();
    }

    private void ClearInput()
    {
        ButtonEffect(_clearButton, .1f, KMSoundOverride.SoundEffect.ButtonPress);

        if (_isModuleSolved || !_hasModInitialised)
            return;

        _submitted = 0;
        _inputText.text = "";
    }

    private void SubmitPress()
    {
        ButtonEffect(_submitButton, .1f, KMSoundOverride.SoundEffect.ButtonPress);

        if (_isModuleSolved || !_hasModInitialised)
            return;

        if (_inputText.text == "")
        {
            _internalIndex++;
            _internalIndex %= _puzzle.Length;
            SetDisplays(_internalIndex);
            return;
        }

        if (_puzzle.CheckAnswer(_submitted))
            Solve();
        else
            Strike("Constraint unsatisfied. Strike!");
    }
#pragma warning restore IDE0051

    public void Strike(string message)
    {
        Log($"{message}");
        _module.HandleStrike();
        ClearInput();
    }

    public void Solve()
    {
        _isModuleSolved = true;
        Log($"{_submitted} satisfies all equations. Module solved!");

        PlaySound(KMSoundOverride.SoundEffect.CorrectChime);
        _module.HandlePass();

        _equationText.text = "";
        _indexText.text = "✓";
    }
}
