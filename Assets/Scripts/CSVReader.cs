using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine.Networking;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static Dictionary<int, Dictionary<int, Dictionary<StageHeader, object>>> ReadStageData()
    {
        var result = new Dictionary<int, Dictionary<int, Dictionary<StageHeader, object>>>();
        // <chapter, <stage, data>>
        try
        {
            // csv 파일을 읽어 와 라인 별로 끊어서 array 생성
            TextAsset data = Resources.Load("Data/Stage") as TextAsset;
            var lines = Regex.Split(data.text, LINE_SPLIT_RE);
            if (lines.Length <= 1) throw new Exception("lines.Length <= 1");

            var headerStr = Regex.Split(lines[0], SPLIT_RE);
            List<StageHeader> header = new List<StageHeader>();

            foreach (string h in headerStr)
            {
                if (Enum.TryParse(h, true, out StageHeader headerEnum))
                    header.Add(headerEnum);
                else throw new Exception($"StageHeader Error [{h}]");
            }

            for (int i = 1; i < lines.Length-1; i++)
            {
                // 일단 각 줄의 data를 하나씩 끊어 
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length <= 1) throw new Exception($"values[{i}].Length <= 1");

                // 각각의 값을 key와 value 쌍으로 집어 느
                var entry = new Dictionary<StageHeader, object>();
                for (int j = 2; j < header.Count && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                        finalvalue = n;
                    else if (float.TryParse(value, out f))
                        finalvalue = f;

                    entry[header[j]] = finalvalue;
                }

                if (int.TryParse(values[0], out int chapter))
                {
                    if (!result.ContainsKey(chapter))
                        result[chapter] = new Dictionary<int, Dictionary<StageHeader, object>>();
                    if (int.TryParse(values[1], out int stage))
                    {
                        if (result[chapter].ContainsKey(stage)) 
                            throw new Exception($"stage[{stage}] is already exists");
                        result[chapter][stage] = entry;
                    }
                    else throw new Exception($"Stage Error (stage == {values[1]})");
                }
                else throw new Exception($"Chapter Error (stage == {values[0]})");
            }

            return result;
        }
        catch(Exception e)
        {
            Debug.LogError($"Read Stage Data 오류: {e.Message}");
            return null;
        }
    }

    public static Dictionary<int, Dictionary<int, List<Dictionary<CatHeader, object>>>> ReadCatData()
    {
        var result = new Dictionary<int, Dictionary<int, List<Dictionary<CatHeader, object>>>>();
        // <chapter, <stage, data>>
        try
        {
            // csv 파일을 읽어 와 라인 별로 끊어서 array 생성
            TextAsset data = Resources.Load("Data/Cat") as TextAsset;
            var lines = Regex.Split(data.text, LINE_SPLIT_RE);
            if (lines.Length <= 1) throw new Exception("lines.Length <= 1");

            var headerStr = Regex.Split(lines[0], SPLIT_RE);
            List<CatHeader> header = new List<CatHeader>();

            foreach (string h in headerStr)
            {
                if (Enum.TryParse(h, true, out CatHeader headerEnum))
                    header.Add(headerEnum);
                else throw new Exception($"CatHeader Error [{h}]");
            }

            for (int i = 1; i < lines.Length-1; i++)
            {
                // 일단 각 줄의 data를 하나씩 끊어
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length <= 1) throw new Exception($"values[{i}].Length <= 1");

                // 각각의 값을 key와 value 쌍으로 집어 느
                var entry = new Dictionary<CatHeader, object>();
                for (int j = 2; j < header.Count && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                        finalvalue = n;
                    else if (float.TryParse(value, out f))
                        finalvalue = f;

                    entry[header[j]] = finalvalue;
                }

                if (int.TryParse(values[0], out int chapter))
                {
                    if (!result.ContainsKey(chapter))
                        result[chapter] = new Dictionary<int, List<Dictionary<CatHeader, object>>>();
                    if (int.TryParse(values[1], out int stage))
                    {
                        if (!result[chapter].ContainsKey(stage))
                            result[chapter][stage] = new List<Dictionary<CatHeader, object>>();
                        result[chapter][stage].Add(entry);
                    }
                    else throw new Exception($"Stage Error (stage == {values[1]})");
                }
                else throw new Exception($"Chapter Error (stage == {values[0]})");
            }

            return result;
        }
        catch(Exception e)
        {
            Debug.LogError($"Read Cat Data 오류: {e.Message}");
            return null;
        }
    }

    public static string[] ParseDollar(string data)
    {
        return data.Split('$');
    }
}