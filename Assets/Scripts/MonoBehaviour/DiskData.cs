using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Concept.DiskConcept;

public class DiskData : MonoBehaviour
{
    [SerializeField] private List<DiskKeySprite> _sprites;
    public Dictionary<ConceptKey, Sprite> GetSpriteDictionary(Disk.SpriteType spriteType = Disk.SpriteType.Stance)
    {
        switch (spriteType)
        {
            case Disk.SpriteType.Stance: return _sprites.ToDictionary(x => x.key, x => x.sprite);
            case Disk.SpriteType.Throw: return _sprites.ToDictionary(x => x.key, x => x.throwSprite);
            default: throw new NotImplementedException();
        }
    }
}
