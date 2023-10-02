using System.Collections.Generic;

[System.Serializable]
public class ScoreData
{
    public int id;
    public int jogador_id;
    public string name;
    public int score;
    public string updated_at;
}

[System.Serializable]
public class ScoreRoot
{
    public List<ScoreData> players;
}

