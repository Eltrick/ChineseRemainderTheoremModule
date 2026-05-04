using KeepCoding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CRTPuzzle : MonoBehaviour
{
    private ChineseRemainderTheoremModule _crtModule;

    public int Length { get; private set; }
    public ulong Secret { get; private set; }
    public List<KeyValuePair<ulong, ulong>> ModuliRemainders { get; private set; }

    public void GeneratePuzzle()
    {
        _crtModule = GetComponent<ChineseRemainderTheoremModule>();
        
        Length = _crtModule.GetRandom(4, 9);
        ModuliRemainders = new List<KeyValuePair<ulong, ulong>>();

        ulong[] moduli;
        ulong lcm;
        do
        {
            moduli = Enumerable.Range(2, 99).ToArray().Shuffle().Take(Length).Select(x => (ulong)x).ToArray();
            lcm = moduli.Aggregate((ulong)1, (a, b) => LCM(a, b));
        } while (lcm < 100);

        Secret = _crtModule.GetRandom(moduli.Max() + 1, lcm);

        foreach (ulong m in moduli)
            ModuliRemainders.Add(new KeyValuePair<ulong, ulong>(m, Secret % m));
    }

    private ulong LCM(ulong a, ulong b)
    {
        return a * b / GCD(a, b);
    }

    private ulong GCD(ulong a, ulong b)
    {
        if (b == 0)
            return a;

        return GCD(b, a % b);
    }

    public bool CheckAnswer(ulong x)
    {
        _crtModule.Log($"{x} submitted, verifying...");

        foreach (KeyValuePair<ulong, ulong> mr in ModuliRemainders)
            if (x % mr.Key != mr.Value)
            {
                _crtModule.Log($"{x} % {mr.Key} = {x % mr.Key} != {mr.Value}");
                return false;
            }

        return true;
    }

    public string FormatEquation(int index)
    {
        return $"N % {ModuliRemainders[index].Key} = {ModuliRemainders[index].Value}";
    }

    public void Log()
    {
        _crtModule.Log("------<GENERATION>------");
        
        for (int i = 0; i < ModuliRemainders.Count; i++)
            _crtModule.Log($"Equation: {FormatEquation(i)}");

        _crtModule.Log($"A value satisfying these constraints: {Secret}");
    }
}
