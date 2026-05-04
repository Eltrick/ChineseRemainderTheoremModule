using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;

#pragma warning disable IDE1006
partial class ChineseRemainderTheoremModule
{
#pragma warning disable 414, IDE0051
    private readonly string TwitchHelpMessage = @"<!{0} submit 381654729> to submit 381654729 as your answer. <!{0} cycle> to cycle through all modular equations. Numbers must at most be 16 digits long.";
#pragma warning restore 414, IDE1006

    private IEnumerator ProcessTwitchCommand(string command) {
        // * Setup for implementing TP using regular expressions for command validation.
        // ! Requires importing the System.Text.RegularExpressions namespace.
        command = command.Trim().ToUpperInvariant();

        if (Regex.IsMatch(command, @"^cycle$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
        {
            for (int i = 0; i < _puzzle.Length; i++)
            {
                yield return new WaitForSeconds(1.5f);
                _submitButton.OnInteract();
            }

            yield break;
        }

        Match m = Regex.Match(command, @"^submit ([0-9]{1,16})$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        if (m.Success)
        {
            foreach (char c in m.Groups[1].Value.ToCharArray())
            {
                yield return new WaitForSeconds(.05f);
                _buttons[c - '0'].OnInteract();
                yield return new WaitForSeconds(.05f);
            }

            _submitButton.OnInteract();
            yield break;
        }

        yield return "sendtochaterror No valid command formats detected.";
    }

    private IEnumerator TwitchHandleForcedSolve() {
        yield return null;

        if (_inputText.text != "")
            _clearButton.OnInteract();

        foreach (char c in _puzzle.Secret.ToString().ToCharArray())
        {
            _buttons[c - '0'].OnInteract();
            yield return new WaitForSeconds(.1f);
        }

        _submitButton.OnInteract();
    }
#pragma warning restore IDE0051
}