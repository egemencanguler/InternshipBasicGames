using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pong
{
    public class GameCore : MonoBehaviour
    {
        public ObjectBounds[] objectsBounds = new ObjectBounds[2];
        private int scoreLeft = 0, scoreRight = 0;
        public TextMeshProUGUI text;


        private void Awake()
        {
            objectsBounds = FindObjectsOfType<ObjectBounds>();
        }

        public void GoScore(ObjectTagList.ObjectTags objectTag)
        {
            if (objectTag.Equals(ObjectTagList.ObjectTags.RightScore))
            {
                scoreLeft++;
            }
            else if (objectTag.Equals(ObjectTagList.ObjectTags.LeftScore))
            {
                scoreRight++;
            }
            text.text = "" + scoreLeft + " - " + scoreRight;

        }
    }

}


