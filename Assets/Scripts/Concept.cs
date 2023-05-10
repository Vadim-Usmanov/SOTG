using System;
using UnityEngine;

namespace Concept
{
    public enum Stance { Backhand, Forehand, Base}
    public enum Hand { Righthanded, Lefthanded }
    namespace DiskConcept
    {
        [Serializable] public struct ConceptKey
        {
            public ConceptKey(Orientation.Value orientation, Stance hold, Hand hand)
            {
                _orientation = orientation;
                _hold = hold;
                _hand = hand;
            }
            public Orientation.Value _orientation;
            public Stance _hold;
            public Hand _hand;
        }
        [Serializable] public struct DiskKeySprite
        {
            public ConceptKey key;
            public Sprite sprite;
            public Sprite throwSprite;
        }
    }
}
