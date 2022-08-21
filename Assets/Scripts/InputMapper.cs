using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputMapper {

    private static Dictionary<string, string> mac = new Dictionary<string, string> {
        { "A4", "A3" },
        { "A5", "A4" },
        { "B7", "B9" },
        { "B6", "B10" },
        { "B0", "B16" },
        { "B1", "B17" },
        { "B2", "B18" },
        { "B3", "B19" },
    };

    private static Dictionary<string, string> linux = new Dictionary<string, string> {

    };

    public static string Map(string input) {
        if (input[0] == 'K')
            return input;
        if (SystemInfo.operatingSystem[0] == 'M') {//Mac
            string key = input.Substring(2, input.Length - 2);
            return mac.ContainsKey(key) ? input.Substring(0, 2) + mac[key] : input;
        } else if (SystemInfo.operatingSystem[0] == 'L') {//Linux
            string key = input.Substring(2, input.Length - 2);
            return linux.ContainsKey(key) ? input.Substring(0, 2) + linux[key] : input;
        } else {//other(?) and windows
            return input;
        }
    }

}