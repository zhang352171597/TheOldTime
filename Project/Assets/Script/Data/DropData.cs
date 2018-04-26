using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

/// <summary>
/// 掉落数据类
/// </summary>
public class enDropData {

    private class enSingleDrop
    {
        /// <summary>
        /// 概率
        /// </summary>
        public float probability;
        public string dropItemID;
        public int count;
        public int randomCount;

        public void ReceivedByMust(JSONNode node)
        {
            probability = 1;
            dropItemID = node[0];
            count = node[1].AsInt;
            randomCount = node[2].AsInt;
        }
        public void ReceivedByRandom(JSONNode node)
        {
            probability = node[0].AsFloat;
            dropItemID = node[1];
            count = node[2].AsInt;
            randomCount = node[3].AsInt;
        }
        public int dropResult
        {
            get
            {
                //temp
                return 1;

                var isDrop = probability >= 1;
                isDrop = isDrop ? isDrop : Random.Range(0, 1f) <= probability;
                if (isDrop)
                {
                    if (randomCount > 0)
                        return Random.Range(count, randomCount) + 1;
                    else
                        return count;
                }
                else
                    return 0;
            }
        }
    }
    public struct strDropResult
    {
        public string id;
        public int count;
    }
    List<enSingleDrop> _mustDrop;
    List<enSingleDrop> _randomDrop;
    List<strDropResult> _dropResultCache;

	public enDropData(string nodeStr)
    {
        var node = JSONNode.Parse(nodeStr);
        _mustDrop = new List<enSingleDrop>();
        _randomDrop = new List<enSingleDrop>();
        _dropResultCache = new List<strDropResult>();
        var mustNode = node[DropTag.mustDrop];
        var randomNode = node[DropTag.randomDrop];
        for(int i = 0 ; i < mustNode.Count; ++i)
        {
            var d = new enSingleDrop();
            d.ReceivedByMust(mustNode[i]);
            _mustDrop.Add(d);
        }
        for (int i = 0; i < randomNode.Count; ++i)
        {
            var d = new enSingleDrop();
            d.ReceivedByRandom(randomNode[i]);
            _randomDrop.Add(d);
        }
    }

    public List<strDropResult> dropResult
    {
        get
        {
            _dropResultCache.Clear();
            for(int i = 0 ; i < _mustDrop.Count; ++i)
            {
                var count = _mustDrop[i].dropResult;
                if(count > 0)
                {
                    strDropResult d;
                    d.id = _mustDrop[i].dropItemID;
                    d.count = count;
                    _dropResultCache.Add(d);
                }
            }

            for (int i = 0; i < _randomDrop.Count; ++i)
            {
                var count = _randomDrop[i].dropResult;
                if (count > 0)
                {
                    strDropResult d;
                    d.id = _randomDrop[i].dropItemID;
                    d.count = count;
                    _dropResultCache.Add(d);
                }
            }
            return _dropResultCache;
        }
    }
}


