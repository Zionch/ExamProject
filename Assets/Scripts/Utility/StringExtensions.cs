using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static partial class ExtensionMethods
{
    public static int GetASCIILength(this string str) {
        ASCIIEncoding ascii = new ASCIIEncoding();

        byte[] s = ascii.GetBytes(str);
        int count = 0;
        for (int i = 0; i < s.Length; i++) {
            if (s[i] == 63) {
                count += 2;
            } else {
                count++;
            }
        }

        return count;
    }

    public static string InsertNewlineEveryStringLength(this string str, int length, bool respectExistedNewline = false) {
        str = Regex.Replace(str, @"\r\n?|\n", Environment.NewLine);

        StringBuilder sb = new StringBuilder();
        int count = 0;

        bool ExistOrgNewline = false;
        int nextExistedNewlineIndex = 0;
        if (str.Contains(Environment.NewLine)) {
            ExistOrgNewline = true;
            nextExistedNewlineIndex = str.IndexOf(Environment.NewLine, nextExistedNewlineIndex);
        }

        for (int i = 0; i < str.Length;) {
            if (respectExistedNewline && ExistOrgNewline && i == nextExistedNewlineIndex) {
                if (nextExistedNewlineIndex + 1 < str.Length) {
                    nextExistedNewlineIndex = str.IndexOf(Environment.NewLine, nextExistedNewlineIndex + 1);
                }
                sb.Append(Environment.NewLine);
                count = 0;//reset
                i += Environment.NewLine.Length;
                continue;
            }
            sb.Append(str[i]);


            ASCIIEncoding ascii = new ASCIIEncoding();

            byte[] s = ascii.GetBytes(new char[] { str[i] });
            count += s[0] == 63 ? 2 : 1;

            if (count >= length && i != str.Length - 1) {
                count = 0;
                sb.Append(Environment.NewLine);
            }

            i++;
        }

        str = sb.ToString();

        if (str.EndsWith(Environment.NewLine + "！") || str.EndsWith(Environment.NewLine + "。") || str.EndsWith(Environment.NewLine + "～")) {
            int i = str.LastIndexOf(Environment.NewLine);
            str = str.Substring(0, i) + str.Substring(i + Environment.NewLine.Length);
        }

        return str;
    }

    public static string ReadLine(this string rawString, ref int position) {
        if (position < 0) {
            return null;
        }

        int length = rawString.Length;
        int offset = position;
        while (offset < length) {
            char ch = rawString[offset];
            switch (ch) {
                case '\r':
                case '\n':
                if (offset > position) {
                    string line = rawString.Substring(position, offset - position);
                    position = offset + 1;
                    if ((ch == '\r') && (position < length) && (rawString[position] == '\n')) {
                        position++;
                    }

                    return line;
                }

                offset++;
                position++;
                break;

                default:
                offset++;
                break;
            }
        }

        if (offset > position) {
            string line = rawString.Substring(position, offset - position);
            position = offset;
            return line;
        }

        return null;
    }
}